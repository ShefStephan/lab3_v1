using System.Data.Entity;

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
    public List<Command> GetCommands()
    {
        using (var context = new TurtleContext())  // using гарантирует освобождение
        {
            return context.Command.ToList();
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