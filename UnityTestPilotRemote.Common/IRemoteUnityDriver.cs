using System;
using System.Threading.Tasks;
using TachyonCommon;

namespace AIR.UnityTestPilotRemote.Common
{

    [GenerateBindings]
    public interface IRemoteUnityDriver {    
        Task<RemoteUiElement> Query(RemoteElementQuery query);
        void Shutdown(Boolean immediate);
        void SetTimeScale(float timeScale);
        void LeftClick(RemoteUiElement element);
        void LeftClickDown(RemoteUiElement element);
        void LeftClickUp(RemoteUiElement element);
    }
}

