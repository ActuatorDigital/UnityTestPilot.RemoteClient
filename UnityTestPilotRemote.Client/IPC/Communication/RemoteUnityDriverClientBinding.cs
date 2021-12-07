using TachyonClientRPC;
using System.Threading.Tasks;
using System;
using AIR.UnityTestPilotRemote.Common;

namespace GeneratedBindings
{
    public class RemoteUnityDriverClientBinding : IRemoteUnityDriver
    {
        ClientRpc _client;
        public void Bind(ClientRpc client) 
            => _client = client;

        public Task<RemoteUiElement> Query(RemoteElementQuery queryArg) 
            => _client.AskTask<RemoteUiElement>("Query", queryArg);

        public void Shutdown(Boolean immediateArg) 
            => _client.Send("Shutdown", immediateArg);

        public void SetTimeScale(Single timeScaleArg) 
            => _client.Send("SetTimeScale", timeScaleArg);

        public void LeftClick(RemoteUiElement elementArg) 
            => _client.Send("LeftClick", elementArg);
        
        public void LeftClickDown(RemoteUiElement elementArg) 
            => _client.Send("LeftClickDown", elementArg);
        
        public void LeftClickUp(RemoteUiElement elementArg)
            => _client.Send("LeftClickUp", elementArg);

        public void RightClick(RemoteUiElement elementArg)
            => _client.Send("RightClick", elementArg);

        public void RightClickDown(RemoteUiElement elementArg)
            => _client.Send("RightClick", elementArg);

        public void RightClickUp(RemoteUiElement elementArg)
            => _client.Send("RightClick", elementArg);
    }

}

