using StartTemplateNew.Shared.Enums;
using StartTemplateNew.Shared.Models.Common;

namespace StartTemplateNew.Shared.Models.Dto
{
    public readonly struct EntityStateInfo
    {
        public EntityStateInfo(EntityId? entityId = null, string? message = null, EntityStatus state = EntityStatus.Unknown)
        {
            State = state;
            Message = message;
            EntityId = entityId;
        }

        public EntityStatus State { get; }
        public string? Message { get; }

        public EntityId? EntityId { get; }

        public bool Succeeded => State == EntityStatus.Added || State == EntityStatus.Updated || State == EntityStatus.Deleted;
    }
}
