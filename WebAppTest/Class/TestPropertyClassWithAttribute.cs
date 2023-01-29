using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2._0.Attributes;

namespace WebAppTest.Class
{
	internal class TestPropertyClassWithAttribute
	{
		[FieldName("record_id")]
		[DisplayName("record_id")]
		public int RecordId { get; set; }

		[FieldName("collection_id")]
		public int CollectionId { get; set; }
		public int Number { get; set; }
		public string Letter { get; set; }
	}
}
