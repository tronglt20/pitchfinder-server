namespace PitchFinder.RambitMQ.Handlers
{
    public class EventHandlerCollection : List<Type>
    {
        public EventHandlerCollection(Type implementType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var derivedTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(implementType) ||
                               (implementType.IsGenericTypeDefinition &&
                                type.BaseType != null &&
                                type.BaseType.IsGenericType &&
                                type.BaseType.GetGenericTypeDefinition() == implementType));

            this.AddRange(derivedTypes);
        }
    }
}
