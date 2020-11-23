﻿using System;
using BeatTogether.Core.Messaging.Abstractions;
using BeatTogether.Core.Messaging.Extensions;
using Krypton.Buffers;

namespace BeatTogether.MasterServer.Messaging.Messages.DedicatedServer
{
    public class GetAvailableMatchmakingServerResponse : IEncryptedMessage, IReliableRequest, IReliableResponse
    {
        public enum ResultCode : byte
        {
            Success,
            NoMatchmakingServersAvailable,
            UnknownError
        }

        public uint SequenceId { get; set; }
        public uint RequestId { get; set; }
        public uint ResponseId { get; set; }
        public string DedicatedServerId { get; set; }
        public DateTimeOffset DedicatedServerCreationTime { get; set; }
        public ResultCode Result { get; set; }
        public string Id { get; set; }
        public int Port { get; set; }
        public int MaximumPlayerCount { get; set; }
        public byte[] Random { get; set; }
        public byte[] PublicKey { get; set; }

        public bool Success => Result == ResultCode.Success;

        public void WriteTo(ref GrowingSpanBuffer buffer)
        {
            buffer.WriteString(DedicatedServerId);
            buffer.WriteInt64(DedicatedServerCreationTime.ToUnixTimeSeconds());
            buffer.WriteUInt8((byte)Result);
            if (!Success)
                return;

            buffer.WriteString(Id);
            buffer.WriteVarUInt((uint)Port);
            buffer.WriteVarInt(MaximumPlayerCount);
            buffer.WriteBytes(Random);
            buffer.WriteVarBytes(PublicKey);
        }

        public void ReadFrom(ref SpanBufferReader bufferReader)
        {
            DedicatedServerId = bufferReader.ReadString();
            DedicatedServerCreationTime = DateTimeOffset.FromUnixTimeSeconds(bufferReader.ReadInt64());
            Result = (ResultCode)bufferReader.ReadByte();
            if (!Success)
                return;

            Id = bufferReader.ReadString();
            Port = (int)bufferReader.ReadVarUInt();
            MaximumPlayerCount = bufferReader.ReadVarInt();
            Random = bufferReader.ReadBytes(32).ToArray();
            PublicKey = bufferReader.ReadVarBytes().ToArray();
        }
    }
}