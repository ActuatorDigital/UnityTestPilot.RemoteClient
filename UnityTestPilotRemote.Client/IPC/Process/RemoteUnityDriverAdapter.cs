using AIR.UnityTestPilot.Agents;
using AIR.UnityTestPilot.Interactions;
using AIR.UnityTestPilot.Queries;

namespace AIR.UnityTestPilotRemote.Client
{
    public class RemoteUnityDriverAdapter : IUnityDriverAgent
    {
        private readonly UnityDriverHostProcess _hostProcess;
        public RemoteUnityDriverAdapter(UnityDriverHostProcess hostProcess)
            => _hostProcess = hostProcess;
        public UiElement[] Query(ElementQuery query) {
            if(query is IProxyQuery proxy)
                proxy.ProxyVia(_hostProcess);
            return query.Search();
        }

        public void Shutdown() => _hostProcess.Shutdown(false);
        public void SetTimeScale(float timeScale) => _hostProcess.SetTimeScale(timeScale);
    }
}