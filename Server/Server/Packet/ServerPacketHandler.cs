using Google.Protobuf;
using Server.Session;
using ServerCore;
using Google.Protobuf.Protocol;

public static class PacketHandler {
    public static void C_ChatHandler(PacketSession s, IMessage packet) {
        C_Chat parsedPacket = (C_Chat)packet;
        ClientSession session = (ClientSession)s;

        throw new NotImplementedException();
    }public static void C_TestHandler(PacketSession s, IMessage packet) {
        C_Test parsedPacket = (C_Test)packet;
        ClientSession session = (ClientSession)s;

        throw new NotImplementedException();
    }
}