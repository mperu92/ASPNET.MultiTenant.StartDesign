using StartTemplateNew.Shared.Helpers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace StartTemplateNew.Shared.Models.Dto.Base.Requests
{
    [DebuggerDisplay("Value = {Value}")]
    public struct RequestSearchFieldOperator : IEquatable<RequestSearchFieldOperator>, IEquatable<string>
    {
        private static readonly RequestSearchFieldOperator equal = "eq";
        private static readonly RequestSearchFieldOperator notequal = "ne";
        private static readonly RequestSearchFieldOperator greaterthan = "gt";
        private static readonly RequestSearchFieldOperator greaterthanorequal = "ge";
        private static readonly RequestSearchFieldOperator lessthan = "lt";
        private static readonly RequestSearchFieldOperator lessthanorequal = "le";
        private static readonly RequestSearchFieldOperator contains = "contains";
        private static readonly RequestSearchFieldOperator startswith = "startswith";
        private static readonly RequestSearchFieldOperator endswith = "endswith";
        private static readonly RequestSearchFieldOperator @in = "in";
        private static readonly RequestSearchFieldOperator notin = "notin";
        private static readonly RequestSearchFieldOperator between = "between";
        private static readonly RequestSearchFieldOperator notbetween = "notbetween";
        private static readonly RequestSearchFieldOperator isnull = "isnull";
        private static readonly RequestSearchFieldOperator isnotnull = "isnotnull";

        public RequestSearchFieldOperator()
        {
            Value = Default;
        }

        public RequestSearchFieldOperator(string value)
        {
            SetValue(value);
        }

        public RequestSearchFieldOperator(string value, StringComparison stringComparison)
        {
            SetValue(value, stringComparison);
        }

        public string Value { get; set; }

        public static RequestSearchFieldOperator Default { get; } = "none";

        [MemberNotNull(nameof(Value))]
        private void SetValue(string value, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            ArgumentException.ThrowIfNullOrEmpty(value);

            if (!value.EqualsMany([equal, notequal, greaterthan, greaterthanorequal, lessthan, lessthanorequal, contains, startswith, endswith, @in, notin, between, notbetween, isnull, isnotnull], stringComparison))
                throw new ArgumentException("Invalid operator value.", nameof(value));

            Value = value;
        }

        public readonly bool Equals(RequestSearchFieldOperator other)
           => Value == other.Value;

        public readonly bool Equals(string? other)
            => Value == other;

        public readonly override bool Equals(object? obj)
            => obj is RequestSearchFieldOperator other && Equals(other);

        public readonly override int GetHashCode()
            => HashCode.Combine(Value);

        public static bool operator ==(RequestSearchFieldOperator left, RequestSearchFieldOperator right)
            => left.Equals(right);

        public static bool operator !=(RequestSearchFieldOperator left, RequestSearchFieldOperator right)
            => !left.Equals(right);

        public readonly override string ToString()
            => Value;

        public static implicit operator string(RequestSearchFieldOperator op) => op.Value;
        public static implicit operator RequestSearchFieldOperator(string value) => new(value);
    }
}
