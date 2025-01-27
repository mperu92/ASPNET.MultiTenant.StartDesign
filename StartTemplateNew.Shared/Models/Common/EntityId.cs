namespace StartTemplateNew.Shared.Models.Common
{
    public readonly struct EntityId
    {
        public EntityId(string? id = null)
        {
            Id = id;
        }

        public string? Id { get; }

        public bool IsEmpty
            => string.IsNullOrWhiteSpace(Id);
        public override string ToString()
            => Id ?? string.Empty;

        public static implicit operator string(EntityId id)
            => id.Id ?? string.Empty;
        public static implicit operator EntityId(string? id)
            => new(id);
        public static bool operator ==(EntityId left, EntityId right)
            => left.Equals(right);
        public static bool operator !=(EntityId left, EntityId right)
            => !left.Equals(right);

        public override bool Equals(object? obj)
            => obj is EntityId id && Equals(id);
        public bool Equals(EntityId other)
            => Id == other.Id;
        public override int GetHashCode()
            => HashCode.Combine(Id);
    }
}
