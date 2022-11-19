using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bike_Backend.Models
{
    /// <summary>
    /// 帳戶資料
    /// </summary>
    public class BikeAccountModel
    {
        /// <summary>
        /// 序列編號
        /// </summary>
        public int BikeAccountID { get; set; }
        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
    }
}
