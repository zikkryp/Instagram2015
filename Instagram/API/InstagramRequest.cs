using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Instagram.API
{
    public class InstagramRequest
    {
        public InstagramRequest(string method)
        {
            InitializeRequest(method);
        }

        public InstagramRequest(string method, string param)
        {
            InitializeRequest(method, param);
        }

        private const string REQUEST_BASE_URI = "https://api.instagram.com/v1/";
        public string requestUri;

        private JsonObject result;
        private RequestStatus status;

        private void InitializeRequest(string method)
        {
            string token = "5946413.039d35e.f9519f0fdd384170b44d7cf1b54afd6f";

            this.requestUri = string.Format("{0}{1}?access_token={2}", REQUEST_BASE_URI, method, token);
        }

        private void InitializeRequest(string method, string param)
        {
            string token = "5946413.039d35e.f9519f0fdd384170b44d7cf1b54afd6f";

            this.requestUri = string.Format("{0}{1}?access_token={2}&{3}", REQUEST_BASE_URI, method, token, param);
        }

        public async Task GetDataAsync()
        {
            var client = new HttpClient();

            var response = await client.GetAsync(this.requestUri);

            if (response.IsSuccessStatusCode)
            {
                Status = RequestStatus.Success;

                var content = await response.Content.ReadAsStringAsync();

                Result = JsonObject.Parse(content);
            }
        }

        public JsonObject Result
        {
            get { return this.result; }
            private set { this.result = value; }
        }

        public RequestStatus Status
        {
            get { return this.status; }
            private set { this.status = value; }
        }
    }

    public enum RequestStatus
    {
        Success,
        Error,
        NoInternetConnection
    }
}
