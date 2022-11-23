using Bike_Backend.Function;
using Bike_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Dapper;
using Bike_Backend.ViewModels;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bike_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BikeStatusController : ControllerBase
    {
        Connection cnClass = new Connection(); // 取得資料庫連線資料
        MethodList methodList = new MethodList();

        /// <summary>
        /// 取得所有單車資料
        /// </summary>
        /// <returns></returns>
        // GET: api/<BikeStatusController>
        [HttpGet]
        public IEnumerable<BikeStatusModel> Get()
        {
            using (var cn = new SqlConnection(cnClass.AzureCn)) //連線資料庫
            {
                string query = @"select bike.BikeStatusID,bike.PurchaseBikeID,pur.BikeName,
                                pur.Manufacturer,bike.RentalStatus,bike.BikeStatus 
                                from BikeStatus bike
                                left join PurchaseBike pur on pur.PurchaseBikeID = bike.PurchaseBikeID";
                var result = cn.Query<BikeStatusModel>(query);

                return result; // 回傳所有單車資料
            }
        }

        /// <summary>
        /// 取得對應採購編號{purchaseBikeID}的單車資料
        /// </summary>
        /// <param name="id">採購編號</param>
        /// <returns></returns>
        // GET api/<BikeStatusController>/5
        [HttpGet("{id}")]
        public IEnumerable<BikeStatusModel> Get(int id)
        {
            using (var cn = new SqlConnection(cnClass.AzureCn)) //連線資料庫
            {
                string query = @"select * from BikeStatus where purchaseBikeID = @id";
                var result = cn.Query<BikeStatusModel>(query, new { id = id });

                return result; // 回傳對應採購編號的單車資料
            }
        }

        /// <summary>
        /// 新增新的單車資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST api/<BikeStatusController>
        [HttpPost]
        public void Post([FromBody] BikeStatusViewModel model)
        {
            using (var cn = new SqlConnection(cnClass.AzureCn)) //連線資料庫
            {
                string query = @"INSERT INTO [dbo].[BikeStatus]
                                       ([PurchaseBikeID]
                                       ,[RentalStatus]
                                       ,[BikeStatus])
                                 VALUES
                                       (@PurchaseBikeID
                                       ,@RentalStatus
                                       ,@BikeStatus)";
                query = methodList.GetQuery(query,false); //加入自訂TSQL交易
                cn.Execute(query, new
                {
                    PurchaseBikeID = model.PurchaseBikeID,
                    RentalStatus = model.RentalStatus,
                    BikeStatus = model.BikeStatus
                });
            }
        }

        /// <summary>
        /// 修改單車資料，暫時關閉－限制admin帳號可使用功能
        /// </summary>
        /// <param name="id">單車編號</param>
        /// <param name="model"></param>
        // PUT api/<BikeStatusController>/5
        //[Authorize(Roles = "admin")] //限制admin帳號可使用
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] BikeStatusViewModel model)
        {
            using (var cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"UPDATE [dbo].[BikeStatus]
                                 SET [PurchaseBikeID] = @PurchaseBikeID
                                 ,[RentalStatus] = @RentalStatus
                                 ,[BikeStatus] = @BikeStatus
                                 WHERE BikeAccountID = @id ";

                query = methodList.GetQuery(query, false); //加入自訂TSQL交易
                cn.Execute(query, new
                {
                    id = id,
                    PurchaseBikeID = model.PurchaseBikeID,
                    RentalStatus = model.RentalStatus,
                    BikeStatus = model.BikeStatus,
                });
            }
        }

        /// <summary>
        /// 刪除指定編號的單車資料，限制admin帳號可使用
        /// </summary>
        /// <param name="id">單車編號</param>
        // DELETE api/<BikeStatusController>/5
        [Authorize(Roles = "admin")] //限制admin帳號可使用
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"delete from [dbo].[BikeStatus]
                                 WHERE BikeAccountID = @id";

                query = methodList.GetQuery(query, false); //加入自訂TSQL交易
                cn.Execute(query, new
                {
                    id = id,
                });
            }
        }
    }
}
