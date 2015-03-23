using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Models;

namespace Instagram.API
{
    public class InstagramAPI
    {
        private static InstagramAPI _api = new InstagramAPI();
        private InstagramRequest instagramRequest;

        private Feed feed = new Feed();
        private User user = new User();

        public User User { get { return this.user; } }
        public Feed Feed { get { return this.feed; } }

        public static async Task<Feed> GetFeedAsync(bool refresh)
        {
            if (!refresh)
            {
                if (_api.Feed.Pagination.HasMorePages)
                {
                    var maxid = _api.Feed.Pagination.NextMaxId;

                    _api.instagramRequest = new InstagramRequest("users/self/feed", "max_id=" + _api.Feed.Pagination.NextMaxId);
                }
            }
            else
            {
                _api = new InstagramAPI();
                _api.instagramRequest = new InstagramRequest("users/self/feed");
            }

            await _api.instagramRequest.GetDataAsync();

            if (_api.instagramRequest.Status == RequestStatus.Success)
            {
                var feedObject = _api.instagramRequest.Result;

                var feed = JsonParser.ParseFeed(feedObject);

                if (refresh)
                {
                    _api.feed = feed;
                }
                else
                {
                    _api.AppendFeed(feed);
                }
            }

            return _api.Feed;
        }

        private void AppendFeed(Feed feed)
        {
            foreach (var item in feed.Items)
            {
                this.feed.Items.Add(item);
            }

            this.feed.Meta = feed.Meta;
            this.feed.Pagination = feed.Pagination;
        }

        public static async Task<User> GetUserAsync(string id)
        {
            _api = new InstagramAPI();

            _api.instagramRequest = new InstagramRequest(string.Format("users/{0}/", id));

            await _api.instagramRequest.GetDataAsync();

            if (_api.instagramRequest.Status == RequestStatus.Success)
            {
                var userObject = _api.instagramRequest.Result;

                _api.user = JsonParser.ParseUser(userObject);
            }

            return _api.user;
        }
    }
}
