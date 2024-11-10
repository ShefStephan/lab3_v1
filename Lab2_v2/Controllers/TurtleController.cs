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
        CommandManager dbManager,
        CommandInvoker invoker,
        DataBaseWriter dbWriter,
        Notificator dbNotificator,
        Turtle turtle,
        DataBaseReader dbReader,
        NewFigureChecker dbChecker
        )
    {
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
        try
        {
            if (string.IsNullOrEmpty(commandRequest.Parameter))
            {
                ICommandsWithoutArgs command = (ICommandsWithoutArgs)dbManager.DefineCommand(commandRequest.Command);
                invoker.Invoke(command);
            }
            else
            {
                ICommandsWithArgs command = (ICommandsWithArgs)dbManager.DefineCommand(commandRequest.Command);
                invoker.Invoke(command, commandRequest.Parameter);
            }

            await dbWriter.SaveCommand(commandRequest.Command + " " + commandRequest.Parameter);
            await dbWriter.SaveTurtleStatus(turtle);
            await dbChecker.Check();

            return Ok(dbNotificator.
                GetNotification(commandRequest.Command));
        }

        catch (InvalidCastException ex)
        {
            return BadRequest("Invalid argument");
        }

        catch (IndexOutOfRangeException ex)
        {
            return BadRequest("Invalid argument, or argument doesn`t exist");
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound("Invalid command, or command doesn`t exist");
        }

        catch (FormatException ex)
        {
            return BadRequest("Invalid argument, please try again or check command list");
        }

        catch (NullReferenceException ex)
        {
            return StatusCode(500, ex.Message);
        }
        
    }




    [HttpGet]
    public IActionResult GetStatus()
    {
        return Ok("статус");
    }
    
    
    
    
}