using Microsoft.AspNetCore.Http;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;


namespace Kavifx.API.helper
{
    public class DechunkingMiddleware
    {
        private readonly RequestDelegate _next;

        public DechunkingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers["Transfer-Encoding"] == "chunked")
            {
                using (var decompressedStream = new MemoryStream())
                {
                    await context.Request.Body.CopyToAsync(decompressedStream);
                    decompressedStream.Seek(0, SeekOrigin.Begin);
                    context.Request.Body = decompressedStream;
                }
            }

            await _next(context);
        }
    }
}
