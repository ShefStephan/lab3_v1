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


        public void SendNotification(string command, Turtle turtle)
        {
            if (command == "history")
            {
                foreach (var comm in dbReader.GetCommands())
                {
                    Console.WriteLine("· " + comm.CommandText);
                }
               
            }

            else if (command == "listfigures")
            {
                foreach (var figure in dbReader.GetFigures())
                {
                    Console.WriteLine("· " + figure.FigureType + " " + figure.Parameters);
                }
            }

            else
            {
                turtleStatus = dbReader.GetTurtleStatus();
                
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
