using Bike_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Bike_Backend.Function;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Bike_Backend.ViewModels;
using static Bike_Backend.Function.TSQLModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bike_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseBikeController : ControllerBase
    {
        MethodList methodList = new MethodList(); // 取得自訂方法
        Connection cnClass = new Connection(); // 取得資料庫連線資料
        /// <summary>
        /// 取得採購單車的所有資料
        /// </summary>
        /// <returns></returns>
        // GET: api/<PurchaseBikeController>
        [HttpGet]
        public IEnumerable<PurchaseBikeModel> Get()
        {
            using (SqlConnection cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"select * from PurchaseBike";

                var result = cn.Query<PurchaseBikeModel>(query);

                return result;

            };
        }
        /// <summary>
        /// 取得對應採購編號的採購資料
        /// </summary>
        /// <param name="id">採購編號</param>
        /// <returns></returns>
        // GET api/<PurchaseBikeController>/5
        [HttpGet("{id}")]
        public IEnumerable<PurchaseBikeModel> Get(int id)
        {
            using (var cn = new SqlConnection(cnClass.AzureCn)) //連線資料庫
            {
                string query = @"select * from PurchaseBike where purchaseBikeID = @id";
                var result = cn.Query<PurchaseBikeModel>(query, new { id = id });

                return result; // 回傳對應採購編號的單車資料
            }
        }

        /// <summary>
        /// 新增採購單
        /// </summary>
        /// <param name="model">輸入資料模型</param>
        // POST api/<PurchaseBikeController>
        [HttpPost]
        public void Post([FromBody] PurchaseBikeViewModel model)
        {
            using (SqlConnection cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"INSERT INTO [dbo].[PurchaseBike]
                                ([BikeSN],[BikeName],[BikeModel],[Manufacturer]
                                ,[Quantity] ,[Price],[Date],[PurchaseStatus])
                            VALUES (@BikeSN,@BikeName,@BikeModel,@Manufacturer
                                ,@Quantity,@Price ,@Date,@PurchaseStatus)";

                query = GetQuery(query, false); //加入自訂TSQL語法
                cn.Execute(query,
                    new
                    {
                        BikeSN = model.BikeSN,
                        BikeName = model.BikeName,
                        BikeModel = model.BikeModel,
                        Manufacturer = model.Manufacturer,
                        Quantity = model.Quantity,
                        Price = model.Price,
                        Date = model.Date,
                        PurchaseStatus = model.PurchaseStatus
                    });
            };
        }

        /// <summary>
        /// 修改採購單狀態是否作廢，限制admin帳號可使用
        /// </summary>
        /// <param name="id">採購編號</param>
        /// <param name="Status">false正常 true作廢</param>
        // PUT api/<PurchaseBikeController>/5
        [Authorize(Roles = "admin")] //限制admin帳號可使用
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] bool Status)
        {
            using (SqlConnection cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"Update PurchaseBike set PurchaseStatus = @PurchaseStatus
                                 where PurchaseBikeID = @PurchaseBikeID";
                
                query = GetQuery(query, false); //加入自訂TSQL語法
                cn.Execute(query,new {  PurchaseStatus = Status,   PurchaseBikeID = id,});
            };
        }
        /// <summary>
        /// 採購不可刪除，只可作廢 (因此無設定)
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<PurchaseBikeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
