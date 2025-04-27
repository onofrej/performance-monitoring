using Microsoft.Extensions.Logging;
using User.Api.Features.User;
using User.Api.Features.User.Update;

namespace User.Api.UnitTests.Features.User.Update;

public class UpdateHandlerTests
{
    private readonly Mock<IUserData> _userDataMock;
    private readonly Mock<IProfileData> _profileDataMock;
    private readonly Mock<IValidator<UpdateCommand>> _validatorMock;
    private readonly UpdateHandler _handler;

    public UpdateHandlerTests()
    {
        _userDataMock = new Mock<IUserData>();
        _profileDataMock = new Mock<IProfileData>();
        _validatorMock = new Mock<IValidator<UpdateCommand>>();
        _handler = new UpdateHandler(_userDataMock.Object,
            _profileDataMock.Object,
            _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsResultWithGuid()
    {
        // Arrange
        var request = new UpdateCommand();
        var expectedId = Guid.NewGuid();
        var validationResult = new ValidationResult();

        _validatorMock.Setup(expression => expression.Validate(request)).Returns(validationResult);

        _userDataMock.Setup(expression => expression.UpdateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
            .Callback<UserEntity, CancellationToken>((entity, _) => entity.Id = expectedId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.HasFailed.Should().BeFalse();
        result.Data.Should().Be(expectedId);

        _userDataMock.Verify(expression => expression.UpdateAsync(It.IsAny<UserEntity>(),
            It.IsAny<CancellationToken>()), Times.Once);

        _profileDataMock.Verify(expression => expression.UpdateAsync(It.IsAny<ProfileEntity>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ReturnsResultWithError()
    {
        // Arrange
        var request = new UpdateCommand();
        var validationErrorMessage = "Validation error message";
        var validationResult = new ValidationResult(new List<ValidationFailure> { new(default, validationErrorMessage, default) });

        _validatorMock.Setup(expression => expression.Validate(request)).Returns(validationResult);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.HasFailed.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(Errors.ReturnInvalidEntriesError(validationErrorMessage));

        _userDataMock.Verify(expression => expression.UpdateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}