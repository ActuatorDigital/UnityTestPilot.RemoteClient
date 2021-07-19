using System;
using TachyonCommon;

namespace AIR.UnityTestPilotRemote.Common
{
    public class RemoteDriverSerializer : ISerializer {
        
        public byte[] SerializeObject<T>(T obj) {
            if (obj is ISerializableAgentMessage serializable)
                return serializable.Serialize();

            throw new ArgumentException(obj.GetType().Name + 
                " does not implement " + typeof(ISerializableAgentMessage).Name);
        }

        public object DeserializeObject(byte[] objBytes, Type type) {
            if (Activator.CreateInstance(type) is ISerializableAgentMessage obj) {
                obj.Deserialize(objBytes);

                return obj;
            }

            throw new ArgumentException(type.Name + 
                " does not implement " + typeof(ISerializableAgentMessage).Name);
        }
        
    }
}