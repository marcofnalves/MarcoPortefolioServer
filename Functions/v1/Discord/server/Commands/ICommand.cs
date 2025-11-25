using Discord;
using Discord.WebSocket;

namespace MarcoPortefolioServer.Functions.v1.Discord.server.Commands
{
    public interface ICommand
    {
        string Name { get; }
        SlashCommandBuilder Build();
        Task Execute(SocketSlashCommand cmd);
    }
}