using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1_v2.DataBase;
using Lab1_v2.Storage;
using Lab1_v2.TurtleObject;

namespace Lab1_v2.ScreenNotificator
{
    public class Notificator
    {
        
        private IDataBaseReader dbReader;
        private TurtleStatus? turtleStatus;
        public Notificator(IDataBaseReader reader)
        {
            dbReader = reader;
        }


        public async Task SendNotification(string command)
        {
            if (command == "history")
            {
                var commands = await dbReader.GetCommands();
                foreach (var comm in commands)
                {
                    Console.WriteLine("· " + comm.CommandText);
                }
               
            }

            else if (command == "listfigures")
            {
                var figures = await dbReader.GetFigures();
                if (figures.Count == 0)
                {
                    Console.WriteLine("empty...");
                }
                foreach (var figure in figures)
                {
                    Console.WriteLine("· " + figure.FigureType + " " + figure.Parameters);
                }
            }

            else
            {
                var turtleStatus = await dbReader.GetTurtleStatus();
                if (turtleStatus != null)
                {
                    Console.WriteLine("состояние: " +
                                      "pos: (" + Math.Round(turtleStatus.Xcoors, 2) +
                                      "; " + Math.Round(turtleStatus.Ycoors, 2) + ")" +
                                      ", pen: " + turtleStatus.PenCondition +
                                      ", angle: " + turtleStatus.Angle +
                                      ", color: " + turtleStatus.Color +
                                      ", width: " + turtleStatus.Width);
                }

            }
           
        }
        
        public async Task<List<string>> GetNotification(string command)
        {
            List<string> result = new List<string>();
            if (command == "history")
            {
                var commands = await dbReader.GetCommands();
                foreach (var comm in commands)
                {
                    result.Add(comm.CommandText);
                }
            }

            else if (command == "listfigures")
            {
                var figures = await dbReader.GetFigures();
                if (figures.Count == 0)
                {
                    return null;
                }
                foreach (var figure in figures)
                {
                    result.Add(figure.FigureType + " " + figure.Parameters);
                }
            }
            else
            {
                var turtleStatus = await dbReader.GetTurtleStatus();
                if (turtleStatus != null)
                {
                    result.Add(Math.Round(turtleStatus.Xcoors, 2).ToString());
                    result.Add(Math.Round(turtleStatus.Ycoors, 2).ToString());
                    result.Add(turtleStatus.PenCondition);
                    result.Add(turtleStatus.Angle.ToString());
                    result.Add(turtleStatus.Color);
                    result.Add(turtleStatus.Width.ToString());
                }
            }

            return result;
            }
        }
}
