using Lab1_v2.CommandsInterface;
using Lab1_v2.CommandsOperation;
using Lab1_v2.ScreenNotificator;
using Lab1_v2.Storage;
using Lab1_v2.TurtleObject;
using Lab1_v2.DataBase;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lab1_v2;

internal class Program
{
    // файлы для записи команд черепашки
    private const string Exit = "exit";

    private static async Task Main(string[] args)
    {
        //инициализация вспомогательных объектов
        var turtle = new Turtle();
        var reader = new CommandReader();
        var invoker = new CommandInvoker(turtle);
        
        var dbReader = new DataBaseReader();
        var dbWriter = new DataBaseWriter();
        var dbManager = new CommandManager(dbReader);
        var dbNotificator = new Notificator(dbReader);
        var dbChecker = new NewFigureChecker(turtle, dbWriter, dbReader);
        
        // пересоздание базы данных
        await using (var context = new TurtleContext())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.InitializeDatabase();
        }
        
        var builder = WebApplication.CreateBuilder(args);
        
        // Добавление сервисов для API
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // Добавление контекста базы данных
        builder.Services.AddDbContext<TurtleContext>(options =>
            options.UseSqlite("Data Source=my.db"));

        var app = builder.Build();

        // Пересоздание базы данных при старте API
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TurtleContext>();
            // Удаляет и создает базу данных заново каждый раз при запуске
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
        
        // Настройка Swagger (доступно только в режиме разработки)
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Стандартные middleware для API
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        // Запуск API
        app.Run();
        
        
        // список команда без аргументов и с аргументами
        var commWithoutArgsList = new List<string>() { "penup", "pendown", "history", "listfigures" };

        // текст введенной пользователем команды
        string userCommand;

        Console.WriteLine("-------Welcome to the TURTLEGAME-------");
        Console.WriteLine();
        Console.WriteLine("Command list: \n" +
                          "- move [number]\n" +
                          "- angle [number]\n" +
                          "- penup\n" +
                          "- pendown\n" +
                          "- history\n" +
                          "- listfigures\n" +
                          "- color [string]\n" +
                          "- width [number]");
        Console.WriteLine();
        Console.WriteLine("Choose the command from list to START the game");
        Console.WriteLine("To leave the game, enter - exit");


        while (true)
        {
            try
            {
                userCommand = reader.Read();

                if (userCommand == Exit)
                {
                    break;
                }

                if (commWithoutArgsList.Contains(userCommand))
                {
                    ICommandsWithoutArgs command = (ICommandsWithoutArgs)dbManager.DefineCommand(userCommand);
                    invoker.Invoke(command);
                    await dbWriter.SaveCommand(userCommand);
                }
                else
                {
                    ICommandsWithArgs command = (ICommandsWithArgs)dbManager.DefineCommand(userCommand.Split(' ')[0]);
                    invoker.Invoke(command, userCommand.Split(' ')[1]);
                    await dbWriter.SaveCommand(userCommand);
                }

                await dbWriter.SaveTurtleStatus(turtle);
                // вывод соообщение после испольнения команды
                await dbNotificator.SendNotification(userCommand);

                // проверка на образование новой фигуры
                await dbChecker.Check();
            }

            // возможные ошибки в ходе выполнения
            catch (InvalidCastException ex)
            {
                Console.WriteLine("Invalid argument");
            }

            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Invalid argument, or argument doesn`t exist");
            }

            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("Invalid command, or command doesn`t exist");
            }

            catch (FormatException ex)
            {
                Console.WriteLine("Invalid argument, please try again or check command list");
            }
            
            catch (NullReferenceException ex)
            {
                Console.WriteLine("empty...");
            }
        }

        Console.WriteLine("GAME END");

        ;
    }
}