// This file is licensed to you under MIT license.

using System.Xml;
using System.Xml.Serialization;

namespace TwinGet.TwincatInterface.Dto;

[XmlRoot("TcSmProject")]
public class TcSmProjectData : ITwincatProjectData
{
    [XmlAttribute("TcSmVersion")]
    public string TcSmVersion { get; set; }

    [XmlAttribute("TcVersion")]
    public string TcVersion { get; set; }

    [XmlElement("Project")]
    public Project Project { get; set; }
}

public class Project
{
    [XmlAttribute("ProjectGUID")]
    public string ProjectGUID { get; set; }

    [XmlAttribute("ShowHideConfigurations")]
    public string ShowHideConfigurations { get; set; }

    [XmlElement("System")]
    public SystemElement System { get; set; }

    [XmlElement("Plc")]
    public Plc? Plc { get; set; }
}

public class SystemElement
{
    [XmlElement("Tasks")]
    public Tasks Tasks { get; set; }
}

public class Tasks
{
    [XmlElement("Task")]
    public Task Task { get; set; }
}

public class Task
{
    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlAttribute("Priority")]
    public int Priority { get; set; }

    [XmlAttribute("CycleTime")]
    public int CycleTime { get; set; }

    [XmlAttribute("AmsPort")]
    public int AmsPort { get; set; }

    [XmlAttribute("AdtTasks")]
    public bool AdtTasks { get; set; }

    [XmlElement("Name")]
    public string Name { get; set; }
}

public class Plc
{
    [XmlElement("Project")]
    public List<ProjectElement> Projects { get; set; }
}

public class ProjectElement
{
    [XmlAttribute("GUID")]
    public string GUID { get; set; }

    [XmlAttribute("Name")]
    public string Name { get; set; }

    [XmlAttribute("PrjFilePath")]
    public string PrjFilePath { get; set; }

    [XmlAttribute("TmcFilePath")]
    public string TmcFilePath { get; set; }

    [XmlAttribute("ReloadTmc")]
    public bool ReloadTmc { get; set; }

    [XmlAttribute("AmsPort")]
    public int AmsPort { get; set; }

    [XmlAttribute("FileArchiveSettings")]
    public string FileArchiveSettings { get; set; }

    [XmlAttribute("SymbolicMapping")]
    public bool SymbolicMapping { get; set; }

    [XmlElement("Instance")]
    public Instance Instance { get; set; }
}

public class Instance
{
    [XmlAttribute("Id")]
    public string Id { get; set; }

    [XmlAttribute("TcSmClass")]
    public string TcSmClass { get; set; }

    [XmlAttribute("KeepUnrestoredLinks")]
    public int KeepUnrestoredLinks { get; set; }

    [XmlAttribute("TmcPath")]
    public string TmcPath { get; set; }

    [XmlAttribute("TmcHash")]
    public string TmcHash { get; set; }

    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("CLSID")]
    public string CLSID { get; set; }

    [XmlElement("Contexts")]
    public Contexts Contexts { get; set; }

    [XmlElement("TaskPouOids")]
    public TaskPouOids TaskPouOids { get; set; }
}

public class Contexts
{
    [XmlElement("Context")]
    public Context Context { get; set; }
}

public class Context
{
    [XmlElement("Id")]
    public int Id { get; set; }

    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("ManualConfig")]
    public ManualConfig ManualConfig { get; set; }

    [XmlElement("Priority")]
    public int Priority { get; set; }

    [XmlElement("CycleTime")]
    public int CycleTime { get; set; }
}

public class ManualConfig
{
    [XmlElement("OTCID")]
    public string OTCID { get; set; }
}

public class TaskPouOids
{
    [XmlElement("TaskPouOid")]
    public TaskPouOid TaskPouOid { get; set; }
}

public class TaskPouOid
{
    [XmlAttribute("Prio")]
    public int Prio { get; set; }

    [XmlAttribute("OTCID")]
    public string OTCID { get; set; }
}
