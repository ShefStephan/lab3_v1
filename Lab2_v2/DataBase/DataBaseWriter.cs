namespace Lab1_v2.DataBase;

public class DataBaseWriter
{
    
    // сохранение команды в таблицу "Command"
    public async Task SaveCommand(string commandText)
    {
        using (var context = new TurtleContext())  
        {
            var command = new Command
            {
                CommandText = commandText,
            };

            context.Command.Add(command);
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