using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BLL;
using BLL.Redis;
using DAL.Model;
using DAL.Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace External
{
    [ApiController]
    [Route("[controller]")]
    public class ExternalController : ControllerBase
    {
        private readonly SoccerCountryService _soccerCountryService;
        private readonly ILogger<ExternalController> _logger;

        public ExternalController(ILogger<ExternalController> logger, IUnitOfWork uow, IRedisService redis)
        {
            _logger = logger;
            _soccerCountryService = new SoccerCountryService(uow, redis);
        }

        /// <summary>
        /// Get all author
        /// </summary>
        /// <response code="200">Request ok.</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<SoccerCountry>), 200)]
        public async Task<ActionResult> GetAllAsync()
        {
            var result = await _soccerCountryService.GetAllSoccerCountryAsync();

            return new OkObjectResult(result);
        }
    }
}
