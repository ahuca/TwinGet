// This file is licensed to you under MIT license.

using FluentValidation.Results;

namespace TwinGet.Core.Packaging
{
    public class PackagingException : Exception, ILogMessageException
    {
        public IDictionary<string, string[]> Errors { get; }

        public PackagingException() => Errors = new Dictionary<string, string[]>();

        public PackagingException(string messsage) : base(messsage) => Errors = new Dictionary<string, string[]>();

        public PackagingException(IEnumerable<ValidationFailure> failures) : base($"Packaging command validation failed.")
        {
            Errors = failures
                .GroupBy(f => f.PropertyName, f => f.ErrorMessage)
                .ToDictionary(propertyName => propertyName.Key, errorMessages => errorMessages.ToArray());
        }

        public string AsLogMessage()
        {
            string message = Message + Environment.NewLine;

            IEnumerable<string> errors = Errors.Select(x => $"{x.Key}:{Environment.NewLine}\t{string.Join(Environment.NewLine + "\t", x.Value)}");

            return message + string.Join(Environment.NewLine, errors);
        }
    }
}
