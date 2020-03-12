using Microsoft.Extensions.Options;
using Moq;

namespace UnitTestSampleTests.Mocks
{
    static class OptionsMonitorMockFactory
    {
        internal static IOptionsMonitor<T> CreateMockOptionsMonitor<T>(this T options)
        {
            var mock = new Mock<IOptionsMonitor<T>>();
            mock.SetupGet(item => item.CurrentValue).Returns(options);

            return mock.Object;
        }
    }
}
