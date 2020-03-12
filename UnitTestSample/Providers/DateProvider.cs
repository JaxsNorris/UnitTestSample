using System;
using UnitTestSample.Interfaces;

namespace UnitTestSample.Providers
{
    public class DateProvider : IDateProvider
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
