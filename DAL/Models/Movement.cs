using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Movement
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }

        public Unit Unit { get; set; }
    }
}
