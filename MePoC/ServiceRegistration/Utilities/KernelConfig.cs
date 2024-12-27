using MePoC.ServiceRegistration.Models;
using System.Runtime.CompilerServices;

namespace MePoC.ServiceRegistration.Utilities;
public class KernelConfig
{
    private readonly IConfigurationRoot _configRoot;
    private static KernelConfig? _kernelConfig;
    public KernelConfig(IConfigurationRoot configRoot)
    {
        _configRoot = configRoot;
    }

    public static void Initialize(IConfigurationRoot configRoot)
    {
        _kernelConfig = new KernelConfig(configRoot);
    }

    public static OpenAIConfig OpenAI => LoadSection<OpenAIConfig>();

    private static T LoadSection<T>([CallerMemberName] string? caller = null)
    {
        if (_kernelConfig == null)
        {
            throw new InvalidOperationException(
                "KernelConfig must be initialized with a call to Initialize(IConfigurationRoot) before accessing configuration values.");
        }

        if (string.IsNullOrEmpty(caller))
        {
            throw new ArgumentNullException(nameof(caller));
        }
        return _kernelConfig._configRoot.GetSection($"SemanticKernel:{caller}").Get<T>() ??
            throw new ArgumentNullException(caller);
    }
}
