using System;
using System.Diagnostics;
#if !UNITY_EDITOR
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
#endif

namespace WindowsSocketWrapper
{

    public delegate void OnReceiveMessengerCallback(string msg);
    public delegate void OnDisconnectCallback(bool connect);

    public class SocketWrapper
    {
#if !UNITY_EDITOR
        private enum State
        {
            NOT_CONNECTED,
            CONNECTED,
            CONNECTING,
            DISCONNECTING,
            DISCONNECTED
        }
#endif
        public event OnReceiveMessengerCallback OnReceiveMessenger;
        public event OnDisconnectCallback onDisconnect;

#if !UNITY_EDITOR
        private State _state;
        private Uri _uri;
        private MessageWebSocket messageWebSocket;
        private DataWriter dataWriter;
#endif
        
        public void Connect(string uri)
        {
#if !UNITY_EDITOR
            try
            {
                Connector(uri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
#endif
        }

        private async void Connector(string uri)
        {
#if !UNITY_EDITOR
            try
            {
                _state = State.CONNECTING;
                messageWebSocket = new MessageWebSocket();
                messageWebSocket.Control.MessageType = SocketMessageType.Utf8;
                messageWebSocket.MessageReceived += WebSocket_MessageArrived;
                messageWebSocket.Closed += WebSocket_Closed;

                _uri = new Uri(uri);
                await messageWebSocket.ConnectAsync(_uri);
                _state = State.CONNECTED;
                dataWriter = new DataWriter(messageWebSocket.OutputStream);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
#endif
        }

        // Send Message
        public void SendMessage(string emiter, string message)
        {
#if !UNITY_EDITOR
            try
            {
                Emit(emiter, message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
#endif
        }


        //Emitter
        private async void Emit(string emiter, string message)
        {
#if !UNITY_EDITOR
            try
            {
                if (messageWebSocket == null)
                {
                    return;
                }
                if (dataWriter == null)
                {
                    return;
                }

                string form = string.Format("42[\"{0}\",\"{1}\"]", emiter, message);

                dataWriter.WriteString(form);
                await dataWriter.StoreAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
#endif
        }

        //Disconnect
        public void Disconnect()
        {
#if !UNITY_EDITOR
            try
            {
                _state = State.DISCONNECTING;
                dataWriter.DetachStream();
                dataWriter = null;
                messageWebSocket.Dispose();
                messageWebSocket = null;
                _state = State.DISCONNECTED;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
#endif
        }

        //Message Arrived Handler
        private void WebSocket_MessageArrived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
#if !UNITY_EDITOR
            try
            {
                if (OnReceiveMessenger == null)
                {
                    return;
                }
                using (DataReader dataReader = args.GetDataReader())
                {
                    if (dataReader != null)
                    {
                        dataReader.UnicodeEncoding = UnicodeEncoding.Utf8;
                        string message = dataReader.ReadString(dataReader.UnconsumedBufferLength);
                        OnReceiveMessenger?.Invoke(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
#endif
        }

        //Websocket Close Handler
        private void WebSocket_Closed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
#if !UNITY_EDITOR
            if (onDisconnect == null)
            {
                return;
            }
            if (_state == State.CONNECTED)
            {
                onDisconnect?.Invoke(false);
            }
            else
            {
                onDisconnect?.Invoke(true);
                Debug.WriteLine("WebSocket_Closed; Code: " + args.Code + ", Reason: \"" + args.Reason + "\"");
            }
            // Add additional code here to handle the WebSocket being closed.
        }
#endif
    }
}
