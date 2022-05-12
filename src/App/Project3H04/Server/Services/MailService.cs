using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace Project3H04.Server.Services {
    public static class MailService {
        public static IRestResponse SendMail(string emailToSendTo, string body, string subject) {
            var client = new RestClient {
                BaseUrl = new Uri("https://api.mailgun.net/v3"),
                Authenticator = new HttpBasicAuthenticator("api", "b3dd1da5c6a6cbefc1e7e6e7a0b507e8-7b8c9ba8-da8a2a1f")
            };
            var request = new RestRequest();

            request.AddParameter ("domain", "sandboxa40d44db91b94246b7d785783ca0f857.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Hooop-Gallery <no-reply@hooop-gallery.be>");
            request.AddParameter("to", emailToSendTo);
            request.AddParameter("subject", subject);
            request.AddParameter("html", body);
            request.Method = Method.POST;

            return client.Execute (request);
        }
    }
}
