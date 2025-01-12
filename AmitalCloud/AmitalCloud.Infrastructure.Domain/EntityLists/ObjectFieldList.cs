namespace AmitalCloud.Infrastructure.Domain.EntityLists
{
    public partial class ObjectFieldList
    {
        public string ShortName { get; set; }
        public string RecordType { get; set; }
        public bool DisplayInAutomationAsEnitity { get; set; }
        public string FieldCode { get; set; }
        public string FullNameTextCodeCode { get; set; }
        public string ShortNameTextCodeCode { get; set; }
        public string HelpTextCodeCode { get; set; }
        public string AdditionalQuerySections { get; set; }
        public bool? DisplayInRequiredFields { get; set; }
        public int NumberOfDigits { get; set; }
        public int DigitsAfterPoint { get; set; }
        public string CustomPickListCode { get; set; }
    }
}
