using Lab1_v2.Storage;
using Lab1_v2.TurtleObject;

namespace Lab1_v2.DataBase;

public class DataBaseWriter: IDataBaseWriter
{
    // сохранение статуса черепашки в таблицу "TurtleStatus"
    public async Task SaveTurtleStatus(Turtle turtle)
    {
        using (var context = new TurtleContext())
        {
            var turtleStatus = new TurtleStatus
            {
                Xcoors = turtle.GetCoordX(),
                Ycoors = turtle.GetCoordY(),
                Angle = turtle.GetAngle(),
                Color = turtle.GetColor(),
                PenCondition = turtle.GetPenCondition(),
                Width = turtle.GetWidth()
            };
            
            context.TurtleStatus.Add(turtleStatus);
            await context.SaveChangesAsync();
        }
    }

    public async Task SaveTurtleCoords(Turtle turtle)
    {
        using (var context = new TurtleContext())
        {
            var turtleCoords = new TurtleCoords
            {
                xCoord = turtle.GetCoordX(),
                yCoord = turtle.GetCoordY()
            };
            
            context.TurtleCoords.Add(turtleCoords);
            await context.SaveChangesAsync();
        }
    }
    
    // сохранение команды в таблицу "Command"
    public async Task SaveCommand(string commandText)
    {
        using (var context = new TurtleContext())  
        {
            var command = new CommandHistory
            {
                CommandText = commandText,
            };

            context.CommandHistory.Add(command);
            await context.SaveChangesAsync();  // Сохраняем изменения в базу данных
        }
    }
    
    // сохранение фигуры в таблицу "Figure"
    public async Task SaveFigure(string figureType, string parameters)
    {
        using (var context = new TurtleContext())
        {
            var figure = new Figure
            {
                FigureType = figureType,
                Parameters = parameters,
            };

            context.Figure.Add(figure);
            await context.SaveChangesAsync(); // Сохраняем изменения в базу данных
        }
    }
}