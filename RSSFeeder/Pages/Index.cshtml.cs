using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeHollow.FeedReader;

namespace RSSFeeder.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public static IList<FeedItem> allfeed;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            string url = "https://habr.com/ru/rss/interesting/";
            var urlsTask = FeedReader.GetFeedUrlsFromUrlAsync(url);
            var urls = urlsTask.Result;
            string feedUrl = url;
            var readerTask = FeedReader.ReadAsync(feedUrl);
            readerTask.ConfigureAwait(false);
            allfeed = readerTask.Result.Items;
            foreach (var item in readerTask.Result.Items)
            {
                Console.WriteLine(item.Title + " - " + item.Link);
            }
        }
    }
}
