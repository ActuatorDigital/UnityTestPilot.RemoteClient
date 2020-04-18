using System;
using System.Threading.Tasks;
using AIR.UnityTestPilot.Agents;
using AIR.UnityTestPilot.Drivers;

namespace AIR.UnityTestPilotRemote.Client
{
    public class UnityDriverRemote : UnityDriver, IAsyncDisposable
    {
        private UnityDriverHostProcess _hostProcess;
        
        private UnityDriverRemote(IUnityDriverAgent agent) 
            : base(agent) { }
        
        public static async Task<UnityDriverRemote> Attach(string pathToAgentExe = "")
        {
            var remoteProcess = await UnityDriverHostProcess.Attach(pathToAgentExe);
            var agentAdapter = new RemoteUnityDriverAdapter(remoteProcess);
            var remoteDriver = new UnityDriverRemote(agentAdapter);
            remoteDriver._hostProcess = remoteProcess;
            return remoteDriver;
        }

        public ValueTask DisposeAsync() => _hostProcess.DisposeAsync();
    }
}