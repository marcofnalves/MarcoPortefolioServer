using MarcoPortefolioServer.Repository.v1.fivem;

namespace MarcoPortefolioServer.Functions.v1.fivem
{
    public class Fivem
    {
        public void CreateWhitelistEntry(string license, string license2, string steamId, string discordId)
        {
            NewWhitelist newWhitelist = new NewWhitelist(license, license2, steamId, discordId);
        }
    }
}
