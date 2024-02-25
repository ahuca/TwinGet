// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface.Exceptions;

public static class ExceptionStrings
{
    public const string ProjectFilesHelpLink =
        "https://infosys.beckhoff.com/english.php?content=../content/1033/tc3_sourcecontrol/18014398915785483.html&id=";
    public const string InvalidPlcProjectExtension =
        $"The file '{{0}}' does not have a valid PLC project extension '{TwincatConstants.PlcProjectExtension}'.";
    public const string InvalidTwincatProjectExtension =
        $"The file '{{0}}' does not have a valid TwinCAT project extension.";
    public const string TreeItemIsNotAPlcProject = "The provided tree item '{0}' is not a PLC project.";
    public const string NotATwincatProject = "The provided project '{0}' is not a TwinCAT project.";
}
