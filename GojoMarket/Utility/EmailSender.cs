using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace GojoMarket.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }
        public async Task Execute(string email, string subject, string body)
        {
            MailjetClient client = new MailjetClient("4f615eb163780d30f76f1cd658a6c634", "28dea08ee2216027bef0a1b69238a8d5")
            {
              
            };
          
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "yousouf.m.salah@gmail.com"},
        {"Name", "yousef"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "Gojo_Market"
         }
        }
       }
      }, {
       "Subject",
       subject
      },  {
       "HTMLPart",
      body
      }
     }
             });
             await client.PostAsync(request);
        }
    }
}
