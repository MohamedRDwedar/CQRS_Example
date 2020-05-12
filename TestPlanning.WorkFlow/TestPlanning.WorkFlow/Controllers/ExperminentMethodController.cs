using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestPlanning.WorkFlow.Models;
using TestPlanning.WorkFlow.Service;

namespace TestPlanning.WorkFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperminentMethodController : ControllerBase
    {
        private readonly ExperimentMethodSagaService _experimentMethodSagaService;
        public ExperminentMethodController(ExperimentMethodSagaService experimentMethodSagaService)
        {
            _experimentMethodSagaService = experimentMethodSagaService;
        }

        // POST: api/Method
        [HttpPost]
        public Result ResultPost([FromBody] ExperimentMethodModel value)
        {
            return _experimentMethodSagaService.CreateExperminentMethodSaga(value);
        }


        // PUT: api/Method/5
        [HttpPut("{id}")]
        public async Task<Result> Put(long id, [FromBody] ExperimentMethodModel value)
        {
            return await _experimentMethodSagaService.UpdateExperminentMethodSaga(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{methodId}/{experminentId}")]
        public async Task<Result> Delete([FromBody] ExperimentMethodModel value)
        {
            return await _experimentMethodSagaService.DeleteExperminentMethodSaga(value);
        }
    }
}