using MarcoPortefolioServer.Models.v1;
using MarcoPortefolioServer.Models.v1.DataModel;
using MarcoPortefolioServer.Repository.v1.DataRepository;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MarcoPortefolioServer.Functions.v1.modules.client
{
    public class Client
    {
        private static List<DataModel> UData = new List<DataModel>();

        public Client()
        {
            try
            {
                using var conn = SQLLiteConnectionModel.GetConnection();
                conn.Open();

                using var command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM user_data";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string[]? dvalue = JsonSerializer.Deserialize<string[]>(reader.GetString(2));

                    UData.Add(new DataModel
                    {
                        id_data = reader.GetInt32(0),
                        dkey = reader.GetString(1),
                        dvalue = dvalue ?? Array.Empty<string>()
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao iniciar Client: " + ex.Message);
                throw;
            }
        }

        public DataModel[] getUData(string key)
        {
            var data = UData.Where(x => x.dkey == key).ToArray();
            return data.Length > 0
                ? data
                : Array.Empty<DataModel>();
        }
        public void remUData(int id_data)
        {
            var data = UData.FirstOrDefault(x => x.id_data == id_data);
            if (data != null)
            {
                UData.Remove(data);
            }
        }
        public DataModel? setUData(string key, string[] value)
        {
            var existingData = UData.FirstOrDefault(x => x.dkey == key);

            if (existingData != null)
            {
                existingData.dvalue = value;
                ClientRepository.UpdateData(existingData.id_data, key, value);
                return existingData;
            }

            var newData = ClientRepository.AddData(key, value);
            UData.Add(newData);
            return newData;
        }
    }
}