using System.Data.Entity;

namespace Lab1_v2.DataBase;

public class DataBaseReader
{
    public async Task<List<Command>> GetCommands()
    {
        using (var context = new TurtleContext())  // using гарантирует освобождение
        {
            return await context.Comand.ToListAsync();
        }
    }
    
    public async Task<List<Figure>> GetFigures()
    {
        using (var context = new TurtleContext())
        {
            return await context.Figure.ToListAsync();
        }
    }
    
}