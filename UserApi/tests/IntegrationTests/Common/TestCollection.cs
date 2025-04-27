using User.Api.IntegrationTests.Fixtures;

namespace User.Api.IntegrationTests.Common;

[CollectionDefinition(CollectionDefinition)]
public class TestCollection : ICollectionFixture<MainFixture>
{
    internal const string CollectionDefinition = "Test Collection";
}