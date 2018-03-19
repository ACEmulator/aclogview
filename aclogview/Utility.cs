using System;

namespace aclogview
{
    /// <summary>
    /// The utility class is for helper methods.
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// The format hex method returns a number as either hex or decimal depending on a setting on the main form.
        /// </summary>
        /// <param name="theValue">
        /// The the value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FormatHex(uint theValue)
        {
            if (Globals.UseHex)
            {
                return "0x" + theValue.ToString("X");
            }

            return theValue.ToString();
        }

        public static string FormatHex(ulong theValue)
        {
            if (Globals.UseHex)
            {
                return "0x" + theValue.ToString("X");
            }

            return theValue.ToString();
        }

        public static string EpochTimeToLocalTime(double epochTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epochTime).ToLocalTime().ToString();
        }
    }
}
