using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bike_Backend.Models
{
    /// <summary>
    /// 使用者資料
    /// </summary>
    public class UserDataModel
    {
        /// <summary>
        /// JWT Token
        /// </summary>
        public string JwtToken { get; set; } 
        /// <summary>
        /// 帳戶的使用者名稱
        /// </summary>
        public string Name { get; set; }
    }
}
