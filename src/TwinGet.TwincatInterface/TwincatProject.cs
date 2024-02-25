// This file is licensed to you under MIT license.

using System.Drawing;
using EnvDTE;
using TwinGet.TwincatInterface.Dto;
using TwinGet.TwincatInterface.Exceptions;
using TwinGet.TwincatInterface.Utils;
using ITcSysManagerAlias = TCatSysManagerLib.ITcSysManager15;

namespace TwinGet.TwincatInterface;

public class TwincatProject : ITwincatProject
{
    private readonly EnvDTE.Project _project;
    private readonly ITcSysManagerAlias _systemManager;
    private readonly List<PlcProject> _plcProjects;
    public IReadOnlyList<IPlcProject> PlcProjects
    {
        get => _plcProjects;
    }
    public string AbsolutePath
    {
        get => FullName;
    }

    public TwincatProject(EnvDTE.Project project)
    {
        if (!project.IsTwincatProject())
        {
            throw new TwincatInterfaceException(string.Format(ExceptionStrings.NotATwincatProject, project.Name));
        }

        _project = project;
        _systemManager = (ITcSysManagerAlias)project.Object;

        if (_systemManager is null)
        {
            throw new TwincatInterfaceException(
                string.Format(ExceptionStrings.CouldNotGetSystemManager, _project.Name)
            );
        }

        _plcProjects = new(TryGetPlcProjects(_systemManager, _project.FullName));
    }

    private static List<PlcProject> TryGetPlcProjects(
        ITcSysManagerAlias systemManager,
        string filePath
    )
    {
        List<PlcProject> plcProjects = [];

        ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

        TcSmProjectData? tcSmProject =
            TwincatUtils.DeserializeXmlFileToProjectData<TcSmProjectData>(filePath);

        string? rootDir = Path.GetDirectoryName(filePath);

        if (tcSmProject.Project.Plc?.Projects is not null)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            plcProjects.AddRange(
                from ProjectElement project in tcSmProject.Project.Plc.Projects
                let plcProject = new PlcProject(
                    systemManager.LookUpPlcProject(project.Name),
                    Path.Join(rootDir, project.PrjFilePath)
                )
                where plcProject is not null
                select plcProject
            );
#pragma warning restore CS8604 // Possible null reference argument.
        }

        return plcProjects;
    }

    public void SaveAs(string NewFileName) => _project.SaveAs(NewFileName);

    public void Save(string FileName = "") => _project.Save(FileName);

    public void Delete() => _project.Delete();

    public string Name
    {
        get => _project.Name;
        set => _project.Name = value;
    }

    public string FileName => _project.FileName;

    public bool IsDirty
    {
        get => _project.IsDirty;
        set => _project.IsDirty = value;
    }

    public Projects Collection => _project.Collection;

    public DTE DTE => _project.DTE;

    public string Kind => _project.Kind;

    public ProjectItems ProjectItems => _project.ProjectItems;

    public Properties Properties => _project.Properties;

    public string UniqueName => _project.UniqueName;

    public object Object => _project.Object;

    public object get_Extender(string ExtenderName) => _project.Extender[ExtenderName];

    public object ExtenderNames => _project.ExtenderNames;

    public string ExtenderCATID => _project.ExtenderCATID;

    public string FullName => _project.FullName;

    public bool Saved
    {
        get => _project.Saved;
        set => _project.Saved = value;
    }

    public ConfigurationManager ConfigurationManager => _project.ConfigurationManager;

    public Globals Globals => _project.Globals;

    public ProjectItem ParentProjectItem => _project.ParentProjectItem;

    public CodeModel CodeModel => _project.CodeModel;
}
