using User.Api.Features.User;
using User.Api.Features.User.GetById;

namespace User.Api.UnitTests.Features.User.GetById;

public class GetByIdHandlerTests
{
    private readonly Mock<IUserData> _userDataMock;
    private readonly Mock<IValidator<GetByIdQuery>> _validatorMock;
    private readonly GetByIdHandler _handler;

    public GetByIdHandlerTests()
    {
        _userDataMock = new Mock<IUserData>();
        _validatorMock = new Mock<IValidator<GetByIdQuery>>();
        _handler = new GetByIdHandler(_userDataMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnResultWithUser()
    {
        // Arrange
        var request = new UserEntity();
        var expectedResultId = request.Id;
        var validationResult = new ValidationResult();
        var cancellationToken = new CancellationToken();

        _validatorMock.Setup(expression => expression.Validate(It.IsAny<GetByIdQuery>())).Returns(validationResult);

        _userDataMock.Setup(expression => expression.GetByIdAsync(request.Id, cancellationToken)).ReturnsAsync(request);

        //Act
        var handler = new GetByIdHandler(_userDataMock.Object, _validatorMock.Object);

        var result = await handler.Handle(new GetByIdQuery(request.Id), cancellationToken);

        //Assert
        result.Should().NotBeNull();
        result.HasFailed.Should().BeFalse();
        result.Data!.Id.Should().Be(expectedResultId);

        _userDataMock.Verify(expression => expression.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ReturnsResultWithError()
    {
        // Arrange
        var request = new UserEntity();
        var expectedResultId = request.Id;
        var cancellationToken = new CancellationToken();
        var validationErrorMessage = "Validation error message";
        var validationResult = new ValidationResult(new List<ValidationFailure> { new(default, validationErrorMessage, default) });

        _validatorMock.Setup(expression => expression.Validate(It.IsAny<GetByIdQuery>())).Returns(validationResult);

        _userDataMock.Setup(expression => expression.GetByIdAsync(request.Id, cancellationToken)).ReturnsAsync(request);

        // Act
        var handler = new GetByIdHandler(_userDataMock.Object, _validatorMock.Object);

        var result = await handler.Handle(new GetByIdQuery(Guid.Empty), cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.HasFailed.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(Errors.ReturnInvalidEntriesError(validationErrorMessage));

        _userDataMock.Verify(expression => expression.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()), Times.Never);
    }
}