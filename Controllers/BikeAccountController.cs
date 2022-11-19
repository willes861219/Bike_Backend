using Bike_Backend.Function;
using Bike_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Bike_Backend.ViewModels;
using static Bike_Backend.Function.TSQLModel;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bike_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BikeAccountController : ControllerBase
    {
        MethodList methodList = new MethodList(); // 取得自訂方法
        Connection cnClass = new Connection(); // 取得資料庫連線資料

        /// <summary>
        /// 取得所有帳戶資訊，限制admin帳號可操作
        /// </summary>
        /// <returns></returns>
        // GET: api/<BikeAccountController>
        [Authorize(Roles = "admin")] //限制admin帳號可使用
        [HttpGet]
        public IEnumerable<BikeAccountModel> Get()
        {
            using (var cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"select * from BikeAccount";

                var result = cn.Query<BikeAccountModel>(query);

                return result;
            }
        }

        /// <summary>
        /// 查詢是否有指定帳戶存在
        /// </summary>
        /// <param name="Account">要查詢帳戶</param>
        /// <returns></returns>
        // GET api/<BikeAccountController>/5
        [HttpGet("{Account}")]
        public IActionResult Get(string Account)
        {
            using (var cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"select 1 from BikeAccount where Account = @Account";

                var result = cn.ExecuteScalar<bool>(query, new { Account = Account });

                if (result)
                {
                    return Ok("有帳號");
                }
                else
                {
                    return BadRequest("無帳號");
                }
            }
        }

        /// <summary>
        /// 建立帳號
        /// </summary>
        /// <param name="model"></param>
        // POST api/<BikeAccountController>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody] BikeAccountViewModel model)
        {
            if (methodList.checkAccount(model.Account))
            {
                return BadRequest("已有帳號存在，請變更帳號名稱");
            };

            var hashPwd = methodList.SHA256(model.Password);

            using (var cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"INSERT INTO [dbo].[BikeAccount]
                                       ([Account]
                                       ,[Password]
                                       ,[Name])
                                 VALUES
                                       (@Account
                                       ,@Password
                                       ,@Name)";
                query = GetQuery(query, false); // 加入自訂TSQL語法
                cn.Execute(query,
                    new
                    {
                        Account = model.Account,
                        Password = hashPwd,
                        Name = model.Name
                    });

                return Ok("已新增帳戶至資料庫");
            }
        }

        /// <summary>
        /// 修改帳戶的密碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // PUT api/<BikeAccountController>
        [HttpPut]
        public IActionResult Put([FromBody] AccountChangeViewModel model)
        {
            if (!methodList.checkOriginPassword(model.Account, model.OldPwd))
            {
                return BadRequest("密碼不符合"); //如果比對失敗直接回傳BadRequest
            };

            var hashPwd = methodList.SHA256(model.NewPwd); // 對新密碼做雜湊

            using (var cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"UPDATE [dbo].[BikeAccount]
                                SET  [Password] = @HashPwd
                                WHERE Account = @Account";
                query = GetQuery(query, false); // 加入自訂TSQL語法
                cn.Execute(query, new
                {
                    Account = model.Account,
                    HashPwd = hashPwd  //將新密碼傳入資料庫
                });

                return Ok("已成功修改密碼");
            }
        }

        /// <summary>
        /// 刪除帳號，限制admin可使用
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<BikeAccountController>/5
        [Authorize(Roles = "admin")] //限制admin帳號可使用
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using(var cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"delete from BikeAccount where id = @id";

                query = GetQuery(query, false); // 帶入自訂TSQL語法
                cn.Execute(query, new { id = id });
            }
        }
    }
}
