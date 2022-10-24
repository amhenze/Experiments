using Microsoft.AspNetCore.Mvc;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Models;

namespace WebApplication2._0.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenerateController : Controller
    {
        private readonly IGenerateManager _generateManager;
        public GenerateController(IGenerateManager generateManager)
        {
            _generateManager = generateManager;
        }

        [HttpPost("~/CollectionGenerate")]
        public ViewResult CollectionGenerate([FromForm] CollectionModel model)
        {
            _generateManager.GenerateCollection(model);
            return View(model);
        }
        [HttpPost("~/RecordsGenerate")]
        public ViewResult RecordsGenerate(CollectionModel model)
        {
            _generateManager.GenerateRecords(model);
            return View();
        }
    }
}
