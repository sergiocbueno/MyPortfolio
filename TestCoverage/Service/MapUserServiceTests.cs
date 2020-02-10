using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyPortfolio.Database.Models;
using MyPortfolio.Database.Repositories;
using MyPortfolio.Services.MapUserService;
using System;
using System.Collections.Generic;
using System.IO;
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
    }
}
