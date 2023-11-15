using System;
using System.Threading.Tasks;
using LinkLair.Api.Health;

namespace LinkLair.Api.Test.Controllers.Health
{
    public class ExceptionHealthIndicator : IHealthIndicator
    {
        public string Identifier => typeof(ExceptionHealthIndicator).FullName;

        public Task<HealthIndicatorModel> CheckStatusAsync()
        {
            throw new Exception("Test exception.");
        }
    }
}
