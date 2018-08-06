using System.IO;
using Hjerpbakk.Tfs.Alerts.TFS;
using Xunit;

namespace Hjerpbakk.Tfs.Alerts.Tests {
    public class ChangedItemTests {
        [Fact]
        public void DescriptionChanged() {
            var xmlString = File.ReadAllText("../../../TestData/DescriptionChanged.xml");

            var changedItem = new ChangedItem(xmlString);

            Assert.Equal("Product Backlog Item changed: New backlog item", changedItem.Title);
            Assert.Equal("181902: Test", changedItem.UrlTitle);
            Assert.Equal("http://vd-tfs03:8080/tfs/DefaultCollection/DIPS/_workitems/edit/181902", changedItem.Url);
        }

        [Fact]
        public void StateChanged() {
            var xmlString = File.ReadAllText("../../../TestData/StateChanged.xml");

            var changedItem = new ChangedItem(xmlString);

            Assert.Equal("Product Backlog Item got state Ready For Test", changedItem.Title);
            Assert.Equal("181902: Test", changedItem.UrlTitle);
            Assert.Equal("http://vd-tfs03:8080/tfs/DefaultCollection/DIPS/_workitems/edit/181902", changedItem.Url);
        }
    }
}
