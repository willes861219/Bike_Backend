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

                return !result;
            }
        }
    }
}
