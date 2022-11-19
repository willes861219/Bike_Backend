using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bike_Backend.ViewModels
{       
    /// <summary>
    /// 使用者登入帳號、密碼
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 使用者密碼
        /// </summary>
        public string Password { get; set; }
    }
}
