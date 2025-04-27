namespace User.Api.IntegrationTests.Common;

public abstract class BaseIntegratedTest
{
    protected static CancellationToken GetCancellationToken
    {
        get
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(60_000);
            return cancellationTokenSource.Token;
        }
    }
}