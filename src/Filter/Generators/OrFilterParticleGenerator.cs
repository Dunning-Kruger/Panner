namespace Panner.Filter.Generators
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Panner.Filter.Particles;

    public class OrFilterParticleGenerator<TEntity> : IFilterParticleGenerator<TEntity>
        where TEntity : class
    {
        internal readonly Type type;

        public OrFilterParticleGenerator()
        {
            this.type = typeof(TEntity);
        }

        public bool TryGenerate(IPContext context, string input, out IFilterParticle<TEntity> particle)
        {
            input = input.Trim();

            // Attempt to split the input by all "||" not enclosed in quotes or parenthesis.
            Regex regex = new Regex("||(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            var splitInput = regex.Split(input);

            // If we dont end up with at least two parts, there's no OR operation here.
            if (splitInput.Length < 2)
            {
                particle = null;
                return false;
            }

            var trimmedInputs = splitInput.Select(x => x.Trim());

            // We parse each split input and OR it to the previous one.
            IFilterParticle<TEntity> resultParticle = null;
            foreach (var filter in trimmedInputs)
            {
                var cleanFilter = filter;

                if (filter.StartsWith("(") && filter.EndsWith(")"))
                {
                    cleanFilter = filter.Substring(1, filter.Length - 2);
                }

                var generators = context.GetGenerators<TEntity, IFilterParticle<TEntity>>();

                IFilterParticle<TEntity> fragmentParticle = null;
                if (!generators.Any(x => x.TryGenerate(context, filter, out fragmentParticle)))
                {
                    particle = null;
                    return false;
                }

                resultParticle = resultParticle is null ? fragmentParticle : new OrFilterParticle<TEntity>(resultParticle, fragmentParticle);
            }

            particle = resultParticle;
            return true;
        }
    }
}
