// This file is licensed to you under MIT license.

namespace TwinGet.AutomationInterface.Test
{
    public class AutomationInterfaceConstantsTests
    {
        [Fact]
        public void SupportedProgIds_ShouldNotBeEmpty()
        {
            AutomationInterfaceConstants.SupportedProgIds.Should().NotBeEmpty();
        }

        [Fact]
        public void TwincatXaeDownloadUrl_ShouldNotBeNullOrEmpty()
        {
            AutomationInterfaceConstants.TwincatXaeDownloadUrl.Should().NotBeNullOrEmpty();
        }
    }
}
