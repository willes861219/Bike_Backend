using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Bike_Backend.Models;
using Microsoft.Data.SqlClient;
using Bike_Backend.Function;
using System.Collections.Generic;
using Bike_Backend.ViewModels;

namespace Bike_Backend.Controllers
{
    [Authorize]
    [ApiController]
    public class TokenController : ControllerBase
    {
           MethodList methodList = new MethodList();
        Connection cnClass = new Connection();

        private readonly JwtHelpers jwt;
        public TokenController(JwtHelpers jwt)
        {
            this.jwt = jwt;
        }

        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("~/signin")]
        public ActionResult<UserDataModel> SignIn(LoginViewModel login)
        {
            if (ValidateUser(login))
            {
                UserDataModel userData = new UserDataModel();
                using(var cn = new SqlConnection(cnClass.AzureCn))
                {
                    string query = @"SELECT Name FROM BikeAccount WHERE Account = @Account";
                    var result = cn.Query<UserDataModel>(query, new { Account = login.Username }).ToList();

                    userData.JwtToken = jwt.GenerateToken(login.Username);
                    userData.Name = result[0].Name;

                    return userData;
                }
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 驗證用戶是否存在資料庫
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        private bool ValidateUser(LoginViewModel login)
        {
            var newPwd = methodList.SHA256(login.Password); //先取得雜湊
            using (var cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"SELECT 1 FROM BikeAccount WHERE Account = @Account AND Password = @Password";

                var result = cn.ExecuteScalar<bool>(query, new { Account = login.Username, Password = newPwd });
                
                return result;
            }
        }

        [HttpGet("~/claims")]
        public IActionResult GetClaims()
        {
            return Ok(User.Claims.Select(p => new { p.Type, p.Value }));
        }

        [HttpGet("~/username")]
        public IActionResult GetUserName()
        {
            return Ok(User.Identity.Name);
        }

        [HttpGet("~/jwtid")]
        public IActionResult GetUniqueId()
        {
            var jti = User.Claims.FirstOrDefault(p => p.Type == "jti");
            return Ok(jti.Value);
        }
    }
}