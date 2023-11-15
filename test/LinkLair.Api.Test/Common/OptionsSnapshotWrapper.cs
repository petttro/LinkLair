using Microsoft.Extensions.Options;

namespace LinkLair.Api.Test.Common
{
    /// <summary>
    /// IOptions wrapper that returns the options instance.
    /// </summary>
    public class OptionsSnapshotWrapper<TOptions> : IOptionsSnapshot<TOptions>
        where TOptions : class, new()
    {
        /// <summary>
        /// Intializes the wrapper with the options instance to return.
        /// </summary>
        /// <param name="options">The options instance to return.</param>
        public OptionsSnapshotWrapper(TOptions options)
        {
            Value = options;
        }

        public TOptions Value { get; }

        public TOptions Get(string name)
        {
            return Value;
        }
    }
}
