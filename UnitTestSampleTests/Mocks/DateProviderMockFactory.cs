using Moq;
using System;
using UnitTestSample.Interfaces;

namespace UnitTestSampleTests.Mocks
{
    public static class DateProviderMockFactory
    {
        public static Mock<IDateProvider> CreateMock(DateTime date)
        {
            var mock = new Mock<IDateProvider>();
            mock.Setup(x => x.GetUtcNow())
                .Returns(date);

            return mock;
        }
    }
}
