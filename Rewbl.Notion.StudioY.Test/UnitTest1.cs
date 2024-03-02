using Notion.Client;
using Rewbl.Notion.StudioY.NotionTables;

namespace Rewbl.Notion.StudioY.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }
        
        [Test]
        public async Task Test1()
        {
            var client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = Config.AuthToken
            });

            var new_page = await client.Pages.CreateAsync(new PagesCreateParameters
            {
                Parent = new DatabaseParentInput {DatabaseId = Config.EmailDatabaseId},
                Icon = new EmojiObject {Emoji = "\u2709\ufe0f"},
                Properties = new Dictionary<string, PropertyValue>(new[]
                {
                    new KeyValuePair<string, PropertyValue>("Username",new RichTextPropertyValue()
                    {
                        RichText = new List<RichTextBase>()
                        {
                            new RichTextText
                            {
                                Text = new Text()
                                {
                                    Content = "Username1"
                                }
                            }
                        }
                    })
                })
            });
            
            
            var filter = new RichTextFilter("Username", isNotEmpty: true);

            var pages = await client.Databases.QueryAsync(Config.EmailDatabaseId,new DatabasesQueryParameters{Filter=filter});
            var page = pages.Results[0] as Page;
            var record = new EmailPage();
            record.LoadProperties(page.Properties);
            var richText=new List<RichTextBase>
            {
                new RichTextText()
                {
                    Text = new Text()
                    {
                        Content = "Hello, World!"
                    }
                }
            };

            var res= await client.Blocks.AppendChildrenAsync(
                new BlockAppendChildrenRequest
                {
                    BlockId = page.Id,
                    Children = new List<IBlockObjectRequest>
                    {
                        new HeadingThreeBlockRequest
                        {
                            Heading_3 = new HeadingThreeBlockRequest.Info(){IsToggleable = true, RichText = richText}
                        }
                    }
                }
            );
            Assert.Pass();
        }
    }
}
