using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverallAuthWPF.APIHelper
{
    public class ApiResponse<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }      // 状态码（如200）

        [JsonProperty("msg")]
        public string Message { get; set; } // 消息（如"获取成功"）

        [JsonProperty("data")]
        public T Data { get; set; }         // 核心业务数据（泛型）

        // 辅助属性：快速判断请求是否成功
        public bool IsSuccess => Code >= 200 && Code < 300;
    }


}
