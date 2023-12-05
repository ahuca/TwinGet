// This file is licensed to you under MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
