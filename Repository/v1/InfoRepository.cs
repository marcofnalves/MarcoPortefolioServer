using MarcoPortefolioServer.Models.v1;

namespace MarcoPortefolioServer.Repository.v1
{
    public class InfoRepository
    {
        public InfoModel GetInfo()
        {
            var infoModel = new InfoModel();
            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM info";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    infoModel.name = reader.GetString(0);
                    var dateOfBirthString = reader.GetString(1);
                    infoModel.dateOfBirth = DateOnly.Parse(dateOfBirthString);
                    infoModel.age = Functions.v1.Age.CalcularIdade(infoModel.dateOfBirth);
                }
            }
            return infoModel;
        }

        public InfoModel UpdateInfo(string name, int age, DateOnly dateBirth)
        {
            var infoModel = new InfoModel();

            using var conn = SQLLiteConnectionModel.GetConnection();
            conn.Open();

            using var command = conn.CreateCommand();
            command.CommandText = "UPDATE info SET name = @name, dateOfBirth = @dateOfBirth WHERE id = 1";
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@dateOfBirth", dateBirth.ToString("yyyy-MM-dd"));

            command.ExecuteNonQuery();

            // Após atualizar, buscar os dados atualizados
            command.CommandText = "SELECT name, dateOfBirth FROM info WHERE id = 1";
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    infoModel.name = reader.GetString(0);
                    var dateOfBirthString = reader.GetString(1);
                    infoModel.dateOfBirth = DateOnly.Parse(dateOfBirthString);
                    infoModel.age = Functions.v1.Age.CalcularIdade(infoModel.dateOfBirth);
                }
            }
            return infoModel;
        }
    }
}
