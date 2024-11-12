using Lab1_v2.TurtleObject;

namespace Lab1_v2.DataBase;

public interface IDataBaseWriter
{
    public Task SaveCommand(string commandText);
    
    public Task SaveFigure(string figureType, string parameters);
    public Task SaveTurtleCoords(Turtle turtle);
    
    public Task SaveTurtleStatus(Turtle turtle);

}