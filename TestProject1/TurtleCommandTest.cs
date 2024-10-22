using Lab1_v2.Commands;
using Lab1_v2.Storage;
using Lab1_v2.TurtleObject;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;
using Moq;


namespace TestProject1
{
    public class TurtleCommandTest
    {

        //�������� ������
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
            // �������������
            Random rnd = new Random();
            Turtle turtle = new Turtle();
            MoveCommand moveCommand = new MoveCommand();

            //������
            string command = rnd.Next(1000).ToString();
            double expectedY = double.Parse(command);

            //��������
            moveCommand.Execute(turtle, command);

            //��������
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
            //�������� ������
            Turtle turtle = new Turtle();
            AngleCommand angleCommand = new AngleCommand();
            string command = str;
            double expected = exp;


            //��������
            angleCommand.Execute(turtle, command);
            double actual = turtle.GetAngle();

            //��������
            Assert.Equal(expected, actual);

        }

        [Fact]
        public void TestPenUpCommandResult()
        {
            //�������� ������
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
            //�������� ������
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
            //�������� ������
            Turtle turtle = new Turtle();
            SetColorCommand setColorCommand = new SetColorCommand();
            string command = "red";
            string expected = "red";


            //��������
            setColorCommand.Execute(turtle, command);
            string actual = turtle.GetColor();

            //��������
            Assert.Equal(expected, actual);

        }

        [Fact]
        public void TestSetWidthCommandWithRandomData()
        {
            //�������� ������
            Random rnd = new Random();
            Turtle turtle = new Turtle();
            SetWidthCommand setWidthCommand = new SetWidthCommand();
            string command = rnd.Next(1000).ToString();
            double expected = int.Parse(command);


            //��������
            setWidthCommand.Execute(turtle, command);
            double actual = turtle.GetWidth();

            //��������
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
                var mockStorageWriter = new Mock<IStorageWriter>();

                // ������ �������� ������
                var savedCommands = new List<string>();

                // ���
                mockStorageWriter.Setup(writer => writer.SaveCommandAsync(It.IsAny<string>()))
                    .Callback<string>(command => savedCommands.Add(command));

                // ������ ������
                foreach (string command in commands)
                {
                    mockStorageWriter.Object.SaveCommandAsync(command);
                }

                // ���������
                Assert.Equal(commands, savedCommands);
            }
        
        
        
        [Fact]
        public async Task TestNewFigureCheckerExpectedTriangleWithMoq()
        {
            // ������� ��� ��� ���������� IStorageWriter
            var mockStorageWriter = new Mock<IStorageWriter>();

            // ������ ��� �������� ����������� ����� (������ �����)
            var savedFigures = new List<string>();

            // ����������� ��� ��� ������ ����� � ������
            mockStorageWriter.Setup(writer => writer.SaveCommandAsync(It.IsAny<string>()))
                .Callback<string>(figure => savedFigures.Add(figure));

            // ������� ����������� �������
            var turtle = new Turtle();
            var moveCommand = new MoveCommand();
            var angleCommand = new AngleCommand();
            var checker = new NewFigureChecker(turtle, mockStorageWriter.Object);

            // ��������� ������ - �����������
            var expectedFigure = "�����������";

            // ��������� ������� ��� �������� ������������
            for (int i = 1; i <= 3; i++)
            {
                moveCommand.Execute(turtle, "10"); // �������� �� 10 ������
                angleCommand.Execute(turtle, "120"); // ������� �� 120 ��������
                await checker.Check(); // ���������, ������������ �� ������
            }

            // ���������, ��� ����������� ������ � �����������
            Assert.Contains(expectedFigure, savedFigures[0].Split(' ')[0]);
        }

        [Fact]
        public async Task TestNewFigureCheckerExpectedSquareWithMoq()
        {
            // ������� ��� ��� ���������� IStorageWriter
            var mockStorageWriter = new Mock<IStorageWriter>();

            // ������ ��� �������� ����������� ����� (������ �����)
            var savedFigures = new List<string>();

            // ����������� ��� ��� ������ ����� � ������
            mockStorageWriter.Setup(writer => writer.SaveCommandAsync(It.IsAny<string>()))
                .Callback<string>(figure => savedFigures.Add(figure));

            // ������� ����������� �������
            var turtle = new Turtle();
            var moveCommand = new MoveCommand();
            var angleCommand = new AngleCommand();
            var checker = new NewFigureChecker(turtle, mockStorageWriter.Object);

            // ��������� ������ - �����������
            var expectedFigure = "�������";

            // ��������� ������� ��� �������� ������������
            for (int i = 1; i <= 4; i++)
            {
                moveCommand.Execute(turtle, "10"); // �������� �� 10 ������
                angleCommand.Execute(turtle, "90"); // ������� �� 120 ��������
                await checker.Check(); // ���������, ������������ �� ������
            }

            // ���������, ��� ����������� ������ � �����������
            Assert.Contains(expectedFigure, savedFigures[0].Split(' ')[0]);
        }

        [Fact]
        public async Task TestNewFigureCheckerExpectedPentagonWithMoq()
        {
            // ������� ��� ��� ���������� IStorageWriter
            var mockStorageWriter = new Mock<IStorageWriter>();

            // ������ ��� �������� ����������� ����� (������ �����)
            var savedFigures = new List<string>();

            // ����������� ��� ��� ������ ����� � ������
            mockStorageWriter.Setup(writer => writer.SaveCommandAsync(It.IsAny<string>()))
                .Callback<string>(figure => savedFigures.Add(figure));

            // ������� ����������� �������
            var turtle = new Turtle();
            var moveCommand = new MoveCommand();
            var angleCommand = new AngleCommand();
            var checker = new NewFigureChecker(turtle, mockStorageWriter.Object);

            // ��������� ������ - �����������
            var expectedFigure = "������������";

            // ��������� ������� ��� �������� ������������
            for (int i = 1; i <= 5; i++)
            {
                moveCommand.Execute(turtle, "10"); // �������� �� 10 ������
                angleCommand.Execute(turtle, "72"); // ������� �� 120 ��������
                await checker.Check(); // ���������, ������������ �� ������
            }

            // ���������, ��� ����������� ������ � �����������
            Assert.Contains(expectedFigure, savedFigures[0].Split(' ')[0]);
        }



        [Fact]
        public async Task TestFigureCoords()
        {
            // ������� ��� ��� ���������� IStorageWriter
            var mockStorageWriter = new Mock<IStorageWriter>();

            // ������ ��� �������� ����������� ������ (������ �����)
            var savedFigures = new List<string>();

            // ����������� ��� ��� ������ ������ � ������
            mockStorageWriter.Setup(writer => writer.SaveCommandAsync(It.IsAny<string>()))
                .Callback<string>(figure => savedFigures.Add(figure));

            // ������� �������� ������� Turtle, MoveCommand � AngleCommand
            var turtle = new Turtle();
            var moveCommand = new MoveCommand();
            var angleCommand = new AngleCommand();

            // ������� ������ NewFigureChecker, ��������� ��������� Turtle � ��� IStorageWriter
            var checker = new NewFigureChecker(turtle, mockStorageWriter.Object);

            // ��������� �������� ���������
            var expectedCoords = "{(0;0)(0;10)(8,66;5)(0;-0)}";

            // ��������� ������� ��� ����������� ��������� � �������� ���������
            moveCommand.Execute(turtle, "10");
            await checker.Check();
            angleCommand.Execute(turtle, "120");
            await checker.Check();
            moveCommand.Execute(turtle, "10");
            await checker.Check();
            angleCommand.Execute(turtle, "120");
            await checker.Check();
            moveCommand.Execute(turtle, "10");
            await checker.Check();

            // ���������, ��� ����������� ������ ������������� ��������� �����������
            Assert.Contains(expectedCoords, savedFigures[0].Split(' ')[1]);
        }


    }
}