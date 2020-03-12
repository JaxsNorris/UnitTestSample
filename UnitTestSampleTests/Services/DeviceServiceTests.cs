using Moq;
using System;
using System.Threading.Tasks;
using UnitTestSample.Enums;
using UnitTestSample.Interfaces;
using UnitTestSample.Models;
using UnitTestSample.Services;
using UnitTestSampleTests.Mocks;
using Xunit;

namespace UnitTestSampleTests.Services
{
    public class DeviceServiceTests
    {
        private readonly DateTime CurrentDate = DateTime.Parse("2020/03/10 08:10:48");
        private readonly DateTime DefaultTime = DateTime.Parse("2020/03/10 08:15:48");
        private const string WorkerServiceReturnValue = "Value that is returned by the worker service DoWork method";

        #region Test Setup
        private DeviceService CreateDeviceService(DeviceStatus deviceStatus, Mock<IWorkerService> workerService = null)
        {
            var settingOptionsMonitor = OptionsMonitorMockFactory.CreateMockOptionsMonitor(CreateDefaultSettingsOption());

            var dateProvider = CreateDateProvider();
            var deviceStatusProvider = DeviceStatusProviderFactory.CreateMock(deviceStatus);

            if (workerService == null)
                workerService = WorkerServiceMockFactory.CreateMock(WorkerServiceReturnValue);

            return new DeviceService(settingOptionsMonitor, dateProvider, deviceStatusProvider.Object, workerService.Object);
        }

        private SettingsOption CreateDefaultSettingsOption()
        {
            return new SettingsOption() { TimeLapseInMinutesConsideredDeviceOffline = 5 };
        }

        private IDateProvider CreateDateProvider()
        {
            /** Can also use return new FakeDateProvider(CurrentDate);
             */
            return DateProviderMockFactory.CreateMock(CurrentDate).Object;
        }
        #endregion

        #region Sync test
        [Fact]
        public void When_DoSomeWork_With_OfflineDevice_Expect_DeviceOfflineString()
        {
            //Arrange
            var workerService = WorkerServiceMockFactory.CreateMock(WorkerServiceReturnValue);
            var service = CreateDeviceService(DeviceStatus.Offline, workerService);

            //Act
            var returnedString = service.DoSomethingComplex(DefaultTime);

            //Assert
            Assert.Equal(DeviceService.DeviceOfflineString, returnedString);
            workerService.Verify(mock => mock.DoSomeWork(), Times.Never);
        }

        [Fact]
        public void When_DoSomeWork_With_OnlineDevice_Expect_WorkToBeCalled()
        {
            //Arrange
            var workerService = WorkerServiceMockFactory.CreateMock(WorkerServiceReturnValue);
            var service = CreateDeviceService(DeviceStatus.Online, workerService);

            //Act
            var returnedString = service.DoSomethingComplex(DefaultTime);
            //Assert
            Assert.Equal(WorkerServiceReturnValue, returnedString);
            workerService.Verify(mock => mock.DoSomeWork(), Times.Once);
        }
        #endregion

        #region Async test
        /** Tips: Tests can be async Task. NEVER make these void. Test will result in inaccurate results
         */
        [Fact]
        public async Task When_DoSomeWorkAsync_With_OfflineDevice_Expect_DeviceOfflineString()
        {
            //Arrange
            var workerService = WorkerServiceMockFactory.CreateMock(WorkerServiceReturnValue);
            var service = CreateDeviceService(DeviceStatus.Offline, workerService);

            //Act
            var returnedString = await service.DoSomethingComplexAsync(DefaultTime);

            //Assert
            Assert.Equal(DeviceService.DeviceOfflineString, returnedString);
            workerService.Verify(mock => mock.DoSomeWorkAsync(), Times.Never);
        }

        [Fact]
        public async Task When_DoSomeWorkAsync_With_OnlineDevice_Expect_WorkToBeCalled()
        {
            //Arrange
            var workerService = WorkerServiceMockFactory.CreateMock(WorkerServiceReturnValue);
            var service = CreateDeviceService(DeviceStatus.Online, workerService);

            //Act
            var returnedString = await service.DoSomethingComplexAsync(DefaultTime);
            //Assert
            Assert.Equal(WorkerServiceReturnValue, returnedString);
            workerService.Verify(mock => mock.DoSomeWorkAsync(), Times.Once);
        }
        #endregion
    }
}
