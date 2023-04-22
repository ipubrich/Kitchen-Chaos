using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;

public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable
{
    public ulong clientId;
    public int colorId;

    public bool Equals(PlayerData other)
    {
        return clientId == other.clientId && colorId == other.colorId; ;
    }

    // serialize client id
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        // INetworkSerializable enables custom data points.

        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref colorId);
    }
}
