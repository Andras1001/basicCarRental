using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly CarDbContext _context;
        public CarController(CarDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return await _context.Cars.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        [HttpPost]
        public async Task<ActionResult<Car>> AddCar([FromBody] Car car)
        {
            if (car == null)
            {
                return BadRequest("Invalid car data");
            }

            try
            {
                _context.Cars.Add(car);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
            }
            catch (Exception ex)
            {
                // Log the error details including inner exception
                Console.WriteLine($"Error adding car: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                return StatusCode(500, "Error adding car. Please check the logs for details.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCar([FromBody] Car car)
        {            

            var existingCar = await _context.Cars.FindAsync(car.Id);
            if (existingCar == null)
            {
                return NotFound("Car not found");
            }

            try
            {
                existingCar.Year = car.Year;
                existingCar.Make = car.Make;
                existingCar.Model = car.Model;
                existingCar.Mileage = car.Mileage;
                existingCar.IsRented = car.IsRented;

                _context.Cars.Update(existingCar);
                await _context.SaveChangesAsync();

                return NoContent(); // Indicates successful update
            }
            catch (Exception ex)
            {
                // Log the error details including inner exception
                Console.WriteLine($"Error updating car: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                return StatusCode(500, "Error updating car. Please check the logs for details.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound("Car not found");
            }

            try
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
                return NoContent(); // Indicates successful deletion
            }
            catch (Exception ex)
            {
                // Log the error details including inner exception
                Console.WriteLine($"Error deleting car: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                return StatusCode(500, "Error deleting car. Please check the logs for details.");
            }
        }
    }
}
