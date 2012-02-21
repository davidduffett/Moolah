using System;

namespace Moolah
{
    /// <summary>
    /// Allows me to mock time. Muwhahaha
    /// </summary>
    public static class SystemTime
    {
        private static Func<DateTime> _now = () => DateTime.Now;
        public static DateTime Now
        {
            get { return _now(); }
            set { _now = () => value; }
        }

        public static void Reset()
        {
            _now = () => DateTime.Now;
        }
    }
}