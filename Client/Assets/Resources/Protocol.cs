using System;
using System.Collections.Generic;

public enum PacketType {
    C_TryConnect = 0,
    S_Accept,
}

public interface IPacket {
    public ushort Type { get; set; }
    public ushort Size { get; set; }

    public ArraySegment<byte> Write();
    public void Read(ArraySegment<byte> segment);
}

public class C_TryConnect : IPacket {
    public ushort Type { get; set; } = ( ushort )PacketType.C_TryConnect;
    public ushort Size { get; set; }

    public int UserID { get; set; }
    public void Read(ArraySegment<byte> segment) {
        int count = 0;
        Size = BitConverter.ToUInt16(segment.Array, count);
        count += sizeof(ushort);
        Type = BitConverter.ToUInt16(segment.Array, count);
        count += sizeof(ushort);
        UserID = BitConverter.ToInt32(segment.Array, count);
    }

    public ArraySegment<byte> Write() {
        byte[] segment = new byte[8];
        int count = 0;
        count += 2;

        Array.Copy(BitConverter.GetBytes(Type), 0, segment, count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(UserID), 0, segment, count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(count), 0, segment, 0, sizeof(ushort));

        return new ArraySegment<byte>(segment);
    }
}

public class S_Accept : IPacket {
    public ushort Type { get; set; } = ( ushort )PacketType.S_Accept;
    public ushort Size { get; set; }

    public int AuthCode { get; set; }

    public void Read(ArraySegment<byte> segment) {
        int count = 0;
        Size = BitConverter.ToUInt16(segment.Array, count);
        count += sizeof(ushort);
        Type = BitConverter.ToUInt16(segment.Array, count);
        count += sizeof(ushort);
        AuthCode = BitConverter.ToUInt16(segment.Array, count);
    }

    public ArraySegment<byte> Write() {
        byte[] segment = new byte[8];
        int count = 0;
        count += 2;

        Array.Copy(BitConverter.GetBytes(Type), 0, segment, count, sizeof(ushort));
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(AuthCode), 0, segment, count, sizeof(int));
        count += sizeof(int);
        Array.Copy(BitConverter.GetBytes(count), 0, segment, 0, sizeof(ushort));

        return new ArraySegment<byte>(segment);
    }
}