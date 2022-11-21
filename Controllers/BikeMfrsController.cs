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
    public class BikeMfrsController : ControllerBase
    {
        Connection cnClass = new Connection();
        MethodList methodList = new MethodList();

        /// <summary>
        /// 取得廠商資料
        /// </summary>
        /// <returns></returns>
        // GET: api/<BikeManufacturerControllerController>
        [HttpGet]
        public IEnumerable<BikeMfrsModel> Get()
        {
            using (var cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"select * from BikeManufacturer";

                var result = cn.Query<BikeMfrsModel>(query);

                return result;
            }
        }

        /// <summary>
        /// 取得對應ID的廠商資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<BikeManufacturerControllerController>/5
        [HttpGet("{id}")]
        public IEnumerable<BikeMfrsModel> Get(int id)
        {
            using (var cn = new SqlConnection(cnClass.AzureCn)) //連線資料庫
            {
                string query = @"select * from BikeManufacturer where BikeManufacturerID = @id";
                var result = cn.Query<BikeMfrsModel>(query, new { id = id });

                return result; // 回傳對應採購編號的單車資料
            }
        }

        /// <summary>
        /// 新增廠商
        /// </summary>
        /// <param name="model"></param>
        // POST api/<BikeManufacturerControllerController>
        [HttpPost]
        public void Post([FromBody] BikeMfrsViewModel model)
        {
            using (SqlConnection cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"INSERT INTO [dbo].[BikeManufacturer]
                                       ([Manufacturer])
                                 VALUES
                                       (@Manufacturer)";

                query = methodList.GetQuery(query, false); //加入自訂TSQL語法
                cn.Execute(query,
                    new
                    {
                        Manufacturer = model.Manufacturer,
                    });
            };
        }

        /// <summary>
        /// 修改對應編號的廠商資料
        /// </summary>
        /// <param name="id">編號</param>
        /// <param name="model"></param>
        // PUT api/<BikeManufacturerControllerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] BikeMfrsViewModel model)
        {
            using (SqlConnection cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"Update BikeManufacturer set Manufacturer = @Manufacturer
                                 where BikeManufacturerID = @BikeManufacturerID";

                query = methodList.GetQuery(query, false); //加入自訂TSQL語法
                cn.Execute(query, new { Manufacturer = model.Manufacturer, BikeManufacturerID = id, });
            };
        }

        /// <summary>
        /// 暫無需要(無設定)
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<BikeManufacturerControllerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
