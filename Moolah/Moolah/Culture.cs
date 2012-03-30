using System;
using System.Globalization;

namespace Moolah
{
    public static class Culture
    {
        private static Func<CultureInfo> _current = () => CultureInfo.CurrentCulture;

        public static CultureInfo Current
        {
            get { return _current(); }
            set { _current = () => value; }
        }

        public static void Reset()
        {
            _current = () => CultureInfo.CurrentCulture;
        }
    }
}