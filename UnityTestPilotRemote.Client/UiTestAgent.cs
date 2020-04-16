using AIR.UnityTestPilot.Remote;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System;

namespace AIR.UnityTestPilotRemote.Client
{
    public class UiTestAgent : IRemoteUnityDriver, IAsyncDisposable
    {

        Task _agentProcess;
        CancellationTokenSource _cancel;
        IRemoteUnityDriver _driver;

        private UiTestAgent(
            CancellationTokenSource cancel,
            IRemoteUnityDriver driver,
            Task agentProcess
        ) {
            _agentProcess = agentProcess;
            _cancel = cancel;
            _driver = driver;
        }

        public static async Task<UiTestAgent> Build(string pathToAgent)
        {
            var agentExeFile = new FileInfo(pathToAgent);
            var agent = new RemoteAgentProcess(agentExeFile);
            var cancel = new CancellationTokenSource();
            await agent.StartAgentProcess(cancel.Token);

            var client = new RemoteUnityDriverClient();
            bool connected = await client.Connect();

            if (connected) {
                var driver = client.Bind();
                var agentProcess = agent.WaitForExit();
                return new UiTestAgent(cancel, driver, agentProcess);
            } else {
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

        public void Shutdown(bool immediate) => _driver.Shutdown(immediate);
        
        public Task<RemoteUiElement> Query(RemoteElementQuery query) => _driver.Query(query);

    }
}
