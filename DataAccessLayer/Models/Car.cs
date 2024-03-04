namespace DataAccessLayer.Models
{
    public class Car
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set;}
        public int Mileage { get; set; }
        public  bool IsRented { get; set; }
    }
}
