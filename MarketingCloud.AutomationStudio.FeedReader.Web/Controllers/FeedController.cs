using System;
using System.Configuration;
using  System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MarketingCloud.AutomationStudio.FeedReader.Core.Interfaces;
using MarketingCloud.AutomationStudio.FeedReader.Core.Services;
using MarketingCloud.AutomationStudio.FeedReader.Web.Models;

namespace MarketingCloud.AutomationStudio.FeedReader.Web.Controllers
{
    public class FeedController : Controller
    {
        #region [Properties]

        private readonly IFeedService _feedService;

        #endregion

        #region [Ctors]

        public FeedController()
        {
            _feedService = new FeedService();
        }

        #endregion

        #region  Actions

        [HttpGet]
        // GET: Feed
        public ActionResult Index()
        {
            var reader = new AppSettingsReader();
            Response.AddHeader("Refresh", reader.GetValue("FeedRefreshTimeInSec", typeof(string)).ToString());
            var feeds = _feedService.GetFeeds(Convert.ToInt16((reader.GetValue("FeedCount", typeof(string)))), reader.GetValue("Username", typeof(string)).ToString()).ToList();
            IList<FeedViewModel> model = new FeedViewModel[feeds.Count()];
            for (int i = 0; i < feeds.Count(); i++)
            {
                model[i] = new FeedViewModel();
                model[i].UserName = feeds[i].UserName;
                model[i].UserScreenName = feeds[i].UserScreenName;
                model[i].UserProfileImageUrl = feeds[i].UserProfileImageUrl;
                model[i].MediaImageUrl = feeds[i].MediaUrl;
                model[i].Content = feeds[i].Content;
                model[i].RetweetCount = feeds[i].RetweetCount;
                model[i].PostedDate = feeds[i].PostedDate;

            }

            return View(model);
        }

        #endregion
        
    }
}