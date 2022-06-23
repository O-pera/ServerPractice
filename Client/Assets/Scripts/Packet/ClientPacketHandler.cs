using Google.Protobuf;
using Client.Session;
using ServerCore;
using Google.Protobuf.Protocol;
using System;
using UnityEngine;

public static class PacketHandler {
    public static void S_Enter_ResponseHandler(PacketSession s, IMessage packet) {
        S_Enter_Response parsedPacket = (S_Enter_Response)packet;
        ServerSession session = (ServerSession)s;

        session.SessionID = parsedPacket.SessionID;

        Debug.Log($"I'm {session.SessionID}!");
    }
    public static void S_Broadcast_MoveHandler(PacketSession s, IMessage packet) {
        S_Broadcast_Move parsedPacket = (S_Broadcast_Move)packet;
        ServerSession session = (ServerSession)s;

        //TODO: Manage Object's Trnasform
        //if(session.SessionID == parsedPacket.SessionID)
        //    return;

        Debug.Log($"Other Client{parsedPacket.SessionID} Moved To X:{parsedPacket.Pos.X} Y: {parsedPacket.Pos.Y} Z: {parsedPacket.Pos.Z}");
    }
}