// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface.Test
{
    public class TwincatConstantsTests
    {
        [Fact]
        public void SupportedProgIds_ShouldNotBeEmpty()
        {
            TwincatConstants.SupportedProgIds.Should().NotBeEmpty();
        }

        [Fact]
        public void TwincatXaeDownloadUrl_ShouldNotBeNullOrEmpty()
        {
            TwincatConstants.TwincatXaeDownloadUrl.Should().NotBeNullOrEmpty();
        }
    }
}
