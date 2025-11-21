using Auth0.ManagementApi.Models;
using MarcoPortefolioServer.Functions.v1;
using MarcoPortefolioServer.Functions.v1.modules.client;
using MarcoPortefolioServer.Functions.v1.modules.server;
using MarcoPortefolioServer.Repository.v1;
using MarcoPortefolioServer.Repository.v1.ProjectRepository;
using SQLitePCL;
using Client = MarcoPortefolioServer.Functions.v1.modules.client.Client;

Console.WriteLine("Marco Portefolio Server");
Console.WriteLine("Server Starting...");

var builder = WebApplication.CreateBuilder(args);

// REMOVE logs default
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

Batteries.Init();

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<TokenValidator>();
builder.Services.AddSingleton<Server>();
builder.Services.AddSingleton<Client>();
builder.Services.AddSingleton<VersionRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Initialize system
DbInitializer.Initialize();
IdeRepository.initializeProjects();

// BANNER COLORIDO
app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("=========================================");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("      🚀 Marco Portefolio Server");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("          ✔ Server Started");
    Console.WriteLine("          ✔ Database Loaded");
    Console.WriteLine("          ✔ Ready");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("=========================================");
    Console.ResetColor();
});

app.Run();