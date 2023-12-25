// This file is licensed to you under MIT license.

using System.Xml;

namespace TwinGet.TwincatInterface.Utils
{
    public class PlcProjectFileHelper
    {
        private readonly string _filePath;

        public PlcProjectFileHelper(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("PLC project file not found.", filePath);
            }

            _filePath = Path.GetFullPath(filePath);
        }

        public async Task<string> GetProjectGuidAsync()
        {
            using XmlReader reader = XmlReader.Create(_filePath, new XmlReaderSettings() { Async = true });

            while (!reader.EOF)
            {
                await reader.ReadAsync();

                if (reader.NodeType == XmlNodeType.Element
                    && reader.Name.Equals("ProjectGuid", StringComparison.OrdinalIgnoreCase))
                {
                    return await reader.ReadInnerXmlAsync();
                }
                // End of PropertyGroup element found, quit early.
                else if (reader.NodeType == XmlNodeType.EndElement
                    && reader.Name.Equals("PropertyGroup", StringComparison.OrdinalIgnoreCase))
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }
    }
}
