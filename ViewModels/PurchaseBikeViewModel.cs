using System;

namespace Bike_Backend.ViewModels
{
    /// <summary>
    /// 使用者輸入採購單車資料
    /// </summary>
    public class PurchaseBikeViewModel
    {
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
        public int Quantity { get; set; }
        /// <summary>
        /// 採購價格 (單件)
        /// </summary>
        public Decimal Price { get; set; }
        /// <summary>
        /// 採購日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 採購狀態 (0 True正常, 1 False作廢)
        /// </summary>
        public bool PurchaseStatus{ get; set; }   

    }
}
