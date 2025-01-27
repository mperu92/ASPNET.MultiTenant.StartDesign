using System.Security.Cryptography;

namespace StartTemplateNew.DAL.Helpers
{
    public static class GuidHelper
    {
        public static Guid NewSequentialGuid()
        {
            byte[] randomBytes = new byte[10];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            long timestamp = DateTime.UtcNow.Ticks / 10000L;
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(timestampBytes);

            byte[] guidBytes = new byte[16];

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);
                    // If running on .NET Core, we can use the new Guid(ReadOnlySpan<byte>) constructor for better performance.
                    // Otherwise, we need to use the byte array constructor.
#if NETCOREAPP
                    return new Guid(guidBytes.AsSpan());
#else
                    return new Guid(guidBytes);
#endif
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    return new Guid(guidBytes);
                default:
                    throw new InvalidOperationException("Platform not supported");
            }
        }
    }
}
