using Bike_Backend.Function;
using Bike_Backend.Models;
using Bike_Backend.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bike_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BikeModelController : ControllerBase
    {
        Connection cnClass = new Connection();
        MethodList methodList = new MethodList();


        /// <summary>
        /// 取得單車類型
        /// </summary>
        /// <returns></returns>
        // GET: api/<BikeModelController>
        [HttpGet]
        public IEnumerable<BikeModel> Get()
        {
            using (var cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"select * from BikeModel";

                var result = cn.Query<BikeModel>(query);

                return result;
            }
        }


        /// <summary>
        /// 取得對應ID的單車類型
        /// </summary>
        /// <param name="id">編號</param>
        /// <returns></returns>
        // GET api/<BikeModelController>/5
        [HttpGet("{id}")]
        public IEnumerable<BikeModel> Get(int id)
        {
            using (var cn = new SqlConnection(cnClass.AzureCn)) //連線資料庫
            {
                string query = @"select * from BikeModel where BikeModelID = @id";
                var result = cn.Query<BikeModel> (query, new { id = id });

                return result; // 回傳對應採購編號的單車資料
            }
        }

        /// <summary>
        /// 新增單車類型
        /// </summary>
        /// <param name="model"></param>
        // POST api/<BikeModelController>
        [HttpPost]
        public void Post([FromBody] BikeViewModel model)
        {
            using (SqlConnection cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"INSERT INTO [dbo].[BikeModel]
                                       ([Model])
                                 VALUES
                                       (@Model)";

                query = methodList.GetQuery(query, false); //加入自訂TSQL語法
                cn.Execute(query,
                    new
                    {
                        Model = model.Model,
                    });
            };
        }

        /// <summary>
        /// 修改對應編號的類型資料
        /// </summary>
        /// <param name="id">編號</param>
        /// <param name="model"></param>
        // PUT api/<BikeModelController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] BikeViewModel model)
        {
            using (SqlConnection cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"Update BikeModel set Model = @Model
                                 where BikeModelID = @BikeModelID";

                query = methodList.GetQuery(query, false); //加入自訂TSQL語法
                cn.Execute(query, new { Model = model.Model, BikeModelID = id, });
            };
        }

        /// <summary>
        /// 暫無需要(無設定)
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<BikeModelController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
