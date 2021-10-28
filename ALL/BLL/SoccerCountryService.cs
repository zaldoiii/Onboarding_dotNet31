using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BLL.Messaging;
using BLL.Redis;
using DAL.Model;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL
{
    public class SoccerCountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redis;

        public SoccerCountryService(IUnitOfWork unitOfWork, IRedisService redis)
        {
            _unitOfWork = unitOfWork;
            _redis = redis;
        }

        public async Task<List<SoccerCountry>> GetAllSoccerCountryAsync()
        {
            return await _unitOfWork.SoccerCountryRepository.GetAll().Include(X => X.Teams).ToListAsync();
        }


        public async Task<SoccerCountry> GetSoccerCountryByIdAsync(int country_id)
        {
            SoccerCountry soccerCountry = await _redis.GetAsync<SoccerCountry>($"SoccerCountry_CountryId: {country_id}");

            if (soccerCountry == null)
            {
                soccerCountry = await _unitOfWork.SoccerCountryRepository.GetAll()
                    .Include(X => X.Teams)
                    .FirstOrDefaultAsync(Y => Y.CountryId == country_id);

                await _redis.SaveAsync($"SoccerCountry_CountryId: {country_id}", soccerCountry);
            }

            return soccerCountry;
        }


        public async Task CreateSoccerCountryAsync(SoccerCountry soccerCountry)
        {
            bool isExist = _unitOfWork.SoccerCountryRepository.IsExist(X => X.CountryAbbrev == soccerCountry.CountryAbbrev);
            if (isExist)
            {
                throw new Exception($"Soccer country with abbreviation {soccerCountry.CountryAbbrev} already exist");
            }
            await _unitOfWork.SoccerCountryRepository.AddAsync(soccerCountry);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateSoccerCountryAsync(SoccerCountry soccerCountry)
        {
            SoccerCountry soccerCountryQuery = await _unitOfWork.SoccerCountryRepository.GetSingleAsync(X => X.CountryId == soccerCountry.CountryId);
            if (soccerCountryQuery == null)
            {
                throw new Exception($"Soccer country with id {soccerCountry.CountryId} not exist");
            }

            _unitOfWork.SoccerCountryRepository.Edit(soccerCountry);
            await _unitOfWork.SaveAsync();
            await _redis.DeleteAsync($"SoccerCountry_CountryId: {soccerCountry.CountryId}");
        }

        public async Task DeleteSoccerCountryAsync(int country_id)
        {
            _unitOfWork.SoccerCountryRepository.Delete(X => X.CountryId == country_id);
            await _unitOfWork.SaveAsync();
            await _redis.DeleteAsync($"soccerCountry_CountryId: {country_id}");
        }

    }
}
