using System.ComponentModel.DataAnnotations;

namespace Playground.Domain.Entities.Base
{
    public interface IBaseEntity : IEntity
    {
        [Key]
        public Guid Guid { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
