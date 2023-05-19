namespace PitchFinder.RambitMQ.Events
{
    public class IntegrationEventCollection : List<Type>
    {
        public IntegrationEventCollection(Type implementType)
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
