using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class MyDbContext : DbContext {

        public DbSet<Car> Cars { get; set; }
        public DbSet<Engine> Engines { get; set; }
    }
}
