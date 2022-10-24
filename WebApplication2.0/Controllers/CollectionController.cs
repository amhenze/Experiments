using Microsoft.AspNetCore.Mvc;
using WebApplication2._0.Entities;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Models;

namespace WebApplication2._0.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CollectionController : Controller
    {
        private readonly ICollectionManager _collectionManager;
        public CollectionController(ICollectionManager collectionManager)
        {
            _collectionManager = collectionManager;
        }

        [HttpPost("~/CreateCollection")]
        public ViewResult CreateCollection([FromForm] CollectionModel model)
        {
            _collectionManager.Create(model);
            return View(model);
        }

        [HttpGet("~/ReadCollections")]
        public async Task<ViewResult> ReadCollections([FromForm] int id)
        {
            List<CollectionModel> dataList = await _collectionManager.Read(id);
            return View(dataList);
        }


        [HttpPut("~/UpdateCollection")]
        public ViewResult UpdateCollection([FromForm] CollectionModel model)
        {
            _collectionManager.Update(model);
            return View();
        }

        [HttpDelete("~/DeleteCollection")]
        public ViewResult DeleteCollection([FromForm] int id)
        {
            _collectionManager.Delete(id);
            return View();
        }

    }
}
