using System;
using System.Text.RegularExpressions;

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

        public static string EpochTimeToLocalTime(uint seconds, uint microseconds)
        {
            var ticks = Convert.ToInt64(microseconds * 10);
            var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds).AddTicks(ticks);
            time = time.ToLocalTime();
            return time.ToString("MM/dd/yyyy hh:mm:ss.ffffff tt");
        }

        public static string EpochTimeToLocalTime(long pcapngMicroseconds)
        {
            long ticks;
            try
            {
                checked
                {
                    ticks = Convert.ToInt64(pcapngMicroseconds * 10);
                }
            }
            catch (OverflowException e)
            {
                ticks = long.MaxValue;
            }
            
            var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddTicks(ticks);
            time = time.ToLocalTime();
            return time.ToString("MM/dd/yyyy hh:mm:ss.ffffff tt");
        }
    }
}
