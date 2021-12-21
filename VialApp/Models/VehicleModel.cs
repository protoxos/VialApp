using VialApp.Tools;

namespace VialApp.Models
{
    public class VehicleModel : BaseModel
    {
        public int UserCreatorId { get; set; }
        public string Alias { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; }
        public string Vin { get; set; } = "";
        public string Color { get; set; } = "";

    }
}