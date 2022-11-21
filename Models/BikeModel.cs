using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bike_Backend.Models
{
    /// <summary>
    /// 單車類型資料
    /// </summary>
    public class BikeModel
    {
        /// <summary>
        /// 序列編號
        /// </summary>
        public int BikeModelID { get; set; }
        /// <summary>
        /// 類型
        /// </summary>
        public string Model { get; set; }

    }
}
