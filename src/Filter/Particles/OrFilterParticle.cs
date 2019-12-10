namespace Panner.Filter.Particles
{
    using System.Linq.Expressions;

    public class OrFilterParticle<TEntity> : IFilterParticle<TEntity>
        where TEntity : class
    {
        readonly IFilterParticle<TEntity> leftParticle;
        readonly IFilterParticle<TEntity> rightParticle;

        public OrFilterParticle(IFilterParticle<TEntity> leftParticle, IFilterParticle<TEntity> rightParticle)
        {
            this.leftParticle = leftParticle;
            this.rightParticle = rightParticle;
        }

        public Expression GetExpression(ParameterExpression parameter)
            => Expression.Or(
                leftParticle.GetExpression(parameter),
                rightParticle.GetExpression(parameter)
            );

    }
}
