// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Utility.cs" company="">
//   
// </copyright>
// <summary>
//   The utility class is for helper methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace aclogview
{
    /// <summary>
    /// The utility class is for helper methods.
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// The format guid method returns a number as either hex or decimal depending on a setting on the main form.
        /// </summary>
        /// <param name="theValue">
        /// The the value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FormatGuid(uint theValue)
        {
            if (Globals.UseHex)
            {
                return "0x" + theValue.ToString("X");
            }

            return theValue.ToString();
        }

        public static string FormatGuid(ulong theValue)
        {
            if (Globals.UseHex)
            {
                return "0x" + theValue.ToString("X");
            }

            return theValue.ToString();
        }
    }
}
