using System;

namespace Moolah
{
    /// <summary>
    /// Allows me to mock time. Muwhahaha
    /// </summary>
    public interface ITimeProvider
    {
        DateTime Now { get; }
    }

    internal class TimeProvider : ITimeProvider
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}