using Google.Protobuf;
using Server.Session;
using ServerCore;
using Google.Protobuf.Protocol;
using System;

public static class PacketHandler {
    public static void C_EnterHandler(PacketSession s, IMessage packet) {
        C_Enter parsedPacket = (C_Enter)packet;
        ClientSession session = (ClientSession)s;

        throw new NotImplementedException();
    }public static void C_MoveHandler(PacketSession s, IMessage packet) {
        C_Move parsedPacket = (C_Move)packet;
        ClientSession session = (ClientSession)s;

        throw new NotImplementedException();
    }
}