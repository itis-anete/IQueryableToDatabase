using System.Linq;
using System.Reflection;

namespace Tests
{
    public abstract class Entity
    {
        protected readonly PropertyInfo[] propertyInfos;

        public Entity()
        {
            propertyInfos = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public int Id { get; set; }

        public override string ToString()
            => string.Join(", ", 
                propertyInfos.Select(prop => $"{prop.Name}: {prop.GetValue(this)}"));
    }
}