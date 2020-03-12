using System;
using System.Collections;
using System.Collections.Generic;
using UnitTestSample.Enums;
using UnitTestSample.Services;
using Xunit;

namespace UnitTestSampleTests.Providers
{
    public class DeviceStatusProviderServiceTests
    {

        private DeviceStatusProvider CreateDeviceStatusProvider()
        {
            return new DeviceStatusProvider();
        }

        #region Simple
        [Fact]
        public void When_GetDeviceStatusBetterMethod_With_TimeLapseLargerThenAllowed_Expect_DeviceOfflineStatus()
        {
            //Arrange
            var provider = CreateDeviceStatusProvider();
            var currentDate = DateTime.Parse("2020/03/10 08:10:48");
            var deviceLastCommunicated = DateTime.Parse("2020/03/10 08:00:48");
            var timeLapseInMinutesConsideredOffline = 5;
            //Act
            var deviceStatus = provider.GetDeviceStatusBetterMethod(currentDate, deviceLastCommunicated, timeLapseInMinutesConsideredOffline);
            //Assert
            Assert.Equal(DeviceStatus.Offline, deviceStatus);
        }

        [Fact]
        public void When_GetDeviceStatusBetterMethod_With_CurrentDateMinValue_Expect_DeviceOfflineStatus()
        {
            //Arrange
            var provider = CreateDeviceStatusProvider();
            //Act
            var exception = Assert.Throws<ArgumentException>(() => provider.GetDeviceStatusBetterMethod(DateTime.MinValue, DateTime.MinValue, 0));
            //Assert
            Assert.Equal("timeLapseInMinutesConsideredOffline", exception.ParamName);
            Assert.Contains("zero", exception.Message);
        }


        [Theory]
        [InlineData("2020/03/10 08:10:00", "2020/03/10 08:00:01", 10, DeviceStatus.Online)]
        [InlineData("2020/03/10 08:10:00", "2020/03/10 08:00:00", 10, DeviceStatus.Online)]
        [InlineData("2020/03/10 08:10:01", "2020/03/10 08:00:00", 10, DeviceStatus.Offline)]
        public void When_GetDeviceStatusBetterMethod_Expect_CorrectDeviceStatus(string nowDateString, string deviceCommDateString, int timeLapseInMinutesConsideredOffline, DeviceStatus expectedStatus)
        {
            //Arrange
            var provider = CreateDeviceStatusProvider();
            var currentDate = DateTime.Parse(nowDateString);
            var deviceLastCommunicated = DateTime.Parse(deviceCommDateString);

            //Act
            var deviceStatus = provider.GetDeviceStatusBetterMethod(currentDate, deviceLastCommunicated, timeLapseInMinutesConsideredOffline);

            //Assert
            Assert.Equal(expectedStatus, deviceStatus);
        }
        #endregion

        #region InlineData Complex examples
        /**Tips: 
         * Inline data requires a constant but there a quite a few was to get around this
         * To illustrate the problem DateTime.MinValue is a static property and not a const you could not use it in a normal inline data. 
         * Below are some of the possible ways to setup Test Data. It has the data from When_GetDeviceStatusBetterMethod_Expect_CorrectDeviceStatus and an additional entry for DateTime.MinValue
         * Article: https://andrewlock.net/creating-parameterised-tests-in-xunit-with-inlinedata-classdata-and-memberdata/
         */
        #region ClassData Example
        [Theory]
        [ClassData(typeof(ClassDataExample))]
        public void ExampleTestFor_ClassData(string nowDateString, string deviceCommDateString, int timeLapseInMinutesConsideredOffline, DeviceStatus expectedStatus)
        {
            //Arrange
            var provider = CreateDeviceStatusProvider();
            var currentDate = DateTime.Parse(nowDateString);
            var deviceLastCommunicated = DateTime.Parse(deviceCommDateString);

            //Act
            var deviceStatus = provider.GetDeviceStatusBetterMethod(currentDate, deviceLastCommunicated, timeLapseInMinutesConsideredOffline);

            //Assert
            Assert.Equal(expectedStatus, deviceStatus);
        }
        private class ClassDataExample : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "2020/03/10 08:10:00", "2020/03/10 08:00:01", 10, DeviceStatus.Online };
                yield return new object[] { "2020/03/10 08:10:00", "2020/03/10 08:00:00", 10, DeviceStatus.Online };
                yield return new object[] { "2020/03/10 08:10:01", "2020/03/10 08:00:00", 10, DeviceStatus.Offline };
                yield return new object[] { DateTime.MinValue, DateTime.MinValue, 10, DeviceStatus.Online };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        #endregion

        #region MemberData Example

        #region MemberData Property Example
        [Theory]
        [MemberData(nameof(MemberDataSampleProperty))]
        public void ExampleTestFor_MemberDataProperty(string nowDateString, string deviceCommDateString, int timeLapseInMinutesConsideredOffline, DeviceStatus expectedStatus)
        {
            //Arrange
            var provider = CreateDeviceStatusProvider();
            var currentDate = DateTime.Parse(nowDateString);
            var deviceLastCommunicated = DateTime.Parse(deviceCommDateString);

            //Act
            var deviceStatus = provider.GetDeviceStatusBetterMethod(currentDate, deviceLastCommunicated, timeLapseInMinutesConsideredOffline);

            //Assert
            Assert.Equal(expectedStatus, deviceStatus);
        }

        public static IEnumerable<object[]> MemberDataSampleProperty =>
               new List<object[]>
               {
                     new object[] { "2020/03/10 08:10:00", "2020/03/10 08:00:01", 10, DeviceStatus.Online },
                      new object[] { "2020/03/10 08:10:00", "2020/03/10 08:00:00", 10, DeviceStatus.Online },
                      new object[] { "2020/03/10 08:10:01", "2020/03/10 08:00:00", 10, DeviceStatus.Offline },
                      new object[] { DateTime.MinValue, DateTime.MinValue, 10, DeviceStatus.Online }
               };
        #endregion

        #region MemberData Method Example
        [Theory]
        [MemberData(nameof(MemberDataMethod), parameters: 10)]
        public void ExampleTestFor_MemberDataMethod(string nowDateString, string deviceCommDateString, int timeLapseInMinutesConsideredOffline, DeviceStatus expectedStatus)
        {
            //Arrange
            var provider = CreateDeviceStatusProvider();
            var currentDate = DateTime.Parse(nowDateString);
            var deviceLastCommunicated = DateTime.Parse(deviceCommDateString);

            //Act
            var deviceStatus = provider.GetDeviceStatusBetterMethod(currentDate, deviceLastCommunicated, timeLapseInMinutesConsideredOffline);

            //Assert
            Assert.Equal(expectedStatus, deviceStatus);
        }

        public static IEnumerable<object[]> MemberDataMethod(int timeLapseInMinutesConsideredOffline) =>
               new List<object[]>
               {
                     new object[] { "2020/03/10 08:10:00", "2020/03/10 08:00:01", timeLapseInMinutesConsideredOffline, DeviceStatus.Online },
                      new object[] { "2020/03/10 08:10:00", "2020/03/10 08:00:00", timeLapseInMinutesConsideredOffline, DeviceStatus.Online },
                      new object[] { "2020/03/10 08:10:01", "2020/03/10 08:00:00", timeLapseInMinutesConsideredOffline, DeviceStatus.Offline },
                      new object[] { DateTime.MinValue, DateTime.MinValue, timeLapseInMinutesConsideredOffline, DeviceStatus.Online }
               };
        #endregion

        #endregion

        #endregion
    }
}
