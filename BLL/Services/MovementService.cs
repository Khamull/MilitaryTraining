using DAL.Context;
using DAL.Models;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class MovementService
    {
        private readonly IDataRepository _context;

        public MovementService(IDataRepository context)
        {
            _context = context;
        }

        public void AddMovement(Movement movement)
        {
            _context.Movements.Add(movement);
            _context.SaveChanges();
        }

        public void AddMovements(List<Movement> movements)
        {
            _context.Movements.AddRange(movements);
            _context.SaveChanges();
        }

        public List<Movement> GetMovementsByUnitId(int? unitID)
        {
            return _context.Movements
                           .Where(m => m.UnitId == unitID)
                           .ToList();
        }
        public List<Movement> GetAllMovements()
        {
            return _context.Movements.ToList();
        }
    }
}
