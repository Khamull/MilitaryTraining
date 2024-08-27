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
    public class UnitServices
    {
        private readonly IDataRepository _context;

        public UnitServices(IDataRepository context)
        {
            _context = context;
        }

        public List<Unit> GetAllUnits()
        {
            return _context.Units.ToList();
        }

        public List<Unit> GetUnitById(int unitID)
        {
            return _context.Units.Where(x => x.Id == unitID).ToList();
        }

        public void AddUnit(Unit unit)
        {
            _context.Units.Add(unit);
            _context.SaveChanges();
        }
    }
}
