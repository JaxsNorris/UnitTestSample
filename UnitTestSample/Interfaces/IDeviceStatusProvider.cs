using System;
using UnitTestSample.Enums;

namespace UnitTestSample.Interfaces
{
    public interface IDeviceStatusProvider
    {
        public DeviceStatus GetDeviceStatusBadMethod(DateTime deviceLastCommunicated);
        public DeviceStatus GetDeviceStatusGoodMethod(DateTime currentDate, DateTime deviceLastCommunicated);
        public DeviceStatus GetDeviceStatusBetterMethod(DateTime currentDate, DateTime deviceLastCommunicated, int timeLapseInMinutesConsideredOffline);
    }
}
