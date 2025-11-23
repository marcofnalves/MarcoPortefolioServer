using MarcoPortefolioServer.Functions.v1.modules.server.Discord;
using MarcoPortefolioServer.Models.fivem;
using MarcoPortefolioServer.Models.v1;
using MarcoPortefolioServer.Models.v1.DataModel;
using MarcoPortefolioServer.Repository.v1.DataRepository;
using System.Text.Json;

namespace MarcoPortefolioServer.Functions.v1.modules.server
{
    public class Server
    {
        private static ServerCacheModel SData = new ServerCacheModel();
        private readonly DiscordBot? _discordBot;

        public Server()
        {
            // ------------------------------
            // 🚀 Inicializar dados do servidor
            // ------------------------------
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM srv_data";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string[]? dvalue = JsonSerializer.Deserialize<String[]>(reader.GetString(2));
                    SData.Add(reader.GetInt32(0), reader.GetString(1), dvalue);
                }
            }

            // ------------------------------
            // 🤖 Inicializar o Discord Bot
            // ------------------------------
            try
            {
                string? discordToken = Environment.GetEnvironmentVariable("MTQ0MTk2OTM5MDg3NzgwNjU5Mg.G8hUv6.NHOohbS-jeS1OiVIoX1IKgn6b52jv3iYIlbhkE");

                if (!string.IsNullOrEmpty(discordToken))
                {
                    _discordBot = new DiscordBot(discordToken);
                    _ = _discordBot.StartAsync();
                    Console.WriteLine("[SERVER] Discord Bot carregado.");
                }
                else
                {
                    Console.WriteLine("[SERVER] Discord Bot não iniciado: token não definido.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[SERVER] Erro ao iniciar bot Discord: " + ex.Message);
            }
        }

        public DataModel[] getSData(string key)
        {
            return SData.Where(x => x.dkey == key).ToArray();
        }

        public void remSData(int id_data)
        {
            var data = SData.FirstOrDefault(x => x.id_data == id_data);
            if (data != null)
                SData.Remove(data);
        }

        public DataModel setSData(string key, string[] value)
        {
            var existing = SData.FirstOrDefault(x => x.dkey == key);

            if (existing != null)
            {
                existing.dvalue = value;
                ServerRepository.UpdateData(existing.id_data, key, value);
                return existing;
            }

            var newData = ServerRepository.AddData(key, value);
            SData.Add(newData);
            return newData;
        }

        public void NewWhitelist(string license, string license2, string steamId, string discordId)
        {
            // 1. Verificar se já existe
            var existing = Repository.v1.fivem.NewWhitelist.GetByLicense(license);

            if (existing != null)
            {
                Console.WriteLine("Já existe whitelist para este license.");
                return;
            }

            // 2. Adicionar à whitelist em memória
            WhitelistModel newWhitelist = Repository.v1.fivem.NewWhitelist.Add(license, license2, steamId, discordId);

            // 3. Serializar para JSON STRING
            string json = JsonSerializer.Serialize(newWhitelist);

            // 4. Buscar whitelists já guardadas na BD
            var existingSData = getSData("whitelists");

            List<string> list = new List<string>();

            if (existingSData.Length > 0)
                list.AddRange(existingSData[0].dvalue); // adicionar as já existentes

            // 5. Adicionar a nova
            list.Add(json);

            // 6. Guardar tudo de volta na BD e cache
            setSData("whitelists", list.ToArray());

            Console.WriteLine("Whitelist adicionada com sucesso!");
        }

        public void UpdateWhitelist(
            int id_data,
            string license,
            string license2,
            string steamId,
            string discordId,
            int whitelisted,
            int banned)
        {
            // 1. Procurar whitelist em memória
            var existing = Repository.v1.fivem.NewWhitelist.GetByLicense(license);
            if (existing == null)
            {
                Console.WriteLine("Whitelist não encontrada para atualização.");
                return;
            }

            // 2. Atualizar campos do modelo
            existing.license2 = license2;
            existing.steam = steamId;
            existing.discord = discordId;
            existing.whitelisted = whitelisted;
            existing.banned = banned;

            // 3. Serializar para JSON
            string updatedJson = JsonSerializer.Serialize(existing);

            // 4. Buscar dados existentes na BD
            var existingSData = getSData("whitelists");
            if (existingSData.Length == 0)
            {
                Console.WriteLine("Nenhuma whitelist encontrada na base de dados.");
                return;
            }

            // 5. Copiar lista atual
            List<string> list = new(existingSData[0].dvalue);

            // 6. Encontrar índice da entrada a atualizar
            int index = list.FindIndex(w =>
            {
                var obj = JsonSerializer.Deserialize<WhitelistModel>(w);
                return obj?.license == license;
            });

            if (index == -1)
            {
                Console.WriteLine("Whitelist não encontrada na lista para atualização.");
                return;
            }

            // 7. Substituir entrada antiga por nova
            list[index] = updatedJson;

            // 8. Guardar tudo na BD e cache
            setSData("whitelists", list.ToArray());

            Console.WriteLine("Whitelist atualizada com sucesso!");
        }

        public void RemoveWhitelist(string license)
        {
            // 1. Procurar na whitelist em memória
            var existing = Repository.v1.fivem.NewWhitelist.GetByLicense(license);

            if (existing == null)
            {
                Console.WriteLine("Whitelist não encontrada para remoção.");
                return;
            }

            // 2. Remover da whitelist em memória
            Repository.v1.fivem.NewWhitelist.Remove(license);

            // 3. Buscar whitelists guardadas na BD
            var existingSData = getSData("whitelists");
            if (existingSData.Length == 0)
            {
                Console.WriteLine("Nenhuma whitelist encontrada na base de dados.");
                return;
            }

            List<string> list = new(existingSData[0].dvalue);

            // 4. Encontrar a posição no JSON guardado
            int index = list.FindIndex(w =>
            {
                var obj = JsonSerializer.Deserialize<WhitelistModel>(w);
                return obj?.license == license;
            });

            if (index == -1)
            {
                Console.WriteLine("Whitelist não encontrada na lista para exclusão.");
                return;
            }

            // 5. Remover da lista
            list.RemoveAt(index);

            // 6. Guardar a lista atualizada
            setSData("whitelists", list.ToArray());

            Console.WriteLine("Whitelist removida com sucesso!");
        }
    }
}