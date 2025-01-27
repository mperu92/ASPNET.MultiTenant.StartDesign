namespace StartTemplateNew.DAL.Models
{
    public readonly struct NullableKey<T> where T : IEquatable<T>
    {
        public NullableKey(T? value)
        {
            Value = value;
        }

        public T? Value { get; }

        public static implicit operator T?(NullableKey<T> key) => key.Value;
        public static implicit operator NullableKey<T>(T? value) => new(value);
    }
}
