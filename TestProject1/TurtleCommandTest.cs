using Lab1_v2.Commands;
using Lab1_v2.Storage;
using Lab1_v2.TurtleObject;
using System.ComponentModel.Design;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using Lab1_v2.DataBase;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using Moq;
using Lab1_v2.Storage;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;


namespace TestProject1
{
    public class TurtleCommandTest
    {

        //
        [Theory]
        [InlineData("-5", 0, -5)]
        [InlineData("0", 0, 0)]
        [InlineData("10", 0, 10)]
        public void TestMoveCommandWithExtremePointsData(string str, double expX, double expY)
        {

            Turtle turtle = new Turtle();
            MoveCommand moveCommand = new MoveCommand();

            string command = str;
            double expectedX = expX;
            double expectedY = expY;

            //��������
            moveCommand.Execute(turtle, command);
            double actualX = turtle.GetCoordX();
            double actualY = turtle.GetCoordY();

            //��������
            Assert.Equal(expectedX, actualX);
            Assert.Equal(expectedY, actualY);

        }

        [Fact]
        public void TestMoveCommandWithRandomData()
        {
            Random rnd = new Random();
            Turtle turtle = new Turtle();
            MoveCommand moveCommand = new MoveCommand();
            
            string command = rnd.Next(1000).ToString();
            double expectedY = double.Parse(command);
            
            moveCommand.Execute(turtle, command);
            
            Assert.Equal(0, turtle.GetCoordX());
            Assert.Equal(expectedY, turtle.GetCoordY());

        }


        [Theory]
        [InlineData("120", 120)]
        [InlineData("400", 40)]
        [InlineData("-120", -120)]
        [InlineData("0", 0)]

        public void TestAngleCommandWithExtremePointsData(string str, double exp)
        {
            Turtle turtle = new Turtle();
            AngleCommand angleCommand = new AngleCommand();
            string command = str;
            double expected = exp;

            
            angleCommand.Execute(turtle, command);
            double actual = turtle.GetAngle();
            
            Assert.Equal(expected, actual);

        }

        [Fact]
        public void TestPenUpCommandResult()
        {
            Turtle turtle = new Turtle();
            PenUpCommand penUpCommand = new PenUpCommand();
            string expected = "penUp";

            penUpCommand.Execute(turtle);
            string actual = turtle.GetPenCondition();

            Assert.Equal(expected, actual);

        }

        [Fact]
        public void TestPenDownCommandResult()
        {
            Turtle turtle = new Turtle();
            PenDownCommand penDownCommand = new PenDownCommand();
            string expected = "penDown";

            penDownCommand.Execute(turtle);
            string actual = turtle.GetPenCondition();

            Assert.Equal(expected, actual);

        }

        [Fact]
        public void TestSetColorCommandResult()
        {
            Turtle turtle = new Turtle();
            SetColorCommand setColorCommand = new SetColorCommand();
            string command = "red";
            string expected = "red";

            
            setColorCommand.Execute(turtle, command);
            string actual = turtle.GetColor();
            
            Assert.Equal(expected, actual);

        }

        [Fact]
        public void TestSetWidthCommandWithRandomData()
        {
            Random rnd = new Random();
            Turtle turtle = new Turtle();
            SetWidthCommand setWidthCommand = new SetWidthCommand();
            string command = rnd.Next(1000).ToString();
            double expected = int.Parse(command);
            
            setWidthCommand.Execute(turtle, command);
            double actual = turtle.GetWidth();
            
            Assert.Equal(expected, actual);

        }
        
        
        
        
            [Theory]
            [InlineData("move 5", "angle 90", "penup")]
            [InlineData("pendown", "penup", "move 5")]
            [InlineData("angle 78", "history", "width 10")]
            [InlineData("pendown", "color red", "move -5")]
            [InlineData("move 33", "penup", "history")]
            public void TestHistoryCommandWithInlineDataWithMoq(params string[] commands)
            {
                var mockStorageWriter = new Mock<IDataBaseWriter>();
                
                var savedCommands = new List<string>();
                
                mockStorageWriter.Setup(writer => writer.SaveCommand(It.IsAny<string>()))
                    .Callback<string>(command => savedCommands.Add(command));
                
                foreach (string command in commands)
                {
                    mockStorageWriter.Object.SaveCommand(command);
                }
                
                Assert.Equal(commands, savedCommands);
            }
            

    }
}