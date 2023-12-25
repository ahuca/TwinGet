// This file is licensed to you under MIT license.

using TwinGet.Core.Commands;

namespace TwinGet.Core.Test.Commands
{
    public class PackCommandValidatorTests
    {
        private readonly PackCommandValidator _sut = new();

        [Fact]
        public async Task ShouldHaveError_WhenPathIsNullAsync()
        {
            var packCommand = new PackCommand();
            var result = await _sut.TestValidateAsync(packCommand);

            result.ShouldHaveValidationErrorFor(p => p.Path).Only();
        }

        [Fact]
        public async Task ShouldHaveError_WhenPathIsNotValidAsync()
        {
            var packCommand = new PackCommand();
            var result = await _sut.TestValidateAsync(packCommand);

            result.ShouldHaveValidationErrorFor(p => p.Path).Only();
        }

        [Fact]
        public async Task ShouldHaveError_WhenPlcProjectDoesNotBelongToSolution()
        {

        }
    }
}
