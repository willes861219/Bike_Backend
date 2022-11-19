using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bike_Backend.Models
{
    /// <summary>
    /// 採購單車
    /// </summary>
    public class PurchaseBikeModel
    {
        /// <summary>
        /// 採購流水號ID
        /// </summary>
        public int PurchaseBikeID { get; set; } 
        /// <summary>
        /// 單車型號
        /// </summary>
        public string BikeSN { get; set; }
        /// <summary>
        /// 單車品名
        /// </summary>
        public string BikeName { get; set; }
        /// <summary>
        /// 車種
        /// </summary>
        public string BikeModel { get; set; }
        /// <summary>
        /// 廠商
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// 採購數量
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        /// 採購價格 (單件)
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 採購日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 採購狀態 (0 正常, 1 作廢)
        /// </summary>
        public string PurchaseStatus{ get; set; } 

    }
}
