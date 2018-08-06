using System.Threading.Tasks;
using SlackConnector.Models;

namespace Hjerpbakk.Tfs.Alerts.Slack {
    public interface ISlackIntegration {
        Task Connect();
        Task Close();
        Task SendMessageToChannel(SlackChatHub channel, string text, params SlackAttachment[] attachments);
    }
}
