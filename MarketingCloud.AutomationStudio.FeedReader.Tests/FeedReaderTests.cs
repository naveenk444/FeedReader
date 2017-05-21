using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketingCloud.AutomationStudio.FeedReader.Core.Services;
using System.Linq;

namespace MarketingCloud.AutomationStudio.FeedReader.Tests
{
    [TestClass]
    public class FeedReaderTests
    {
        [TestMethod]
        public void ValidateConfigEntries()
        {
            var feedService = new FeedService();
            Assert.IsTrue(feedService.ValidateConfigurationEntries());
        }
        [TestMethod]
        public void ValidCredentials()
        {
            var feedService = new FeedService();
            string strAccessToken = feedService.GetAccessToken().ToString();
            Assert.IsTrue(!string.IsNullOrEmpty(strAccessToken));

        }
        [TestMethod]
        public void GetFeeds()
        {
            var feedService = new FeedService();
            var feeds = feedService.GetFeeds(10, "salesforce");
            Assert.IsTrue(feeds.Count() == 10);
        }
        
    }
}
