using TachyonClientRPC;
using System.Threading.Tasks;
using System;
using AIR.UnityTestPilot.Remote;

namespace GeneratedBindings
{
    public class RemoteUnityDriverClientBinding : IRemoteUnityDriver
    {
        ClientRpc _client;
        public void Bind(ClientRpc client)
        {
            _client = client;
        }

        public Task<RemoteUiElement> Query(RemoteElementQuery queryArg)
        {
            return _client.AskTask<RemoteUiElement>("Query",queryArg);
        }

        public void Shutdown(Boolean immediateArg)
        {
            _client.Send("Shutdown", immediateArg);
        }

        public void SetTimeScale(Single timeScaleArg)
        {
            _client.Send("SetTimeScale", timeScaleArg);
        }

        public void LeftClick(RemoteUiElement elementArg)
        {
            _client.Send("LeftClick", elementArg);
        }

    }

}

