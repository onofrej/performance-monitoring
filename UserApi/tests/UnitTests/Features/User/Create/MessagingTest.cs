using User.Api.Features.User.Create;

namespace User.Api.UnitTests.Features.User.Create;

public class MessagingTest
{
    private readonly Mock<IAmazonSQS> _amazonSQSMock;
    private readonly Mock<IOptions<Aws>> _awsOptionsMock;
    private readonly Messaging _messaging;

    public MessagingTest()
    {
        _amazonSQSMock = new Mock<IAmazonSQS>();
        _awsOptionsMock = new Mock<IOptions<Aws>>();
        _awsOptionsMock.Setup(x => x.Value).Returns(new Aws { Sqs = new Sqs { QueueName = "test-queue" } });
        _messaging = new Messaging(_amazonSQSMock.Object, _awsOptionsMock.Object);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldReturnTrue_WhenMessageIsSentSuccessfully()
    {
        // Arrange
        var queueUrlResponse = new GetQueueUrlResponse { QueueUrl = "http://test-queue-url" };
        var sendMessageResponse = new SendMessageResponse { HttpStatusCode = HttpStatusCode.OK };

        _amazonSQSMock.Setup(x => x.GetQueueUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queueUrlResponse);

        _amazonSQSMock.Setup(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sendMessageResponse);

        // Act
        var result = await _messaging.SendMessageAsync("test message", CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        _amazonSQSMock.Verify(expression => expression.SendMessageAsync(It.IsAny<SendMessageRequest>(),
           It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldReturnFalse_WhenMessageIsNotSentSuccessfully()
    {
        // Arrange
        var queueUrlResponse = new GetQueueUrlResponse { QueueUrl = "http://test-queue-url" };
        var sendMessageResponse = new SendMessageResponse { HttpStatusCode = HttpStatusCode.BadRequest };

        _amazonSQSMock.Setup(x => x.GetQueueUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queueUrlResponse);

        _amazonSQSMock.Setup(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sendMessageResponse);

        // Act
        var result = await _messaging.SendMessageAsync("test message", CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        _amazonSQSMock.Verify(expression => expression.SendMessageAsync(It.IsAny<SendMessageRequest>(),
           It.IsAny<CancellationToken>()), Times.Once);
    }
}