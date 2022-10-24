using Microsoft.AspNetCore.Mvc;
using WebApplication2._0.Entities;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Models;

namespace WebApplication2._0.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecordsController : Controller
    {
        private readonly IRecordManager _recordManager;
        public RecordsController(IRecordManager recordManager)
        {
            _recordManager = recordManager;
        }

        [HttpGet("~/ReadRecords")]
        public async Task<ViewResult> ReadRecords(int collectionId)
        {
            List<RecordModel> dataList = await _recordManager.Read(collectionId);
            return View(dataList);
        }

        [HttpPost("~/CreateRecord")]
        public ViewResult CreateRecord([FromForm] RecordModel model)
        {
            _recordManager.Create(model);
            return View();
        }

        [HttpPut("~/UpdateRecord")]
        public ViewResult UpdateRecord([FromForm] RecordModel model)
        {
            _recordManager.Update(model);
            return View();
        }

        [HttpDelete("~/DeleteRecord")]
        public ViewResult DeleteRecord([FromForm] RecordModel model)
        {
            _recordManager.Delete(model);
            return View();
        }
    }
}
