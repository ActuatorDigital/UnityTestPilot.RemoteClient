using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Threading;
using AIR.UnityTestPilotRemote.Client;

namespace AIR.UnityTestPilotRemote.Tests
{
    [TestClass]
    public class RemoteAgentProcessTests
    {
        const int EXE_START_TIME = 10;
        const string AGENT_EXE_NAME = "./Agent/RemoteHost.exe";

        [TestMethod]
        public void Constructor_MissingFile_ThrowsFileNotFoundException()
        {
            // Arrange
            var missingFile = new FileInfo("./FileThatDoesntExist");

            // Act
            var act = new Action(() => _ = new RemoteAgentProcess(missingFile));

            // Assert
            Assert.ThrowsException<FileNotFoundException>(act);
        }

        [TestMethod]
        public void Constructor_WithExecutableFile_ThrowsWhenNotExecutable()
        {
            // Arrange
            const string FILE_NAME = "./File.bad";
            File.Create(FILE_NAME).Dispose();
            var wrongExtension = new FileInfo(FILE_NAME);

            // Act
            var act = new Action(() => _ = new RemoteAgentProcess(wrongExtension));

            // Assert
            Assert.ThrowsException<FormatException>(act);
            File.Delete(FILE_NAME);
        }

        [TestMethod]
        public void Constructor_WithFileNotExecutable_ThrowsNoExceptions()
        {
            // Arrange
            string NAMED_EXE_FILE = "./Executable.exe";
            File.Create(NAMED_EXE_FILE).Dispose();
            var correctExtension = new FileInfo(NAMED_EXE_FILE);
            Exception exception = null;

            // Act
            try {
                var _ = new RemoteAgentProcess(correctExtension);
            }
            catch (Exception ex) {
                exception = ex;
            }

            // Assert
            Assert.IsNull(exception);
            File.Delete(NAMED_EXE_FILE);
        }

        [TestMethod]
        public async Task StartAgent_WithExecutableFile_ThrowsNoException()
        {
            // Arrange
            Exception exception = null;
            var cancel = new CancellationTokenSource();
            cancel.CancelAfter(TimeSpan.FromSeconds(EXE_START_TIME));

            try {
                // Act
                using var remoteProcess = new RemoteAgentProcess(new FileInfo(AGENT_EXE_NAME));
                await remoteProcess.StartAgentProcess(cancel.Token);
                await remoteProcess.WaitForExit();
            }
            catch (Exception ex) {
                exception = ex;
            }

            // Assert
            Assert.IsNull(exception);
            cancel.Dispose();
        }

        [TestMethod]
        public async Task CancelToken_ForRunningAgent_ClosesProcess()
        {
            // Arrange
            var cancel = new CancellationTokenSource();
            cancel.CancelAfter(TimeSpan.FromSeconds(8));

            // Act
            using var remoteProcess = new RemoteAgentProcess(new FileInfo(AGENT_EXE_NAME));
            await remoteProcess.StartAgentProcess(cancel.Token);
            await remoteProcess.WaitForExit();

            // Assert
            Assert.IsFalse(remoteProcess.AgentIsRunning);
        }

        [TestMethod]
        public async Task StartAgent_WithExecutableFile_StartsProcess()
        {
            // Arrange
            var exeFile = new FileInfo(AGENT_EXE_NAME);
            var cancel = new CancellationTokenSource();

            // Act
            using (var remoteProcess = new RemoteAgentProcess(exeFile)) {
                cancel.CancelAfter(TimeSpan.FromSeconds(6));
                await remoteProcess.StartAgentProcess(cancel.Token);

                // Assert
                Assert.IsTrue(remoteProcess.AgentIsRunning);
                await remoteProcess.WaitForExit();
            }
            
        }
    }
}