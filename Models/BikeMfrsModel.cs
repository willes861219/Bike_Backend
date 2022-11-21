using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bike_Backend.Models
{
    /// <summary>
    /// 廠商資料
    /// </summary>
    public class BikeMfrsModel
    {
        /// <summary>
        /// 序列編號
        /// </summary>
        public int BikeManufacturerID { get; set; }
        /// <summary>
        /// 廠商
        /// </summary>
        public string  Manufacturer { get; set; }
    }
}
