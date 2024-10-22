﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1_v2.CommandsInterface;
using Lab1_v2.Storage;
using Lab1_v2.Commands;
using Lab1_v2.DataBase;

namespace Lab1_v2.CommandsOperation
{
    public class CommandManager
    {
        
        private DataBaseReader dbReader;
        
        public CommandManager(DataBaseReader reader)
        {
            dbReader = reader;
            FillDict();

        }

        private readonly Dictionary<string, ICommands> commandsDict = new() { };

        private void FillDict()
        {
            commandsDict.Add("history", new HistoryCommand(dbReader));
            commandsDict.Add("listfigures", new ListFiguresCommand(dbReader));
            commandsDict.Add("move", new MoveCommand());
            commandsDict.Add("penup", new PenUpCommand());
            commandsDict.Add("angle", new AngleCommand());
            commandsDict.Add("pendown", new PenDownCommand());
            commandsDict.Add("color", new SetColorCommand());
            commandsDict.Add("width", new SetWidthCommand());
        }
        
        public ICommands DefineCommand(string command)
        {
            return commandsDict[command];
        }
    }
}