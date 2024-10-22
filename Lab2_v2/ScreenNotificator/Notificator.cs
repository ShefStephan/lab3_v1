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
                Console.WriteLine("состояние: " +
                "pos: (" + Math.Round(turtle.GetCoordX(), 2) +
                "; " + Math.Round(turtle.GetCoordY(), 2) + ")" +
                ", pen: " + turtle.GetPenCondition() +
                ", angle: " + turtle.GetAngle() +
                ", color: " + turtle.GetColor() +
                ", width: " + turtle.GetWidth());
            }
           
        }
    }
}
