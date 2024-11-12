using Lab1_v2.CommandsInterface;
using Lab1_v2.CommandsOperation;
using Lab1_v2.Controllers;
using Lab1_v2.ScreenNotificator;
using Lab1_v2.Storage;
using Lab1_v2.TurtleObject;
using Lab1_v2.DataBase;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lab1_v2;

public class Program
{
    // файлы для записи команд черепашки
    private const string Exit = "exit";

    private static async Task Main(string[] args)
    {
        
        var builder = WebApplication.CreateBuilder(args);
        builder.Environment.EnvironmentName = Environments.Development;
        
        // Регистрация сервисов в DI контейнере
        builder.Services.AddDbContext<TurtleContext>(options =>
            options.UseSqlite("Data Source=my.db"));
        builder.Services.AddSingleton<CommandManager>();
        builder.Services.AddSingleton<CommandInvoker>();
        builder.Services.AddSingleton<IDataBaseWriter, DataBaseWriter>();
        builder.Services.AddSingleton<Notificator>();
        builder.Services.AddSingleton<Turtle>();
        builder.Services.AddSingleton<IDataBaseReader, DataBaseReader>();
        builder.Services.AddSingleton<NewFigureChecker>();
        
        // Добавление сервисов для API
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        var app = builder.Build();
        
        // чтобы сразу открывался swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = ""; 
            });
        }
        app.UseAuthorization();
        app.MapControllers();
        app.Run();

    }
}