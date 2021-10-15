using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Model;
using DAL.Repositories;
using BLL;
using BLL.Redis;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoccerCountryController : ControllerBase
    {
        private static List<SoccerCountry> SoccerCountryList = new List<SoccerCountry>();

        private readonly ILogger<SoccerCountryController> _logger;
        private readonly SoccerCountryService _soccerCountryService;
        private readonly IMapper _mapper;
        public SoccerCountryController(ILogger<SoccerCountryController> logger, IUnitOfWork uow, IRedisService redis)
        {
            _logger = logger;
            _soccerCountryService = new SoccerCountryService(uow, redis);
        }

        /// <summary>
        /// Get all soccer countries
        /// </summary>
        /// <response code="200">Request ok.</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<SoccerCountry>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> GetAllCountriesAsync()
        {
            List<SoccerCountry> result = await _soccerCountryService.GetAllSoccerCountryAsync();
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Get soccer country by id
        /// </summary>
        /// <response code="200">Request ok.</response>
        /// <response code="404">Request not found.</response>
        [HttpGet]
        [Route("{country_id}")]
        [ProducesResponseType(typeof(SoccerCountry), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<ActionResult> GetCountryByIdAsync([FromRoute] int country_id)
        {
            SoccerCountry result = await _soccerCountryService.GetSoccerCountryByIdAsync(country_id);
            if (result != null)
            {
                return new OkObjectResult(result);
            }
            else
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// Insert soccer country
        /// </summary>
        /// <response code="200">Request ok.</response>
        /// <response code="404">Request not found.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(SoccerCountry), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> CreateCountryAsync([FromBody] SoccerCountry soccerCountry)
        {
            try
            {
                await _soccerCountryService.CreateSoccerCountryAsync(soccerCountry);
                return new OkResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// Update population soccer country by id
        /// </summary>
        /// <response code="200">Request ok.</response>
        /// <response code="404">Request not found.</response>
        [HttpPut]
        [Route("{country_id}")]
        [ProducesResponseType(typeof(SoccerCountry), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<ActionResult> UpdateCountryAsync([FromRoute] int country_id, [FromBody] SoccerCountry soccerCountry)
        {
            SoccerCountry result = await _soccerCountryService.GetSoccerCountryByIdAsync(country_id);
            if (result != null)
            {
                await _soccerCountryService.UpdateSoccerCountryAsync(soccerCountry);
                return new OkResult();
            }
            else
            {
                return new NotFoundResult();
            }
        }


        /// <summary>
        /// Delete soccer team by id
        /// </summary>
        /// <response code="200">Request ok.</response>
        /// <response code="405">Request not found.</response>
        [HttpDelete]
        [Route("{country_id}")]
        [ProducesResponseType(typeof(SoccerTeam), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<ActionResult> DeleteCountryById([FromRoute] int country_id)
        {
            await _soccerCountryService.DeleteSoccerCountryAsync(country_id);
            return new OkResult();
        }
    }
}
