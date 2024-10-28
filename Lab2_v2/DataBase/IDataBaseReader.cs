


using Lab1_v2.DataBase;

public interface IDataBaseReader
{
    public TurtleStatus? GetTurtleStatus();
    
    public TurtleCoords? GetTurtleCoords();
    
    public Task<List<CommandHistory>> GetCommands();
    
    public Task<List<Figure>> GetFigures();
}

