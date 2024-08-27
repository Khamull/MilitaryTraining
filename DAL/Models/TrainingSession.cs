using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class TrainingSession
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Movement> Movements { get; set; } = new List<Movement>();
    }
}
