using MarcoPortefolioServer.Models.fivem;
using MarcoPortefolioServer.Models.v1;

namespace MarcoPortefolioServer.Repository.v1.fivem
{
    public class NewWhitelist
    {
        private List<Whitelist> whitelists;
        public NewWhitelist(string license, string license2, string steamId, string discordId)
        {
            whitelists = new List<Whitelist>();
            whitelists.Add(new Whitelist
            {
                license = license,
                license2 = license2,
                steam = steamId,
                discord = discordId,
                whitelisted = 1,
                banned = 0
            });
            SaveToDatabase();
        }
        private void SaveToDatabase()
        {
            SQLLiteConnectionModel.GetConnection();
            foreach (var whitelist in whitelists)
            {
                using var conn = SQLLiteConnectionModel.GetConnection();
                conn.Open();

                using var command = conn.CreateCommand();
                command.CommandText = "INSERT INTO fivem_whitelist (license, license2, steam, discord, whitelisted, banned) " +
                                      "VALUES (@license, @license2, @steam, @discord, @whitelisted, @banned)";
                command.Parameters.AddWithValue("@license", whitelist.license);
                command.Parameters.AddWithValue("@license2", whitelist.license2);
                command.Parameters.AddWithValue("@steam", whitelist.steam);
                command.Parameters.AddWithValue("@discord", whitelist.discord);
                command.Parameters.AddWithValue("@whitelisted", whitelist.whitelisted);
                command.Parameters.AddWithValue("@banned", whitelist.banned);
                command.ExecuteNonQuery();
            }
        }
    }
}
