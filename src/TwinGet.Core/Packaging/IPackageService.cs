// This file is licensed to you under MIT license.

using TwinGet.Core.Commands;

namespace TwinGet.Core.Packaging
{
    public interface IPackageService
    {
        public Task<bool> PackAsync(IPackCommand packCommand);
        public bool Pack(IPackCommand packCommand);
    }
}
