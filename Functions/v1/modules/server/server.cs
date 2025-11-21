using MarcoPortefolioServer.Models.v1;
using MarcoPortefolioServer.Models.v1.DataModel;
using MarcoPortefolioServer.Repository.v1.DataRepository;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MarcoPortefolioServer.Functions.v1.modules.server
{
    public class Server
    {
        private static List<DataModel> SData = new List<DataModel>();

        public Server()
        {
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM srv_data";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string[]? dvalue = JsonSerializer.Deserialize<String[]>(reader.GetString(2));
                    SData.Add(new DataModel
                    {
                        id_data = reader.GetInt32(0),
                        dkey = reader.GetString(1),
                        dvalue = dvalue ?? Array.Empty<string>()
                    });
                }
            }
        }
        public DataModel[] getSData(string key)
        {
            var data = SData.Where(x => x.dkey == key).ToArray();
            return data.Length > 0
                ? data
                : Array.Empty<DataModel>();
        }

        public void remSData(int id_data)
        {
            var data = SData.FirstOrDefault(x => x.id_data == id_data);
            if (data != null)
            {
                SData.Remove(data);
            }
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
            else
            {
                var newData = ServerRepository.AddData(key, value);
                SData.Add(newData);

                return newData;
            }
        }
    }
}