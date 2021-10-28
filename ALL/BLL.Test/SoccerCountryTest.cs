using FluentAssertions;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using BLL;
using BLL.Redis;
using DAL.Model;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BLL.Test
{
    public class SoccerCountryTest
    {
        private readonly IEnumerable<SoccerCountry> soccerCountries;
        private readonly Mock<IRedisService> redis;
        private readonly Mock<IUnitOfWork> uow;
        
        public SoccerCountryTest()
        {
            soccerCountries = CommonHelper.LoadDataFromFile<IEnumerable<SoccerCountry>>(@"SoccerCountryData.json");
            redis = MockRedis();
            uow = MockUnitOfWork();
        }

        private SoccerCountryService CreateSoccerCountryService()
        {
            return new SoccerCountryService(uow.Object, redis.Object);
        }

        private Mock<IRedisService> MockRedis()
        {
            var mockRedis = new Mock<IRedisService>();

            mockRedis
                .Setup(X => X.GetAsync<SoccerCountry>(It.Is<string>(X => X.Equals("SoccerCountry_CountryId: 3"))))
                .ReturnsAsync(soccerCountries.FirstOrDefault(X => X.CountryId == 3))
                .Verifiable();

            mockRedis
                .Setup(X => X.SaveAsync(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            mockRedis
              .Setup(X => X.DeleteAsync(It.IsAny<string>())).Verifiable();

            return mockRedis;
        }

        private Mock<IUnitOfWork> MockUnitOfWork()
        {
            var soccerCountryQueryable = soccerCountries.AsQueryable().BuildMock().Object;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.SoccerCountryRepository.GetAll())
                .Returns(soccerCountryQueryable);

            mockUnitOfWork
                .Setup(u => u.SoccerCountryRepository.IsExist(It.IsAny<Expression<Func<SoccerCountry, bool>>>()))
                .Returns((Expression<Func<SoccerCountry, bool>> condition) => soccerCountryQueryable.Any(condition));

            mockUnitOfWork
               .Setup(u => u.SoccerCountryRepository.GetSingleAsync(It.IsAny<Expression<Func<SoccerCountry, bool>>>()))
               .ReturnsAsync((Expression<Func<SoccerCountry, bool>> condition) => soccerCountryQueryable.FirstOrDefault(condition));

            mockUnitOfWork
               .Setup(u => u.SoccerCountryRepository.AddAsync(It.IsAny<SoccerCountry>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((SoccerCountry soccerCountry, CancellationToken token) =>
               {
                   Guid g = Guid.NewGuid();
                   soccerCountry.CountryId = BitConverter.ToInt32(g.ToByteArray(), 0);
                   return soccerCountry;
               });

            mockUnitOfWork
                .Setup(u => u.SoccerCountryRepository.Delete(It.IsAny<Expression<Func<SoccerCountry, bool>>>()))
                .Verifiable();


            mockUnitOfWork
                .Setup(x => x.SaveAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            return mockUnitOfWork;
        }
        
        //#endregion method mock depedencies

        [Fact]
        public async Task GetAllAsync_Success()
        {
            //arrange
            var expected = soccerCountries;

            var svc = CreateSoccerCountryService();

            // act
            var actual = await svc.GetAllSoccerCountryAsync();

            // assert      
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task GetByUserId_Success(int country_id)
        {
            //arrange
            //var id = Guid.Parse(country_id);
            var expected = soccerCountries.First(X => X.CountryId  == country_id);

            var svc = CreateSoccerCountryService();

            //act
            var actual = await svc.GetSoccerCountryByIdAsync(country_id);

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(6)]
        public async Task GetByUserId_InRedis_Success(int country_id)
        {

            //arrange
            var expected = soccerCountries.First(X => X.CountryId == country_id);

            var svc = CreateSoccerCountryService();

            //act
            var actual = await svc.GetSoccerCountryByIdAsync(country_id);

            //assert
            actual.Should().BeEquivalentTo(expected);

            redis.Verify(x => x.GetAsync<SoccerCountry>($"SoccerCountry_CountryId: {country_id}"), Times.Once);
            redis.Verify(x => x.SaveAsync($"SoccerCountry_CountryId: {country_id}", It.IsAny<SoccerCountry>()), Times.Never);
        }

        [Theory]
        [InlineData(8)]
        public async Task GetByUserId_NotInRedis_Success(int country_id)
        {
            //arrange
            var expected = soccerCountries.First(X => X.CountryId == country_id);

            var svc = CreateSoccerCountryService();

            //act
            var actual = await svc.GetSoccerCountryByIdAsync(country_id);

            //assert
            actual.Should().BeEquivalentTo(expected);

            redis.Verify(x => x.GetAsync<SoccerCountry>($"SoccerCountry_CountryId: {country_id}"), Times.Once);
            redis.Verify(x => x.SaveAsync($"SoccerCountry_CountryId: {country_id}", It.IsAny<SoccerCountry>()), Times.Once);
        }


        [Fact]
        public async Task CreateUser_Success()
        {
            //arrange
            var expected = new SoccerCountry
            {
                CountryName = "Argentina",
                CountryAbbrev = "ARG"
            };

            var svc = CreateSoccerCountryService();

            //act
            Func<Task> act = async () => { await svc.CreateSoccerCountryAsync(expected); };

            await act.Should().NotThrowAsync<Exception>();

            //assert
            uow.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(3, "England", "ENG")]
        public async Task UpdateUser_Success(int country_id, string country_name, string country_abbrev)
        {
            //arrange
            var expected = new SoccerCountry
            {
                CountryId = country_id,
                CountryName = country_name,
                CountryAbbrev = country_abbrev
            };

            var svc = CreateSoccerCountryService();

            //act
            Func<Task> act = async () => { await svc.UpdateSoccerCountryAsync(expected); };

            //assert
            await act.Should().NotThrowAsync<Exception>();
            uow.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            redis.Verify(x => x.DeleteAsync(It.Is<string>(y => y.Equals($"SoccerCountry_CountryId: {expected.CountryId}"))), Times.Once);
        }

        [Theory]
        [InlineData(999)]
        public async Task UpdateUser_NotFoundAsync(int country_id)
        {
            //arrange
            var expected = new SoccerCountry
            {
                CountryId = country_id
            };

            var svc = CreateSoccerCountryService();

            //act
            Func<Task> act = async () => { await svc.UpdateSoccerCountryAsync(expected); };

            //assert
            await act.Should().ThrowAsync<Exception>().WithMessage($"Soccer country with id {expected.CountryId} not exist");
        }

        [Theory]
        [InlineData(11)]
        public async Task DeleteUser_Success(int country_id)
        {
            //arrange

            var svc = CreateSoccerCountryService();

            //act
            Func<Task> act = async () => { await svc.DeleteSoccerCountryAsync(country_id); };

            await act.Should().NotThrowAsync<Exception>();

            //assert
            uow.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);

            redis.Verify(x => x.DeleteAsync(It.Is<string>(Y => Y.Equals($"soccerCountry_CountryId: {country_id}"))), Times.Once);
        }
    }
}
