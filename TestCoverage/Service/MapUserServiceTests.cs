using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyPortfolio.Database.Models;
using MyPortfolio.Database.Repositories;
using MyPortfolio.Services.MapUserService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;

namespace TestCoverage.Service
{
    [TestClass]
    public class MapUserServiceTests
    {
        private readonly string geoLocationDbPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.FullName +
                Path.DirectorySeparatorChar + "MyPortfolio" +
                Path.DirectorySeparatorChar + "GeoLocationDB" +
                Path.DirectorySeparatorChar + "GeoLite2-City.mmdb";

        #region Method: GetUserLocationByIpAddress 

        [TestMethod]
        public void GetUserLocationByIpAddress_WhenGeoLocationDBPathIsNull_ShouldNotCallDb()
        {
            // Setup test environment
            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            mapUserService.GetUserLocationByIpAddress(new IPAddress(127001), null);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Never);
            accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        [TestMethod]
        public void GetUserLocationByIpAddress_WhenGeoLocationDBPathIsEmpty_ShouldNotCallDb()
        {
            // Setup test environment
            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            mapUserService.GetUserLocationByIpAddress(new IPAddress(127001), string.Empty);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Never);
            accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        [TestMethod]
        public void GetUserLocationByIpAddress_WhenIpAddressIsNull_ShouldNotCallDb()
        {
            // Setup test environment
            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            mapUserService.GetUserLocationByIpAddress(null, geoLocationDbPath);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Never);
            accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        [TestMethod]
        public void GetUserLocationByIpAddress_WhenIpAddressIsInvalid_ShouldNotCallDb()
        {
            // Setup test environment
            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();
            var data = new byte[4];
            data[0] = 127;
            data[1] = 0;
            data[2] = 0;
            data[3] = 1;
            var ipAddress = new IPAddress(data);

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            mapUserService.GetUserLocationByIpAddress(ipAddress, geoLocationDbPath);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Never);
            accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        [TestMethod]
        public void GetUserLocationByIpAddress_WhenIpAddressIsNullAndGeoLocationDBPathIsNull_ShouldNotCallDb()
        {
            // Setup test environment
            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            mapUserService.GetUserLocationByIpAddress(null, null);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Never);
            accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        [TestMethod]
        public void GetUserLocationByIpAddress_WhenCityDoesNotExistInGeoDb_ShouldNotCallDb()
        {
            // Setup test environment
            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            mapUserService.GetUserLocationByIpAddress(new IPAddress(127001), geoLocationDbPath);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Never);
            accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        [TestMethod]
        public void GetUserLocationByIpAddress_WhenCityExistsInDb_ShouldNotSaveAnyDataInDb()
        {
            // Setup test environment
            var sydneyCity = new AccessMap { Id = 5, City = "sydney" };
            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();
            accessMapRepositoryMock.Setup(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()))
                .Returns(new List<AccessMap> { sydneyCity });

            var data = new byte[4];
            data[0] = 113;
            data[1] = 197;
            data[2] = 7;
            data[3] = 177;
            var ipAddress = new IPAddress(data);

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            mapUserService.GetUserLocationByIpAddress(ipAddress, geoLocationDbPath);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Once);
            accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Never);
        }

        [TestMethod]
        public void GetUserLocationByIpAddress_WhenCityDoesNotExistsInDb_ShouldSaveThisDataInDb()
        {
            // Setup test environment
            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();
            accessMapRepositoryMock.Setup(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()))
                .Returns(new List<AccessMap>());

            var data = new byte[4];
            data[0] = 113;
            data[1] = 197;
            data[2] = 7;
            data[3] = 177;
            var ipAddress = new IPAddress(data);

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            mapUserService.GetUserLocationByIpAddress(ipAddress, geoLocationDbPath);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetByExpressionMultiThread(It.IsAny<Expression<Func<AccessMap, bool>>>()), Times.Once);
            accessMapRepositoryMock.Verify(x => x.SaveMultiThreadIncludingSaveContext(It.IsAny<AccessMap>()), Times.Once);
        }

        #endregion

        #region Method: FindUserInsideMap

        [TestMethod]
        public void FindUserInsideMap_WhenGeoLocationDBPathIsNull_ShouldReturnViewModelWithoutUserFound()
        {
            const string itIsNotYou = "Unlucky, this was another person access, try again!";

            // Setup test environment
            var romeCity = new AccessMap { Id = 1, City = "rome" };
            var torontoCity = new AccessMap { Id = 2, City = "toronto" };
            var accessMaps = new List<AccessMap> { romeCity, torontoCity };
            var expectedReturn = accessMaps.AsQueryable();

            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();
            accessMapRepositoryMock.Setup(x => x.GetAllSingleThread()).Returns(expectedReturn);

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            var actualResult = mapUserService.FindUserInsideMap(new IPAddress(127001), null);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetAllSingleThread(), Times.Once);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(2, actualResult.Count);
            Assert.AreEqual(romeCity.City, actualResult[0].CityName);
            Assert.AreEqual(itIsNotYou, actualResult[0].Message);
            Assert.AreEqual(torontoCity.City, actualResult[1].CityName);
            Assert.AreEqual(itIsNotYou, actualResult[1].Message);
        }

        [TestMethod]
        public void FindUserInsideMap_WhenGeoLocationDBPathIsEmpty_ShouldReturnViewModelWithoutUserFound()
        {
            const string itIsNotYou = "Unlucky, this was another person access, try again!";

            // Setup test environment
            var romeCity = new AccessMap { Id = 1, City = "rome" };
            var torontoCity = new AccessMap { Id = 2, City = "toronto" };
            var accessMaps = new List<AccessMap> { romeCity, torontoCity };
            var expectedReturn = accessMaps.AsQueryable();

            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();
            accessMapRepositoryMock.Setup(x => x.GetAllSingleThread()).Returns(expectedReturn);

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            var actualResult = mapUserService.FindUserInsideMap(new IPAddress(127001), string.Empty);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetAllSingleThread(), Times.Once);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(2, actualResult.Count);
            Assert.AreEqual(romeCity.City, actualResult[0].CityName);
            Assert.AreEqual(itIsNotYou, actualResult[0].Message);
            Assert.AreEqual(torontoCity.City, actualResult[1].CityName);
            Assert.AreEqual(itIsNotYou, actualResult[1].Message);
        }

        [TestMethod]
        public void FindUserInsideMap_WhenIpAddressIsNull_ShouldReturnViewModelWithoutUserFound()
        {
            const string itIsNotYou = "Unlucky, this was another person access, try again!";

            // Setup test environment
            var romeCity = new AccessMap { Id = 1, City = "rome" };
            var torontoCity = new AccessMap { Id = 2, City = "toronto" };
            var accessMaps = new List<AccessMap> { romeCity, torontoCity };
            var expectedReturn = accessMaps.AsQueryable();

            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();
            accessMapRepositoryMock.Setup(x => x.GetAllSingleThread()).Returns(expectedReturn);

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            var actualResult = mapUserService.FindUserInsideMap(null, geoLocationDbPath);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetAllSingleThread(), Times.Once);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(2, actualResult.Count);
            Assert.AreEqual(romeCity.City, actualResult[0].CityName);
            Assert.AreEqual(itIsNotYou, actualResult[0].Message);
            Assert.AreEqual(torontoCity.City, actualResult[1].CityName);
            Assert.AreEqual(itIsNotYou, actualResult[1].Message);
        }

        [TestMethod]
        public void FindUserInsideMap_WhenIpAddressIsInvalid_ShouldReturnViewModelWithoutUserFound()
        {
            const string itIsNotYou = "Unlucky, this was another person access, try again!";

            // Setup test environment
            var romeCity = new AccessMap { Id = 1, City = "rome" };
            var torontoCity = new AccessMap { Id = 2, City = "toronto" };
            var accessMaps = new List<AccessMap> { romeCity, torontoCity };
            var expectedReturn = accessMaps.AsQueryable();

            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();
            accessMapRepositoryMock.Setup(x => x.GetAllSingleThread()).Returns(expectedReturn);

            var data = new byte[4];
            data[0] = 127;
            data[1] = 0;
            data[2] = 0;
            data[3] = 1;
            var ipAddress = new IPAddress(data);

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            var actualResult = mapUserService.FindUserInsideMap(ipAddress, geoLocationDbPath);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetAllSingleThread(), Times.Once);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(2, actualResult.Count);
            Assert.AreEqual(romeCity.City, actualResult[0].CityName);
            Assert.AreEqual(itIsNotYou, actualResult[0].Message);
            Assert.AreEqual(torontoCity.City, actualResult[1].CityName);
            Assert.AreEqual(itIsNotYou, actualResult[1].Message);
        }

        [TestMethod]
        public void FindUserInsideMap_WhenGeoLocationDBPathAndIpAddressAreNull_ShouldReturnViewModelWithoutUserFound()
        {
            const string itIsNotYou = "Unlucky, this was another person access, try again!";

            // Setup test environment
            var romeCity = new AccessMap { Id = 1, City = "rome" };
            var torontoCity = new AccessMap { Id = 2, City = "toronto" };
            var accessMaps = new List<AccessMap> { romeCity, torontoCity };
            var expectedReturn = accessMaps.AsQueryable();

            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();
            accessMapRepositoryMock.Setup(x => x.GetAllSingleThread()).Returns(expectedReturn);

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            var actualResult = mapUserService.FindUserInsideMap(null, null);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetAllSingleThread(), Times.Once);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(2, actualResult.Count);
            Assert.AreEqual(romeCity.City, actualResult[0].CityName);
            Assert.AreEqual(itIsNotYou, actualResult[0].Message);
            Assert.AreEqual(torontoCity.City, actualResult[1].CityName);
            Assert.AreEqual(itIsNotYou, actualResult[1].Message);
        }

        [TestMethod]
        public void FindUserInsideMap_WhenHaveACityAndItMatchsWithDb_ShouldReturnViewModelWithUserFound()
        {
            const string itIsNotYou = "Unlucky, this was another person access, try again!";
            const string itIsYou = "Hey, you found yourself, it's you here!";

            // Setup test environment
            var romeCity = new AccessMap { Id = 1, City = "rome" };
            var torontoCity = new AccessMap { Id = 2, City = "toronto" };
            var accessMaps = new List<AccessMap> { romeCity, torontoCity };
            var expectedReturn = accessMaps.AsQueryable();

            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();
            accessMapRepositoryMock.Setup(x => x.GetAllSingleThread()).Returns(expectedReturn);

            var data = new byte[4];
            data[0] = 138;
            data[1] = 197;
            data[2] = 130;
            data[3] = 102;
            var ipAddress = new IPAddress(data);

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            var actualResult = mapUserService.FindUserInsideMap(ipAddress, geoLocationDbPath);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetAllSingleThread(), Times.Once);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(2, actualResult.Count);
            Assert.AreEqual(romeCity.City, actualResult[0].CityName);
            Assert.AreEqual(itIsNotYou, actualResult[0].Message);
            Assert.AreEqual(torontoCity.City, actualResult[1].CityName);
            Assert.AreEqual(itIsYou, actualResult[1].Message);
        }

        [TestMethod]
        public void FindUserInsideMap_WhenHaveACityAndItDoesNotMatchWithDb_ShouldReturnViewModelWithoutUserFound()
        {
            const string itIsNotYou = "Unlucky, this was another person access, try again!";

            // Setup test environment
            var romeCity = new AccessMap { Id = 1, City = "rome" };
            var torontoCity = new AccessMap { Id = 2, City = "toronto" };
            var accessMaps = new List<AccessMap> { romeCity, torontoCity };
            var expectedReturn = accessMaps.AsQueryable();

            var accessMapRepositoryMock = new Mock<IBaseRepository<AccessMap>>();
            accessMapRepositoryMock.Setup(x => x.GetAllSingleThread()).Returns(expectedReturn);

            var data = new byte[4];
            data[0] = 142;
            data[1] = 93;
            data[2] = 170;
            data[3] = 2;
            var ipAddress = new IPAddress(data);

            // Action
            var mapUserService = new MapUserService(accessMapRepositoryMock.Object);
            var actualResult = mapUserService.FindUserInsideMap(ipAddress, geoLocationDbPath);

            // Asserts
            accessMapRepositoryMock.Verify(x => x.GetAllSingleThread(), Times.Once);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(2, actualResult.Count);
            Assert.AreEqual(romeCity.City, actualResult[0].CityName);
            Assert.AreEqual(itIsNotYou, actualResult[0].Message);
            Assert.AreEqual(torontoCity.City, actualResult[1].CityName);
            Assert.AreEqual(itIsNotYou, actualResult[1].Message);
        }

        #endregion
    }
}
