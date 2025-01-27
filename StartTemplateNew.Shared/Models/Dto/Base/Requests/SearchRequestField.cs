namespace StartTemplateNew.Shared.Models.Dto.Base.Requests
{
    public class RequestSearchField
    {
        public RequestSearchField() { }
        public RequestSearchField(string fieldName, string searchValue, RequestSearchFieldOperator @operator)
        {
            FieldName = fieldName;
            SearchValue = searchValue;
            Operator = @operator;
        }

        public string FieldName { get; set; } = string.Empty;
        public string SearchValue { get; set; } = string.Empty;
        public RequestSearchFieldOperator Operator { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(FieldName) && !string.IsNullOrWhiteSpace(SearchValue) && Operator != RequestSearchFieldOperator.Default;
    }
}
