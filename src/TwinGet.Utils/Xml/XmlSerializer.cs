// This file is licensed to you under MIT license.

namespace TwinGet.Utils.Xml
{
    public static class XmlSerializer
    {
        /// <summary>
        /// Try to deserialize XML file to object.
        /// </summary>
        /// <param name="filePath">Path to the xml-formatted project file.</param>
        /// <returns>The deserialized object if successful. Otherwise null.</returns>
        public static T? TryDeserializeXmlFileTo<T>(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty("path", nameof(filePath));

            string xmlContent = File.ReadAllText(filePath);
            System.Xml.Serialization.XmlSerializer serializer = new(typeof(T));

            T? projectFile;
            using (StringReader reader = new(xmlContent))
            {
                try
                {
                    projectFile = (T?)serializer?.Deserialize(reader);
                    return projectFile;
                }
                catch { }

            }

            return default;
        }
    }
}
