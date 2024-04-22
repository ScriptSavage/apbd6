using Apka2.DTOs;
using Apka2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Apka2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AnimalsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult GetAnimals([FromQuery] string orderBy = "Name")
    {
        orderBy = new List<string> { "Name", "Description", "Category", "Area" }.Contains(orderBy) ? orderBy : "Name";
        List<Animal> animals = new List<Animal>();
        using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();
            var query = $"SELECT * FROM Animal ORDER BY {orderBy}";
            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        animals.Add(new Animal()
                        {
                            IdAnimal = (int)reader["IdAnimal"],
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Category = reader["Category"].ToString(),
                            Area = reader["Area"].ToString()
                        });
                    }
                }
            }
        }
        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal([FromBody] AddAnimal addAnimal)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();
            var query = "INSERT INTO Animal (Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", addAnimal.Name);
                command.Parameters.AddWithValue("@Description", addAnimal.Description);
                command.Parameters.AddWithValue("@Category", addAnimal.Category);
                command.Parameters.AddWithValue("@Area", addAnimal.Area);
                command.ExecuteNonQuery();
            }
        }
        return CreatedAtAction(nameof(GetAnimals), new { addAnimal.Name }, addAnimal);
    }

    [HttpPut("{idAnimal}")]
    public IActionResult UpdateAnimal(int idAnimal, [FromBody] AddAnimal updateAnimal)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();
            var query = "UPDATE Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", updateAnimal.Name);
                command.Parameters.AddWithValue("@Description", updateAnimal.Description);
                command.Parameters.AddWithValue("@Category", updateAnimal.Category);
                command.Parameters.AddWithValue("@Area", updateAnimal.Area);
                command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                command.ExecuteNonQuery();
            }
        }
        return NoContent();
    }

    [HttpDelete("{idAnimal}")]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();
            var query = "DELETE FROM Animal WHERE IdAnimal = @IdAnimal";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                command.ExecuteNonQuery();
            }
        }
        return NoContent();
    }
}
