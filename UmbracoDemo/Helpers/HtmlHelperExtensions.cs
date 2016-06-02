using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.WebPages;

namespace UmbracoTest.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static HelperResult FormFieldFor<TModel, TResult>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TResult>> expression,
            Func<HtmlHelper, string, bool, HelperResult> template)
            where TModel : class
        {
            var isRequired = expression.IsFieldRequired();
            var propertyName = ExpressionHelper.GetExpressionText(expression);

            var result = template(html, propertyName, isRequired);
            return result;
        }
    }
}