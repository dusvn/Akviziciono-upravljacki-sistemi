using Common;
using System;
using System.Collections.Generic;

namespace dCom.Configuration
{
    internal class ConfigItem : IConfigItem
	{
		#region Fields

		private PointType registryType; //0 
		private ushort numberOfRegisters; //1
		private ushort startAddress; // 2
		private ushort decimalSeparatorPlace; // 3
		private ushort minValue; //4
		private ushort maxValue; //5
		private ushort defaultValue; //6
		private string processingType; //7
		private string description; //8 
		private int acquisitionInterval; //9 
		private double scalingFactor; //10
		private double deviation; //11
		private double egu_max; //12 --> u config fajlu sam prvo unosio min pa onda max 
		private double egu_min; //13 --> znaci da je na [12] mestu egu_min a na [13] egu max 
		private ushort abnormalValue; //14
		private double highLimit; //15
		private double lowLimit; //16
        private int secondsPassedSinceLastPoll; //17

		#endregion Fields

		#region Properties

		public PointType RegistryType
		{
			get
			{
				return registryType;
			}

			set
			{
				registryType = value;
			}
		}

		public ushort NumberOfRegisters
		{
			get
			{
				return numberOfRegisters;
			}

			set
			{
				numberOfRegisters = value;
			}
		}

		public ushort StartAddress
		{
			get
			{
				return startAddress;
			}

			set
			{
				startAddress = value;
			}
		}

		public ushort DecimalSeparatorPlace
		{
			get
			{
				return decimalSeparatorPlace;
			}

			set
			{
				decimalSeparatorPlace = value;
			}
		}

		public ushort MinValue
		{
			get
			{
				return minValue;
			}

			set
			{
				minValue = value;
			}
		}

		public ushort MaxValue
		{
			get
			{
				return maxValue;
			}

			set
			{
				maxValue = value;
			}
		}

		public ushort DefaultValue
		{
			get
			{
				return defaultValue;
			}

			set
			{
				defaultValue = value;
			}
		}

		public string ProcessingType
		{
			get
			{
				return processingType;
			}

			set
			{
				processingType = value;
			}
		}

		public string Description
		{
			get
			{
				return description;
			}

			set
			{
				description = value;
			}
		}

		public int AcquisitionInterval
		{
			get
			{
				return acquisitionInterval;
			}

			set
			{
				acquisitionInterval = value;
			}
		}

		public double ScaleFactor
		{
			get
			{
				return scalingFactor;
			}
			set
			{
				scalingFactor = value;
			} 
		}

		public double Deviation
		{
			get
			{
				return deviation;
			}

			set
			{
				deviation = value;
			}
		}

		public double EGU_Max
		{
			get
			{
				return egu_max;
			}

			set
			{
				egu_max = value;
			}
		}

		public double EGU_Min
		{
			get
			{
				return egu_min;
			}

			set
			{
				egu_min = value;
			}
		}

		public ushort AbnormalValue
		{
			get
			{
				return abnormalValue;
			}

			set
			{
				abnormalValue = value;
			}
		}

		public double HighLimit
		{
			get
			{
				return highLimit;
			}

			set
			{
				highLimit = value;
			}
		}

		public double LowLimit
		{
			get
			{
				return lowLimit;
			}

			set
			{
				lowLimit = value;
			}
		}

        public int SecondsPassedSinceLastPoll
        {
            get
            {
                return secondsPassedSinceLastPoll;
            }

            set
            {
                secondsPassedSinceLastPoll = value;
            }
        }

        #endregion Properties

        public ConfigItem(List<string> configurationParameters)
		{
			RegistryType = GetRegistryType(configurationParameters[0]);
			int temp;
			double doubleTemp;
			Int32.TryParse(configurationParameters[1], out temp);
			NumberOfRegisters = (ushort)temp;
			Int32.TryParse(configurationParameters[2], out temp);
			StartAddress = (ushort)temp;
			Int32.TryParse(configurationParameters[3], out temp);
			DecimalSeparatorPlace = (ushort)temp;
			Int32.TryParse(configurationParameters[4], out temp);
			MinValue = (ushort)temp;
			Int32.TryParse(configurationParameters[5], out temp);
			MaxValue = (ushort)temp;
			Int32.TryParse(configurationParameters[6], out temp);
			DefaultValue = (ushort)temp;
			ProcessingType = configurationParameters[7];
			Description = configurationParameters[8].TrimStart('@');
            if (configurationParameters[9].Equals("#"))
            {
                AcquisitionInterval = 1;
            }
            else
            {
                Int32.TryParse(configurationParameters[9], out temp);
                AcquisitionInterval = temp;
            }
			if (configurationParameters[10].Equals("#"))
			{
				ScaleFactor = 1;
			}else
			{
				Double.TryParse(configurationParameters[10], out doubleTemp);
				ScaleFactor = doubleTemp; 
			}

			if (configurationParameters[11].Equals("#"))
			{
				Deviation = 0; 
			}else
			{
                Double.TryParse(configurationParameters[11], out doubleTemp);
                Deviation = doubleTemp;
            }
			if (configurationParameters[12].Equals("#"))
			{
				EGU_Min = 0;
			}
            else
            {
                Double.TryParse(configurationParameters[12], out doubleTemp);
                EGU_Min = doubleTemp;
            }

            if (configurationParameters[13].Equals("#"))
            {
                EGU_Max = 1;
            }
            else
            {
                Double.TryParse(configurationParameters[13], out doubleTemp);
                EGU_Max = doubleTemp;
            }
			if (configurationParameters[14].Equals("#"))
			{
				AbnormalValue = 0; // zbog alarma 
			}
			else
			{
				Int32.TryParse(configurationParameters[14],out temp);
				AbnormalValue = (ushort)temp; 
			}

			if (configurationParameters[15].Equals("#"))
			{
				LowLimit= 0;
			}
			else
			{
				Double.TryParse(configurationParameters[15], out doubleTemp);
				LowLimit = doubleTemp;
			}
            if (configurationParameters[16].Equals("#"))
            {
                HighLimit = 0;
            }
            else
            {
                Double.TryParse(configurationParameters[16], out doubleTemp);
                HighLimit = doubleTemp;
            }


        }

		private PointType GetRegistryType(string registryTypeName)
		{
			PointType registryType;
			switch (registryTypeName)
			{
				case "DO_REG":
					registryType = PointType.DIGITAL_OUTPUT;
					break;

				case "DI_REG":
					registryType = PointType.DIGITAL_INPUT;
					break;

				case "IN_REG":
					registryType = PointType.ANALOG_INPUT;
					break;

				case "HR_INT":
					registryType = PointType.ANALOG_OUTPUT;
					break;

				default:
					registryType = PointType.HR_LONG;
					break;
			}
			return registryType;
		}
	}
}