using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestPlanning.Common.Helpers;
using TestPlanning.Common.Models;
using TestPlanning.Method.CommandHandlers;
using TestPlanning.Method.Queries;

namespace TestPlanning.Method.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MethodController : ControllerBase
    {
        private readonly Messages _messages;
        public MethodController(Messages messages)
        {
            _messages = messages;
        }

        // GET: api/Method
        [HttpGet]
        public IEnumerable<MethodModel> Get()
        {
            return null;
        }

        // GET: api/Method/5
        [HttpGet("{id}", Name = "Get")]
        public Result<MethodModel> Get(long id)
        {
            return _messages.Dispatch(new GetMethodQuery(id));
        }

        // POST: api/Method
        [HttpPost]
        public async Task<Result> Post([FromBody] MethodModel value)
        {
            Result result = await _messages.Dispatch(new AddMethodCommand(value.Name)); ;
            return result;
        }

        //// PUT: api/Method/5
        //[HttpPut("{id}")]
        //public async Task<Result> Put(long id, [FromBody] MethodModel value)
        //{
        //    return await _messages.Dispatch(new EditMethodCommand(id, value.Name));
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public async Task<Result> Delete(long id)
        //{
        //    return await _messages.Dispatch(new DeleteMethodCommand(id));
        //}
    }
}
