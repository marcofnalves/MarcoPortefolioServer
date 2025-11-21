using MarcoPortefolioServer.Models.v1;
using MarcoPortefolioServer.Models.v1.DataModel;
using System.Text.Json;

namespace MarcoPortefolioServer.Repository.v1.DataRepository
{
    public static class ServerRepository
    {
        internal static DataModel AddData(string key, string[] value)
        {
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO srv_data (dkey, dvalue) VALUES (@dkey, @dvalue)";
            cmd.Parameters.AddWithValue("@dkey", key);
            cmd.Parameters.AddWithValue("@dvalue", JsonSerializer.Serialize(value));
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";
            long lastId = (long)cmd.ExecuteScalar()!;

            return new DataModel
            {
                id_data = (int)lastId,
                dkey = key,
                dvalue = value
            };
        }
        internal static void UpdateData(int id_data, string key, string[] value)
        {
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE srv_data SET dkey = @dkey, dvalue = @dvalue WHERE id_data = @id_data";
            cmd.Parameters.AddWithValue("@dkey", key);
            cmd.Parameters.AddWithValue("@dvalue", JsonSerializer.Serialize(value));
            cmd.Parameters.AddWithValue("@id_data", id_data);

            cmd.ExecuteNonQuery();
        }
        internal static void RemoveData(int id_data)
        {
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM srv_data WHERE id_data = @id_data";
            cmd.Parameters.AddWithValue("@id_data", id_data);
            cmd.ExecuteNonQuery();
        }
    }
}
