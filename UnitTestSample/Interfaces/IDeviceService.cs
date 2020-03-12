
using System;
using System.Threading.Tasks;

namespace UnitTestSample.Interfaces
{
    public interface IDeviceService
    {
        public string DoSomethingComplex(DateTime deviceLastCommunicated);
        public Task<string> DoSomethingComplexAsync(DateTime deviceLastCommunicated);
    }
}
