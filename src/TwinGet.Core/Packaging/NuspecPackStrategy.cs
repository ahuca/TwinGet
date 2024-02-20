// This file is licensed to you under MIT license.

using FluentValidation;
using TwinGet.Core.Commands;

namespace TwinGet.Core.Packaging;

public class NuspecPackStrategy : IPackStrategy
{
    public NuspecPackStrategy()
    {
        throw new NotImplementedException();
    }

    public IPackStrategy DoCustomValidation(IValidationContext validationContext) =>
        throw new NotImplementedException();

    public bool Pack(IPackCommand packCommand) => throw new NotImplementedException();

    public Task<bool> PackAsync(IPackCommand packCommand) => throw new NotImplementedException();
}
