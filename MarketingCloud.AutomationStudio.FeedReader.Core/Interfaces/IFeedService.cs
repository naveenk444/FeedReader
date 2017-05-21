using System.Collections.Generic;
using MarketingCloud.AutomationStudio.FeedReader.Core.Entities;

namespace MarketingCloud.AutomationStudio.FeedReader.Core.Interfaces
{
    public interface IFeedService
    {
        IEnumerable<Feed> GetFeeds(int noOfFeeds, string strUserName);
    }
}
