﻿namespace Network
{
    public delegate void NetworkMessageHandler(string data);
    public interface INetworkMessageHandlerRegister
    {
        void addNetworkMessageHandler(string token, NetworkMessageHandler handler);
    }
}