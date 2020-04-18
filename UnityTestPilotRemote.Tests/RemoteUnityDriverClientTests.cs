using System;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using AIR.UnityTestPilotRemote.Client;

namespace AIR.UnityTestPilotRemote.Tests
{
    [TestClass]
    public class RemoteUnityDriverClientTests {
        
        [TestMethod]
        public async Task Connect_NoReceivingAgent_FailsToConnect() {
            
            // Arrange
            var timeout = TimeSpan.FromSeconds(.5f);
            var test = new RemoteUnityDriverClient(timeout);

            // Act
            var connected = await test.Connect();

            // Assert
            Assert.IsFalse(
                connected,
                "Expected connection failure, but succeeded." );
        }

        [TestMethod]
        public async Task Connect_NoReceivingAgent_TimesOut() {
            // Arrange
            var timeout = TimeSpan.FromSeconds(.5f);
            var test = new RemoteUnityDriverClient(timeout);
            var testStartTime = DateTime.Now;

            // Act
            await test.Connect();

            // Assert
            Assert.IsTrue(
                DateTime.Now > testStartTime + timeout, 
                "Connected, or timed out before specified time" ); 
        }

        [TestMethod]
        public async Task Connect_RemoteAgentRunning_ConnectsToAgent() {
            // Arrange
            const string AGENT_EXE_NAME = "./Agent/RemoteHost.exe";
            const int CONNECT_TIMEOUT_SECONDS = 20;
            var agentExecFile = new FileInfo(AGENT_EXE_NAME);

            var cancel = new CancellationTokenSource();
            using var remoteProcess = new RemoteAgentProcess(agentExecFile);
            await remoteProcess.StartAgentProcess(cancel.Token);

            // Act
            var connectTimeout = TimeSpan.FromSeconds(CONNECT_TIMEOUT_SECONDS);
            var client = new RemoteUnityDriverClient(connectTimeout);
            var connected = await client.Connect();

            // Assert
            cancel.Cancel();
            await remoteProcess.WaitForExit();

            Assert.IsTrue(connected, "Failed to connect.");
        }

        [TestMethod]
        public async Task Connect_TwiceConsecutively_BothConnect() {
            await Connect_RemoteAgentRunning_ConnectsToAgent();
            await Connect_RemoteAgentRunning_ConnectsToAgent();
        }

    }
}