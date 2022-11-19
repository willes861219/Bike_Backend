using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Bike_Backend.Function
{
    public class MethodList
    {
        Connection cnClass = new Connection();
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

        /// <summary>
        /// 檢查資料庫是否有帳號存在
        /// </summary>
        /// <param name="account">要查詢的帳號</param>
        /// <returns>True or False</returns>
        public bool checkAccount(string account)
        {
            using (var cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"select 1 from BikeAccount where Account = @Account";

                var result = cn.ExecuteScalar<bool>(query, new { Account = account });

                return result;
            }
        }

        /// <summary>
        /// 檢查輸入的密碼是否符合原始密碼
        /// </summary>
        /// <param name="account">使用者帳號</param>
        /// <param name="pwd">使用者輸入密碼</param>
        /// <returns></returns>
        public bool checkOriginPassword(string account, string pwd)
        {
            var hashPwd = SHA256(pwd);
            using (var cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"selcet 1 from BikeAccount 
                                 where Account = @Account 
                                 and Password = @Password ";

                var result = cn.ExecuteScalar<bool>(query, new { Account = account, Password = hashPwd });

                return result;
            }
        }
    }
}
