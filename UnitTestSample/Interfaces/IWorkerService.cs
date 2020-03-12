using System.Threading.Tasks;

namespace UnitTestSample.Interfaces
{
    public interface IWorkerService
    {
        public string DoSomeWork();
        public Task<string> DoSomeWorkAsync();
    }
}
