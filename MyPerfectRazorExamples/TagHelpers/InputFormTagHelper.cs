using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Server.HttpSys;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace MyPerfectRazorExamples.TagHelpers;

public enum InputFormMethod
{
    Get, Post
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class FormIgnoreAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class FormInputAttribute : Attribute
{
    public string? Name { get; set; }
    public string? PlaceHolder { get; set; }
    public string? Tooltip { get; set; }
}

[HtmlTargetElement("input-form", Attributes = "model, action-url, method")]
public class InputFormTagHelper : TagHelper
{
    [HtmlAttributeName("model")]
    public Type? ModelType { get; set; }

    [HtmlAttributeName("action-url")]
    public string Action { get; set; } = "/";

    [HtmlAttributeName("method")]
    public InputFormMethod Method { get; set; } = InputFormMethod.Post;


    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";

        if (ModelType is null)
        {
            output.Content.SetContent("No data");
            return;
        }
        output.TagMode = TagMode.StartTagAndEndTag;
        var properties = GetWritableProperties(ModelType);
        BeginForm(output.Content);
        foreach (var property in properties)
        {
            if (IsString(property.PropertyType))
            {
                AppendTextInput(output.Content, property);
            }
            else if (IsChar(property.PropertyType))
            {
                AppendCharInput(output.Content, property);
            }
            else if (IsInteger(property.PropertyType))
            {
                AppendIntegerInput(output.Content, property);
            }
            else if (IsFloat(property.PropertyType))
            {
                AppendFloatInput(output.Content, property);
            }
            else if (IsBool(property.PropertyType))
            {
                AppendBoolInput(output.Content, property);
            }

        }
        EndForm(output.Content);
    }
    private void BeginForm(TagHelperContent content)
    {
        content.AppendHtml($"<form method='{Method}' action='{Action}'");
    }
    private void EndForm(TagHelperContent content)
    {
        content.AppendHtml($@"
    <div>
        <input type='submit' value='save'></input>
    </div>
</form>
");
    }
    private static bool IsString(Type type) => type == typeof(string);
    private bool IsBool(Type type) => type == typeof(bool) || type == typeof(bool?);
    private bool IsInteger(Type type)
        => type == typeof(int) || type == typeof(int?) ||
             type == typeof(Int32) || type == typeof(Int32?);
    private static bool IsChar(Type type) => type == typeof(char) || type == typeof(char?);

    private static bool IsFloat(Type type) =>
         type == typeof(float) || type == typeof(float?)
         || type == typeof(double) || type == typeof(double?)
         || type == typeof(decimal) || type == typeof(decimal?);


    private void AppendTextInput(TagHelperContent content, PropertyInfo property)
    {
        var (name, placeholder, tooltip) = GetFormInputInfos(property);
        var required = GetRequired(property);
        var req = required ? "required" : "";
        content.AppendHtml(
            @$"
                    <div>   
                        <label for='{name}'>{name}:</label><br>
                        <input type='text' id='{name}' name='{name}' {req} placeholder='{placeholder}' title='{tooltip}'>
                    </div>
                ");
    }
    private void AppendBoolInput(TagHelperContent content, PropertyInfo property)
    {
        var name = GetName(property);
        var tooltip = GetToolTip(property);
        var required = GetRequired(property) ? "required" : "";
        content.AppendHtml(
             @$"
                    <div>   
                        <label for='{name}'>{name}</label>
                        <input type='checkbox' id='{name}' name='{name}'  {required} title='{tooltip}'>
                    </div>
                ");
    }
    private void AppendIntegerInput(TagHelperContent content, PropertyInfo property)
    {
        var (name, placeholder, tooltip) = GetFormInputInfos(property);
        var (min, max) = GetFloatRange(property);
        var required = GetRequired(property) ? "required" : "";
        content.AppendHtml(
             @$"
                    <div>   
                        <label for='{name}'>{name}:</label><br>
                        <input type='number' id='{name}' name='{name}' step='1' min='{min}' max='{max}' {required} placeholder='{placeholder}' title='{tooltip}'>
                    </div>
                ");
    }

    private void AppendCharInput(TagHelperContent content, PropertyInfo property)
    {
        var (name, placeholder, tooltip) = GetFormInputInfos(property);
        var required = GetRequired(property);
        var req = required ? "required" : "";
        content.AppendHtml(
            @$"
                    <div>   
                        <label for='{name}'>{name}:</label><br>
                        <input type='text' id='{name}' name='{name}' {req} placeholder='{placeholder}' title='{tooltip}' maxlength='1' minlength='0'>
                    </div>
                ");
    }

    private void AppendFloatInput(TagHelperContent content, PropertyInfo prop)
    {
        var (name, placeholder, tooltip) = GetFormInputInfos(prop);
        var (min, max) = GetFloatRange(prop);
        var required = GetRequired(prop) ? "required" : "";
        content.AppendHtml(
             @$"
                    <div>   
                        <label for='{name}'>{name}:</label><br>
                        <input type='number' id='{name}' name='{name}' min='{min}' max='{max}' {required} placeholder='{placeholder}' title='{tooltip}'>
                    </div>
                ");

    }


    private static (string Name, string? Placeholder, string? Tooltip) GetFormInputInfos(PropertyInfo prop)
    {
        var name = GetName(prop);
        var placeholder = GetPlaceholder(prop);
        var tooltip = GetToolTip(prop);

        return (name, placeholder, tooltip);
    }

    private static string GetName(PropertyInfo prop) =>
         prop.GetCustomAttribute<FormInputAttribute>()?.Name ?? prop.Name;

    private static string? GetToolTip(PropertyInfo prop) =>
        prop.GetCustomAttribute<FormInputAttribute>()?.Tooltip;

    private static string? GetPlaceholder(PropertyInfo prop) =>
        prop.GetCustomAttribute<FormInputAttribute>()?.PlaceHolder;

    private static bool GetRequired(PropertyInfo prop) =>
        prop.GetCustomAttribute<RequiredAttribute>() is not null;

    private static (double Min, double Max) GetFloatRange(PropertyInfo prop)
    {
        var rangeAttribute = prop.GetCustomAttribute<RangeAttribute>();

        double min;
        double max;

        if (rangeAttribute is not null)
        {
            min = Convert.ToDouble(rangeAttribute.Minimum);
            max = Convert.ToDouble(rangeAttribute.Maximum);
        }
        else
        {
            min = Convert.ToDouble(prop.PropertyType.GetField("MinValue").GetValue(null));
            max = Convert.ToDouble(prop.PropertyType.GetField("MaxValue").GetValue(null));
        }

        return (min, max);
    }


    private static ICollection<PropertyInfo> GetWritableProperties(Type type) =>
        type.GetProperties()
        .Where(e => e.CanWrite && e.GetCustomAttribute<FormIgnoreAttribute>() is null)
        .ToList();

}


