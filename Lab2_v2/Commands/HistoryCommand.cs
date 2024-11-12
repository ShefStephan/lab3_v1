using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1_v2.CommandsInterface;
using Lab1_v2.DataBase;
using Lab1_v2.Storage;
using Lab1_v2.TurtleObject;

namespace Lab1_v2.Commands
{
    public class HistoryCommand : ICommandsWithoutArgs
    {
        private IDataBaseReader dbReader;
        public HistoryCommand(IDataBaseReader reader)
        {
            dbReader = reader;
        }

        public void Execute(Turtle turtle)
        {
            // Console.WriteLine(dbReader.GetCommands());
        }
    }
}
