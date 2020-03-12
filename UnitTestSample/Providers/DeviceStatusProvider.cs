using System;
using UnitTestSample.Enums;
using UnitTestSample.Interfaces;

namespace UnitTestSample.Services
{
    public class DeviceStatusProvider : IDeviceStatusProvider
    {
        #region Untestable method - Bad method
        public DeviceStatus GetDeviceStatusBadMethod(DateTime deviceLastCommunicated)
        {
            /*Tips: 
             * By using the Datetime.Now this method is tightly coupled to a external concrete data source
             * It is also lying about it's inputs 
             * The calling code thinks that it only requires the deviceLastCommunicated but it actually needs 2 dates to determine the device status.
             * Only by inspecting the code can you determine what is been used. 
             * Methods names and signatures should be setup so that the implementation details are irrelevant 
             * Writing a unit test on this will be difficult and result in flaky tests since you can't control DateTime.Now so the code will be hard to predict
             */
            DateTime now = DateTime.Now;
            var timeLapsed = now.Subtract(deviceLastCommunicated);
            if (timeLapsed.TotalMinutes > 10)
                return DeviceStatus.Offline;
            return DeviceStatus.Online;
        }
        #endregion

        #region Good method 
        public DeviceStatus GetDeviceStatusGoodMethod(DateTime currentDate, DateTime deviceLastCommunicated)
        {
            /* Tips: 
             * This method is better the method signature makes it clear that 2 dates are required.
             * It removes the dependency on the external concrete data source
             * Writing a unit test on this will be much easier and will result in better test coverage as you can test in detail how these 2 dates interact.
             * Although this method is better and writing unit tests will be easier it still hides the 10 minutes from the calling code.
             * What if there is a new business requirement that different devices have more or less time before been considered offline?
             * All unit tests will fail and existing code will need to be update with new signature
             */
            var timeLapsed = currentDate.Subtract(deviceLastCommunicated);
            if (timeLapsed.TotalMinutes > 10)
                return DeviceStatus.Offline;
            return DeviceStatus.Online;
        }
        #endregion

        #region Better method 
        public DeviceStatus GetDeviceStatusBetterMethod(DateTime currentDate, DateTime deviceLastCommunicated, int timeLapseInMinutesConsideredOffline)
        {
            /* Tips: 
             * This method is better the method signature makes it clear that 2 dates are required. It also no longer hides the hard-coded 10 minutes
             * This method has a single responsibility it is truly a single unit of work. Given 3 inputs expect 1 output     
             */
            if (timeLapseInMinutesConsideredOffline == 0)
                throw new ArgumentException($"Parameter can not be zero", nameof(timeLapseInMinutesConsideredOffline));

            var timeLapsed = currentDate.Subtract(deviceLastCommunicated);
            if (timeLapsed.TotalMinutes > timeLapseInMinutesConsideredOffline)
                return DeviceStatus.Offline;
            return DeviceStatus.Online;
        }
        #endregion
    }
}
