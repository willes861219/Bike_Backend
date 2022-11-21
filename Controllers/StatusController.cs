using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bike_Backend.Controllers
{
    /// <summary>
    /// 拿來驗證Token是否過期
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        /// <summary>
        /// 驗證Token
        /// </summary>
        [HttpGet]
        public bool status()
        {
            return true;
        }
    }
}
