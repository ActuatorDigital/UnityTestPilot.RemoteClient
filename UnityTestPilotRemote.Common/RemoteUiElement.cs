using System;
using System.Text;

namespace AIR.UnityTestPilotRemote.Common
{
    public struct RemoteUiElement : ISerializableAgentMessage {

        public string Name;
        public string FullPath;
        public bool IsActive;
        public string Text;
        public float XPos, YPos, ZPos;
        public float XRot, YRot, ZRot;

        public byte[] Serialize() {
            var elementStr = String.Join("|",
                Name,
                FullPath,
                IsActive ? "Active" : "Inactive",
                XPos, YPos, ZPos,
                XRot, YRot, ZRot,
                Text );
            return Encoding.ASCII.GetBytes(elementStr);
        }

        public void Deserialize(byte[] objBytes)
        {
            var elementStr = Encoding.ASCII.GetString(objBytes);
            if(string.IsNullOrEmpty(elementStr))
                return;

            var elementParts = elementStr.Split('|');
            int index = 0;
            Name = elementParts[index++];
            FullPath = elementParts[index++];
            IsActive = elementParts[index++] == "Active";

            XPos = float.Parse(elementParts[index++]);
            YPos = float.Parse(elementParts[index++]);
            ZPos = float.Parse(elementParts[index++]);

            XRot = float.Parse(elementParts[index++]);
            YRot = float.Parse(elementParts[index++]);
            ZRot = float.Parse(elementParts[index++]);

            Text = elementParts[index++];
        }
    }
}

