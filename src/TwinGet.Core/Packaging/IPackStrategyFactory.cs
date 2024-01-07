// This file is licensed to you under MIT license.

using TwinGet.Core.Commands;

namespace TwinGet.Core.Packaging;

public interface IPackStrategyFactory
{
    public IPackStrategy CreateStrategy(string filePath);
}
