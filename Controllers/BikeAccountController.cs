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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bike_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeAccountController : ControllerBase
    {
        Connection cnClass = new Connection();
        MethodList methodList = new MethodList();

        // GET: api/<BikeAccountController>
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

                using(var tran = cn.BeginTransaction())
                {
                    cn.Execute(query,
                       new
                       {
                           Account = model.Account,
                           Password = hashPwd,
                           Name = model.Name
                       });
                    tran.Commit();
                }

                return Ok("已新增帳戶至資料庫");
            }
        }

        // PUT api/<BikeAccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BikeAccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
