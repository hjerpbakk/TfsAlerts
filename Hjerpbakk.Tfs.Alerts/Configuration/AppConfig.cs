using System;

namespace Hjerpbakk.Tfs.Alerts.Configuration {
    public class AppConfig : IAppConfig {
        public string User => "D4UR74ZAS";
        public string SlackAPIToken { get; set; }
    }
}
