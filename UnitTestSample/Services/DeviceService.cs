using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using UnitTestSample.Interfaces;
using UnitTestSample.Models;

namespace UnitTestSample.Services
{
    public class DeviceService : IDeviceService
    {
        public const string DeviceOfflineString = "Device offline";
        private readonly IOptionsMonitor<SettingsOption> _settingOptionsMonitor;
        private readonly IDateProvider _dateProvider;
        private readonly IDeviceStatusProvider _deviceStatusProvider;
        private readonly IWorkerService _workerService;

        public DeviceService(IOptionsMonitor<SettingsOption> settingOptionsMonitor, IDateProvider dateProvider, IDeviceStatusProvider deviceStatusProvider, IWorkerService workerService)
        {
            _settingOptionsMonitor = settingOptionsMonitor;
            _dateProvider = dateProvider;
            _deviceStatusProvider = deviceStatusProvider;
            _workerService = workerService;
        }

        public string DoSomethingComplex(DateTime deviceLastCommunicated)
        {
            var deviceStatus = _deviceStatusProvider.GetDeviceStatusBetterMethod(_dateProvider.GetUtcNow(), deviceLastCommunicated, _settingOptionsMonitor.CurrentValue.TimeLapseInMinutesConsideredDeviceOffline);
            if (deviceStatus == Enums.DeviceStatus.Offline)
                return DeviceOfflineString;

            return _workerService.DoSomeWork();
        }

        public async Task<string> DoSomethingComplexAsync(DateTime deviceLastCommunicated)
        {
            var deviceStatus = _deviceStatusProvider.GetDeviceStatusBetterMethod(_dateProvider.GetUtcNow(), deviceLastCommunicated, _settingOptionsMonitor.CurrentValue.TimeLapseInMinutesConsideredDeviceOffline);
            if (deviceStatus == Enums.DeviceStatus.Offline)
                return DeviceOfflineString;

            return await _workerService.DoSomeWorkAsync();
        }
    }
}
