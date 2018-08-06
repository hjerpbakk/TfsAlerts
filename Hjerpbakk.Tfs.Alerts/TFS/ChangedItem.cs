using System.Linq;
using System.Xml;
using Hjerpbakk.Tfs.Alerts.Extensions;

namespace Hjerpbakk.Tfs.Alerts.TFS {
    public readonly struct ChangedItem {
        public ChangedItem(string tfsChangeXml) {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(tfsChangeXml);
            var workItemChangedEvent = xmlDocument.Deserialize<WorkItemChangedEvent>();
            var kind = workItemChangedEvent.CoreFields.StringFields.Single(f => f.Name == "Work Item Type").NewValue;
            var assignedTo = workItemChangedEvent.CoreFields.StringFields.Single(f => f.Name == "Assigned To");
            var oldOwner = assignedTo.OldValue;
            var newOwner = assignedTo.NewValue;
            var state = workItemChangedEvent.CoreFields.StringFields.Single(f => f.Name == "State");
            var oldState = state.OldValue;
            var newState = state.NewValue;
            var reason = workItemChangedEvent.CoreFields.StringFields.Single(f => f.Name == "Reason").NewValue;
            if (oldState != newState && newState == "Ready For Test") {
                Title = kind + " got state " + newState;
            } else if (oldOwner != newOwner) {
                Title = kind + " was assigned to you";
            } else {
                Title = kind + " changed: " + reason;
            }

            var id = workItemChangedEvent.CoreFields.IntegerFields.Single(f => f.Name == "ID").NewValue;
            Url = "http://vd-tfs03:8080/tfs/DefaultCollection/DIPS/_workitems/edit/" + id;
            UrlTitle = id + ": " + workItemChangedEvent.WorkItemTitle;
        }

        public string Title { get; }
        public string UrlTitle { get; }
        public string Url { get; }


    }
}
