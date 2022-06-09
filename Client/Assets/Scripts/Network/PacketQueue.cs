using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketData {
    public ushort ID { get; set; }
    public IMessage Packet { get; set; }
}

public class PacketQueue{
    private static PacketQueue _instance = new PacketQueue();
    public static PacketQueue Instance { get { return _instance; } }

    private Queue<PacketData> _pq = new Queue<PacketData>();
    object l_pq = new object();

    public void Push(ushort id, IMessage packet) {
        lock(l_pq) {
            _pq.Enqueue(new PacketData() { ID = id, Packet = packet });
        }
    }

    public List<PacketData> PopAll() {
        if(_pq.Count == 0)
            return null;

        List<PacketData> list = new List<PacketData>();
        lock(l_pq) {
            list.Add(_pq.Dequeue());
        }

        return list;
    }
}
