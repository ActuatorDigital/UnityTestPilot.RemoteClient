using System;
using System.Text;

namespace AIR.UnityTestPilotRemote.Common
{
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
}

