using User.Api.Features.User;
using User.Api.Features.User.Delete;

namespace User.Api.UnitTests.Features.User.Delete;

public class DeleteHandlerTests
{
    private readonly Mock<IUserData> _userDataMock;
    private readonly Mock<IValidator<DeleteCommand>> _validatorMock;
    private readonly DeleteHandler _handler;

    public DeleteHandlerTests()
    {
        _userDataMock = new Mock<IUserData>();
        _validatorMock = new Mock<IValidator<DeleteCommand>>();
        _handler = new DeleteHandler(_userDataMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsResultWithGuid()
    {
        // Arrange
        var request = new DeleteCommand { Id = Guid.NewGuid() };
        var expectedId = request.Id;
        var validationResult = new ValidationResult();

        _validatorMock.Setup(expression => expression.Validate(request)).Returns(validationResult);

        _userDataMock.Setup(expression => expression.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserEntity { Id = request.Id });

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.HasFailed.Should().BeFalse();
        result.Data.Should().Be(expectedId);

        _userDataMock.Verify(expression => expression.DeleteAsync(request.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ReturnsResultWithError()
    {
        // Arrange
        var request = new DeleteCommand();
        var validationErrorMessage = "Validation error message";
        var validationResult = new ValidationResult(new List<ValidationFailure> { new(default, validationErrorMessage, default) });

        _validatorMock.Setup(expression => expression.Validate(request)).Returns(validationResult);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.HasFailed.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(Errors.ReturnInvalidEntriesError(validationErrorMessage));

        _userDataMock.Verify(expression => expression.DeleteAsync(Guid.Empty, It.IsAny<CancellationToken>()), Times.Never);
    }
}