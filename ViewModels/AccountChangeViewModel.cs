using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bike_Backend.ViewModels
{
    /// <summary>
    /// 使用者輸入變更帳戶資料
    /// </summary>
    public class AccountChangeViewModel
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 原密碼
        /// </summary>
        public string OldPwd { get; set; }

        /// <summary>
        /// 新密碼
        /// </summary>
        public string NewPwd { get; set; }
    }
}
