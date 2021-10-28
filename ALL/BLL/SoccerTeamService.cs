using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Redis;
using BLL.Messaging;
using DAL.Model;
using DAL.Repositories;

namespace BLL
{
    public class SoccerTeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IRedisService _redis;
        private readonly IConfiguration _config;
        private readonly IMessageSenderFactory _msgSernderFactory;

        public SoccerTeamService(IUnitOfWork unitOfWork, IConfiguration config, IMessageSenderFactory msgSernderFactory)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _msgSernderFactory = msgSernderFactory;
        }

        public async Task<List<SoccerTeam>> GetAllSoccerTeamAsync()
        {
            return await _unitOfWork.SoccerTeamRepository
                .GetAll()
                //.Include(X => X.Country)
                .ToListAsync();
        }


        public async Task<SoccerTeam> GetSoccerTeamByIdAsync(int team_id)
        {
            return await _unitOfWork.SoccerTeamRepository
                .GetAll()
                //.Include(X => X.CountryId)
                .FirstOrDefaultAsync(X => X.TeamId == team_id);
        }


        public async Task CreateSoccerTeamAsync(SoccerTeam soccerTeam)
        {
            bool isExist = _unitOfWork.SoccerTeamRepository.IsExist(X => X.TeamId == soccerTeam.TeamId);
            bool isCountryExist = _unitOfWork.SoccerCountryRepository.IsExist(X => X.CountryId == soccerTeam.CountryId);
            if (!isExist && isCountryExist)
            {
                _unitOfWork.SoccerTeamRepository.Add(soccerTeam);
                await _unitOfWork.SaveAsync();

                await SendToEventHub(soccerTeam);
            }
            else
            {
                throw new Exception($"Soccer Team with {soccerTeam.TeamId} already exist");
            }
        }

        private async Task SendToEventHub(SoccerTeam soccerTeam)
        {
            string topic = _config.GetValue<string>("EventHub:EventHubNameTest");

            //create event hub producer
            using IMessageSender message = _msgSernderFactory.Create(_config, topic);

            //create batch
            await message.CreateEventBatchAsync();

            //add message, ini bisa banyak sekaligus
            message.AddMessage(soccerTeam);

            //send message
            await message.SendMessage();
        }

        public async Task DeleteSoccerTeamAsync(int team_id)
        {
            _unitOfWork.SoccerTeamRepository.Delete(X => X.TeamId == team_id);
            await _unitOfWork.SaveAsync();
        }

    }
}
