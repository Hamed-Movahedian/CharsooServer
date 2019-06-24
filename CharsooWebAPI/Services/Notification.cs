using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using Newtonsoft.Json.Linq;


namespace CharsooWebAPI.Services
{
    [RoutePrefix("api/Notification")]
    public class Notification : ApiController
    {
        private static string _token = "587ebf53ba61daa4124a9ccc2e780b64d0aecc73";
        private static string _title = "Title";
        private static string _message = "Message";
        private static string _pushUrl = "http://api.pushe.co/v2/messaging/notifications/";


        [HttpGet, Route("Send")]
        public async Task<IHttpActionResult> SendNotification()
        {
            HttpClient client = new HttpClient();

            var content = new JObject(new
            {
                app_ids="com.Matarsak.charsoo",
                data = new JObject(new
                {
                    title=_title,
                    content=_message
                })
            });
            var stringContent = new StringContent(content.ToString(), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue(_token);
            var response = await client.PostAsync(_pushUrl, stringContent);

            return StatusCode(HttpStatusCode.NoContent);
        }


    }
}