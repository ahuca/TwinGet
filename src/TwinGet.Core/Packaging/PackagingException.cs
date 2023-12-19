// This file is licensed to you under MIT license.

using FluentValidation.Results;

namespace TwinGet.Core.Packaging
{
    public class PackagingException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public PackagingException() => Errors = new Dictionary<string, string[]>();

        public PackagingException(string messsage) : base(messsage) => Errors = new Dictionary<string, string[]>();

        public PackagingException(IEnumerable<ValidationFailure> failures) : base($"{failures.Count()} packaging command validation failures have occured.")
        {
            Errors = failures
                .GroupBy(f => f.PropertyName, f => f.ErrorMessage)
                .ToDictionary(propertyName => propertyName.Key, errorMessages => errorMessages.ToArray());
        }
    }
}
