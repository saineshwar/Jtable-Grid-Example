using System.Data.Entity;

namespace DemojTable.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DBConnection")
        {

        }
        public DbSet<Customers> Customers { get; set; }
    }
}