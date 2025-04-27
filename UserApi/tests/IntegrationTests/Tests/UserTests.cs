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

    private static Entity CreateEntity()
    {
        return new Faker<Entity>().StrictMode(true)
            .RuleFor(property => property.Email, setter => setter.Internet.Email(setter.Person.FirstName.ToLower()))
            .RuleFor(property => property.Id, _ => Guid.NewGuid())
            .RuleFor(property => property.Name, setter => setter.Name.FullName(Bogus.DataSets.Name.Gender.Male))
            .Generate();
    }

    [Fact(DisplayName = "User id received is valid and user is returned")]
    public async Task User_id_received_is_valid_and_user_is_returned()
    {
        //Arrange
        var userEntity = CreateEntity();

        await mainFixture.PostgreSqlFixture.CreateAsync(userEntity, InsertUserQuery, GetCancellationToken);

        //Act
        var rawResponse = await mainFixture.HttpClient.GetAsync($"{RequestUri}/{userEntity.Id}", GetCancellationToken);

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
        const int NumberOfItemsToCreate = 10;

        var userEntities = new Faker<Entity>().StrictMode(true)
            .RuleFor(property => property.Email, setter => setter.Internet.Email(setter.Person.FirstName.ToLower()))
            .RuleFor(property => property.Id, _ => Guid.NewGuid())
            .RuleFor(property => property.Name, setter => setter.Name.FullName(Bogus.DataSets.Name.Gender.Male))
            .Generate(NumberOfItemsToCreate);

        foreach (var userEntity in userEntities)
        {
            await mainFixture.PostgreSqlFixture.CreateAsync(userEntity, InsertUserQuery, GetCancellationToken);
        }

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