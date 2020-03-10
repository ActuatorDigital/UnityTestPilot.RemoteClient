using System;
using AIR.UnityTestPilot.Remote;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace AIR.UnityTestPilotRemote.Tests
{
    [TestClass]
    public class UiTestAgentTests
    {

        [TestMethod]
        public async Task Build_WithInstalledExecutable_ThrowsNoExceptions() {
            // Arrange
            try {
                // Act
                await using (await UiTestAgent.Build()) { }
            } catch (Exception ex) {
                // Assert
                Assert.Fail("Expected no exception, but caught: " + ex.Message );  
            }

        }

    }
}
