using System.Data.Entity;

namespace Lab1_v2.DataBase;

public class DataBaseReader
{
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