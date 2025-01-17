﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper_ORM.Models;
using Dapper_ORM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_ORM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IDapper _dapper;
        public HomeController(IDapper dapper)
        {
            _dapper = dapper;
        }
        [HttpPost(nameof(Create))]
        public async Task<int> Create(Parameters data)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("BlogId", data.BlogId, DbType.Int32);
            var result = await Task.FromResult(_dapper.Insert<int>("[dbo].[SP_Add_Article]"
                , dbparams,
                commandType: CommandType.StoredProcedure));
            return result;
        }
        [HttpGet(nameof(GetById))]
        public async Task<Parameters> GetById(int BlogId)
        {
            var result = await Task.FromResult(_dapper.Get<Parameters>($"Select * from [Blog] where BlogId = {BlogId}", null, commandType: CommandType.Text));
            return result;
        }
        [HttpDelete(nameof(Delete))]
        public async Task<int> Delete(int BlogId)
        {
            var result = await Task.FromResult(_dapper.Execute($"Delete [Blog] where BlogId = {BlogId}", null, commandType: CommandType.Text));
            return result;
        }
        [HttpGet(nameof(Count))]
        public Task<int> Count(int num)
        {
            var totalcount = Task.FromResult(_dapper.Get<int>($"select COUNT(*) from [Blog] WHERE Url like '%{num}%'", null,
                    commandType: CommandType.Text));
            return totalcount;
        }
        [HttpPatch(nameof(Update))]
        public Task<int> Update(Parameters data)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("BlogId", data.BlogId);
            dbPara.Add("Url", data.Url, DbType.String);

            var updateArticle = Task.FromResult(_dapper.Update<int>("[dbo].[SP_Update_Article]",
                            dbPara,
                            commandType: CommandType.StoredProcedure));
            return updateArticle;
        }
    }
}