#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using JetBrains.Annotations;
	using Internal;
	using UnityEngine;
	
	/// <summary>
	/// Contains phone state and service related information. The following phone information is included in returned ServiceState:
	/// Service state: IN_SERVICE, OUT_OF_SERVICE, EMERGENCY_ONLY, POWER_OFF
	/// Duplex mode: UNKNOWN, FDD, TDD
	/// Roaming indicator
	/// Operator name, short name and numeric id
	/// Network selection mode
	/// </summary>
	[PublicAPI]
	public class ServiceStates
	{
		public AndroidJavaObject ajo;

		public ServiceStates()
		{
			ajo = new AndroidJavaObject(C.AndroidTelephonyServiceState);
		}

		public ServiceStates(ServiceStates other)
		{
			ajo = other.ajo;
		}
		
		public ServiceStates(AndroidJavaObject ajo)
		{
			this.ajo = ajo;
		}

		/// <summary>
		/// Duplex mode for the phone
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public enum DuplexModes
		{
			/// <summary>
			/// Duplex mode for the phone is frequency-division duplexing.
			/// </summary>
			FDD = 1,
			/// <summary>
			/// Duplex mode for the phone is time-division duplexing.
			/// </summary>
			TDD = 2,
			/// <summary>
			/// Duplex mode for the phone is unknown.
			/// </summary>
			Unknown = 0
		}
		
		/// <summary>
		/// Phone states
		/// </summary>
		[PublicAPI]
		public enum States
		{
			/// <summary>
			/// The phone is registered and locked. Only emergency numbers are allowed.
			/// </summary>
			EmergencyOnly = 2,
			/// <summary>
			/// Normal operation condition, the phone is registered with an operator either in home network or in roaming.
			/// </summary>
			InService = 0,
			/// <summary>
			/// Phone is not registered with any operator, the phone can be currently searching a new operator to register to,
			/// or not searching to registration at all, or registration is denied, or radio signal is not available.
			/// </summary>
			OutOfService = 1,
			/// <summary>
			/// Radio of telephony is explicitly powered off.
			/// </summary>
			PowerOff = 3
		}

		/// <summary>
		/// Unknown ID. Could be returned by CdmaNetworkId() or CdmaSystemId()
		/// </summary>
		[PublicAPI]
		public const int UnknownId = -1;

		/// <summary>
		/// The CDMA NID (Network Identification Number), a number uniquely identifying a network within a wireless system.
		/// (Defined in 3GPP2 C.S0023 3.4.8)
		/// </summary>
		[PublicAPI]
		public int CdmaNetworkId
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return UnknownId;
				}

				return ajo.CallInt("getCdmaNetworkId");
			}
		}
		
		/// <summary>
		/// The CDMA SID (System Identification Number), a number uniquely identifying a wireless system.
		/// (Defined in 3GPP2 C.S0023 3.4.8)
		/// </summary>
		[PublicAPI]
		public int CdmaSystemId
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return UnknownId;
				}

				return ajo.CallInt("getCdmaSystemId");
			}
		}

		/// <summary>
		/// Get an array of cell bandwidths (kHz) for the current serving cells
		/// </summary>
		[PublicAPI]
		public int[] CellBandwidths
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return new []{0};
				}

				return ajo.Call<int[]>("getCellBandwidths");
			}
		}
		
		/// <summary>
		/// Get the channel number of the current primary serving cell, or -1 if unknown
		/// This is EARFCN for LTE, UARFCN for UMTS, and ARFCN for GSM.
		/// </summary>
		[PublicAPI]
		public int ChannelNumber
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return UnknownId;
				}

				return ajo.CallInt("getChannelNumber");
			}
		}
		
		/// <summary>
		/// Get the current duplex mode
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public DuplexModes DuplexMode
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return DuplexModes.Unknown;
				}

				return (DuplexModes) ajo.CallInt("getDuplexMode");
			}
		}

		/// <summary>
		/// Get current network selection mode.
		/// </summary>
		/// <returns> True if manual mode, false if automatic mode </returns>
		[PublicAPI]
		public bool IsManualSelection
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return false;
				}

				return ajo.CallBool("getIsManualSelection");
			}
			set
			{
				ajo.Call("setIsManualSelection", value);
			}
		}

		/// <summary>
		/// Get current registered operator name in long alphanumeric format.
		/// In GSM/UMTS, long format can be up to 16 characters long.
		/// In CDMA, returns the ERI text, if set. Otherwise, returns the ONS.
		/// </summary>
		[PublicAPI]
		public string OperatorAlphaLong
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}

				return ajo.CallStr("getOperatorAlphaLong");
			}
			set
			{
				ajo.Call("setOperatorName", value, OperatorAlphaShort, OperatorNumeric);
			}
		}
		
		/// <summary>
		/// Get current registered operator name in short alphanumeric format.
		/// In GSM/UMTS, short format can be up to 8 characters long.
		/// </summary>
		[PublicAPI]
		public string OperatorAlphaShort
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}

				return ajo.CallStr("getOperatorAlphaShort");
			}
			set
			{
				ajo.Call("setOperatorName", OperatorAlphaLong, value, OperatorNumeric);
			}
		}
		
		/// <summary>
		/// Get current registered operator numeric id. In GSM/UMTS, numeric format is 3 digit country code plus 2 or 3 digit network code.
		/// </summary>
		[PublicAPI]
		public string OperatorNumeric
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}

				return ajo.CallStr("getOperatorAlphaNumeric");
			}
			set
			{
				ajo.Call("setOperatorName", OperatorAlphaLong, OperatorAlphaShort, value);
			}
		}
		
		/// <summary>
		/// Get current roaming indicator of phone (note: not just decoding from TS 27.007 7.2)
		/// </summary>
		/// <returns> True if TS 27.007 7.2 roaming is true and ONS is different from SPN </returns>
		[PublicAPI]
		public bool Roaming
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return false;
				}

				return ajo.CallBool("getRoaming");
			}
			set
			{
				ajo.Call("setRoaming", value);
			}
		}

		/// <summary>
		/// Current voice service state
		/// </summary>
		[PublicAPI]
		public States State
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return States.PowerOff;
				}

				return (States) ajo.CallInt("getState");
			}
			set
			{
				ajo.Call("setState", (int) value);
			}
		}
	}
}

#endif
