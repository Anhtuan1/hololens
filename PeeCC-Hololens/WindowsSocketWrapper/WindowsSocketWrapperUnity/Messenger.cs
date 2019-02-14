using System;

namespace WindowsSocketWrapper
{
    public delegate void OnReceiveMessengerCallback(string msg);
    public delegate void OnDisconnectCallback(bool connect);

    public class SocketWrapper
    {
        public event OnReceiveMessengerCallback OnReceiveMessenger;
        public event OnDisconnectCallback onDisconnect;

        // Connect to a socket host
        public void Connect(string uri)
        {
            throw new NotImplementedException();
        }

        // Send Message
        public void SendMessage(string emiter, string message)
        {
            throw new NotImplementedException();
        }

        // Disconnect socket host
        public void Disconnect()
        {
            throw new NotImplementedException();
        }
    }
}
