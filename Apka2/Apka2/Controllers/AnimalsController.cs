using Apka2.DTOs;
using Apka2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Apka2.Controllers;

[ApiController]
//[Route("api/animals")]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{

    [HttpGet]
    public IActionResult GetAnimals()
    {
        // polaczenie do bazy
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        
        
        
        //Definicja Command

        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT  * FROM Animal";
        
        // Wykonanie zapytania

        var reader = command.ExecuteReader();

        List<Animal> animals = new List<Animal>();

        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");
        
        // odczyt wartosci
        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOrdinal),
                Name = reader.GetString(nameOrdinal)
            });    
        }
        
        return Ok(animals);
    }

    private readonly IConfiguration _configuration;

    public AnimalsController(IConfiguration configuration)
    {
        _configuration = configuration;
        
    }



    [HttpPost]
    public IActionResult AddAnimal(AddAnimal addAnimal)
    {
        // polaczenie do bazy
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        
        
        
        //Definicja Command

        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = $"INSERT INTO Animal VALUES('{addAnimal}','','','')";
        //command.Parameters.AddWithValue(@animalName, addAnimal.Name);

        return Created();
    }


}