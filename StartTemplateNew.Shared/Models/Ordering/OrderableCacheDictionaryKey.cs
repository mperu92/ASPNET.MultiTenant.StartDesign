namespace StartTemplateNew.Shared.Models.Ordering
{
    internal readonly struct OrderableCacheDictionaryKey(Type entityType, string propertyName)
    {
        public Type EntityType { get; } = entityType;
        public string PropertyName { get; } = propertyName;

        public override bool Equals(object? obj)
        {
            return obj is OrderableCacheDictionaryKey key &&
                   EntityType == key.EntityType &&
                   PropertyName == key.PropertyName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EntityType, PropertyName);
        }
    }
}
