using User.Api.Common;
using User.Api.Features.User;
using User.Api.IntegrationTests.Common;
using User.Api.IntegrationTests.Fixtures;

namespace User.Api.IntegrationTests.Tests;

[Collection(TestCollection.CollectionDefinition)]
public class UserTests(MainFixture mainFixture) : BaseIntegratedTest
{
    internal const string InsertUserQuery = @"INSERT INTO ""user"" (id, name, email) VALUES (@Id, @Name, @Email)";
    private const string RequestUri = "/users";

    private static List<Entity> CreateEntity(int numberOfItemsToCreate)
    {
        return new Faker<Entity>().StrictMode(true)
            .RuleFor(property => property.Email, setter => setter.Internet.Email(setter.Person.FirstName.ToLower()))
            .RuleFor(property => property.Id, _ => Guid.NewGuid())
            .RuleFor(property => property.Name, setter => setter.Name.FullName(Bogus.DataSets.Name.Gender.Male))
            .Generate(numberOfItemsToCreate);
    }

    [Fact(DisplayName = "User id received is valid and user is returned")]
    public async Task User_id_received_is_valid_and_user_is_returned()
    {
        //Arrange
        var userEntity = CreateEntity(numberOfItemsToCreate: 1).FirstOrDefault();

        await mainFixture.PostgreSqlFixture.TruncateTableAsync(@"""user""", GetCancellationToken);

        await mainFixture.PostgreSqlFixture.CreateAsync(userEntity, InsertUserQuery, GetCancellationToken);

        //Act
        var rawResponse = await mainFixture.HttpClient.GetAsync($"{RequestUri}/{userEntity!.Id}", GetCancellationToken);

        //Assert
        rawResponse.Should().NotBeNull();
        rawResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await rawResponse.Content.ReadFromJsonAsync<Response<Response>>(GetCancellationToken);

        response!.Data.Should().BeOfType<Response>();

        userEntity.Should().BeEquivalentTo(response!.Data);
    }

    [Fact(DisplayName = "Request received is valid and users are returned")]
    public async Task Request_received_is_valid_and_users_are_returned()
    {
        //Arrange
        var userEntities = CreateEntity(numberOfItemsToCreate: 10);

        await mainFixture.PostgreSqlFixture.TruncateTableAsync(@"""user""", GetCancellationToken);

        userEntities.ForEach(async userEntity =>
            await mainFixture.PostgreSqlFixture.CreateAsync(userEntity, InsertUserQuery, GetCancellationToken));

        //Act
        var rawResponse = await mainFixture.HttpClient.GetAsync(RequestUri, GetCancellationToken);

        //Assert
        rawResponse.Should().NotBeNull();
        rawResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await rawResponse.Content.ReadFromJsonAsync<Response<IEnumerable<Response>>>(GetCancellationToken);

        response!.Data.Should().BeOfType<List<Response>>();
        response!.Data!.Count().Should().BeGreaterThanOrEqualTo(userEntities.Count);
    }
}