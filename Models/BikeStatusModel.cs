using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bike_Backend.Models
{
    /// <summary>
    /// 單車狀態資料
    /// </summary>
    public class BikeStatusModel
    {
        /// <summary>
        /// 序列編號
        /// </summary>
        public int BikeStatusID { get; set; }
        /// <summary>
        /// 對應PurchaseBike的序列編號(PurchaseBikeID)
        /// </summary>
        public int PurchaseBikeID { get; set; }

        /// <summary>
        /// 租借狀態 RentalStatus ( 0 表示未出借 , 1 表示已出借)
        /// </summary>
        public bool RentalStatus { get; set; }

        /// <summary>
        /// 車況 BikeStatus( null 表示正常, 0 表示送修中, 1 表示報廢)
        /// </summary>
        public bool? BikeStauts { get; set; }

    }
}
