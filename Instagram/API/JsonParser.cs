using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Instagram.API
{
    public static class JsonParser
    {
        public static Feed ParseFeed(JsonObject feedObject)
        {
            Feed feed = new Feed();

            var jsonArray = feedObject["data"].GetArray();

            feed.Pagination = GetPagination(feedObject["pagination"].GetObject());

            foreach (JsonValue feedItem in jsonArray)
            {
                var itemObject = feedItem.GetObject();

                //await new MessageDialog(itemObject["type"].GetString()).ShowAsync();
                var Type = GetType(itemObject["type"].GetString());
                //MediaType Type = MediaType.Image;
                var user = GetUser(itemObject["user"].GetObject());
                //User user = null;
                var tags = GetTags(itemObject["tags"].GetArray());
                //Tags tags = null;
                var location = GetLocation(itemObject["location"]);
                //Location location = null;
                var comments = GetComments(itemObject["comments"].GetObject());
                //Comment comments = null;
                var filter = itemObject["filter"].GetString();
                //string filter = "";
                var createdTime = itemObject["created_time"].GetString();
                //string createdTime = "";
                var link = itemObject["link"].GetString();
                //string link = "";
                var likes = GetLikes(itemObject["likes"].GetObject());
                //Likes likes = null;
                var images = GetImages(itemObject["images"].GetObject());
                //Images images = null;
                var usersInPhoto = GetUsersInPhoto(itemObject["users_in_photo"].GetArray());
                //UsersInPhoto usersInPhoto = null;
                var caption = GetCaption(itemObject["caption"]);
                //Caption caption = null;
                var userhasLiked = itemObject["user_has_liked"].GetBoolean();
                //bool userhasLiked = false;
                var id = itemObject["id"].GetString();
                //string id = "";

                FeedItem item;

                if (Type == MediaType.Video)
                {
                    var videos = GetVideos(itemObject["videos"].GetObject());

                    item = new FeedItem(null, tags, Type, location, comments, filter, createdTime, link, likes, images, usersInPhoto, caption, userhasLiked, id, user, videos);
                }
                else
                {
                    item = new FeedItem(null, tags, Type, location, comments, filter, createdTime, link, likes, images, usersInPhoto, caption, userhasLiked, id, user);
                }

                feed.Items.Add(item);
            }

            return feed;
        }

        public static User ParseUser(JsonObject userObject)
        {
            var dataObject = userObject["data"].GetObject();
            var counts = GetCounts(dataObject["counts"].GetObject());

            return new User(
                dataObject["username"].GetString(),
                dataObject["profile_picture"].GetString(),
                dataObject["full_name"].GetString(),
                dataObject["id"].GetString(),
                dataObject["website"].GetString(),
                dataObject["bio"].GetString(),
                counts);
        }

        private static Pagination GetPagination(JsonObject paginationObject)
        {
            if (paginationObject.ContainsKey("next_url") && paginationObject.ContainsKey("next_max_id"))
            {
                var nextUrl = paginationObject["next_url"].GetString();
                var nextMaxId = paginationObject["next_max_id"].GetString();

                return new Pagination(nextUrl, nextMaxId);
            }

            return new Pagination();
        }

        private static Meta GetMeta(JsonObject metaObject)
        {
            int code = (int)metaObject["code"].GetNumber();

            if (code == 200)
            {
                return new Meta(code);
            }

            string errorType = metaObject["error_type"].GetString();
            string errorMessage = metaObject["error_message"].GetString();

            return new Meta(code, errorType, errorMessage);
        }

        private static Tags GetTags(JsonArray tagsArray)
        {
            if (tagsArray.Count == 0)
            {
                return null;
            }

            List<Tag> tags = new List<Tag>();
            foreach (var tag in tagsArray)
            {
                tags.Add(new Tag(tag.GetString()));
            }

            return new Tags(tags);
        }

        private static MediaType GetType(string type)
        {
            if (type.Equals("video"))
            {
                return MediaType.Video;
            }

            return MediaType.Image;
        }

        private static Location GetLocation(IJsonValue locationValue)
        {
            if (locationValue.ValueType == JsonValueType.Null)
            {
                return null;
            }

            var locationObject = locationValue.GetObject();

            double latitude = 0;
            double longitude = 0;

            if (locationObject.ContainsKey("latitude") && locationObject.ContainsKey("longitude"))
            {
                latitude = locationObject["latitude"].GetNumber();
                longitude = locationObject["longitude"].GetNumber();
            }

            if (locationObject.ContainsKey("name") && locationObject.ContainsKey("id"))
            {
                var name = locationObject["name"].GetString();
                var id = locationObject["id"].GetNumber();

                return new Location(latitude, longitude, name, id);
            }

            if (locationObject.ContainsKey("id"))
            {
                var id = locationObject["id"].GetNumber();

                return new Location(id);
            }

            return new Location(latitude, longitude);
        }

        private static Comments GetComments(JsonObject commentsObject)
        {
            int count = (int)commentsObject["count"].GetNumber();

            JsonArray commentsArray = commentsObject["data"].GetArray();

            ObservableCollection<Comment> comments = new ObservableCollection<Comment>();

            foreach (JsonValue commentValue in commentsArray)
            {
                var commentObject = commentValue.GetObject();
                var createdTime = commentObject["created_time"].GetString();
                var text = commentObject["text"].GetString();
                var id = commentObject["id"].GetString();
                var from = GetUser(commentObject["from"].GetObject());

                comments.Add(new Comment(createdTime, text, from, id));
            }

            return new Comments(count, comments);
        }

        private static Likes GetLikes(JsonObject likesObject)
        {
            int count = (int)likesObject["count"].GetNumber();

            JsonArray likesArray = likesObject["data"].GetArray();

            ObservableCollection<User> liked = new ObservableCollection<User>();
            foreach (JsonValue likeValue in likesArray)
            {
                var likeObject = likeValue.GetObject();
                var user = GetUser(likeObject);
                liked.Add(user);
            }

            return new Likes(count, liked);
        }

        private static Images GetImages(JsonObject imagesObject)
        {
            var standardObject = imagesObject["standard_resolution"].GetObject();
            StandardResolution standardResolution = new StandardResolution(standardObject["url"].GetString(), standardObject["width"].GetNumber(), standardObject["height"].GetNumber());

            var lowObject = imagesObject["low_resolution"].GetObject();
            LowResolution lowResolution = new LowResolution(lowObject["url"].GetString(), lowObject["width"].GetNumber(), lowObject["height"].GetNumber());

            var thumbObject = imagesObject["thumbnail"].GetObject();
            Thumbnail thumbnail = new Thumbnail(thumbObject["url"].GetString(), thumbObject["width"].GetNumber(), thumbObject["height"].GetNumber());

            return new Images(standardResolution, lowResolution, thumbnail);
        }

        private static Videos GetVideos(JsonObject videoObject)
        {
            var standardObject = videoObject["standard_resolution"].GetObject();
            StandardResolution standardResolution = new StandardResolution(standardObject["url"].GetString(), standardObject["width"].GetNumber(), standardObject["height"].GetNumber());

            var lowObject = videoObject["low_resolution"].GetObject();
            LowResolution lowResolution = new LowResolution(lowObject["url"].GetString(), lowObject["width"].GetNumber(), lowObject["height"].GetNumber());

            var thumbObject = videoObject["low_bandwidth"].GetObject();
            LowBandwidth lowBandwidth = new LowBandwidth(thumbObject["url"].GetString(), thumbObject["width"].GetNumber(), thumbObject["height"].GetNumber());

            return new Videos(lowBandwidth, lowResolution, standardResolution);
        }

        private static UsersInPhoto GetUsersInPhoto(JsonArray usersInPhotoArray)
        {
            ObservableCollection<UserInPhoto> users = new ObservableCollection<UserInPhoto>();

            foreach (var userInPhotoValue in usersInPhotoArray)
            {
                var userInPhotoObject = userInPhotoValue.GetObject();

                Position position = new Position(userInPhotoObject["position"].GetObject()["x"].GetNumber(), userInPhotoObject["position"].GetObject()["y"].GetNumber());
                User user = GetUser(userInPhotoObject["user"].GetObject());
            }

            return new UsersInPhoto(users);
        }

        private static Caption GetCaption(IJsonValue captionValue)
        {
            if (captionValue.ValueType == JsonValueType.Null)
            {
                return null;
            }

            var captionObject = captionValue.GetObject();
            var createdTime = captionObject["created_time"].GetString();
            var text = captionObject["text"].GetString();
            var from = GetUser(captionObject["from"].GetObject());
            var id = captionObject["id"].GetString();

            return new Caption(createdTime, text, from, id);
        }

        private static User GetUser(JsonObject userObject)
        {
            var username = userObject["username"].GetString();
            var profilePicture = userObject["profile_picture"].GetString();
            var id = userObject["id"].GetString();
            var fullName = userObject["full_name"].GetString();

            return new User(username, profilePicture, fullName, id);
        }

        private static Counts GetCounts(JsonObject countsObject)
        {
            var media = (int)countsObject["media"].GetNumber();
            var follows = (int)countsObject["follows"].GetNumber();
            var followedBy = (int)countsObject["followed_by"].GetNumber();

            return new Counts(media, follows, followedBy);
        }
    }
}
