using System.Linq.Expressions;
using System.Reflection;

namespace Pancake.Core
{
    /// <summary>
    /// Represents reflection extension methods.
    /// </summary>
    internal static class ReflectionExtensions
    {
        /// <summary>
        /// Retrieves the property info from the given expression.
        /// </summary>
        /// <param name="expression">The expression</param>
        /// <returns>A <c>System.Reflection.PropertyInfo</c> if one can be determined, null otherwise.</returns>
        public static PropertyInfo FindPropertyInfo(this Expression expression)
        {
            var me = (expression as MemberExpression);

            if (me != null)
                return (me.Member as PropertyInfo);

            var lambda = (expression as LambdaExpression);

            if (lambda != null)
                return FindPropertyInfo(lambda.Body);

            var ue = (expression as UnaryExpression);

            return ue != null ? FindPropertyInfo(ue.Operand) : null;
        }
    }
}
