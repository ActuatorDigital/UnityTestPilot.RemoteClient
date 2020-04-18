using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using AIR.UnityTestPilotRemote.Client;

namespace AIR.UnityTestPilotRemote.Tests
{
    [TestClass]
    public class UnityDriverHostProcessTests
    {

        [TestMethod]
        public async Task Build_WithInstalledExecutable_ThrowsNoExceptions() {
            // Arrange
            try {
                // Act
                await using (await UnityDriverHostProcess.Attach("./Agent/RemoteHost.exe")) { }
            } catch (Exception ex) {
                // Assert
                Assert.Fail("Expected no exception, but caught: " + ex.Message );  
            }

        }

    }
}
