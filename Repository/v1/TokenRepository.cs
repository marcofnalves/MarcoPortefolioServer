using MarcoPortefolioServer.Models.v1;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace MarcoPortefolioServer.Repository.v1
{
    public class TokenRepository
    {
        public TokenModel? CreateToken(string name, string type, string tokenValue, string conta)
        {
            if (TokenTypeExists(tokenValue, type, conta))
                return null;

            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var command = conn.CreateCommand();
            command.CommandText = @"
                INSERT INTO tokens (name, type, token, conta)
                VALUES (@name, @type, @token, @conta)";

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@type", type);
            command.Parameters.AddWithValue("@token", tokenValue);
            command.Parameters.AddWithValue("@conta", conta);

            command.ExecuteNonQuery();

            command.CommandText = "SELECT last_insert_rowid()";
            long lastId = (long)command.ExecuteScalar()!;

            return new TokenModel
            {
                id_token = Convert.ToInt32(lastId),
                name = name,
                type = type,
                token = tokenValue,
                conta = conta
            };
        }

        public TokenModel? GetTokenByValue(string value)
        {
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM tokens WHERE token = @token";
            command.Parameters.AddWithValue("@token", value);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new TokenModel
                {
                    id_token = Convert.ToInt32(reader["id_token"]!),
                    name = reader["name"].ToString()!,
                    type = reader["type"].ToString()!,
                    token = reader["token"].ToString()!,
                    conta = reader["conta"].ToString()!
                };
            }

            return null;
        }

        public List<TokenModel> GetTokensByConta(string conta)
        {
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM tokens WHERE conta = @conta";
            command.Parameters.AddWithValue("@conta", conta);

            using var reader = command.ExecuteReader();
            List<TokenModel> tokens = new();

            while (reader.Read())
            {
                tokens.Add(new TokenModel
                {
                    id_token = Convert.ToInt32(reader["id_token"]!),
                    name = reader["name"].ToString()!,
                    type = reader["type"].ToString()!,
                    token = reader["token"].ToString()!,
                    conta = reader["conta"].ToString()!
                });
            }

            return tokens;
        }

        public List<string> GetTypesForToken(string tokenValue, string conta)
        {
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var command = conn.CreateCommand();
            command.CommandText = "SELECT type FROM tokens WHERE token = @token AND conta = @conta";
            command.Parameters.AddWithValue("@token", tokenValue);
            command.Parameters.AddWithValue("@conta", conta);

            using var reader = command.ExecuteReader();
            List<string> types = new();

            while (reader.Read())
                types.Add(reader["type"].ToString()!);

            return types;
        }

        public Dictionary<string, List<string>> GetTokensAndTypesByConta(string conta)
        {
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();
            using var command = conn.CreateCommand();
            command.CommandText = "SELECT token, type FROM tokens WHERE conta = @conta";
            command.Parameters.AddWithValue("@conta", conta);

            using var reader = command.ExecuteReader();
            var result = new Dictionary<string, List<string>>();

            while (reader.Read())
            {
                string token = reader["token"].ToString()!;
                string type = reader["type"].ToString()!;

                if (!result.ContainsKey(token))
                    result[token] = new List<string>();

                result[token].Add(type);
            }

            return result;
        }

        public TokenModel? GetToken(string tokenValue, string type, string conta)
        {
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM tokens WHERE token = @token AND type = @type AND conta = @conta";

            command.Parameters.AddWithValue("@token", tokenValue);
            command.Parameters.AddWithValue("@type", type);
            command.Parameters.AddWithValue("@conta", conta);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new TokenModel
                {
                    id_token = Convert.ToInt32(reader["id_token"]!),
                    name = reader["name"].ToString()!,
                    type = reader["type"].ToString()!,
                    token = reader["token"].ToString()!,
                    conta = reader["conta"].ToString()!
                };
            }

            return null;
        }

        public bool TokenTypeExists(string tokenValue, string type, string conta)
        {
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();
            using var command = conn.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM tokens WHERE token = @token AND type = @type AND conta = @conta";

            command.Parameters.AddWithValue("@token", tokenValue);
            command.Parameters.AddWithValue("@type", type);
            command.Parameters.AddWithValue("@conta", conta);

            long count = (long)command.ExecuteScalar()!;
            return count > 0;
        }
    }
}