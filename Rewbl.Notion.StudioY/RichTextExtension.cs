using System.Runtime.CompilerServices;
using Notion.Client;

namespace Rewbl.Notion.StudioY;

public static class RichTextExtension
{
    public static RichTextPropertyValue RichText(this string s)
    {
        return new RichTextPropertyValue()
        {
            RichText = new List<RichTextBase> { new RichTextText { Text = new Text() { Content = s } } }
        };
    }

    public static KeyValuePair<string, PropertyValue> RichTextProperty(this string name, string value)
    {
        return new KeyValuePair<string, PropertyValue>(name, value.RichText());
    }
}

public static class  PropertyValueExtension
{
    public static string PlainText(this RichTextPropertyValue property)
    {
        return property.RichText != null 
            ? string.Join("", property.RichText.Select(rt => rt.PlainText)) 
            : string.Empty;
    }

    public static string FirstId(this RelationPropertyValue property)
    {
        return property.Relation != null && property.Relation.Count > 0
            ? property.Relation[0].Id
            : string.Empty;
    }

    public static List<string> IdList(this RelationPropertyValue property)
    {
        return property.Relation != null
            ? property.Relation.Select(r => r.Id).ToList()
            : new List<string>();
    }
    
    
}
