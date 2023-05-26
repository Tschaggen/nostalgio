using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace StdsSocialMediaBackend.Domain.Middleware
{
    public class IpSafeListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IpSafeListMiddleware> _logger;
        private readonly byte[][] _safelist;

        public IpSafeListMiddleware(
            RequestDelegate next,
            ILogger<IpSafeListMiddleware> logger,
            string safelist)
        {
            var ips = safelist.Split(';');
            _safelist = new byte[ips.Length][];
            for (var i = 0; i < ips.Length; i++)
            {
                _safelist[i] = IPAddress.Parse(ips[i]).GetAddressBytes();
            }

            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != HttpMethod.Get.Method)
            {
                var remoteIp = context.Connection.RemoteIpAddress;
                _logger.LogDebug("Request from Remote IP address: {RemoteIp}", remoteIp);

                if (remoteIp == null) throw new ArgumentNullException(nameof(remoteIp));
                var bytes = remoteIp.GetAddressBytes();
                var badIp = true;
                foreach (var address in _safelist)
                {
                    if (address.SequenceEqual(bytes))
                    {
                        badIp = false;
                        break;
                    }
                }

                if (badIp)
                {
                    _logger.LogWarning(
                        "Forbidden Request from Remote IP address: {RemoteIp}", remoteIp);
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }
}
