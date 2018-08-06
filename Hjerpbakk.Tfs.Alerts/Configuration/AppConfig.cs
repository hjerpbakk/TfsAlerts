using System;

namespace Hjerpbakk.Tfs.Alerts.Configuration {
    public class AppConfig : IAppConfig {
        public string User => "U23H62Z5Y";
        public string SlackAPIToken { get; set; }
    }
}
