namespace Lab1_v2.DataBase;

public interface IDataBaseWriter
{
    Task SaveCommand(string commandText);
    
    Task SaveFigure(string figureType, string parameters);
    
}