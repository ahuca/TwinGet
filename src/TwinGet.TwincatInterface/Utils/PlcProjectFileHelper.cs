// This file is licensed to you under MIT license.

using System.Xml;
using TwinGet.TwincatInterface.Dto;
using TwinGet.TwincatInterface.Exceptions;
using TwinGet.Utils.Xml;

namespace TwinGet.TwincatInterface.Utils;

public class PlcProjectFileHelper
{
    private readonly string _filePath;
    private readonly Lazy<PlcProjectData> _projectData;
    private Lazy<TcSmProjectData> _parentProjectData;

    public TcSmProjectData ParentProject
    {
        get => _parentProjectData.Value;
    }

    public PlcProjectFileHelper(string filePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("PLC project file not found.", filePath);
        }

        _filePath = Path.GetFullPath(filePath);
        _projectData = new Lazy<PlcProjectData>(
            () =>
                XmlSerializer.TryDeserializeXmlFile<PlcProjectData>(_filePath)
                ?? throw new InvalidProjectFileFormat(
                    ErrorStrings.InvalidProjectFileFormat,
                    filePath
                )
        );
    }

    public async Task<string> GetProjectGuidAsync()
    {
        using var reader = XmlReader.Create(_filePath, new XmlReaderSettings() { Async = true });

        while (!reader.EOF)
        {
            await reader.ReadAsync();

            if (
                reader.NodeType == XmlNodeType.Element
                && reader.Name.Equals("ProjectGuid", StringComparison.OrdinalIgnoreCase)
            )
            {
                return await reader.ReadInnerXmlAsync();
            }
            // End of PropertyGroup element found, quit early.
            else if (
                reader.NodeType == XmlNodeType.EndElement
                && reader.Name.Equals("PropertyGroup", StringComparison.OrdinalIgnoreCase)
            )
            {
                return string.Empty;
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// Try to find the TwinCAT project that the given PLC project belongs to.
    /// </summary>
    /// <param name="upwardDepth">The upward depth of parents to search.</param>
    /// <returns>The absolute path to the TwinCAT project file if successfully found, otherwise <see cref="string.Empty"/>.</returns>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<string> GetParentTwincatProjectAsync(int upwardDepth = 5)
    {
        ArgumentException.ThrowIfNullOrEmpty(_filePath, nameof(_filePath));
        if (!TwincatUtils.IsPlcProjectFileExtension(_filePath))
        {
            return string.Empty;
        }

        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException(
                $"Provided plc project file path does not exists.",
                _filePath
            );
        }

        /// We recursively look up TwinCAT project files in parent folders with a depth of <param name="upwardDepth"/param>.
        string parent = _filePath;
        for (int i = 0; i < upwardDepth - 1; i++)
        {
            parent = Directory.GetParent(parent)?.FullName ?? string.Empty;

            if (string.IsNullOrEmpty(parent))
            {
                // If we can't even get the first parent, we return.
                return string.Empty;
            }

            var candidates = Directory
                .EnumerateFiles(
                    parent,
                    $"*{TwincatConstants.TwincatProjectWildcardExtension}",
                    SearchOption.TopDirectoryOnly
                )
                .Where(TwincatUtils.IsTwincatProjectFileExtension);

            foreach (string? candidate in candidates)
            {
                var tcSmFileHelper = new TcSmProjectFileHelper(candidate);

                if (await tcSmFileHelper.HasPlcProjectAsync(_filePath))
                {
                    _parentProjectData = new(
                        () =>
                            XmlSerializer.TryDeserializeXmlFile<TcSmProjectData>(candidate)
                            ?? throw new InvalidProjectFileFormat(
                                ErrorStrings.InvalidProjectFileFormat,
                                candidate
                            )
                    );
                    return candidate;
                }
            }
        }

        return string.Empty;
    }
}
