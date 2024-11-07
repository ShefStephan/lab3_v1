using Lab1_v2.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab1_v2.Controllers;


[Route("api/[controller]")]
[ApiController]
public class TurtleController: ControllerBase
{
    private readonly TurtleContext context;

    public TurtleController(TurtleContext context)
    {
        this.context = context;
    }
    
    [HttpGet("/")]
    public IActionResult Home()
    {
        return Ok("Welcome to the Turtle API");
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TurtleStatus>>> GetStatus()
    {
        return await context.TurtleStatus.ToListAsync();
    }
    
    [HttpPost]
    public async Task<ActionResult<TurtleStatus>> PostTodoItem(TurtleStatus todoItem)
    {
        context.TurtleStatus.Add(todoItem);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStatus), new { id = todoItem.Id }, todoItem);
    }
    
}