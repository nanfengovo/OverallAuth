using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OverallAuthWPF.APIHelper
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    namespace OverallAuthWPF.APIHelper
    {
        public sealed class HttpRequest : IDisposable
        {
            // 单例模式管理HttpClient（避免端口耗尽）
            private static readonly Lazy<HttpClient> _sharedClient = new Lazy<HttpClient>(() =>
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                return client;
            });

            private const string BaseURL = "http://127.0.0.1:5141/api";

            // 私有构造函数（强制通过Instance访问）
            private HttpRequest() { }

            public static HttpRequest Instance { get; } = new HttpRequest();

            // 统一请求入口
            public async Task<Response<T>> SendAsync<T>(
                HttpMethod method,
                string endpoint,
                object data = null,
                string bearerToken = null,
                TimeSpan? timeout = null,
                CancellationToken ct = default)
            {
                try
                {
                    using var request = new HttpRequestMessage(method, $"{BaseURL}/{endpoint}");

                    // 添加认证头
                    //if (!string.IsNullOrEmpty(bearerToken))
                    //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                    // 序列化请求体
                    if (data != null)
                        request.Content = new StringContent(
                            JsonSerializer.Serialize(data),
                            Encoding.UTF8,
                            "application/json"
                        );

                    // 设置超时
                    using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                    if (timeout.HasValue) cts.CancelAfter(timeout.Value);

                    // 发送请求
                    var response = await _sharedClient.Value.SendAsync(request, cts.Token);
                    return await ProcessResponse<T>(response);
                }
                catch (HttpRequestException ex)
                {
                    return Response<T>.Fail($"网络错误: {ex.Message}", HttpStatusCode.ServiceUnavailable);
                }
                catch (TaskCanceledException)
                {
                    return Response<T>.Fail("请求超时", HttpStatusCode.RequestTimeout);
                }
                catch (JsonException)
                {
                    return Response<T>.Fail("JSON解析失败", HttpStatusCode.BadRequest);
                }
            }

            // 处理响应
            private static async Task<Response<T>> ProcessResponse<T>(HttpResponseMessage response)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    return Response<T>.Fail($"HTTP错误: {response.StatusCode}", response.StatusCode);

                try
                {
                    var result = JsonSerializer.Deserialize<T>(content);
                    return Response<T>.Success(result, response.StatusCode);
                }
                catch
                {
                    return Response<T>.Fail("响应数据格式错误", response.StatusCode);
                }
            }

            public void Dispose() => _sharedClient.Value.Dispose();
        }

        // 响应包装类
        public class Response<T>
        {
            //public bool Success { get; set; }
            public T Data { get; set; }
            public string Error { get; set; }
            public HttpStatusCode StatusCode { get; set; }

            public static Response<T> Success(T data, HttpStatusCode statusCode) => new()
            {
                //Success = true,
                Data = data,
                StatusCode = statusCode
            };

            public static Response<T> Fail(string error, HttpStatusCode statusCode) => new()
            {
                //Success = false,
                Error = error,
                StatusCode = statusCode
            };
        }
    }
}
