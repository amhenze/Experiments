using Microsoft.EntityFrameworkCore;
using WebApplication2._0.Attributes;

namespace WebApplication2._0.Models
{
    public class RecordModel
    {
        public int RecordId { get; set; }
        public int CollectionId { get; set; }
        public int Number { get; set; }
        public string Letter { get; set; }
    }
}
