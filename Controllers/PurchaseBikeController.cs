using Bike_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Dapper;
using Bike_Backend.Function;
using Microsoft.AspNetCore.Authorization;

using Bike_Backend.ViewModels;
using System;
using System.Linq;

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
                string query = @"SELECT PurchaseBikeID 
                                ,BikeName 
                                ,BikeModel 
                                ,Manufacturer 
                                ,Quantity 
                                ,Price 
                                ,convert(varchar(10),Date,111) 'Date'
                                ,PurchaseStatus 
                                 FROM PurchaseBike";

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
        /// <param name="model"></param>
        /// <returns>回傳採購編號</returns>
        // POST api/<PurchaseBikeController>
        [HttpPost]
        public int Post([FromBody] PurchaseBikeViewModel model)
        {
            using (SqlConnection cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"INSERT INTO [dbo].[PurchaseBike]
                                ([BikeName],[BikeModel],[Manufacturer]
                                ,[Quantity] ,[Price],[Date],[PurchaseStatus])
                            VALUES (@BikeName,@BikeModel,@Manufacturer
                                ,@Quantity,@Price ,@Date,@PurchaseStatus)
                                SELECT @@IDENTITY";

                query = methodList.GetQuery(query, false); //加入自訂TSQL語法
                var result = cn.Query<int>(query,
                    new
                    {
                        BikeName = model.BikeName,
                        BikeModel = model.BikeModel,
                        Manufacturer = model.Manufacturer,
                        Quantity = model.Quantity,
                        Price = model.Price,
                        Date = model.Date,
                        PurchaseStatus = model.PurchaseStatus,
                    }).ToList();
                return result[0];
            };
        }

        /// <summary>
        /// 修改採購單，限制admin帳號可使用
        /// </summary>
        /// <param name="id">採購編號</param>
        /// <param name="model">false正常 true作廢</param>
        // PUT api/<PurchaseBikeController>/5
        [Authorize(Roles = "admin")] //限制admin帳號可使用
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] PurchaseBikeViewModel model)
        {
            using (SqlConnection cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"UPDATE [dbo].[PurchaseBike]
                                SET [BikeName] = @BikeName
                                    ,[BikeModel] = @BikeModel
                                    ,[Manufacturer] = @Manufacturer
                                    ,[Quantity] = @Quantity
                                    ,[Price] = @Price
                                    ,[Date] = @Date
                                    ,[PurchaseStatus] = @PurchaseStatus
                                 where PurchaseBikeID = @PurchaseBikeID";

                query = methodList.GetQuery(query, false); //加入自訂TSQL語法
                cn.Execute(query,
                    new
                    {
                        BikeName = model.BikeName,
                        BikeModel = model.BikeModel,
                        Manufacturer = model.Manufacturer,
                        Quantity = model.Quantity,
                        Price = model.Price,
                        Date = model.Date,
                        PurchaseStatus = model.PurchaseStatus,
                        PurchaseBikeID = id,
                    });
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
