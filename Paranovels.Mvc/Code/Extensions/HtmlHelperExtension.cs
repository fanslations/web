using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Microsoft.Ajax.Utilities;
using Thi.Core;

namespace Paranovels.Mvc
{
    public static class HtmlHelperExtension
    {
        public static MvcHtmlString ImproveThis<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expressionUpdateField,
            IDictionary<string, object> htmlAttributes = null,
            string customHtmlIcon = null) where TModel : class, IModel, new()
        {
            //ModelMetadata idField = ModelMetadata.FromLambdaExpression(expressionIDField, html.ViewData);
            ModelMetadata updateField = ModelMetadata.FromLambdaExpression(expressionUpdateField, html.ViewData);
            //string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            //string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

            //var inlineEdit = @"{{""Model"":""{0}"", ""ID"":""{1}"", ""Name"":""{2}"",""Value"":""{3}"",""Type"":""{4}""}}";
            //inlineEdit = string.Format(inlineEdit, html.ViewData.ModelMetadata.ModelType.Name, html.ViewData.Model.ID, updateField.PropertyName, "", updateField.ModelType);

            var inlineEditModelJson = string.Format(@"{{ ""ID"" : {0}, ""InlineEditProperty"" : ""{1}"" }}", html.ViewData.Model.ID, updateField.PropertyName);

            var tag = new TagBuilder("a");
            tag.AddCssClass("improve-this");
            tag.InnerHtml = customHtmlIcon ?? @"<i class=""fa fa-edit"">Edit</i>";
            tag.Attributes.Add("data-inline-edit", inlineEditModelJson);
            if (htmlAttributes != null)
            {
                tag.MergeAttributes(htmlAttributes);
            }
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));

        }

        public static MvcHtmlString ImproveThis<TModel,TValue>(this TModel model,
            Expression<Func<TModel, TValue>> expressionUpdateField,
            IDictionary<string, object> htmlAttributes = null,
            string customHtmlIcon = null) where TModel : class, IModel, new()
        {
            var propertyName = model.GetPropertyInfo(expressionUpdateField).Name;
            var inlineEditModelJson = string.Format(@"{{ ""ID"" : {0}, ""InlineEditProperty"" : ""{1}"" }}", model.ID, propertyName);

            var tag = new TagBuilder("a");
            tag.AddCssClass("improve-this");
            tag.InnerHtml = customHtmlIcon ?? @"<i class=""fa fa-edit"">Edit</i>";
            tag.Attributes.Add("data-inline-edit", inlineEditModelJson);
            if (htmlAttributes != null)
            {
                tag.MergeAttributes(htmlAttributes);
            }
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));

        }

        public static MvcHtmlString JsMinify(this HtmlHelper html, Func<object, object> markup)
        {
            string notMinifiedJs = (markup.DynamicInvoke(html.ViewContext) ?? "").ToString();

            var minifier = new Minifier();
            var minifiedJs = minifier.MinifyJavaScript(notMinifiedJs, new CodeSettings
            {
                EvalTreatment = EvalTreatment.MakeImmediateSafe,
                PreserveImportantComments = false
            });
            return new MvcHtmlString(minifiedJs);
        }

    }
}