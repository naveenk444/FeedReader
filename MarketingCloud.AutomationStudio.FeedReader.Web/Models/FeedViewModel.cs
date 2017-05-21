using System;
using System.ComponentModel;

namespace MarketingCloud.AutomationStudio.FeedReader.Web.Models
{
    public class FeedViewModel
    {
        [DisplayName("Username")]
        public string UserName { get; set; }

        [DisplayName("User Screen Name")]
        public string UserScreenName { get; set; }

        [DisplayName("User Profile Image")]
        public string UserProfileImageUrl { get; set; }

        [DisplayName("Media Image")]
        public string MediaImageUrl { get; set; }

        [DisplayName("Tweet Content")]
        public string Content { get; set; }

        [DisplayName("Retweet Count")]
        public int RetweetCount { get; set; }

        [DisplayName("Tweet Date")]
        public DateTime PostedDate { get; set; }
    }
}