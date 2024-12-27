using MePoC.Hume.Models;

namespace MePoC.Hume;
public class HumeRequestHandler : DelegatingHandler
{
    private readonly string _apiKey;
    private readonly string _apiSecret;

    public HumeRequestHandler(IConfiguration configuration, HttpMessageHandler? innerHandler = null)
        : base(innerHandler ?? new HttpClientHandler())
    {
        var humeAICfg = configuration.GetRequiredSection("HumeAI").Get<HumeAIConfig>();
        _apiKey = humeAICfg!.ApiKey;
        _apiSecret = humeAICfg!.ApiSecret;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("ApiKey", _apiKey);
        request.Headers.Add("ApiSecret", _apiSecret);

        return await base.SendAsync(request, cancellationToken);
    }
}
