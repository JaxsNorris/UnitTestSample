using Moq;
using System;
using UnitTestSample.Enums;
using UnitTestSample.Interfaces;

namespace UnitTestSampleTests.Mocks
{
    public static class DeviceStatusProviderFactory
    {
        public static Mock<IDeviceStatusProvider> CreateMock(DeviceStatus deviceStatus)
        {
            var mock = new Mock<IDeviceStatusProvider>();
            mock.Setup(x => x.GetDeviceStatusBetterMethod(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(deviceStatus);
            /**Below are so examples of different ways to setup mocks. 
                They are bad uses cases
             */

            /**It is possible to change return values based on inputs*/
            mock.Setup(x => x.GetDeviceStatusBetterMethod(DateTime.MinValue, It.IsAny<DateTime>(), It.Is<int>(lapse => lapse == int.MinValue)))
                .Returns(DeviceStatus.Unknown);

            /**Return can be a method that takes in parameters*/
            mock.Setup(x => x.GetDeviceStatusBetterMethod(DateTime.MinValue, It.IsAny<DateTime>(), It.Is<int>(lapse => lapse == int.MinValue)))
                .Returns((DateTime currentDate, DateTime deviceComm, int lapse) => GetDeviceStatus(deviceComm, lapse));

            return mock;
        }

        private static DeviceStatus GetDeviceStatus(DateTime deviceComm, int lapse)
        {
            if (lapse == int.MaxValue)
                return DeviceStatus.Offline;
            if (deviceComm == DateTime.MinValue)
                return DeviceStatus.Offline;
            return DeviceStatus.Unknown;
        }
    }
}
