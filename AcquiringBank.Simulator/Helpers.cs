using System;
using System.Collections.Generic;
using System.Text;

namespace AcquiringBank.Simulator
{
    public static class Helpers
    {
        public static readonly List<string> BlacklistedCards = new List<string>
        {
            "4132490097488921",
            "4122490097488921",
            "4111490097488921",
            "4178490097488921",
            "4198490097488921",
        };
        
        public static readonly List<string> AllowableLocations = new List<string>
        {
            "GB",
            "IE"
        };

        public static T GetEnumValue<T>(this string str, bool ignoreCase)
            where T : struct, IConvertible
        {
            return Enum.TryParse<T>(str, ignoreCase, out var result) ? result : default;
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

    }
}
