namespace Hjerpbakk.Tfs.Alerts.Configuration {
    public interface IAppConfig {
        string SlackAPIToken { get; }
        string User { get; }
    }
}
