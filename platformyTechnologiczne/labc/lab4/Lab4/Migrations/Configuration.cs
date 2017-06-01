namespace Lab2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Lab2.MyDbContext>
    {

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Lab2.MyDbContext context)
        {
            context.Cars.AddOrUpdate(
                new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
                new Car("E350", new Engine(3.5, 292, "CGI"), 2009), 
                new Car("A6", new Engine(2.5, 187, "FSI"), 2012), 
                new Car("A6", new Engine(2.8, 220, "FSI"), 2012), 
                new Car("A6", new Engine(3.0, 295, "TFSI"), 2012), 
                new Car("A6", new Engine(2.0, 175, "TDI"), 2011), 
                new Car("A6", new Engine(3.0, 309, "TDI"), 2011), 
                new Car("S6", new Engine(4.0, 414, "TFSI"), 2012), 
                new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
                );
        }
    }
}
