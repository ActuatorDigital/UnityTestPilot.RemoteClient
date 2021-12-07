// Copyright (c) AIR Pty Ltd. All rights reserved.

using System;
using System.Threading;
using System.Threading.Tasks;
using AIR.UnityTestPilotRemote.Client;
using AIR.UnityTestPilotRemote.Common;

namespace AIR.UnityTestPilot.Interactions
{
    public class UiElementRemote : UiElement
    {
        private RemoteUiElement _remoteUiElement;
        private readonly UnityDriverHostProcess _process;

        public UiElementRemote(
            RemoteUiElement remoteUiElement,
            UnityDriverHostProcess process
        ) : base(remoteUiElement) {
            _remoteUiElement = remoteUiElement;
            _process = process;
        }

        public override string Name => _remoteUiElement.Name;

        public override string FullPath => _remoteUiElement.FullPath;

        public override bool IsActive => _remoteUiElement.IsActive;

        public override string Text => _remoteUiElement.Text;

        public override Float3 LocalPosition
            => new Float3(_remoteUiElement.XPos, _remoteUiElement.YPos, _remoteUiElement.ZPos);

        public override Float3 EulerRotation
            => new Float3(_remoteUiElement.XRot, _remoteUiElement.YRot, _remoteUiElement.ZRot);

        public override void LeftClick() => _process.LeftClick(_remoteUiElement);

        public override void LeftClickDown() => _process.LeftClickDown(_remoteUiElement);

        public override void LeftClickUp() => _process.LeftClickUp(_remoteUiElement);

        public override void LeftClickAndHold(TimeSpan holdTime)
        {
            LeftClickDown();
            Task.Run(() => {
                Thread.Sleep(holdTime);
                LeftClickUp();
            });
        }

        public override void MiddleClick() =>
            throw new NotImplementedException("Remote process does not yet support middle click.");

        public override void RightClick() => _process.RightClick(_remoteUiElement);

        public override void RightClickDown() => _process.RightClickDown(_remoteUiElement);

        public override void MiddleClickDown() =>
            throw new NotImplementedException("Remote process does not yet support middle click.");

        public override void RightClickUp() => _process.RightClickUp(_remoteUiElement);

        public override void MiddleClickUp() =>
            throw new NotImplementedException("Remote process does not yet support middle click up.");

        public override void SimulateKeys(KeyCode[] keys)
            => throw new NotImplementedException("Send keys to unity input system.");

        public override void SimulateKeys(string keys)
            => throw new NotImplementedException("Convert string to keys then call SimulateKeys.");
    }
}