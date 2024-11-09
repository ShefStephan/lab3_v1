namespace Lab1_v2.DataBase;

public class TurtleCommandRequest
{
    public string Command { get; set; } // Название команды, например "move" или "penup"
    public string? Parameter { get; set; } // Параметр команды, например "5" для "move 5", null для команд без параметра
}