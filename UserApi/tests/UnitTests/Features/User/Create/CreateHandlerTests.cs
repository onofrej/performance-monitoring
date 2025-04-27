using User.Api.Features.User;
using User.Api.Features.User.Create;

namespace User.Api.UnitTests.Features.User.Create;

public class CreateHandlerTests
{
    private readonly Mock<IMessaging> _messagingMock;
    private readonly Mock<IUserData> _userDataMock;
    private readonly Mock<IProfileData> _profileDataMock;
    private readonly Mock<IValidator<CreateCommand>> _validatorMock;
    private readonly CreateHandler _handler;

    public CreateHandlerTests()
    {
        _messagingMock = new Mock<IMessaging>();
        _userDataMock = new Mock<IUserData>();
        _profileDataMock = new Mock<IProfileData>();
        _validatorMock = new Mock<IValidator<CreateCommand>>();
        _handler = new CreateHandler(_userDataMock.Object,
            _profileDataMock.Object,
            _validatorMock.Object,
            _messagingMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsResultWithGuid()
    {
        // Arrange
        var request = new Fixture().Create<CreateCommand>();
        var expectedId = Guid.NewGuid();
        var validationResult = new ValidationResult();

        _validatorMock.Setup(expression => expression.Validate(request)).Returns(validationResult);

        _userDataMock.Setup(expression => expression.CreateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
            .Callback<UserEntity, CancellationToken>((entity, _) => entity.Id = expectedId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.HasFailed.Should().BeFalse();
        result.Data.Should().Be(expectedId);

        _userDataMock.Verify(expression => expression.CreateAsync(It.IsAny<UserEntity>(),
            It.IsAny<CancellationToken>()), Times.Once);

        _profileDataMock.Verify(expression => expression.CreateAsync(It.IsAny<ProfileEntity>(),
            It.IsAny<CancellationToken>()), Times.Once);

        _messagingMock.Verify(expression => expression.SendMessageAsync(It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ReturnsResultWithError()
    {
        // Arrange
        const string ValidationErrorMessage = "Validation error message";
        var request = new Fixture().Create<CreateCommand>();
        var validationResult = new ValidationResult(new List<ValidationFailure> { new(default, ValidationErrorMessage, default) });

        _validatorMock.Setup(expression => expression.Validate(request)).Returns(validationResult);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.HasFailed.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(Errors.ReturnInvalidEntriesError(ValidationErrorMessage));

        _userDataMock.Verify(expression => expression.CreateAsync(It.IsAny<UserEntity>(),
            It.IsAny<CancellationToken>()), Times.Never);

        _profileDataMock.Verify(expression => expression.CreateAsync(It.IsAny<ProfileEntity>(),
            It.IsAny<CancellationToken>()), Times.Never);

        _messagingMock.Verify(expression => expression.SendMessageAsync(It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }
}