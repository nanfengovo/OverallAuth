using Newtonsoft.Json;
using OverallAuthWPF.APIHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps;

namespace OverallAuthWPF.APIHelper
{
    public class ApiService
    {
        private static readonly HttpClient _client = new HttpClient();

        public async Task<List<User>> GetDataAsync()
        {
            try
            {
                // 一次性获取所有数据
                var response = await _client.GetAsync("http://127.0.0.1:5141/api/OverallAuth/GetAllUser");
                response.EnsureSuccessStatusCode(); // 确保HTTP请求成功

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResponse<List<User>>>(json);

                // 验证业务状态码
                if (result.Code != 200)
                {
                    throw new Exception($"API错误: {result.Message} (Code: {result.Code})");
                }

                return result.Data; // 修复：返回 ApiResponse 的 Data 属性
            }
            catch (HttpRequestException ex)
            {
                // 网络层异常处理
                throw new Exception($"网络请求失败: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                // 数据解析异常
                throw new Exception($"数据解析失败: {ex.Message}", ex);
            }
        }
    }
}
