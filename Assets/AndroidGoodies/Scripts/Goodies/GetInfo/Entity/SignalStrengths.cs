#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	/// Contains phone signal strength related information.
	/// </summary>
	[PublicAPI]
	[AndroidApi(AGDeviceInfo.VersionCodes.P)]
	public class SignalStrengths
	{
		public SignalStrengths()
		{
			
		}
		
		public SignalStrengths(AndroidJavaObject ajo)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.P))
			{
				return;
			}

			if (ajo.IsJavaNull())
			{
				return;
			}
			
			cdmaDbm = ajo.CallInt("getCdmaDbm");
			cdmaEcio = ajo.CallInt("getCdmaEcio");
			evdoDbm = ajo.CallInt("getEvdoDbm");
			evdoEcio = ajo.CallInt("getEvdoEcio");
			evdoSnr = ajo.CallInt("getEvdoSnr");
			gsmBitErrorRate = ajo.CallInt("getGsmBitErrorRate");
			gsmSignalStrength = ajo.CallInt("getGsmSignalStrength");
			level = ajo.CallInt("getLevel");
			isGsm = ajo.CallBool("isGsm");
		}

		/// <summary>
		/// The CDMA RSSI value in dBm
		/// </summary>
		[PublicAPI]
		public int cdmaDbm;

		/// <summary>
		/// The CDMA Ec/Io value in dB*10
		/// </summary>
		[PublicAPI]
		public int cdmaEcio;
		
		/// <summary>
		/// The EVDO RSSI value in dBm
		/// </summary>
		[PublicAPI]
		public int evdoDbm;
		
		/// <summary>
		/// The EVDO Ec/Io value in dB*10
		/// </summary>
		[PublicAPI]
		public int evdoEcio;
		
		/// <summary>
		/// The signal to noise ratio. Valid values are 0-8. 8 is the highest.
		/// </summary>
		[PublicAPI]
		public int evdoSnr;
		
		/// <summary>
		/// The GSM bit error rate (0-7, 99) as defined in TS 27.007 8.5
		/// </summary>
		[PublicAPI]
		public int gsmBitErrorRate;
		
		/// <summary>
		/// The GSM Signal Strength, valid values are (0-31, 99) as defined in TS 27.007 8.5
		/// </summary>
		[PublicAPI]
		public int gsmSignalStrength;
		
		/// <summary>
		/// Retrieve an abstract level value for the overall signal strength.
		/// </summary>
		/// <returns>
		/// A single integer from 0 to 4 representing the general signal quality.
		/// This may take into account many different radio technology inputs.
		/// 0 represents very poor signal strength while 4 represents a very strong signal strength.
		/// </returns>
		[PublicAPI]
		public int level;

		/// <summary>
		/// True if this is for GSM
		/// </summary>
		[PublicAPI]
		public bool isGsm;

		public override string ToString()
		{
			return string.Format("CDMA RSSI value is {0} dBm, CDMA Ec/Io value is {1} dB*10, EVDO RSSI value is {2} dBm, EVDO Ec/Io value is {3} dB*10, signal to noise ratio is {4}, GSM bit error rate is {5}, GSM Signal Strength is {6}, signal level is {7}, is GSM - {8}.", 
				cdmaDbm, cdmaEcio, evdoDbm, evdoEcio, evdoSnr, gsmBitErrorRate, gsmSignalStrength, level, isGsm);
		}
	}
}
#endif
