using Playground.Domain.Entities.Base;

namespace Playground.Domain.Extensions
{
    public static class BaseEntityExtension
    {
        public static void Initialize(this BaseEntity entity)
        {
            entity.Guid = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        public static void RefreshUpdatedAt(this BaseEntity entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
