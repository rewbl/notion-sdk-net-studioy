﻿using System.ComponentModel;
using Notion.Client;
using NUnit.Framework;

namespace Rewbl.Notion.StudioY.NotionTables;

public class EmailPage
{
    public RelationPropertyValue Device { get; set; }
    [DisplayName("TikTok Account")]
    public RelationPropertyValue TikTokAccount { get; set; }
    public SelectPropertyValue Status { get; set; }
    public UniqueIdPropertyValue ID { get; set; }

    [DisplayName("Recovery Email")]
    public RichTextPropertyValue RecoveryEmail { get; set; }
    [DisplayName("Recovery Password")]
    public  RichTextPropertyValue RecoveryPassword { get; set; }
    public SelectPropertyValue Provider { get; set; }
    public MultiSelectPropertyValue Tags { get; set; }
    public RelationPropertyValue Tasks { get; set; }
    public RichTextPropertyValue Password { get; set; }
    public RichTextPropertyValue Username { get; set; }
    public TitlePropertyValue Name { get; set; }

    public void LoadProperties(IDictionary<string, PropertyValue> properties)
    {
        foreach (var property in GetType().GetProperties())
        {
            // If property is not a kind of PropertyValue, skip it
            if (!typeof(PropertyValue).IsAssignableFrom(property.PropertyType)) continue;

            var displayNameAttribute = property.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute;
            var key = displayNameAttribute != null ? displayNameAttribute.DisplayName : property.Name;        
            if (properties.TryGetValue(key, out var propertyValue))
            {
                property.SetValue(this, Convert.ChangeType(propertyValue, property.PropertyType));
            }
        }
    }
}

public class NewEmailPage
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string RecoveryEmail { get; set; }

    public async Task<Page> CreateAsync(NotionClient client)
    {
        return  await client.Pages.CreateAsync(new PagesCreateParameters
        {
            Parent = new DatabaseParentInput {DatabaseId = Config.EmailDatabaseId},
            Icon = new EmojiObject {Emoji = "\u2709\ufe0f"},
            Properties = new Dictionary<string, PropertyValue>(new[]
            {
                "Username".RichTextProperty(Email),
                "Password".RichTextProperty(Password),
                "Recovery Email".RichTextProperty(RecoveryEmail)
            })
        });
        
    }

    

    [Test]
    public async Task TestMe()
    {
        var lines = File.ReadAllLines("c:/temp/gmail.txt");
        var client = NotionClientFactory.Create(new ClientOptions
        {
            AuthToken = Config.AuthToken
        });
        
        foreach (var line in lines)
        {
            if(string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split(':');
            Email = parts[0];
            Password = parts[1];
            RecoveryEmail = parts[2];
            var page = await CreateAsync(client);
        }
    }

    
}


