namespace AwsCs.Core.Contracts
{
    public interface ISendMessage
    {
        string Text { get; }
        decimal Value { get; }
    }
}
