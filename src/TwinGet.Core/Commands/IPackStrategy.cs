﻿// This file is licensed to you under MIT license.

namespace TwinGet.Core.Commands;

public interface IPackStrategy
{
    /// <summary>
    /// Pack a TwinGet package given a <see cref="IPackCommand"/>.
    /// </summary>
    /// <param name="packCommand">The <see cref="IPackCommand"/> to handle.</param>
    /// <returns>True if pack successfully, otherwise false.</returns>
    public Task<bool> PackAsync(IPackCommand packCommand);

    /// <summary>
    /// Pack a TwinGet package given a <see cref="IPackCommand"/>.
    /// </summary>
    /// <param name="packCommand">The <see cref="IPackCommand"/> to handle.</param>
    /// <returns>True if pack successfully, otherwise false.</returns>
    public bool Pack(IPackCommand packCommand);
}
