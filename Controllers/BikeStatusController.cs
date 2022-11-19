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
    /// <summary>
    /// 目前單車狀態
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BikeStatusController : ControllerBase
    {
        Connection cnClass = new Connection();

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
                string query = @"select * from BikeStatus";
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

                //Dapper-TSQL交易
                using (var tran = cn.BeginTransaction())
                {
                    cn.Execute(query, new
                    {
                        PurchaseBikeID = model.PurchaseBikeID,
                        RentalStatus = model.RentalStatus,
                        BikeStatus = model.BikeStatus
                    });
                    tran.Commit(); //確認交易
                }

            }
        }

        /// <summary>
        /// 修改單車資料
        /// </summary>
        /// <param name="id">單車編號</param>
        /// <param name="model"></param>
        // PUT api/<BikeStatusController>/5
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

                //Dapper-TSQL交易
                using (var tran = cn.BeginTransaction())
                {
                    cn.Execute(query, new
                    {
                        id = id,
                        PurchaseBikeID = model.PurchaseBikeID,
                        RentalStatus = model.RentalStatus,
                        BikeStatus = model.BikeStatus,
                    });

                    tran.Commit();   //確認交易
                }
            }
        }

        // DELETE api/<BikeStatusController>/5
        [HttpDelete("{id}")]

        public void Delete(int id)
        {
        }
    }
}
