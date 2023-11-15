using System.Threading.Tasks;

namespace LinkLair.Api.Health;

/// <summary>
/// Specification for a health check. Implementations of this interface are automatically loaded
/// by the IoC container.
/// </summary>
public interface IHealthIndicator
{
    // Name of the indicator describing what it is indicating to return to a client
    string Identifier { get; }

    // Perform the concrete health check.
    Task<HealthIndicatorModel> CheckStatusAsync();
}
