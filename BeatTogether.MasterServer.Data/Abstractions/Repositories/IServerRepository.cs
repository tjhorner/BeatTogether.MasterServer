﻿using System.Threading.Tasks;
using BeatTogether.MasterServer.Data.Entities;

namespace BeatTogether.MasterServer.Data.Abstractions.Repositories
{
    public interface IServerRepository
    {
        Task<Server> GetServer(string code);
        Task<Server> GetServerByHostUserId(string userId);
        Task<Player> GetPlayer(string userId);

        Task<bool> AddServer(Server server);
        Task<bool> RemoveServer(string code);

        Task<Server> GetAvailablePublicServerAndAddPlayer(string userId, string userName);
        Task<Server> GetServerWithCodeAndAddPlayer(string code, string userId, string userName);
        Task<bool> RemovePlayer(string userId);

        void UpdateCurrentPlayerCount(string code, int currentPlayerCount);
        void UpdateSecret(string code, string secret);
    }
}
