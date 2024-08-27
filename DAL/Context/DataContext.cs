using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set;}

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Unit>().HasData
                (
                    new Unit 
                    {
                        Id = 1
                        , UnitName = "John Doe"
                        , Rank = "Sergent"
                        , Country = "USA"
                    }
                    , new Unit
                    {
                        Id = 2
                        ,
                        UnitName = "Jane Smith"
                        ,
                        Rank = "Corporal"
                        ,
                        Country = "UK"
                    }
                    , new Unit
                    {
                        Id = 3
                        ,
                        UnitName = "Jane Smith The Third"
                        ,
                        Rank = "Corporal Level III"
                        ,
                        Country = "UK"
                    }
                );

        }
    }
}
