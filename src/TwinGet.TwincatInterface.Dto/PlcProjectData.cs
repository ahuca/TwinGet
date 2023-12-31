// This file is licensed to you under MIT license.

using System.Xml;
using System.Xml.Serialization;

namespace TwinGet.TwincatInterface.Dto;

[XmlRoot("Project", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
public class PlcProjectData : ITwincatProjectData
{
    [XmlAttribute("DefaultTargets")]
    public string DefaultTargets { get; set; }

    [XmlElement("PropertyGroup")]
    public PropertyGroup PropertyGroup { get; set; }
}

public class PropertyGroup
{
    [XmlElement("FileVersion")]
    public string FileVersion { get; set; }

    [XmlElement("SchemaVersion")]
    public string SchemaVersion { get; set; }

    [XmlElement("ProjectGuid")]
    public string ProjectGuid { get; set; }

    [XmlElement("SubObjectsSortedByName")]
    public string SubObjectsSortedByName { get; set; }

    [XmlElement("DownloadApplicationInfo")]
    public string DownloadApplicationInfo { get; set; }

    [XmlElement("WriteProductVersion")]
    public string WriteProductVersion { get; set; }

    [XmlElement("GenerateTpy")]
    public string GenerateTpy { get; set; }

    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("ProgramVersion")]
    public string ProgramVersion { get; set; }

    [XmlElement("Application")]
    public string Application { get; set; }

    [XmlElement("TypeSystem")]
    public string TypeSystem { get; set; }

    [XmlElement("Implicit_Task_Info")]
    public string Implicit_Task_Info { get; set; }

    [XmlElement("Implicit_KindOfTask")]
    public string Implicit_KindOfTask { get; set; }

    [XmlElement("Implicit_Jitter_Distribution")]
    public string Implicit_Jitter_Distribution { get; set; }

    [XmlElement("LibraryReferences")]
    public string LibraryReferences { get; set; }

    [XmlElement("Released")]
    public string? Released { get; set; }

    [XmlElement("Title")]
    public string? Title { get; set; }

    [XmlElement("Company")]
    public string? Company { get; set; }

    [XmlElement("ProjectVersion")]
    public string? ProjectVersion { get; set; }

    [XmlElement("LibraryCategories")]
    public LibraryCategories? LibraryCategories { get; set; }

    [XmlElement("SelectedLibraryCategories")]
    public SelectedLibraryCategories? SelectedLibraryCategories { get; set; }

    [XmlElement("Author")]
    public string? Author { get; set; }

    [XmlElement("Description")]
    public string? Description { get; set; }
}

public class LibraryCategories
{
    [XmlElement("LibraryCategory")]
    public LibraryCategory LibraryCategory { get; set; }
}

public class LibraryCategory
{
    [XmlElement("Id")]
    public string Id { get; set; }

    [XmlElement("Version")]
    public string Version { get; set; }

    [XmlElement("DefaultName")]
    public string DefaultName { get; set; }
}

public class SelectedLibraryCategories
{
    [XmlElement("Id")]
    public string Id { get; set; }
}

public class TypeList
{
    [XmlElement("Type")]
    public List<string> Types { get; set; }
}
