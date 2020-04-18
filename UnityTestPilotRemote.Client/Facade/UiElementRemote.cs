// Copyright (c) AIR Pty Ltd. All rights reserved.

using System;
using AIR.UnityTestPilot.Remote;
using AIR.UnityTestPilotRemote.Client;

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

        public override bool IsActive => _remoteUiElement.IsActive;

        public override string Text => _remoteUiElement.Text;

        public override void LeftClick() => _process.LeftClick(_remoteUiElement);
        
        public override void MiddleClick() =>
            throw new NotImplementedException("Remote process does not yet support middle click.");
        public override void RightClick() =>
            throw new NotImplementedException("Remote process does not yet support right click.");
        public override void LeftClickDown() => 
            throw new NotImplementedException("Remote process does not yet support left click without release.");
        public override void RightClickDown() => 
            throw new NotImplementedException("Remote process does not yet support right click without release.");
        public override void MiddleClickDown() => 
            throw new NotImplementedException("Remote process does not yet support middle click.");
        public override void LeftClickUp() => 
            throw new NotImplementedException("Remote process does not yet support left  click up.");
        public override void RightClickUp() => 
            throw new NotImplementedException("Remote process does not yet support right click up.");
        public override void MiddleClickUp() => 
            throw new NotImplementedException("Remote process does not yet support middle click up.");
        
        public override void SimulateKeys(KeyCode[] keys) 
            => throw new NotImplementedException("Send keys to unity input system.");

        public override void SimulateKeys(string keys) 
            => throw new NotImplementedException("Convert string to keys then call SimulateKeys.");
    }
}