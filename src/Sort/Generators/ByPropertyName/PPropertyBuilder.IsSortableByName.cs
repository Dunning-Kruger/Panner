﻿namespace Panner.Builders //Namespace is intentional so extensions are picked up when builder is used.
{
    using Panner.Sort.Generators;

    public static partial class PPropertyBuilderExtensions
    {
        /// <summary>Marks the property as sortable, referenced by its name.</summary>
        public static PPropertyBuilder<T> IsSortableByName<T>(this PPropertyBuilder<T> pPropertyBuilder)
            where T : class
        {
            var generator = pPropertyBuilder.Entity.GetOrCreateGenerator<ISortParticle<T>, ByPropertyNameParticleGenerator<T>>();
            generator.Add(pPropertyBuilder.PropertyInfo);
            return pPropertyBuilder;
        }
    }
}
