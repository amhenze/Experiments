namespace WebApplication2._0.Attributes
{
    public class FieldNameAttribute : Attribute
    {
        public string FieldName { get; set; }
        public string FieldName2 { get; set; }
        public FieldNameAttribute(string fieldName,string value)
        {
            this.FieldName = fieldName;
            this.FieldName2 = value;
        }
        public FieldNameAttribute(string fieldName)
        {
            this.FieldName = fieldName;
            this.FieldName2 = "Text";
        }
    }
}
