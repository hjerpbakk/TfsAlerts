using System.Xml;
using System.Xml.Serialization;

namespace Hjerpbakk.Tfs.Alerts.Extensions {
    public static class XmlDocumentExtensions {
        /// <summary>
        /// Deserializes XmlDocument object to Serializable object of type T.
        /// </summary>
        /// <typeparam name="T">Serializable object type as output type.</typeparam>
        /// <param name="document">XmlDocument object to be deserialized.</param>
        /// <returns>Deserialized serializable object of type T.</returns>
        public static T Deserialize<T>(this XmlDocument document) where T : class {
            XmlReader reader = new XmlNodeReader(document);
            var serializer = new XmlSerializer(typeof(T));
            T result = (T)serializer.Deserialize(reader);
            return result;
        }
    }
}
