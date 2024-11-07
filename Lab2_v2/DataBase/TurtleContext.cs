using Lab1_v2.TurtleObject;

namespace Lab1_v2.DataBase;
using Microsoft.EntityFrameworkCore;

public class TurtleContext: DbContext
{
    public DbSet<CommandList> CommandLists { get; set; } = null!;
    public DbSet<TurtleStatus> TurtleStatus { get; set; } = null!;
    public DbSet<TurtleCoords> TurtleCoords { get; set; } = null!;
    public DbSet<CommandHistory> CommandHistory {get;set; } = null!;
    public DbSet<Figure> Figure {get;set; } = null!;
    
    public TurtleContext(DbContextOptions<TurtleContext> options) : base(options) { }
    public TurtleContext() {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=my.db"); //move connection string to appsetting.json
    }
    
    
    
    public void InitializeDatabase()
    {
        using (var context = new TurtleContext())
        {
            // Проверка есть ли записи в таблице TurtleStatus
            if (!context.TurtleStatus.Any())
            {
                var initialStatus = new TurtleStatus
                {
                    Xcoors = 0,           // начальная координата X
                    Ycoors = 0,           // начальная координата Y
                    PenCondition = "down",// начальное состояние пера
                    Angle = 0,            // начальный угол поворота
                    Color = "black",      // начальный цвет
                    Width = 1             // начальная ширина пера
                };

                context.TurtleStatus.Add(initialStatus);
            }

            // Проверка есть ли записи в таблице TurtleCoords
            if (!context.TurtleCoords.Any())
            {
                var initialCoords = new TurtleCoords
                {
                    xCoord = 0,           // начальная координата X
                    yCoord = 0            // начальная координата Y
                };

                context.TurtleCoords.Add(initialCoords);
            }

            context.SaveChanges();
        }
    }

}