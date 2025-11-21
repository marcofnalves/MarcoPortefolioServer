namespace MarcoPortefolioServer.Functions.v1
{
    public static class Age
    {
        public static int CalcularIdade(DateOnly dataNascimento)
        {
            var hoje = DateOnly.FromDateTime(DateTime.Today);
            int idade = hoje.Year - dataNascimento.Year;
            if (dataNascimento > hoje.AddYears(-idade))
            {
                idade--;
            }
            return idade;
        }

        public static int CalcularIdade(DateTime dataNascimento)
        {
            return CalcularIdade(DateOnly.FromDateTime(dataNascimento));
        }
    }
}
