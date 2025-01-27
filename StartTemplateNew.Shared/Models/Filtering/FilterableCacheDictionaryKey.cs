using StartTemplateNew.Shared.Models.Dto.Base.Requests;

namespace StartTemplateNew.Shared.Models.Filtering
{
    internal readonly struct FilterableCacheDictionaryKey(Type entityType, string propertyName, string propertyValue, RequestSearchFieldOperator @operator)
    {
        public Type EntityType { get; } = entityType;
        public string PropertyName { get; } = propertyName;
        public string PropertyValue { get; } = propertyValue;
        public RequestSearchFieldOperator Operator { get; } = @operator;

        public override bool Equals(object? obj)
        {
            return obj is FilterableCacheDictionaryKey key &&
                   EntityType == key.EntityType &&
                   PropertyName == key.PropertyName &&
                   PropertyValue == key.PropertyValue &&
                   Operator == key.Operator;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EntityType, PropertyName, PropertyValue, Operator);
        }
    }
}
