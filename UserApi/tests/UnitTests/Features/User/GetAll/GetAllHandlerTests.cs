using User.Api.Features.User.GetAll;

namespace User.Api.UnitTests.Features.User.GetAll;

public class GetAllHandlerTests
{
    private readonly Mock<IUserData> _userDataMock;
    private readonly Mock<IValidator<GetAllQuery>> _validatorMock;
    private readonly GetAllHandler _handler;

    public GetAllHandlerTests()
    {
        _userDataMock = new Mock<IUserData>();
        _validatorMock = new Mock<IValidator<GetAllQuery>>();
        _handler = new GetAllHandler(_userDataMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnResultWithUsers()
    {
        // Arrange
        var request = new List<UserEntity> { new() };

        var validationResult = new ValidationResult();
        var cancellationToken = new CancellationToken();

        _validatorMock.Setup(expression => expression.Validate(It.IsAny<GetAllQuery>())).Returns(validationResult);

        _userDataMock.Setup(expression => expression.GetAllAsync(cancellationToken)).ReturnsAsync(request);

        // Act
        var handler = new GetAllHandler(_userDataMock.Object);

        var result = await handler.Handle(new GetAllQuery(), cancellationToken);

        //Assert
        result.Should().NotBeNull();
        result.HasFailed.Should().BeFalse();

        var userEntities = result.Data!.ToList();
        userEntities.Should().NotBeNull();
        userEntities.Should().HaveCount(request.Count);

        foreach (var expectedUsers in request)
        {
            userEntities.Should().ContainEquivalentOf(expectedUsers);
        }

        _userDataMock.Verify(expression => expression.GetAllAsync(cancellationToken), Times.Once);
    }
}