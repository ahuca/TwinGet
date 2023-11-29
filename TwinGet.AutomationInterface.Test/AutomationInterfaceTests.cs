namespace TwinGet.AutomationInterface.Test
{
    public class AutomationInterfaceTests
    {
        [Fact]
        public void ProgIdShouldBeValid()
        {
            var ai = new AutomationInterface();
            AutomationInterfaceConstants.SupportedProgIds.Should().Contain(ai.ProgId);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}