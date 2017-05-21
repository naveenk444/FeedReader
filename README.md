# FeedReader
Read the twitter feeds for a given user screen name
The solution consists of three projects
  MarketingCloud.AutomationStudio.FeedReader.Core - Backend layer to call the Twitter API
  MarketingCloud.AutomationStudio.FeedReader.Tests - Unit Testing
  MarketingCloud.AutomationStudio.FeedReader.Web - ASP.NET MVC Front End
  
Application displays the latest 10 feeds for userscreenname 'salesforce'. The feeds will be refreshed every 1 minute. There is a Filter text box
on the top right corner to filter the feeds based on the tweet content.

All the paramters such as userscreenname, time to refresh, OAuthConsumerKey, OAuthConsumerSecret, TokenURL, APIURL can be changed and parameterized
in web.config.

The application has been hosted at http://twitterfeedreader.azurewebsites.net/
