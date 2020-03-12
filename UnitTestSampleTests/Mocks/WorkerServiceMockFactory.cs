
using Moq;
using System.Threading.Tasks;
using UnitTestSample.Interfaces;

namespace UnitTestSampleTests.Mocks
{
    public static class WorkerServiceMockFactory
    {
        public static Mock<IWorkerService> CreateMock(string returnValue)
        {
            var mock = new Mock<IWorkerService>();

            mock.Setup(x => x.DoSomeWork())
                  .Returns(returnValue);
            mock.Setup(x => x.DoSomeWorkAsync())
                  .Returns(Task.FromResult(returnValue));

            return mock;
        }
    }
}
