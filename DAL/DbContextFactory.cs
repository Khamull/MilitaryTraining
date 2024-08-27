using DAL.Context;
using DAL.Models;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class DbContextFactory
    {
        public static DataContext Create()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("MilitaryTrainingDB")
                .Options;

            return new DataContext(options);
        }
    }

    public class InMemoryDataRepository : IDataRepository
    {
        private readonly DataContext _context;

        public InMemoryDataRepository()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("SharedInMemoryDB")
                .Options;

            _context = new DataContext(options);
            _context.Database.EnsureCreated();
        }

        public DbSet<Unit> Units => _context.Unit;
        public DbSet<Movement> Movements => _context.Movements;
        public DbSet<TrainingSession> TrainingSessions => _context.TrainingSessions;
        public int SaveChanges() => _context.SaveChanges();
        public void SaveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _context.Set<TEntity>().AddRange(entities);
            _context.SaveChanges();
        }
    }
}
