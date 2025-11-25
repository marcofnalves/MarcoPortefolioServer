global using System;
global using System.Collections.Generic;
global using static MarcoPortefolioServer.Functions.v1.framework.ModuleExecutor;
global using static MarcoPortefolioServer.Functions.v1.lib.client.ClientInternalEvents;  // Fazendo os métodos do ServerInternalEvents globais
global using static MarcoPortefolioServer.Functions.v1.lib.server.ServerInternalEvents;  // Fazendo os métodos do ClientInternalEvents globais
using MarcoPortefolioServer.Functions.v1.Discord.server;
using MarcoPortefolioServer.Functions.v1.framework;
using MarcoPortefolioServer.Functions.v1.framework.server;
using MarcoPortefolioServer.Functions.v1.lib.server;
using MarcoPortefolioServer.Repository.v1;
using MarcoPortefolioServer.Repository.v1.ProjectRepository;
using SQLitePCL;
using Client = MarcoPortefolioServer.Functions.v1.lib.client.Client;
using Server = MarcoPortefolioServer.Functions.v1.lib.server.Server;

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
builder.Services.AddSingleton<DiscordBot>(static sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    string SystemType = config["System:type"] ?? "DEV";
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine(SystemType.ToString());
    Console.ResetColor();
    var token = config[$"Discord:{SystemType}:Token"];

    _ = new WhitelistServer();

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
ModuleExecutor.MarkServerAsStarted();

app.Run();