using System;
using System.Threading.Tasks;
using Hjerpbakk.Tfs.Alerts.Configuration;
using SlackConnector;
using SlackConnector.Models;

namespace Hjerpbakk.Tfs.Alerts.Slack {
    class SlackIntegration : ISlackIntegration {
        readonly ISlackConnector connector;
        readonly string slackAPIToken;

        ISlackConnection connection;

        public SlackIntegration(ISlackConnector connector, IAppConfig config) {
            this.connector = connector ?? throw new ArgumentNullException(nameof(connector));
            slackAPIToken = config.SlackAPIToken;
        }

        /// <summary>
        ///     Connects the bot to Slack.
        /// </summary>
        /// <returns>No object or value is returned by this method when it completes.</returns>
        public async Task Connect() {
            connection = await connector.Connect(slackAPIToken);
            if (connection == null) {
                throw new ArgumentException("Could not connect to Slack.");
            }
        }

        public async Task Close() {
            await connection.Close();
        }

        public async Task SendMessageToChannel(SlackChatHub channel, string text, params SlackAttachment[] attachments) {
            try {
                await Connect();
                await connection.IndicateTyping(channel);
                var message = new BotMessage { ChatHub = channel, Text = text };
                if (attachments.Length > 0) {
                    message.Attachments = attachments;
                }

                await connection.Say(message);
            } finally {
                await Close();
            }
        }
    }
}
