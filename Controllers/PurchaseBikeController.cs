﻿using Bike_Backend.Models;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bike_Backend.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseBikeController : ControllerBase
    {
        MethodList methodList = new MethodList(); // 取得自訂方法
        Connection cnClass = new Connection(); // 取得資料庫連線資料

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

        // GET api/<PurchaseBikeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PurchaseBikeController>
        [HttpPost]
        public IEnumerable<PurchaseBikeModel> Post([FromBody] ValueViewModel value)
        {
            using (SqlConnection cn = new SqlConnection(cnClass.AzureCn))
            {
                string query = @"select * from PurchaseBike where PurchaseBikeID = @value";

                var result = cn.Query<PurchaseBikeModel>(query, new { value = value.value });

                return result;
            };
        }

        // PUT api/<PurchaseBikeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PurchaseBikeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public class ValueViewModel
        {
            public string value { get; set; }
        }
    }
}
