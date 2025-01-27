using Newtonsoft.Json;

namespace StartTemplateNew.Shared.Models.Dto.Base
{
    public abstract class KeyedDto : IKeyedDto
    {
        public virtual string Id { get; set; } = default!;

        protected KeyedDto() { }

        protected KeyedDto(string id)
        {
            Id = id;
        }
    }

    public abstract class KeyedDto<TKey> : IKeyedDto<TKey>
        where TKey : notnull, new()
    {
        public virtual TKey Id { get; set; } = default!;

        protected KeyedDto() { }

        protected KeyedDto(TKey id)
        {
            Id = id;
        }
    }

    public abstract class HiddenKeyedDto<TKey> : KeyedDto<TKey>
        where TKey : notnull, new()
    {
        protected HiddenKeyedDto() { }

        protected HiddenKeyedDto(TKey id)
            : base(id)
        {
            Id = base.Id;
        }

        [JsonIgnore]
        public override TKey Id { get; set; } = default!;
    }
}
