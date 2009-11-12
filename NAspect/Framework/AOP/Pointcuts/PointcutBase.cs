using System.Collections;
using System.Reflection;

namespace Puzzle.NAspect.Framework.Aop
{
    /// <summary>
    /// Base class for pointcuts
    /// </summary>
    public abstract class PointcutBase : IPointcut
    {
        private IList interceptors = new ArrayList();

        /// <summary>
        /// Untyped list of <c>IInterceptor</c>s and <c>BeforeDelegate</c>, <c>AroundDelegate</c> and <c>AfterDelegate</c>
        /// </summary>
        public IList Interceptors
        {
            get { return interceptors; }
            set { interceptors = value; }
        }

        /// <summary>
        /// Matches a method with the pointuct
        /// </summary>
        /// <param name="method">The method to match</param>
        /// <returns>True if the pointcut matched the method, otherwise false</returns>
        public abstract bool IsMatch(MethodBase method);
    }
}