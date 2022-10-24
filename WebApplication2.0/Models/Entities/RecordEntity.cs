using Microsoft.EntityFrameworkCore;
using WebApplication2._0.Attributes;

namespace WebApplication2._0.Entities
{
    public class RecordEntity
    {
        [FieldNameAttribute("record_id")]
        public int RecordId { get; set; }

        [FieldNameAttribute("collection_id")]
        public int CollectionId { get; set; }

        [FieldNameAttribute("number")]
        public int Number { get; set; }

        [FieldNameAttribute("letter")]
        public string Letter { get; set; }
    }
}
