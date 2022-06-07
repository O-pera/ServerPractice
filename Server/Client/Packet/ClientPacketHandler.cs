using Google.Protobuf;
using Client.Session;
using ServerCore;
using Google.Protobuf.Protocol;

public static class PacketHandler {
    public static void S_Broadcast_ChatHandler(PacketSession s, IMessage packet) {
        S_Broadcast_Chat parsedPacket = (S_Broadcast_Chat)packet;
        ServerSession session = (ServerSession)s;

        throw new NotImplementedException();
    }public static void S_Broadcast_TestHandler(PacketSession s, IMessage packet) {
        S_Broadcast_Test parsedPacket = (S_Broadcast_Test)packet;
        ServerSession session = (ServerSession)s;

        throw new NotImplementedException();
    }
}