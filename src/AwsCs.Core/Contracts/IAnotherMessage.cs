using System;

namespace AwsCs.Core.Contracts
{
    public interface IAnotherMessage
    {
        string Username { get; }
        long Cpf { get; }
        DateTime Birth { get; }
    }
}
