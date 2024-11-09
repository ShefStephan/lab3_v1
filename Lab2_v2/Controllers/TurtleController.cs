using Lab1_v2.CommandsInterface;
using Lab1_v2.CommandsOperation;
using Lab1_v2.DataBase;
using Lab1_v2.ScreenNotificator;
using Lab1_v2.Storage;
using Lab1_v2.TurtleObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Lab1_v2.Controllers;


[Route("api/[controller]")]
[ApiController]
public class TurtleController: ControllerBase
{
    private readonly TurtleContext context;
    private readonly CommandManager dbManager;
    private readonly CommandInvoker invoker;
    private readonly DataBaseWriter dbWriter;
    private readonly Notificator dbNotificator;
    private readonly Turtle turtle;
    private readonly DataBaseReader dbReader;
    private readonly NewFigureChecker dbChecker;

    public TurtleController(
        TurtleContext context,
        CommandManager dbManager,
        CommandInvoker invoker,
        DataBaseWriter dbWriter,
        Notificator dbNotificator,
        Turtle turtle,
        DataBaseReader dbReader,
        NewFigureChecker dbChecker
        )
    {
        this.context = context;
        this.dbManager = dbManager;
        this.invoker = invoker;
        this.dbWriter = dbWriter;
        this.dbNotificator = dbNotificator;
        this.turtle = turtle;
        this.dbReader = dbReader;
        this.dbChecker = dbChecker;
    }

    // POST метод для выполнения команды
    [HttpPost]
    public async Task<IActionResult> ExecuteCommand([FromBody] TurtleCommandRequest commandRequest)
    {
            
            
        // Проверяем, есть ли аргументы для команды
        if (string.IsNullOrEmpty(commandRequest.Parameter))
        {
            ICommandsWithoutArgs command = (ICommandsWithoutArgs)dbManager.DefineCommand(commandRequest.Command);
                
            if (command == null)
            {
                return BadRequest("Invalid command");
            }
            invoker.Invoke(command);
        }
        else
        {
            ICommandsWithArgs command = (ICommandsWithArgs)dbManager.DefineCommand(commandRequest.Command);
                
            if (command == null)
            {
                return BadRequest("Invalid command with argument");
            }
                
            invoker.Invoke(command, commandRequest.Parameter);
        }

        // Сохраняем команду в базе данных
        await dbWriter.SaveCommand(commandRequest.Command);
        await dbWriter.SaveTurtleStatus(turtle);

        // Отправляем уведомление о выполнении команды
        await dbNotificator.SendNotification(commandRequest.Command);

        // Проверка на образование новой фигуры
        await dbChecker.Check();

        // Возвращаем актуальное состояние черепашки в ответе
        return Ok(await dbReader.GetTurtleStatus());
    }
    
    
    [HttpGet]
    public IActionResult GetStatus()
    {
        return Ok("статус");
    }
    
    
    
    
}