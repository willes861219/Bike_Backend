using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bike_Backend.Function
{
    public class MethodList
    {
        /// <summary>
        /// 把使用者輸入的密碼做SHA256雜湊
        /// </summary>
        /// <param name="pwd">密碼</param>
        public string SHA256(string pwd)
        {
            byte[] sha256Bytes = Encoding.UTF8.GetBytes(pwd);
            SHA256Managed sha256 = new SHA256Managed();
            byte[] bytes = sha256.ComputeHash(sha256Bytes);
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
