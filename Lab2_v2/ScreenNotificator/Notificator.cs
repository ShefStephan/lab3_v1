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
        
        private DataBaseReader dbReader;
        private TurtleStatus? turtleStatus;
        public Notificator(DataBaseReader reader)
        {
            dbReader = reader;
        }


        public async Task SendNotification(string command)
        {
            if (command == "history")
            {
                var commands = dbReader.GetCommands();
                foreach (var comm in commands)
                {
                    Console.WriteLine("· " + comm.CommandText);
                }
               
            }

            else if (command == "listfigures")
            {
                var figures = dbReader.GetFigures();
                foreach (var figure in figures)
                {
                    Console.WriteLine("· " + figure.FigureType + " " + figure.Parameters);
                }
            }

            else
            {
                var turtleStatus = dbReader.GetTurtleStatus();
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
    }
}
