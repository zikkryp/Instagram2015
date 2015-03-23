using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Models;

namespace Instagram.API
{
    public class API
    {
        private static API _api = new API();

        private Feed feed = new Feed();

        public Feed Feed { get { return this.feed; } }

        private InstagramRequest instagramRequest;

        public static async Task<Feed> GetFeedAsync()
        {
            _api = new API();
            _api.instagramRequest = new InstagramRequest("users/self/feed");
            
            await _api.instagramRequest.GetDataAsync();

            if (_api.instagramRequest.Status == RequestStatus.Success)
            {
                var feedObject = _api.instagramRequest.Result;

                _api.feed = JsonParser.ParseFeed(feedObject);
            }

            return _api.Feed;
        }
    }
}
