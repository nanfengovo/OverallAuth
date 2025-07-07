using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverallAuthWPF.APIHelper.Model
{
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rolesName")]
        public List<string> RoleNames { get; set; } // 角色名称列表

        [JsonProperty("describe")]
        public string Description { get; set; }     // 描述信息

        [JsonProperty("isEnable")]
        public bool IsEnabled { get; set; }         // 是否启用

        [JsonProperty("createTime")]
        public DateTime CreateTime { get; set; }     // 创建时间

        [JsonProperty("updateTime")]
        public DateTime? UpdateTime { get; set; }    // 更新时间（可为空）
    }
}
