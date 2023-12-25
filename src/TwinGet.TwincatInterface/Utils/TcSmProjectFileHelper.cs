// This file is licensed to you under MIT license.

using System.Xml;

namespace TwinGet.TwincatInterface.Utils
{
    public class TcSmProjectFileHelper
    {
        private readonly string _filePath;

        public TcSmProjectFileHelper(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("TwinCAT project file not found.", filePath);
            }

            _filePath = Path.GetFullPath(filePath);
        }

        public async Task<bool> HasPlcProject(string GUID)
        {
            ArgumentException.ThrowIfNullOrEmpty(GUID, nameof(GUID));

            using XmlReader reader = XmlReader.Create(_filePath, new XmlReaderSettings() { Async = true });

            while (!reader.EOF)
            {
                await reader.ReadAsync();

                // Parse until Project element
                if (reader.NodeType == XmlNodeType.Element
                    && reader.Name.Equals("Project", StringComparison.OrdinalIgnoreCase))
                {
                    if (!reader.HasAttributes)
                    {
                        continue;
                    }

                    var candidateGuid = reader.GetAttribute("GUID");
                    if (GUID.Equals(candidateGuid, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                // End of PLC element found, quit early.
                else if (reader.NodeType == XmlNodeType.EndElement
                    && reader.Name.Equals("Plc", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return false;
        }
    }
}
