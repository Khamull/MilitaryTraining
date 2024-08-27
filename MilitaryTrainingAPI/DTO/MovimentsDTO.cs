using DAL.Models;

namespace MilitaryTrainingAPI.DTO
{
    public class MovimentsDTO
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }
}
