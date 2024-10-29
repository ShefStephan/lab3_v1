using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1_v2.DataBase;
using Lab1_v2.TurtleObject;
using Microsoft.EntityFrameworkCore;

namespace Lab1_v2.Storage
{
    public class NewFigureChecker
    {
        private Turtle turtle;
        private double lastX;
        private double lastY;
        private string figure;
        private IDataBaseWriter dbWriter;
        private IDataBaseReader dbReader;
        private string param;
        private int rowCount;
        private TurtleStatus firstRow;
        private TurtleStatus lastRow;

        public NewFigureChecker(Turtle turtle, IDataBaseWriter writer, IDataBaseReader reader)
        {
            this.turtle = turtle;
            lastX = 0;
            lastY = 0;
            dbWriter = writer;
            dbReader = reader;
        }

        public async Task Check()
        {
            var turtleStatus = await dbReader.GetTurtleStatus();
            if (turtleStatus?.PenCondition == "penDown")
            {
                var latestStatus = await dbReader.GetTurtleStatus();
                if (latestStatus != null && 
                    (lastX != latestStatus.Xcoors || lastY != latestStatus.Ycoors))
                {
                    await dbWriter.SaveTurtleCoords(turtle);
                    
                    var lastCoords = await dbReader.GetTurtleCoords();
                    if (lastCoords != null)
                    {
                        lastX = lastCoords.xCoord;
                        lastY = lastCoords.yCoord;
                    }

                    using (var context = new TurtleContext())
                    {
                        rowCount = await context.TurtleCoords.CountAsync();
                    }
                    
                    using (var context = new TurtleContext())
                    {
                        // Получаем первую запись в таблице 
                        // IQueryable<TurtleStatus> firstRowIQuer = context.TurtleStatus;
                        firstRow = await context.TurtleStatus.OrderBy(t => t.Id).FirstOrDefaultAsync();
    
                        // Получаем последнюю запись в таблице 
                        // IQueryable<TurtleStatus> lastRowIQuer = context.TurtleStatus;
                        lastRow = await context.TurtleStatus.OrderByDescending(t => t.Id).FirstOrDefaultAsync();
                    }
                    
                    if (rowCount > 2 && 
                        firstRow != null && lastRow != null &&
                        (firstRow.Xcoors == lastRow.Xcoors && firstRow.Ycoors == lastRow.Ycoors))
                    {

                        switch (rowCount-1)
                        {
                            case 3:
                                figure = "треугольник";
                                break;
                            case 4:
                                figure = "квадрат";
                                break;
                            case 5:
                                figure = "пятиугольник";
                                break;
                            case 6:
                                figure = "шестиугольник";
                                break;
                            case 7:
                                figure = "семиугольник";
                                break;

                        }

                        Console.Write("Образована новая фигура: " + figure);
                        Console.WriteLine();

                        param = await CoordArrayToString();
                        if (dbWriter != null)
                        {
                            await dbWriter.SaveFigure(figure, param);
                        }
                        
                        await ClearTurtleCoords();
                    }
                }
            }
            else
            {
                await ClearTurtleCoords();
            }

        }

        private async Task<string> CoordArrayToString()
        {
            using (var context = new TurtleContext())
            {
                var allRows = await context.TurtleCoords.ToListAsync();
                var result = new StringBuilder();

                foreach (var row in allRows)
                {
                    result.Append($"({row.xCoord}; {row.yCoord})");
                }
                
                return result.ToString();;
            }
        }

        private async Task ClearTurtleCoords()
        {
            using (var context = new TurtleContext())
            {
                context.TurtleCoords.RemoveRange(context.TurtleCoords);
                await context.SaveChangesAsync();
            }
        }
    }
}
