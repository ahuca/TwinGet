// This file is licensed to you under MIT license.

namespace TwinGet.Utils.Xml
{
    public static class XmlSerializer
    {
        /// <summary>
        /// Try to deserialize XML file to object.
        /// </summary>
        /// <typeparam name="T">The type of the deserialized object.</typeparam>
        /// <param name="filePath">Path to the xml-formatted project file.</param>
        /// <returns>The deserialized object if successful. Otherwise null.</returns>
        public static T? TryDeserializeXmlFile<T>(string filePath)
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

        /// <summary>
        /// Try to deserialize XML file to object.
        /// </summary>
        /// <typeparam name="T">The type of the deserialized object.</typeparam>
        /// <param name="filePath">Path to the xml-formatted project file.</param>
        /// <returns>The deserialized object if successful. Otherwise null.</returns>
        public static async Task<T?> TryDeserializeXmlFileAsync<T>(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty("path", nameof(filePath));

            var readXmlContentTask = File.ReadAllTextAsync(filePath);
            System.Xml.Serialization.XmlSerializer serializer = new(typeof(T));

            T? projectFile;

            string xmlContent = await readXmlContentTask;
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
