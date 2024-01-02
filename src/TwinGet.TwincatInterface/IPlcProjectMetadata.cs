// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface;

public interface IPlcProjectMetadata
{
    /// <summary>
    /// PLC project GUID.
    /// </summary>
    public string ProjectGuid { get; }

    /// <summary>
    /// PLC project name. Note that the PLC project <see cref="Name"/> can be different than the PLC project <see cref="Title"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// PLC project company. See <see href="https://infosys.beckhoff.com/english.php?content=../content/1033/tc3_plc_intro/3260045067.html">PLC project properties</see>.
    /// </summary>
    public string? Company { get; }

    /// <summary>
    /// PLC project title. See <see href="https://infosys.beckhoff.com/english.php?content=../content/1033/tc3_plc_intro/3260045067.html">PLC project properties</see>.
    /// </summary>
    public string? Title { get; }

    /// <summary>
    /// PLC project version. See <see href="https://infosys.beckhoff.com/english.php?content=../content/1033/tc3_plc_intro/3260045067.html">PLC project properties</see>.
    /// </summary>
    public string? ProjectVersion { get; }

    /// <summary>
    /// The absolute path to the corresponding <c>.plcproj</c>.
    /// </summary>
    public string AbsolutePath { get; }
}
