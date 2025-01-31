namespace StartTemplateNew.Shared.Models
{
    /// <summary>
    /// Represents a unit, a valueless type that is used to represent the absence of a specific value.
    /// </summary>
    public readonly struct Unit
    {
        /// <summary>
        /// Gets the sole instance of the <see cref="Unit"/> type.
        /// </summary>
        public static Unit Value { get; } = new Unit();

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => "()";

        public static bool operator ==(Unit _, Unit __) => true;

        public static bool operator !=(Unit _, Unit __) => false;

        public override bool Equals(object? obj) => obj is Unit;

        public override int GetHashCode() => 0;
    }
}
