using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace UmbracoTest.Helpers
{
    public static class PropertyExpressionExtensions
    {
        public static bool HasAttribute<TModel, TPropertyType, TAttribute>(
            this Expression<Func<TModel, TPropertyType>> propertyExpression,
            Func<TAttribute, bool> attributePredicate = null)
            where TAttribute : Attribute
        {
            var expressionText = ExpressionHelper.GetExpressionText(propertyExpression);
            var type = typeof(TModel);
            var property = type.GetProperty(expressionText);

            var attributeType = typeof(TAttribute);

            var attributes = (TAttribute[])property.GetCustomAttributes(attributeType, true);

            if (attributes.Any() == false)
                return false;

            if (attributePredicate != null && attributes.Any(attributePredicate) == false)
                return false;

            return true;
        }
        
        public static bool IsFieldRequired<TModel, TPropertyType>(
            this Expression<Func<TModel, TPropertyType>> propertyExpression)
        {
            var hasRequiredAttribute = 
                propertyExpression.HasAttribute<TModel, TPropertyType, RequiredAttribute>();
            return hasRequiredAttribute;
        }
    }
}