using Microsoft.EntityFrameworkCore;
using WebApplication2._0.Attributes;

namespace WebApplication2._0.Entities
{
    public class CollectionEntity
    {
        [FieldNameAttribute("collection_id")]
        public int CollectionId { get; set; }

        [FieldNameAttribute("collection_name")]
        public string CollectionName { get; set; }
    }
}

