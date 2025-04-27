using User.Api.Features.User;

namespace User.Api.UnitTests.Features.User;

public class MapperTest
{
    private readonly Fixture _fixture;

    public MapperTest()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void MapToResponse_ShouldMapUserEntityToUserResponse()
    {
        // Arrange
        var userEntity = _fixture.Create<UserEntity>();

        // Act
        var userResponse = userEntity.MapToResponse();

        // Assert
        userResponse.Should().NotBeNull();
        userResponse.BaptismDate.Should().Be(userEntity.BaptismDate);
        userResponse.BirthDate.Should().Be(userEntity.BirthDate);
        userResponse.CreationDate.Should().Be(userEntity.CreationDate);
        userResponse.CongregationId.Should().Be(userEntity.CongregationId);
        userResponse.Id.Should().Be(userEntity.Id);
        userResponse.Email.Should().Be(userEntity.Email);
        userResponse.Gender.Should().Be(userEntity.Gender);
        userResponse.Grade.Should().Be(userEntity.Grade);
        userResponse.Name.Should().Be(userEntity.Name);
        userResponse.PhoneNumber.Should().Be(userEntity.PhoneNumber);
        userResponse.Privilege.Should().Be(userEntity.Privilege);
    }

    [Fact]
    public void MapToResponse_ShouldMapUserEntitiesToUserResponses()
    {
        // Arrange
        var userEntities = _fixture.CreateMany<UserEntity>();

        // Act
        var userResponses = userEntities.MapToResponse();

        // Assert
        userResponses.Should().NotBeNull();
        userResponses.Should().HaveCount(userEntities.Count());
        userResponses.Should().BeEquivalentTo(userEntities, options => options
            .ExcludingMissingMembers());
    }

    [Fact]
    public void MapToResponse_ShouldMapDomainEntitiesToDomainResponses()
    {
        // Arrange
        var domainEntities = _fixture.CreateMany<DomainEntity>();

        // Act
        var domainResponses = domainEntities.MapToResponse();

        // Assert
        domainResponses.Should().NotBeNull();
        domainResponses.Should().HaveCount(domainEntities.Count());
        domainResponses.Should().BeEquivalentTo(domainEntities, options => options
            .ExcludingMissingMembers());
    }
}