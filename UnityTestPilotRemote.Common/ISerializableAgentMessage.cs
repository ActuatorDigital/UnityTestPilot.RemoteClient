namespace AIR.UnityTestPilotRemote.Common
{
    public interface ISerializableAgentMessage {
        byte[] Serialize();
        void Deserialize(byte[] objBytes);
    }
}