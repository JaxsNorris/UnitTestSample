using System;
using UnitTestSample.Interfaces;

namespace UnitTestSampleTests
{
    class FakeDateProvider : IDateProvider
    {
        public DateTime UtcNowValue { get; set; }

        public FakeDateProvider(DateTime utcNowValue)
        {
            UtcNowValue = utcNowValue;
        }

        public DateTime GetUtcNow()
        {
            return UtcNowValue;
        }
    }
}
