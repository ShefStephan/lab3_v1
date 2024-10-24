using Lab1_v2.TurtleObject;

namespace Lab1_v2.DataBase;
using Microsoft.EntityFrameworkCore;

public class TurtleContext: DbContext
{
    public DbSet<TurtleStatus> TurtleStatus { get; set; } = null!;
    public DbSet<Command> Command {get;set; } = null!;
    public DbSet<Figure> Figure {get;set; } = null!;
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=mydb.db");
    }
}