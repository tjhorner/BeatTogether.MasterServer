﻿using System;
using BeatTogether.Core.Messaging.Abstractions;
using BeatTogether.Core.Messaging.Extensions;
using Krypton.Buffers;

namespace BeatTogether.MasterServer.Messaging.Messages.DedicatedServer
{
    public class AuthenticateDedicatedServerRequest : IEncryptedMessage, IReliableRequest, IReliableResponse
    {
        public uint SequenceId { get; set; }
        public uint RequestId { get; set; }
        public uint ResponseId { get; set; }
        public string DedicatedServerId { get; set; }
        public byte[] Nonce { get; set; }
        public byte[] Hash { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public void WriteTo(ref GrowingSpanBuffer buffer)
        {
            buffer.WriteString(DedicatedServerId);
            buffer.WriteBytes(Nonce);
            buffer.WriteBytes(Hash);
            buffer.WriteInt64(Timestamp.ToUnixTimeSeconds());
        }

        public void ReadFrom(ref SpanBufferReader bufferReader)
        {
            DedicatedServerId = bufferReader.ReadString();
            Nonce = bufferReader.ReadBytes(16).ToArray();
            Hash = bufferReader.ReadBytes(32).ToArray();
            Timestamp = DateTimeOffset.FromUnixTimeSeconds(bufferReader.ReadInt64());
        }
    }
}