using System.Threading.Tasks;
using System.Threading;
using TachyonClientRPC;
using GeneratedBindings;
using System;
using AIR.UnityTestPilotRemote.Common;

namespace AIR.UnityTestPilotRemote.Client
{

    public class RemoteUnityDriverClient {

        private int CONNECTION_TIMEOUT_DEFAULT_SECONDS = 5;
        private TimeSpan _connectTimeout;
        private ClientRpc _clientRpc;

        public RemoteUnityDriverClient(TimeSpan connectTimeout = default) {
            _clientRpc = new ClientRpc(new TachyonClientIO.Client(), new RemoteDriverSerializer());
            _connectTimeout = connectTimeout == default 
                ? TimeSpan.FromSeconds(CONNECTION_TIMEOUT_DEFAULT_SECONDS) 
                : connectTimeout;
        }

        public async Task<bool> Connect() {

            bool connected = false;
            bool failedToConnect = false;
            
            _clientRpc.OnConnected += () => connected = true;
            _clientRpc.OnFailedToConnect += (ex) => failedToConnect = true;
            _clientRpc.OnDisconnected += () => connected = false;
            _clientRpc.Connect("localhost", 13);

            var timeout = new CancellationTokenSource();
            timeout.CancelAfter(_connectTimeout);
            await Task.Run(() => {
                while (!connected && !failedToConnect) 
                    Task.Delay(TimeSpan.FromSeconds(.5f));
            }, timeout.Token);

            return connected & !failedToConnect;
        }

        public IRemoteUnityDriver Bind() {
            var binding = new RemoteUnityDriverClientBinding();
            binding.Bind(_clientRpc);
            return binding;
        }

    }
    
}