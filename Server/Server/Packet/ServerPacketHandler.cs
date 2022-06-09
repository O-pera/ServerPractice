using Google.Protobuf;
using Server.Session;
using ServerCore;
using Google.Protobuf.Protocol;
using System;
using Server.Contents;

public static class PacketHandler {
    public static void C_EnterHandler(PacketSession s, IMessage packet) {
        C_Enter parsedPacket = (C_Enter)packet;
        ClientSession session = (ClientSession)s;

        GameRoomManager.Instance.TryEnterRoom(session);
    }
    public static void C_MoveHandler(PacketSession s, IMessage packet) {
        C_Move parsedPacket = (C_Move)packet;
        ClientSession session = (ClientSession)s;        

        GameRoom room = session.Room;
        if(room == null)
            return;
        S_Broadcast_Move movePacket = new S_Broadcast_Move(){ Pos = new Position() };
        movePacket.SessionID = session.SessionID;
        movePacket.Pos = parsedPacket.Pos;
        room.Push(() => { room.HandleMove(session, movePacket); });

        Console.WriteLine($"Client{session.SessionID} Moved To X:{parsedPacket.Pos.X} Y: {parsedPacket.Pos.Y} Z: {parsedPacket.Pos.Z}");
    }
}