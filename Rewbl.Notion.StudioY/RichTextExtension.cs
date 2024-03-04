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

    public static KeyValuePair<string, PropertyValue>? RichTextProperty(this string name, string value)
    {
        if (string.IsNullOrWhiteSpace(name) | string.IsNullOrWhiteSpace(value))
            return null;
        return new KeyValuePair<string, PropertyValue>(name, value.RichText());
    }
}

public static class TitleExtension
{
    public static TitlePropertyValue TitleValue(this string s)
    {
        return new TitlePropertyValue
        {
            Title = new List<RichTextBase> { new RichTextText { Text = new Text { Content = s } } }
        };
    }

    public static KeyValuePair<string, PropertyValue>? TitleProperty(this string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return null;
        return new KeyValuePair<string, PropertyValue>("Name", s.TitleValue());
    }
}

public static class SelectExtension
{
    public static SelectPropertyValue SelectValue(this string s)
    {
        return new SelectPropertyValue { Select = new SelectOption() {Name = s} };
    }

    public static KeyValuePair<string, PropertyValue>? SelectProperty(this string name, string value)
    {
        if (string.IsNullOrWhiteSpace(name) | string.IsNullOrWhiteSpace(value))
            return null;
        return new KeyValuePair<string, PropertyValue>(name, value.SelectValue());
    }
}

public static class RelationExtension
{
    
    public static RelationPropertyValue RelationValue(this string page_id)
    {
        return new RelationPropertyValue
        {
            Relation = [new ObjectId { Id = page_id }]
        };
    }

    public static RelationPropertyValue RelationValue(this IEnumerable<string> page_ids)
    {
        return new RelationPropertyValue { Relation = page_ids.Select(id => new ObjectId { Id = id }).ToList() };
    }
    
    public static KeyValuePair<string, PropertyValue>? RelationProperty(this string name, string page_id)
    {
        if (string.IsNullOrWhiteSpace(name) | string.IsNullOrWhiteSpace(page_id))
            return null;
        return new KeyValuePair<string, PropertyValue>(name, page_id.RelationValue());
    }

    public static KeyValuePair<string, PropertyValue>? RelationProperty(this string name, IEnumerable<string> page_ids)
    {
        if (string.IsNullOrWhiteSpace(name) | page_ids.All(string.IsNullOrWhiteSpace))
            return null;

        return new KeyValuePair<string, PropertyValue>(name, page_ids.RelationValue());
    }

    public static RelationPropertyValue Add(this RelationPropertyValue value, string page_id)
    {
        return new RelationPropertyValue { Relation = 
                new HashSet<string>(value.Relation.Select(id=>id.Id)) { page_id }
                    .Select(id=> new ObjectId {Id = id})
                    .ToList() };
    }

    public static RelationPropertyValue Remove(this RelationPropertyValue value, string page_id)
    {
        return new RelationPropertyValue
        {
            Relation = value.Relation.Where(id => id.Id != page_id).ToList()
        };
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
