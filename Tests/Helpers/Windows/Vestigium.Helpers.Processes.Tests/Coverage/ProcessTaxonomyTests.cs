using Vestigium.Helpers;
using Vestigium.Helpers.Processes;

namespace Vestigium.Helpers.Processes.Tests;

[Collection("Logger")]
public sealed class ProcessTaxonomyTests
{
    [Fact]
    public void Processes_subcategories_are_registered()
    {
        string[] required =
        [
            TestLog.Subcategories.Probe,
            TestLog.Subcategories.Identity,
            TestLog.Subcategories.Inventory,
            TestLog.Subcategories.Process,
            TestLog.Subcategories.Thread,
            TestLog.Subcategories.Watch,
            TestLog.Subcategories.Start,
            TestLog.Subcategories.Kill,
            TestLog.Subcategories.System,
            TestLog.Subcategories.Campaign,
            TestLog.Subcategories.Jsonl
        ];
        foreach (var sub in required)
            Assert.True(TestLog.Taxonomy.IsSubcategoryRegistered(TestLog.Category, sub), sub);
    }

    [Fact]
    public void Identity_is_processes()
        => Assert.Equal("Vestigium.Helpers.Processes", ProcessHelper.Identity);
}
