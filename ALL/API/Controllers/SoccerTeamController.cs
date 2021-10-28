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
using BLL.Messaging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoccerTeamController : ControllerBase
    {
        private static List<SoccerTeam> SoccerTeamList = new List<SoccerTeam>();

        private readonly ILogger<SoccerTeamController> _logger;
        private readonly SoccerTeamService _soccerTeamService;
        private readonly IMapper _mapper;
        public SoccerTeamController(ILogger<SoccerTeamController> logger, IUnitOfWork uow, IConfiguration configuration, IMessageSenderFactory msgSernderFactory)
        {
            _logger = logger;
            //_mapper = mapper;
            _soccerTeamService = new SoccerTeamService(uow, configuration, msgSernderFactory);
        }

        /// <summary>
        /// Get all teams
        /// </summary>
        /// <response code="200">Request ok.</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<SoccerTeam>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> GetAllTeamsAsync()
        {
            List<SoccerTeam> result = await _soccerTeamService.GetAllSoccerTeamAsync();
            //List<SoccerTeam> mappedResult = _mapper.Map<List<SoccerTeam>>(result);

            return new OkObjectResult(result);
        }

        /// <summary>
        /// Get soccer team by id
        /// </summary>
        /// <response code="200">Request ok.</response>
        /// <response code="404">Request not found.</response>
        [HttpGet]
        [Route("{team_id}")]
        [ProducesResponseType(typeof(SoccerTeam), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<ActionResult> GetTeamByIdAsync([FromRoute]int team_id)
        {
            SoccerTeam result = await _soccerTeamService.GetSoccerTeamByIdAsync(team_id);
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
        /// Insert soccer team
        /// </summary>
        /// <response code="200">Request ok.</response>
        /// <response code="404">Request not found.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(SoccerTeam), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> CreateTeamAsync([FromBody]SoccerTeam soccerTeam)
        {
            try
            {
                await _soccerTeamService.CreateSoccerTeamAsync(soccerTeam);
                return new OkResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// Update soccer team name
        /// </summary>
        /// <response code="200">Request ok.</response>
        /// <response code="404">Request not found.</response>
        /*[HttpPut]
        [Route("{team_id}")]
        [ProducesResponseType(typeof(SoccerTeam), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public ActionResult UpdateTeamNameById([FromRoute]int team_id, [FromBody]string _TeamName)
        {
            var isExist = SoccerTeamList.Where(X => X.TeamId == team_id).Any();
            if (isExist)
            {
                SoccerTeamList.Where(X => X.TeamId == team_id).ToList().ForEach(M => M.TeamName = _TeamName); ;
                return new OkResult();
            }
            else
            {
                return new NotFoundResult();
            }
        }*/


        /// <summary>
        /// Delete soccer team by id
        /// </summary>
        /// <response code="200">Request ok.</response>
        /// <response code="405">Request not found.</response>
        [HttpDelete]
        [Route("{team_id}")]
        [ProducesResponseType(typeof(SoccerTeam), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<ActionResult> DeleteTeamByIdAsync([FromRoute]int team_id)
        {
            await _soccerTeamService.DeleteSoccerTeamAsync(team_id);
            return new OkResult();
        }
    }
}
