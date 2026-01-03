using System.Reflection;

namespace JustTouch_ApiServices.Helpers
{
    public static class Mapper
    {
        public static TDestination Map<TSource, TDestination>(TSource source) where TDestination : class, new()
        {
            if(source == null) return default!;

            var destination = new TDestination();

            var sourceProps = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var destProps = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                .Where(x => x.CanWrite)
                                                .ToDictionary(d => d.Name, d => d, StringComparer.OrdinalIgnoreCase);

            foreach (var src in sourceProps)
            {
                if (!src.CanWrite) continue;

                if (!destProps.TryGetValue(src.Name, out var dest)) continue;

                if(!dest.PropertyType.IsAssignableFrom(src.PropertyType)) continue;

                var value = src.GetValue(source);
                dest.SetValue(destination, value);
            }

            return destination;
        }
    }
}
