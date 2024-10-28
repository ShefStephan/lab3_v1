using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Lab1_v2.DataBase;

public class DataBaseReader 
{
    public TurtleStatus? GetTurtleStatus()
    {
        using (var context = new TurtleContext())
        {
            return context.TurtleStatus.OrderByDescending(t => t.Id).FirstOrDefault();
                
        }
    }

    public TurtleCoords? GetTurtleCoords()
    {
        using (var context = new TurtleContext())
        {
            return context.TurtleCoords.OrderByDescending(t => t.Id).FirstOrDefault();
        }
    }
    public List<CommandHistory> GetCommands()
    {
        using (var context = new TurtleContext())  // using гарантирует освобождение
        {
            return context.CommandHistory.ToList();
        }
    }
    
    public List<Figure> GetFigures()
    {
        using (var context = new TurtleContext())
        {
            return context.Figure.ToList();
        }
    }
    
}