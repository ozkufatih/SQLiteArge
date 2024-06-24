using System.Reflection;

namespace Playground.Domain.Dtos.Base
{
    public class BaseDto
    {
        public Guid Guid { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            var properties = this.GetType()
                                 .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .Where(prop => prop.CanRead)
                                 .Select(prop => $"{prop.Name}: {prop.GetValue(this, null)}")
                                 .ToArray();

            return string.Join(", ", properties);
        }
    }
}
