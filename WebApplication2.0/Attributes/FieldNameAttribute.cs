namespace WebApplication2._0.Attributes
{
    public class FieldNameAttribute : Attribute
    {
        public string FieldName { get; set; }
        public FieldNameAttribute(string fieldName)
        {
            this.FieldName = fieldName;
        }
    }
}
