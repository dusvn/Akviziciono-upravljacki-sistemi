using System;

namespace ProcessingModule
{
    /// <summary>
    /// Class containing logic for engineering unit conversion.
    /// </summary>
    public class EGUConverter
	{
        /// <summary>
        /// Converting raw values to ing 
        /// Formula -> egu = rawValue * scaling_factor - deviation 
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <param name="deviation">The deviation</param>
        /// <param name="rawValue">The raw value.</param>
        /// <returns>The value in engineering units.</returns>
		public double ConvertToEGU(double scalingFactor, double deviation, ushort rawValue)
		{
            return rawValue * scalingFactor+deviation;
		}

        /// <summary>
        /// Converting to ing values to raw value 
        /// Formula -> egu +deviation /scaling_fasctor= rawValue  
        /// <param name="scalingFactor">The scaling factor.</param>
        /// <param name="deviation">The deviation.</param>
        /// <param name="eguValue">The EGU value.</param>
        /// <returns>The raw value.</returns>
		public ushort ConvertToRaw(double scalingFactor, double deviation, double eguValue)
        {
            return (ushort)((eguValue+deviation)/scalingFactor);
		}
	}
}
