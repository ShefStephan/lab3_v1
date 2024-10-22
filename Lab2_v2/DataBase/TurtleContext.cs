namespace Lab1_v2.DataBase;
using Microsoft.EntityFrameworkCore;

public class TurtleContext: DbContext
{
    
    public DbSet<Command> Comand {get;set; } = null!;
    public DbSet<Figure> Figure {get;set; } = null!;
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=mydb.db");
    }
}