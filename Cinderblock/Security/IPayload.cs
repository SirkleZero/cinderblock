using System;

namespace Cinderblock.Security
{
    public interface IPayload : IDisposable
    {
        byte[] Data { get; }
    }
}
