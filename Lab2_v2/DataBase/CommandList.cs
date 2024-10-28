using System.ComponentModel.DataAnnotations.Schema;
using Lab1_v2.CommandsInterface;

namespace Lab1_v2.DataBase;

public class CommandList
{
    public int Id { get; set; }
    public string? Name { get; set; }
    
    [NotMapped]
    public ICommands Command { get; set; }
    
}