using MarcoPortefolioServer.Models.v1;
using MarcoPortefolioServer.Models.v1.ProjectsModels;

namespace MarcoPortefolioServer.Repository.v1.ProjectRepository
{
    public class IdeRepository
    {
        // Remover o operador de nullable e inicializar diretamente
        private static List<IDEModel> ideAllModel = new List<IDEModel>();
        public static void initializeProjects()
        {
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM ide";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ideAllModel.Add(new IDEModel
                    {
                        id_ide = reader.GetInt32(0),
                        name = reader.GetString(1),
                        usage_count = reader.GetInt32(2)
                    });
                }
            }
        }

        public static List<IDEModel> getAllIde()
        {
            return ideAllModel;
        }

        public static IDEModel addNewIde(string name)
        {
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();
            using var command = conn.CreateCommand();
            command.CommandText = "INSERT INTO ide (name, usage_count) VALUES (@name, 0);";
            command.Parameters.AddWithValue("@name", name);
            command.ExecuteNonQuery();
            command.CommandText = "SELECT last_insert_rowid();";
            object? result = command.ExecuteScalar();
            if (result is null)
                throw new InvalidOperationException("Não foi possível obter o ID do novo IDE inserido.");
            long lastId = Convert.ToInt64(result);
            IDEModel newIde = new IDEModel
            {
                id_ide = (int)lastId,
                name = name,
                usage_count = 0
            };
            ideAllModel.Add(newIde);
            return newIde;
        }
    }
}
