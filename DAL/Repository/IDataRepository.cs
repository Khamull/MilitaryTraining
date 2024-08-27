using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IDataRepository
    {
        DbSet<Unit> Units { get; }
        DbSet<Movement> Movements { get; }
        DbSet<TrainingSession> TrainingSessions { get; }
        int SaveChanges();
        void SaveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    }
}
