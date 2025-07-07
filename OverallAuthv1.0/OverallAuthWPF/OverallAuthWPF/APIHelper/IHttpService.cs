using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OverallAuthWPF.APIHelper
{
    public interface IHttpService
    {
        Task<ApiResponse<T>> GetAsync<T>(string endpoint, CancellationToken ct = default);
        Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data, CancellationToken ct = default);
        //void SetBearerToken(string token); // 认证令牌管理
    }
}
