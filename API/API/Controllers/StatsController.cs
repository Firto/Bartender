using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers.Attributes.Validation;
using API.DtoModels;
using API.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/stats/[action]")]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _statsService;
        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet]
        public async Task<ResultDto> Get()
        {
            return ResultDto.Create(_statsService.GetStats().Select(x => new StatDto { Name = x.Name, Points = x.Points }));
        }

        [HttpGet]
        public async Task<ResultDto> Set(
            [FromQuery, ArgValid("str-input", "name")] string name,
            [FromQuery, ArgValid("str-input", "stats")] string stats)
        {
            _statsService.AddStat(name, Int32.Parse(stats));
            return ResultDto.Create(null);
        }

        [HttpGet]
        public async Task<ResultDto> Name(
           [FromQuery, ArgValid("str-input", "name")] string name)
        {
            return ResultDto.Create(null);
        }
    }
}
