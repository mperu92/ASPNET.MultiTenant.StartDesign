using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Models.Dto.Base.Requests
{
    public class BaseQueryRequest : IQueryRequest, IFilterableRequest
    {
        public string? SearchText { get; set; }

        public string? OrderField { get; set; }
        public bool IsAscending { get; set; }

        public ICollection<RequestSearchField> SearchFields { get; set; } = [];

        [JsonIgnore]
        [MemberNotNullWhen(true, nameof(SearchText))]
        public bool HasSearchText
            => !string.IsNullOrWhiteSpace(SearchText);

        [JsonIgnore]
        [MemberNotNullWhen(true, nameof(OrderField))]
        public bool HasOrder
            => !string.IsNullOrWhiteSpace(OrderField);

        [JsonIgnore]
        public bool HasSearchFilters
            => SearchFields.Count != 0;
    }
}
