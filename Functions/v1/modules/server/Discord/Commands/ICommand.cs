using Discord;
using Discord.WebSocket;

namespace MarcoPortefolioServer.Functions.v1.modules.server.Discord.Commands
{
    public interface ICommand
    {
        string Name { get; }
        SlashCommandBuilder Build();
        Task Execute(SocketSlashCommand cmd);
    }
}