using Notion.Client;

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
                AuthToken = "secret_cGSFBPz0ezy05hFJI2MjYgXEdHMaaPE8LD38ZIElk20"
            });

            var filter = new RelationFilter("Current Google", isNotEmpty: true);
            var pages = await client.Databases.QueryAsync("e30ad27589ef4a80a3e7ccba979aa05b",new DatabasesQueryParameters{Filter=filter});
            var page = pages.Results[0];

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
