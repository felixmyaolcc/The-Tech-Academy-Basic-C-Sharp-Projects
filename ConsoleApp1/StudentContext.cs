using System.Data.Entity;

namespace ConsoleApp1
{ 

    public class StudentContext : DbContext
    {

        public StudentContext() : base("name=StudentDBConnection") { }
        public DbSet<Student> Students { get; set; }
    
    }

  
}

