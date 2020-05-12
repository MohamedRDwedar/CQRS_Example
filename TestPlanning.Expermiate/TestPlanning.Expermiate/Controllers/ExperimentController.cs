using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestPlanning.Common.Models;
using TestPlanning.Experiment.Services;

namespace TestPlanning.Experiment.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ExperimentController : ControllerBase
    {
        private readonly ExperimentCommandsService _experimentCommandsService;
        private readonly ExperimentHandlersService _experimentHandlersService;
        public ExperimentController(ExperimentCommandsService experimentCommandsService, ExperimentHandlersService experimentHandlersService)
        {
            _experimentCommandsService = experimentCommandsService;
            _experimentHandlersService = experimentHandlersService;
        }

        // https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/query-dsl.html
        // https://www.red-gate.com/simple-talk/dotnet/net-development/how-to-build-a-search-page-with-elasticsearch-and-net/
        // GET: api/Experiment
        [HttpGet]
        public IEnumerable<ExperimentModel> Get()
        {
            return null;
        }

        // GET: api/Experiment/5
        [HttpGet("{id}", Name = "Get")]
        public Result<ExperimentModel> Get(long id)
        {
            return _experimentHandlersService.GetExperiment(id);
        }

        // POST: api/Experiment
        [HttpPost]
        public Result Post([FromBody] ExperimentModel value)
        {
            return _experimentCommandsService.CreateExperimentCommand(value);
        }

        // PUT: api/Experiment/5
        [HttpPut("{id}")]
        public async Task<Result> Put(long id, [FromBody] ExperimentModel value)
        {
            return await _experimentCommandsService.UpadateExperimentCommand(id, value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<Result> Delete([FromBody] ExperimentModel value)
        {
            return await _experimentCommandsService.DeleteExperimentCommand(value);
        }
    }
}