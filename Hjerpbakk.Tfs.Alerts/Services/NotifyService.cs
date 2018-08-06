using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Hjerpbakk.Tfs.Alerts.Configuration;
using Hjerpbakk.Tfs.Alerts.Slack;
using Hjerpbakk.Tfs.Alerts.TFS;
using Polly;
using SlackConnector.Models;

namespace Hjerpbakk.Tfs.Alerts.Services {
    [ServiceContract(Namespace = "http://schemas.microsoft.com/TeamFoundation/2005/06/Services/Notification/03")]
    public interface INotifyService {
        [OperationContract(Action = "http://schemas.microsoft.com/TeamFoundation/2005/06/Services/Notification/03/Notify")]
        [XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        Task Notify(string eventXml, string tfsIdentityXml);
    }

    public class NotifyService : INotifyService {
        readonly ISlackIntegration slackIntegration;
        readonly string user;

        public NotifyService(ISlackIntegration slackIntegration, IAppConfig appConfig) {
            this.slackIntegration = slackIntegration;
            user = appConfig.User;
        }

        async Task INotifyService.Notify(string eventXml, string tfsIdentityXml) {
            if (string.IsNullOrEmpty(eventXml)) {
                throw new ArgumentException(nameof(eventXml));
            }

            var changedItem = new ChangedItem(eventXml);
            var link = new SlackAttachment {
                Title = changedItem.UrlTitle,
                TitleLink = changedItem.Url
            };

            Console.WriteLine(changedItem.Title + " - " + changedItem.UrlTitle);
            var slackHub = new SlackChatHub { Id = user };
            var policyResult = await Policy
                        .Handle<Exception>()
                        .WaitAndRetryAsync(new[] {
                            TimeSpan.FromSeconds(2),
                            TimeSpan.FromSeconds(4),
                            TimeSpan.FromSeconds(8)
                        })
                        .ExecuteAndCaptureAsync(
                    () => slackIntegration.SendMessageToChannel(slackHub, changedItem.Title, link));
            if (policyResult.FinalException != null) {
                Console.WriteLine($"Could not post to Slack {policyResult.FinalException}");
            }
        }
    }
}
