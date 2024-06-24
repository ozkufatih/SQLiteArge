
using System.ComponentModel.DataAnnotations;

namespace Playground.Domain.Entities.Base
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        public Guid Guid { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
