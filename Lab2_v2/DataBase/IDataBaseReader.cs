


using Lab1_v2.DataBase;

public interface IDataBaseReader
{
    public Task<TurtleStatus?> GetTurtleStatus();
    
    public Task<TurtleCoords?> GetTurtleCoords();
    
    public Task<List<CommandHistory>> GetCommands();
    
    public Task<List<Figure>> GetFigures();
}

