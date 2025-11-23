using Auth0.ManagementApi.Models;
using MarcoPortefolioServer.Functions.v1;
using MarcoPortefolioServer.Functions.v1.modules.client;
using MarcoPortefolioServer.Functions.v1.modules.server;
using MarcoPortefolioServer.Functions.v1.modules.server.Discord;
using MarcoPortefolioServer.Repository.v1;
using MarcoPortefolioServer.Repository.v1.ProjectRepository;
using SQLitePCL;
using Client = MarcoPortefolioServer.Functions.v1.modules.client.Client;

Console.WriteLine("Marco Portefolio Server");
Console.WriteLine("Server Starting...");

var builder = WebApplication.CreateBuilder(args);

// =========================================
// LOGGING
// =========================================
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// =========================================
// SQLITE INIT
// =========================================
Batteries.Init();

// =========================================
// SERVICES
// =========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Singleton services
builder.Services.AddSingleton<TokenValidator>();
builder.Services.AddSingleton<Server>();
builder.Services.AddSingleton<Client>();
builder.Services.AddSingleton<VersionRepository>();

// DiscordBot como Singleton
builder.Services.AddSingleton<DiscordBot>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var token = config["Discord:Token"];

    if (string.IsNullOrEmpty(token))
        Console.WriteLine("⚠ Discord Token está vazio! Adiciona em appsettings.json ou variável de ambiente.");

    return new DiscordBot(token!);
});

// =========================================
// CORS
// =========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// =========================================
// SWAGGER
// =========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// =========================================
// PIPELINE
// =========================================
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// =========================================
// INITIALIZATION
// =========================================
DbInitializer.Initialize();
IdeRepository.initializeProjects();

// =========================================
// DISCORD BOT START
// =========================================
var discordBot = app.Services.GetRequiredService<DiscordBot>();
_ = discordBot.StartAsync(); // inicia sem bloquear

Console.WriteLine("🤖 Discord Bot ligado em background...");

// =========================================
// RUN
// =========================================
app.Run();