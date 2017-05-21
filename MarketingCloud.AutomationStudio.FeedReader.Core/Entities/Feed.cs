using System;

namespace MarketingCloud.AutomationStudio.FeedReader.Core.Entities
{
    public class Feed
    {
        public string UserName { get; set; }
        public string UserScreenName { get; set; }
        public string UserProfileImageUrl { get; set; }
        public string MediaUrl { get; set; }
        public string Content { get; set; }
        public int RetweetCount { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
