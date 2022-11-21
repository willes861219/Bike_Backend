using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Bike_Backend.Models;
using Microsoft.Data.SqlClient;
using Bike_Backend.Function;
using Bike_Backend.ViewModels;

namespace Bike_Backend.Controllers
{
    [Authorize]
    [ApiController]
    public class TokenController : ControllerBase
    {
        MethodList methodList = new MethodList(); // 取得自訂方法
        Connection cnClass = new Connection(); // 取得資料庫連線資料

        private readonly JwtHelpers jwt;
        public TokenController(JwtHelpers jwt)
        {
            this.jwt = jwt;
        }

        /// <summary>
        /// 使用者登入 取得Token資訊
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("~/signin")]
        public ActionResult<UserDataModel> SignIn(LoginViewModel login)
        {
            if (ValidateUser(login)) //驗證帳戶
            {
                UserDataModel userData = new UserDataModel();
                using(var cn = new SqlConnection(cnClass.AzureCn))
                {
                    string query = @"SELECT Name FROM BikeAccount WHERE Account = @Account";
                    var result = cn.Query<UserDataModel>(query, new { Account = login.Username }).ToList();

                    userData.JwtToken = jwt.GenerateToken(login.Username); // 取得JWT Token
                    userData.Name = result[0].Name;  // 取得帳戶的使用者名稱

                    return userData; 
                }
            }
            else
            {
                return BadRequest(); //驗證失敗回傳BadRequest
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
        /// <summary>
        /// 取得所有 Claims 聲明資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet("~/claims")]
        public IActionResult GetClaims()
        {
            return Ok(User.Claims.Select(p => new { p.Type, p.Value }));
        }

        /// <summary>
        /// 取得使用者名稱
        /// </summary>
        /// <returns></returns>
        [HttpGet("~/username")]
        public IActionResult GetUserName()
        {
            return Ok(User.Identity.Name);
        }

        /// <summary>
        /// 取得 jti 聲明值
        /// </summary>
        /// <returns></returns>
        [HttpGet("~/jwtid")]
        public IActionResult GetUniqueId()
        {
            var jti = User.Claims.FirstOrDefault(p => p.Type == "jti");
            return Ok(jti.Value);
        }
    }
}