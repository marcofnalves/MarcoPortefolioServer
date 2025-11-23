using MarcoPortefolioServer.Models.fivem;

namespace MarcoPortefolioServer.Repository.v1.fivem
{
    public static class NewWhitelist
    {
        private static List<WhitelistModel> whitelists = new();

        public static WhitelistModel? GetByLicense(string license)
        {
            return whitelists.FirstOrDefault(w => w.license == license);
        }

        public static WhitelistModel Add(string license, string license2, string steamId, string discordId)
        {
            var whitelist = new WhitelistModel
            {
                license = license,
                license2 = license2,
                steam = steamId,
                discord = discordId,
                whitelisted = 0,
                banned = 0
            };

            whitelists.Add(whitelist);
            return whitelist;
        }

        public static bool Remove(string license)
        {
            var entry = whitelists.FirstOrDefault(w => w.license == license);
            if (entry == null)
                return false;

            return whitelists.Remove(entry);
        }
    }
}