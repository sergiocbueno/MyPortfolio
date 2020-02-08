﻿using MaxMind.GeoIP2;
using System;
using System.Net;
using System.Threading;

namespace MyPortfolio.Services.MapUserService
{
    public class MapUserService : IMapUserService
    {
        public void GetUserLocationByIpAddress(IPAddress ipAddress, string geoLocationDBPath)
        {
            using (var reader = new DatabaseReader(geoLocationDBPath))
            {
                var city = reader.City("92.251.65.190");//(ipAddress);
            }
        }
    }
}
