using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace UnityTestPilotRemote {
    public class RemoteAgentProcess : IDisposable {
        
        Task _keepAlive;

        private const float REST_SECONDS = .5f;
        private readonly ProcessStartInfo _processStart;
        private Process _agentProcess;
        private CancellationToken _token;

        public bool AgentIsRunning { get; private set; }
        
        public RemoteAgentProcess(FileInfo executableFile) {

            if(!executableFile.Exists)
                throw new FileNotFoundException(executableFile.FullName + " missing.");

            if (!executableFile.IsExecutable()) 
                throw new FormatException("Provided file was not executable.");

            _processStart = new ProcessStartInfo {
                FileName = executableFile.FullName,
                WorkingDirectory = executableFile.Directory.FullName,
                Arguments = "-testAgent",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
        }

        public async Task StartAgentProcess(
            CancellationToken token = default
        ) {
            _agentProcess = Process.Start(_processStart);

            await Task.Delay(TimeSpan.FromSeconds(5));
            AgentIsRunning = true;

            _token = token;
            _keepAlive = KeepAlive();

        }

        public async Task WaitForExit() {
            await _keepAlive;
        }

        private async Task KeepAlive() {

            while (!_token.IsCancellationRequested)
                await Task.Delay(TimeSpan.FromSeconds(REST_SECONDS));
            
            _agentProcess.CloseMainWindow();
            _agentProcess.WaitForExit();
            _agentProcess.Dispose();

            AgentIsRunning = false;
        }

        public void Dispose() {
            if (AgentIsRunning) {
                _agentProcess.CloseMainWindow();
                _agentProcess.WaitForExit();
                _agentProcess.Dispose();
            }
        }

        ~RemoteAgentProcess() {
            Dispose();
        }

    }
    
    static class FileInfoExtensions {
        public static bool IsExecutable(this FileInfo file) {
            
            // TODO: Check headers on windows, and chmod on other platforms.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                return file.Extension == ".exe";
            } 
            
            throw new PlatformNotSupportedException(
                "UnityTestPilot Not checking for executables on your current platform.");

        }
    }
}