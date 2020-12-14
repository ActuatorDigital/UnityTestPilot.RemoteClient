using System;
using System.Text;
using System.Threading.Tasks;
using TachyonCommon;

namespace AIR.UnityTestPilotRemote.Common {

    [GenerateBindings]
    public interface IRemoteUnityDriver {    
        Task<RemoteUiElement> Query(RemoteElementQuery query);
        void Shutdown(Boolean immediate);
        void SetTimeScale(float timeScale);
        void LeftClick(RemoteUiElement element);
        void LeftClickDown(RemoteUiElement element);
        void LeftClickUp(RemoteUiElement element);
    }

    public struct RemoteUiElement : ISerializableAgentMessage {

        public string Name;
        public bool IsActive;
        public string Text;
        public float XPos, YPos, ZPos;
        public float XRot, YRot, ZRot;

        public byte[] Serialize() {
            var elementStr = String.Join("|",
                Name, 
                IsActive ? "Active" : "Inactive",
                XPos, YPos, ZPos,
                XRot, YRot, ZRot,
                Text );
            return Encoding.ASCII.GetBytes(elementStr);
        }

        public void Deserialize(byte[] objBytes)
        {
            var elementStr = Encoding.ASCII.GetString(objBytes);
            var elementParts = elementStr.Split('|');
            Name = elementParts[0];
            IsActive = elementParts[1] == "Active";

            XPos = float.Parse(elementParts[2]);
            YPos = float.Parse(elementParts[3]);
            ZPos = float.Parse(elementParts[4]);

            XRot = float.Parse(elementParts[5]);
            YRot = float.Parse(elementParts[6]);
            ZRot = float.Parse(elementParts[7]);

            Text = elementParts[8];
        }
    }

    public struct RemoteElementQuery : ISerializableAgentMessage {
    
        public RemoteElementQuery(
            QueryFormat format, 
            string name, 
            string targetType
        ) {
            Format = format;
            Name = name;
            TargetType = targetType;
        }

        public QueryFormat Format;
        public string Name;
        public string TargetType;

        public byte[] Serialize() {
            var queryString = String.Join("|", 
                Format.ToString(), 
                Name, 
                TargetType
            );

            return Encoding.ASCII.GetBytes(queryString);
        }

        public void Deserialize(byte[] objBytes) {
            var queryString = Encoding.ASCII.GetString(objBytes);
            var elementParts = queryString.Split('|');
            Format = (QueryFormat)Enum.Parse(typeof(QueryFormat), elementParts[0]);
            Name = elementParts[1];
            TargetType = elementParts[2];
        }
    }

    public enum QueryFormat {
        NamedQuery = 0,
        TypedQuery = 1,
        Invalid
    }
    
}

