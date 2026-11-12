<#
.SYNOPSIS
    Wipe bin, obj, .vs, and TestResults for every project in a solution, then restore and rebuild.

.DESCRIPTION
    Reads a .slnx (preferred) or a .sln. Close Visual Studio first or the deletes will fail on locked files.

.PARAMETER SolutionPath
    Path to a .slnx, a .sln, or the directory that contains one. A directory prefers .slnx.

.PARAMETER SkipBuild
    Wipe only. Do not restore or build.

.EXAMPLE
    .\Nuke-Solution.ps1 -SolutionPath D:\Source\Clone\Vestigium
    .\Nuke-Solution.ps1 -SolutionPath D:\Source\Clone\Vestigium\Vestigium.slnx -SkipBuild
#>
[CmdletBinding()]
param (
    [Parameter(Mandatory = $true)]
    [string]$SolutionPath,

    [switch]$SkipBuild
)

$ErrorActionPreference = "Stop"
# dotnet writes progress to stderr. Do not treat that as a terminating error.
if (Get-Variable -Name PSNativeCommandUseErrorActionPreference -ErrorAction SilentlyContinue) {
    $PSNativeCommandUseErrorActionPreference = $false
}

function Resolve-Solution {
    param([string]$InputPath)

    if (-not (Test-Path $InputPath)) {
        throw "Path not found: $InputPath"
    }

    $item = Get-Item $InputPath
    if (-not $item.PSIsContainer) {
        $ext = $item.Extension.ToLowerInvariant()
        if ($ext -notin ".slnx", ".sln") {
            throw "Expected a .slnx, a .sln, or a directory. Got $($item.Name)"
        }
        return $item.FullName
    }

    $slnx = Get-ChildItem -Path $item.FullName -Filter *.slnx -File | Select-Object -First 1
    if ($slnx) { return $slnx.FullName }

    $sln = Get-ChildItem -Path $item.FullName -Filter *.sln -File | Select-Object -First 1
    if ($sln) { return $sln.FullName }

    throw "No .slnx or .sln in $($item.FullName)"
}

function Get-ProjectPaths {
    param([string]$SolutionFile)

    $ext = [System.IO.Path]::GetExtension($SolutionFile).ToLowerInvariant()
    $paths = New-Object System.Collections.Generic.List[string]

    if ($ext -eq ".slnx") {
        [xml]$xml = Get-Content -Path $SolutionFile -Raw
        foreach ($node in $xml.SelectNodes("//Project")) {
            $path = $node.GetAttribute("Path")
            if (-not [string]::IsNullOrWhiteSpace($path)) {
                $paths.Add($path)
            }
        }
        return $paths
    }

    $regex = 'Project\("\{[A-F0-9\-]+\}"\) = "[^"]+", "(?<Path>[^"]+\.(csproj|vbproj|fsproj|shproj|vcxproj))", "\{[A-F0-9\-]+\}"'
    foreach ($line in Get-Content $SolutionFile) {
        if ($line -match $regex) {
            $paths.Add($Matches.Path)
        }
    }
    return $paths
}

function Remove-Tree {
    param([string]$Path)

    if (-not (Test-Path $Path)) { return $false }

    Write-Host "Obliterating: $Path" -ForegroundColor DarkGray
    try {
        Remove-Item -Path $Path -Recurse -Force -ErrorAction Stop
        return $true
    }
    catch {
        Write-Host "FAILED to delete $Path. Close Visual Studio and any testhost, then rerun." -ForegroundColor Yellow
        Write-Host $_.Exception.Message -ForegroundColor Yellow
        return $false
    }
}

$solution = Resolve-Solution $SolutionPath
$slnDir = Split-Path $solution -Parent
$projects = @(Get-ProjectPaths $solution)

Write-Host "=====================================================" -ForegroundColor Red
Write-Host " INITIATING AUTOMATED PRECISION NUCLEAR STRIKE       " -ForegroundColor Red
Write-Host "=====================================================" -ForegroundColor Red
Write-Host "Target: $solution" -ForegroundColor Yellow
Write-Host "Projects: $($projects.Count)" -ForegroundColor Yellow
Write-Host ""

Write-Host "[PHASE 1] Official SDK clean..." -ForegroundColor Cyan
& dotnet clean $solution 2>&1 | ForEach-Object { Write-Host $_ }
if ($LASTEXITCODE -ne 0) {
    Write-Host "dotnet clean reported an error. The wipe still runs." -ForegroundColor Yellow
}
Write-Host ""

Write-Host "[PHASE 2] Wiping caches..." -ForegroundColor Cyan
$failed = 0

foreach ($name in @(".vs", "TestResults", ".vscode")) {
    $folder = Join-Path $slnDir $name
    if ((Test-Path $folder) -and -not (Remove-Tree $folder)) { $failed++ }
}

if ($projects.Count -eq 0) {
    Write-Host "No projects parsed from the solution. Sweeping the tree for bin and obj." -ForegroundColor Yellow
}

$seen = @{}
foreach ($rel in $projects) {
    $projDir = Split-Path (Join-Path $slnDir $rel) -Parent
    foreach ($leaf in @("bin", "obj")) {
        $dir = Join-Path $projDir $leaf
        if ($seen.ContainsKey($dir)) { continue }
        $seen[$dir] = $true
        if ((Test-Path $dir) -and -not (Remove-Tree $dir)) { $failed++ }
    }
}

# Catch a project the solution file does not list.
Get-ChildItem -Path $slnDir -Recurse -Directory -Force -ErrorAction SilentlyContinue |
    Where-Object { $_.Name -in @("bin", "obj") -and $_.FullName -notmatch '\\(\.git|node_modules)\\' } |
    ForEach-Object {
        if ($seen.ContainsKey($_.FullName)) { return }
        $seen[$_.FullName] = $true
        if (-not (Remove-Tree $_.FullName)) { $failed++ }
    }

Write-Host ""
if ($failed -gt 0) {
    Write-Host "$failed directories could not be deleted. Close Visual Studio and rerun." -ForegroundColor Yellow
}

if ($SkipBuild) {
    Write-Host "SkipBuild set. Wipe only." -ForegroundColor Green
    return
}

Write-Host "[PHASE 3] Restore and rebuild..." -ForegroundColor Cyan
& dotnet restore $solution 2>&1 | ForEach-Object { Write-Host $_ }
& dotnet build $solution --no-incremental 2>&1 | ForEach-Object { Write-Host $_ }

Write-Host ""
if ($failed -gt 0) {
    Write-Host ".vs was locked. Close Visual Studio and rerun if Test Explorer still shows stale results." -ForegroundColor Yellow
}
if ($LASTEXITCODE -eq 0) {
    Write-Host "=====================================================" -ForegroundColor Green
    Write-Host " SEQUENCE COMPLETE. ENVIRONMENT IS STERILE AND BUILT." -ForegroundColor Green
    Write-Host "=====================================================" -ForegroundColor Green
}
else {
    Write-Host "=====================================================" -ForegroundColor Red
    Write-Host " BUILD FAILED. CHECK THE COMPILER OUTPUT ABOVE.      " -ForegroundColor Red
    Write-Host "=====================================================" -ForegroundColor Red
    exit 1
}
