// this avoids CA1416 when accessing Windows specific platform features
#if NET5_0_OR_GREATER
[assembly: System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif