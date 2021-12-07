using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System;
using AIR.UnityTestPilotRemote.Common;

namespace AIR.UnityTestPilotRemote.Client
{
    public class UnityDriverHostProcess : IRemoteUnityDriver, IAsyncDisposable
    {
        private Task _agentProcess;
        private readonly CancellationTokenSource _cancel;
        private readonly IRemoteUnityDriver _driver;

        private UnityDriverHostProcess(
            CancellationTokenSource cancel,
            IRemoteUnityDriver driver,
            Task agentProcess
        ) {
            _agentProcess = agentProcess;
            _cancel = cancel;
            _driver = driver;
        }

        public static async Task<UnityDriverHostProcess> Attach(string pathToAgent, string[] args = default)
        {
            if (string.IsNullOrEmpty(pathToAgent)) {
                var cancel = new CancellationTokenSource();
                var client = new RemoteUnityDriverClient();
                bool connected = await client.Connect();
            
                if (connected) {
                    var driver = client.Bind();
                    var agentProcess = Task.Run(() => {
                        while (!cancel.IsCancellationRequested) 
                            Task.Delay(100, cancel.Token);
                    });
                    return new UnityDriverHostProcess(cancel, driver, agentProcess);
                }
            
                throw new IOException("Unable to connect to remote UI Test agent.");
            } else {
                var agentExeFile = new FileInfo(pathToAgent);
                var agent = new RemoteAgentProcess(agentExeFile, args);
                var cancel = new CancellationTokenSource();
                await agent.StartAgentProcess(cancel.Token);

                var client = new RemoteUnityDriverClient();
                bool connected = await client.Connect();

                if (connected) {
                    var driver = client.Bind();
                    var agentProcess = agent.WaitForExit();
                    return new UnityDriverHostProcess(cancel, driver, agentProcess);
                }

                cancel.Cancel();
                await agent.WaitForExit();
                throw new IOException("Unable to connect to remote UI Test agent.");   
            }
        }

        public async ValueTask DisposeAsync() { 
            _cancel.Cancel();
            await _agentProcess;
            _cancel.Dispose();
        }

        public void SetTimeScale(float timeScale) => _driver.SetTimeScale(timeScale);

        public void LeftClick(RemoteUiElement element) => _driver.LeftClick(element);

        public void LeftClickDown(RemoteUiElement element) => _driver.LeftClickDown(element);

        public void LeftClickUp(RemoteUiElement element) => _driver.LeftClickUp(element);

        public void RightClick(RemoteUiElement element) => _driver.RightClick(element);

        public void RightClickDown(RemoteUiElement element) => _driver.RightClickDown(element);

        public void RightClickUp(RemoteUiElement element) => _driver.RightClickUp(element);

        public void Shutdown(bool immediate) => _driver.Shutdown(immediate);
        
        public Task<RemoteUiElement> Query(RemoteElementQuery query) => _driver.Query(query);
        
    }
}
