using CoviIDApiCore.V1.DTOs.TestResult;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Interfaces.Services
{
    public interface ITestResultService
    {
        Task AddTestResult(TestResultRequest testResultRequest);
    }
}
