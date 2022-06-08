using EVT_Server.Models;
using EVT_Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace EVT_Server.Controllers{

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    public PizzaController()
    {
        
    }

    [HttpGet]
    public ActionResult<List<Pizza>> GetAll()
    {
        return PizzaService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Pizza> Get(int id)
    {
        var pizza = PizzaService.Get(id);
        if (pizza == null)
        {
            return NotFound();
        }

        return pizza;
    }
}}