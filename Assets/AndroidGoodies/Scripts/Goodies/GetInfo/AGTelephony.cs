// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGTelephony.cs
//

using System;
using UnityEngine;

#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using Internal;
	using JetBrains.Annotations;
	using System.Collections.Generic;

	/// <summary>
	/// Telephony.
	/// Required permissions:
	/// <uses-permission android:name="android.permission.READ_PHONE_STATE" />
	/// </summary>
	[PublicAPI]
	public static class AGTelephony
	{
		/// <summary>
		/// Open the voicemail settings activity to make changes to voicemail configuration.
		/// The EXTRA_PHONE_ACCOUNT_HANDLE extra indicates which PhoneAccountHandle to configure voicemail.
		/// The EXTRA_HIDE_PUBLIC_SETTINGS hides settings the dialer will modify through public API if set.
		/// </summary>
		[PublicAPI]
		public const string ActionConfigureVoiceMail = "android.telephony.action.CONFIGURE_VOICEMAIL";

		/// <summary>
		/// The EXTRA_STATE extra indicates the new call state.
		/// If a receiving app has Manifest.permission.READ_CALL_LOG permission, a second extra EXTRA_INCOMING_NUMBER
		/// provides the phone number for incoming and outgoing calls as a String.
		/// If the receiving app has Manifest.permission.READ_CALL_LOG and Manifest.permission.READ_PHONE_STATE permission,
		/// it will receive the broadcast twice; one with the EXTRA_INCOMING_NUMBER populated with the phone number,
		/// and another with it blank. Due to the nature of broadcasts, you cannot assume the order in which these broadcasts will arrive,
		/// however you are guaranteed to receive two in this case. Apps which are interested in the EXTRA_INCOMING_NUMBER
		/// can ignore the broadcasts where EXTRA_INCOMING_NUMBER is not present in the extras (e.g. where Intent.hasExtra(String) returns false).
		/// </summary>
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE" />
		[PublicAPI]
		public const string ActionPhoneStateChanged = "android.intent.action.PHONE_STATE";

		/// <summary>
		/// The Phone app sends this intent when a user opts to respond-via-message during an incoming call.
		/// By default, the device's default SMS app consumes this message and sends a text message to the caller.
		/// A third party app can also provide this functionality by consuming this Intent with a Service
		/// and sending the message using its own messaging system.
		/// </summary>
		[PublicAPI]
		public const string ActionRespondViaMessage = "android.intent.action.RESPOND_VIA_MESSAGE";

		/// <summary>
		/// Broadcast intent action for letting the default dialer to know to show voicemail notification.
		/// </summary>
		[PublicAPI]
		public const string ActionShowVoiceMailNotification = "android.telephony.action.SHOW_VOICEMAIL_NOTIFICATION";

		/// <summary>
		/// Broadcast Action: The subscription carrier identity has changed. This intent could be sent on the following events:
		/// Subscription absent. Carrier identity could change from a valid id to UNKNOWN_CARRIER_ID.
		/// Subscription loaded. Carrier identity could change from UNKNOWN_CARRIER_ID to a valid id.
		/// The subscription carrier is recognized after a remote update.
		/// </summary>
		[PublicAPI]
		public const string ActionSubscriptionCarrierIdentityChanged =
			"android.telephony.action.SUBSCRIPTION_CARRIER_IDENTITY_CHANGED";
		
		/// <summary>
		/// An unknown carrier id. It could either be subscription unavailable or the subscription carrier cannot be recognized.
		/// Unrecognized carriers here means MCC+MNC cannot be identified.
		/// </summary>
		[PublicAPI]
		public const int UnknownCarrierId = -1;

		/// <summary>
		/// UICC application types
		/// </summary>
		[PublicAPI]
		public enum AppStates
		{
			CSIM = 4,
			ISIM = 5,
			RUIM = 3,
			SIM = 2,
			USIM = 2
		}

		/// <summary>
		/// Authentication type for UICC challenge
		/// </summary>
		[PublicAPI]
		public enum AuthTypes
		{
			/// <summary>
			/// See https://tools.ietf.org/html/rfc4187 for details.
			/// </summary>
			EAP_AKA = 129,

			/// <summary>
			///  See https://tools.ietf.org/html/rfc4186 for details.
			/// </summary>
			EAP_SIM = 128
		}

		/// <summary>
		/// Device call state
		/// </summary>
		[PublicAPI]
		public enum PhoneStates
		{
			/// <summary>
			/// No activity.
			/// </summary>
			Idle = 0,

			/// <summary>
			/// At least one call exists that is dialing, active, or on hold, and no calls are ringing or waiting.
			/// </summary>
			OffHook = 2,

			/// <summary>
			/// A new call arrived and is ringing or waiting. In the latter case, another call is already active.
			/// </summary>
			Ringing = 1
		}

		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public enum CDMARoamingModes
		{
			/// <summary>
			/// Value which permits roaming on affiliated networks.
			/// </summary>
			Affiliated = 1,

			/// <summary>
			/// Value which permits roaming on any network.
			/// </summary>
			Any = 2,

			/// <summary>
			/// Value which only permits connections on home networks.
			/// </summary>
			Home = 0,

			/// <summary>
			/// Value which leaves the roaming mode set to the radio default or to the user's preference if they've indicated one.
			/// </summary>
			RadioDefault = -1
		}

		/// <summary>
		/// Data connection activity
		/// </summary>
		[PublicAPI]
		public enum DataActivities
		{
			/// <summary>
			/// Data connection is active, but physical link is down
			/// </summary>
			Dormant = 4,
			/// <summary>
			/// Currently receiving IP PPP traffic.
			/// </summary>
			In = 1,
			/// <summary>
			/// Currently both sending and receiving IP PPP traffic.
			/// </summary>
			InOut = 3,
			/// <summary>
			/// No traffic.
			/// </summary>
			None = 0,
			/// <summary>
			/// Currently sending IP PPP traffic.
			/// </summary>
			Out = 2
		}

		/// <summary>
		/// Data connection state
		/// </summary>
		[PublicAPI]
		public enum DataConnectionStates
		{
			/// <summary>
			/// Connected. IP traffic should be available.
			/// </summary>
			Connected = 2,
			/// <summary>
			/// Currently setting up a data connection.
			/// </summary>
			Connecting = 1,
			/// <summary>
			/// Disconnected. IP traffic not available.
			/// </summary>
			Disconnected = 0,
			/// <summary>
			/// Suspended. The connection is up, but IP traffic is temporarily unavailable.
			/// For example, in a 2G network, data activity may be suspended when a voice call arrives.
			/// </summary>
			Suspended = 3
		}
		
		#region Extras

		/// <summary>
		/// The intent to call voicemail.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public const string ExtraCallVoiceMailIntent = "android.telephony.extra.CALL_VOICEMAIL_INTENT";
		
		/// <summary>
		/// An int extra used with <see cref="ActionSubscriptionCarrierIdentityChanged"/> which indicates the updated carrier id SimCarrierId()
		/// of the current subscription. Will be UNKNOWN_CARRIER_ID if the subscription is unavailable or the carrier cannot be identified.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public const string ExtraCarrierId = "android.telephony.extra.CARRIER_ID";
		
		/// <summary>
		/// An string extra used with <see cref="ActionSubscriptionCarrierIdentityChanged"/> which indicates the updated carrier name of the current subscription.
		/// Carrier name is a user-facing name of the carrier id <see cref="ExtraCarrierId"/>, usually the brand name of the subsidiary (e.g. T-Mobile).
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public const string ExtraCarrierName = "android.telephony.extra.CARRIER_NAME";
		
		/// <summary>
		/// The boolean value indicating whether the voicemail settings activity launched by
		/// <see cref="ActionConfigureVoiceMail"/> should hide settings accessible through public API.
		/// This is used by dialer implementations which provides their own voicemail settings UI,
		/// but still needs to expose device specific voicemail settings to the user.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public const string ExtraHidePublicSettings = "android.telephony.extra.HIDE_PUBLIC_SETTINGS";
		
		/// <summary>
		/// Extra key used with the <see cref="ActionPhoneStateChanged"/> broadcast for a String containing the incoming or outgoing phone number.
		/// </summary>
		[PublicAPI]
		public const string ExtraIncomingNumber = "incoming_number";
		
		/// <summary>
		/// Boolean value representing whether the <see cref="ActionShowVoiceMailNotification"/> is new or a refresh of an existing notification.
		/// Notification refresh happens after reboot or connectivity changes. The user has already been notified for the voicemail so it should not alert the user, and should not be shown again if the user has dismissed it.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O_MR1)]
		public const string ExtraIsRefresh = "android.telephony.extra.IS_REFRESH";
		
		/// <summary>
		/// The intent to launch voicemail settings.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public const string ExtraLaunchVoiceMailSettingsIntent = "android.telephony.extra.LAUNCH_VOICEMAIL_SETTINGS_INTENT";
		
		/// <summary>
		/// The number of voice messages associated with the notification.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public const string ExtraNotificationCount = "android.telephony.extra.NOTIFICATION_COUNT";
		
		/// <summary>
		/// The extra used with an <see cref="ActionConfigureVoiceMail"/> and <see cref="ActionShowVoiceMailNotification"/> Intent
		/// to specify the PhoneAccountHandle the configuration or notification is for.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public const string ExtraPhoneAccountHandle = "android.telephony.extra.PHONE_ACCOUNT_HANDLE";
		
		/// <summary>
		/// The lookup key used with the <see cref="ActionPhoneStateChanged"/> broadcast for a String containing the new call state.
		/// </summary>
		[PublicAPI]
		public const string ExtraState = "state";
		
		/// <summary>
		/// An int extra used with <see cref="ActionSubscriptionCarrierIdentityChanged"/> to indicate the subscription which has changed.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public const string ExtraSubscriptionId = "android.telephony.extra.SUBSCRIPTION_ID";
		
		/// <summary>
		/// The voicemail number.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public const string ExtraVoiceMailNumber = "android.telephony.extra.VOICEMAIL_NUMBER";
		
		#endregion

		/// <summary>
		/// Current network type
		/// </summary>
		[PublicAPI]
		public enum NetworkTypes
		{
			_1xRTT = 7,
			CDMA = 4,
			EDGE = 2,
			EHRPD = 14,
			EVDO_0 = 5,
			EVDO_A = 6,
			EVDO_B = 12,
			GPRS = 1,
			[AndroidApi(AGDeviceInfo.VersionCodes.N_MR1)]
			GSM = 16,
			HSDPA = 8,
			HSPA = 10,
			HSPAP = 15,
			HSUPA = 9,
			IDEN = 11,
			[AndroidApi(AGDeviceInfo.VersionCodes.N_MR1)]
			IWLAN = 18,
			LTE = 13,
			[AndroidApi(AGDeviceInfo.VersionCodes.N_MR1)]
			TD_SCDMA = 17,
			UMTS = 3,
			Unknown = 0
		}

		[PublicAPI]
		public enum PhoneTypes
		{
			CDMA = 2,
			GSM = 1,
			None = 0,
			SIP = 3
		}
		
		/// <summary>
		/// Current sim state
		/// </summary>
		[PublicAPI]
		public enum SimStates
		{
			/// <summary>
			/// SIM card state: Unknown. Signifies that the SIM is in transition between states
			/// </summary>
			Unknown = 0,
			
			/// <summary>
			/// SIM card state: no SIM card is available in the device
			/// </summary>
			Absent = 1,

			/// <summary>
			/// Locked: requires the user's SIM PIN to unlock
			/// </summary>
			PinRequired = 2,

			/// <summary>
			/// Locked: requires the user's SIM PUK to unlock
			/// </summary>
			PukRequired = 3,
			
			/// <summary>
			/// Locked: requires a network PIN to unlock
			/// </summary>
			NetworkLocked = 4,

			/// <summary>
			/// Ready
			/// </summary>
			Ready = 5,

			/// <summary>
			/// SIM Card is NOT READY
			/// </summary>
			[AndroidApi(AGDeviceInfo.VersionCodes.O)]
			NotReady = 6,

			/// <summary>
			/// SIM Card Error, permanently disabled
			/// </summary>
			[AndroidApi(AGDeviceInfo.VersionCodes.O)]
			PermDisabled = 7,
			
			/// <summary>
			/// SIM Card Error, present but faulty
			/// </summary>
			[AndroidApi(AGDeviceInfo.VersionCodes.O)]
			CardIoError = 8,
			
			/// <summary>
			/// SIM Card restricted, present but not usable due to carrier restrictions.
			/// </summary>
			[AndroidApi(AGDeviceInfo.VersionCodes.O)]
			CardRestricted = 9
		}

		/// <summary>
		/// Returns a constant indicating the state of the default SIM card.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static SimStates SimState
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return SimStates.Unknown;
				}
				return (SimStates) AGSystemService.TelephonyService.CallInt("getSimState");
			}
		}

		/// <summary>
		/// Gets the telephony device identifier.
		/// </summary>
		/// <value>The telephony device identifier.</value>
		[PublicAPI]
		public static string TelephonyDeviceId
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return AGSystemService.TelephonyService.CallStrSafe("getDeviceId");
			}
		}

		/// <summary>
		/// Gets the telephony sim serial number.
		/// </summary>
		/// <value>The telephony sim serial number.</value>
		[PublicAPI]
		public static string TelephonySimSerialNumber
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return AGSystemService.TelephonyService.CallStrSafe("getSimSerialNumber");
			}
		}

		/// <summary>
		/// Returns the ISO country code equivalent of the current registered operator's MCC (Mobile Country Code).
		/// </summary>
		/// <value>The ISO country code equivalent of the current registered operator's MCC (Mobile Country Code)..</value>
		[PublicAPI]
		public static string NetworkCountryIso
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}

				return AGSystemService.TelephonyService.CallStrSafe("getNetworkCountryIso");
			}
		}

		/// <summary>
		/// Returns all observed cell information from all radios on the device including the primary and neighboring cells.
		/// This method returns valid data for registered cells on devices with PackageManager.FEATURE_TELEPHONY.
		/// In cases where only partial information is available for a particular CellInfo entry,
		/// unavailable fields will be reported as Integer.MAX_VALUE. All reported cells will include at least
		/// a valid set of technology-specific identification info and a power level measurement.
		/// <uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION"/>
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.JELLY_BEAN_MR1)]
		public static List<CellInfo> AllCellInfo
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.JELLY_BEAN_MR1))
				{
					return new List<CellInfo>();
				}
				
				var result = new List<CellInfo>();
				var cellsListAjo = AGSystemService.TelephonyService.CallAJOSafe("getAllCellInfo");

				if (cellsListAjo == null)
				{
					return new List<CellInfo>();
				}
				
				var ajos = cellsListAjo.FromJavaList<AndroidJavaObject>();
				foreach (var ajo in ajos)
				{
					result.Add(new CellInfo(ajo));
				}

				return result;
			}
		}

		/// <summary>
		/// Returns one of the constants that represent the current state of all phone calls.
		/// </summary>
		[PublicAPI]
		public static PhoneStates CallState
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return PhoneStates.Idle;
				}
				
				return (PhoneStates) AGSystemService.TelephonyService.CallInt("getCallState");
			}
		}

		/// <summary>
		/// A constant indicating the type of activity on a data connection (cellular).
		/// </summary>
		[PublicAPI]
		public static DataActivities DataActivity
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return DataActivities.None;
				}
				
				return (DataActivities) AGSystemService.TelephonyService.CallInt("getDataActivity");
			}
		}
		
		/// <summary>
		/// A constant indicating the radio technology (network type) currently in use on the device for data transmission. 
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.N)]
		public static NetworkTypes DataNetworkType
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return NetworkTypes.Unknown;
				}
				
				return (NetworkTypes) AGSystemService.TelephonyService.CallIntSafe("getDataNetworkType");
			}
		}
		
		/// <summary>
		/// A constant indicating the current data connection state (cellular).
		/// </summary>
		[PublicAPI]
		public static DataConnectionStates DataState
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return DataConnectionStates.Disconnected;
				}
				
				return (DataConnectionStates) AGSystemService.TelephonyService.CallIntSafe("getDataState");
			}
		}

		/// <summary>
		/// Returns the unique device ID, for example, the IMEI for GSM and the MEID or ESN for CDMA phones.
		/// Return null if device ID is not available.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		[Obsolete("Was deprecated in API level 26. Use IMEI and MEID instead.")]
		[AndroidApi(AGDeviceInfo.VersionCodes.M)]
		public static string DeviceId
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
				{
					return string.Empty;
				}

				return TelephonyDeviceId;
			}
		}
		
		/// <summary>
		/// The software version number for the device, for example, the IMEI/SV for GSM phones. Return null if the software version is not available.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		public static string DeviceSoftwareVersion
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}

				return AGSystemService.TelephonyService.CallStrSafe("getDeviceSoftwareVersion");
			}
		}
		
		/// <summary>
		/// An array of Forbidden PLMNs from the USIM App Returns null if the query fails.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static string[] ForbiddenPlmns
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return new []{string.Empty};
				}


				try
				{

				return AGSystemService.TelephonyService.Call<string[]>("getForbiddenPlmns");
				}
				catch (Exception e)
				{
					Debug.Log(e);
					return new string[] {};
				}
			}
		}
		
		/// <summary>
		/// The Group Identifier Level1 for a GSM phone. Returns null if it is unavailable.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.JELLY_BEAN_MR2)]
		public static string GroupIdLevel1
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.JELLY_BEAN_MR2))
				{
					return string.Empty;
				}

				return AGSystemService.TelephonyService.CallStrSafe("getGroupIdLevel1");
			}
		}
		
		/// <summary>
		/// Returns the IMEI (International Mobile Equipment Identity). Returns null if IMEI is not available.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static string IMEI
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return string.Empty;
				}

				return AGSystemService.TelephonyService.CallStrSafe("getImei");
			}
		}
		
		/// <summary>
		/// Returns the phone number string for line 1, for example, the MSISDN for a GSM phone. Return null if it is unavailable.
		/// <uses-permissions android:name="android.permission.READ_PHONE_STATE, android.permission.READ_SMS, android.permission.READ_PHONE_NUMBERS"/>,
		/// that the caller is the default SMS app, or that the caller has carrier privileges (see hasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		public static string Line1Number
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}

				return AGSystemService.TelephonyService.CallStrSafe("getLine1Number");
			}
		}
		
		/// <summary>
		/// Returns the MEID (Mobile Equipment Identifier). Return null if MEID is not available.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static string MEID
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return string.Empty;
				}

				return AGSystemService.TelephonyService.CallStrSafe("getMeid");
			}
		}
		
		/// <summary>
		/// Returns the MMS user agent profile URL.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.KITKAT)]
		public static string MmsUaProfUrl
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.KITKAT))
				{
					return string.Empty;
				}

				return AGSystemService.TelephonyService.CallStrSafe("getMmsUAProfUrl");
			}
		}
		
		/// <summary>
		/// Returns the MMS user agent.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.KITKAT)]
		public static string MmsUserAgent
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.KITKAT))
				{
					return string.Empty;
				}

				return AGSystemService.TelephonyService.CallStrSafe("getMmsUserAgent");
			}
		}
		
		/// <summary>
		/// Returns the Network Access Identifier (NAI). Return null if NAI is not available.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public static string Nai
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.P))
				{
					return string.Empty;
				}

				return AGSystemService.TelephonyService.CallStrSafe("getNai");
			}
		}
		
		/// <summary>
		/// Returns the numeric name (MCC+MNC) of current registered operator.
		/// Availability: Only when user is registered to a network. Result may be unreliable on CDMA networks (use PhoneType()
		/// to determine if on a CDMA network).
		/// </summary>
		[PublicAPI]
		public static string NetworkOperator
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}

				return AGSystemService.TelephonyService.CallStrSafe("getNetworkOperator");
			}
		}
		
		/// <summary>
		/// Returns the alphabetic name of current registered operator.
		/// Availability: Only when user is registered to a network. Result may be unreliable on CDMA networks
		/// (use PhoneType() to determine if on a CDMA network).
		/// </summary>
		[PublicAPI]
		public static string NetworkOperatorName
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}

				return AGSystemService.TelephonyService.CallStrSafe("getNetworkOperatorName");
			}
		}
		
		/// <summary>
		/// Returns the network specifier of the subscription ID pinned to the TelephonyManager.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static string NetworkSpecifier
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return string.Empty;
				}

				return AGSystemService.TelephonyService.CallStrSafe("getNetworkSpecifier");
			}
		}

		/// <summary>
		/// The Network Type for current data connection.
		/// </summary>
		[PublicAPI]
		public static NetworkTypes NetworkType
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return NetworkTypes.Unknown;
				}

				return (NetworkTypes) AGSystemService.TelephonyService.CallIntSafe("getNetworkType");
			}
		}

		/// <summary>
		/// Returns the number of phones available.
		/// Returns 0 if none of voice, sms, data is not supported
		/// Returns 1 for Single standby mode (Single SIM functionality)
		/// Returns 2 for Dual standby mode.(Dual SIM functionality)
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.M)]
		public static int PhoneCount
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
				{
					return 0;
				}

				return AGSystemService.TelephonyService.CallIntSafe("getPhoneCount");
			}
		}
		
		/// <summary>
		/// Returns a constant indicating the device phone type. This indicates the type of radio used to transmit voice calls.
		/// </summary>
		[PublicAPI]
		public static PhoneTypes PhoneType
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return PhoneTypes.None;
				}

				return (PhoneTypes) AGSystemService.TelephonyService.CallIntSafe("getPhoneType");
			}
		}

		/// <summary>
		/// The current ServiceState information.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static ServiceStates ServiceState
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return null;
				}

				return new ServiceStates(AGSystemService.TelephonyService.CallAJOSafe("getServiceState"));
			}
		}

		/// <summary>
		/// Get the most recently available signal strength information.
		/// Get the most recent SignalStrength information reported by the modem.
		/// Due to power saving this information may not always be current.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public static SignalStrengths SignalStrength
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.P))
				{
					return new SignalStrengths();
				}
				
				return new SignalStrengths(AGSystemService.TelephonyService.CallAJOSafe("getSignalStrength"));
			}
		}

		/// <summary>
		/// Returns carrier id of the current subscription.
		/// To recognize a carrier (including MVNO) as a first-class identity, Android assigns each carrier with a canonical integer a.k.a. carrier id.
		/// The carrier ID is an Android platform-wide identifier for a carrier.
		/// Apps which have carrier-specific configurations or business logic can use the carrier id as an Android platform-wide identifier for carriers.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public static int SimCarrierId
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.P))
				{
					return UnknownCarrierId;
				}
				
				return AGSystemService.TelephonyService.CallIntSafe("getSimCarrierId");
			}
		}
		
		/// <summary>
		///Returns carrier id name of the current subscription.
		/// Carrier id name is a user-facing name of carrier id getSimCarrierId(), usually the brand name of the subsidiary (e.g. T-Mobile).
		/// Each carrier could configure multiple SPN but should have a single carrier name.
		/// Carrier name is not a canonical identity, use getSimCarrierId() instead.
		/// The returned carrier name is unlocalized.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public static string SimCarrierIdName
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.P))
				{
					return string.Empty;
				}
				
				return AGSystemService.TelephonyService.CallStrSafe("getSimCarrierIdName");
			}
		}
		
		/// <summary>
		/// Returns the ISO country code equivalent for the SIM provider's country code.
		/// </summary>
		[PublicAPI]
		public static string SimCountryIso
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return AGSystemService.TelephonyService.CallStrSafe("getSimCountryIso");
			}
		}
		
		/// <summary>
		/// Returns the MCC+MNC (mobile country code + mobile network code) of the provider of the SIM. 5 or 6 decimal digits.
		/// Availability: SIM state must be <see cref="SimStates.Ready"/>
		/// </summary>
		[PublicAPI]
		public static string SimOperator
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return AGSystemService.TelephonyService.CallStrSafe("getSimOperator");
			}
		}
		
		/// <summary>
		/// Returns the Service Provider Name (SPN).
		/// Availability: SIM state must be <see cref="SimStates.Ready"/>
		/// </summary>
		[PublicAPI]
		public static string SimOperatorName
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return AGSystemService.TelephonyService.CallStrSafe("getSimOperatorName");
			}
		}
		
		/// <summary>
		/// Returns the serial number of the SIM, if applicable. Return null if it is unavailable.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		public static string SimSerialNumber
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return AGSystemService.TelephonyService.CallStrSafe("getSimSerialNumber");
			}
		}
		
		/// <summary>
		/// Returns the unique subscriber ID, for example, the IMSI for a GSM phone. Return null if it is unavailable.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		public static string SubscriberId
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return AGSystemService.TelephonyService.CallStrSafe("getSubscriberId");
			}
		}
		
		/// <summary>
		/// Returns the package responsible of processing visual voicemail for the subscription ID pinned to the TelephonyManager.
		/// Returns null when there is no package responsible for processing visual voicemail for the subscription.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static string VisualVoicemailPackageName
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return string.Empty;
				}
				
				return AGSystemService.TelephonyService.CallStrSafe("getVisualVoicemailPackageName");
			}
		}
		
		/// <summary>
		/// Retrieves the alphabetic identifier associated with the voice mail number.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		public static string VoiceMailAlphaTag
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return AGSystemService.TelephonyService.CallStrSafe("getVoiceMailAlphaTag");
			}
		}
		
		/// <summary>
		/// Returns the voice mail number. Return null if it is unavailable.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		public static string VoiceMailNumber
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return AGSystemService.TelephonyService.CallStrSafe("getVoiceMailNumber");
			}
		}

		/// <summary>
		/// Returns the network type for voice.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.N)]
		public static NetworkTypes VoiceNetworkType
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return NetworkTypes.Unknown;
				}
				
				return (NetworkTypes) AGSystemService.TelephonyService.CallIntSafe("getVoiceNetworkType");
			}
		}

		/// <summary>
		/// Has the calling application been granted carrier privileges by the carrier.
		/// If any of the packages in the calling UID has carrier privileges, the call will return true.
		/// This access is granted by the owner of the UICC card and does not depend on the registered carrier.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP_MR1)]
		public static bool HasCarrierPrivileges
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP_MR1))
				{
					return false;
				}
				
				return AGSystemService.TelephonyService.CallBool("hasCarrierPrivileges");
			}
		}
		
		/// <summary>
		/// True if a ICC card is present
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.ECLAIR)]
		public static bool HasIccCard
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.ECLAIR))
				{
					return false;
				}
				
				return AGSystemService.TelephonyService.CallBool("hasIccCard");
			}
		}

		/// <summary>
		/// Closes a previously opened logical channel to the ICC card. Input parameters equivalent to TS 27.007 AT+CCHC command.
		/// <uses-permission android:name="android.permission.READ_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		/// <param name="channelId"> The channel id to be closed as returned by a successful IccOpenLogicalChannel. </param>
		/// <uses-permission android:name="android.permission.MODIFY_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP)]
		public static void IccCloseLogicalChannel(int channelId)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP))
			{
				return;
			}
			
			AGSystemService.TelephonyService.CallBool("iccCloseLogicalChannel", channelId);
		}
		
		/// <summary>
		/// Opens a logical channel to the ICC card. Input parameters equivalent to TS 27.007 AT+CCHO command.
		/// <uses-permission android:name="android.permission.MODIFY_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		/// <param name="appId">  Application id. See ETSI 102.221 and 101.220. </param>
		[PublicAPI]
		[Obsolete("This method was deprecated in API level 26. Use IccOpenLogicalChannel(string, int)")]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP)]
		public static IccOpenLogicalChannelResponse IccOpenLogicalChannel(string appId)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP))
			{
				return new IccOpenLogicalChannelResponse();
			}
			
			return new IccOpenLogicalChannelResponse(AGSystemService.TelephonyService.CallAJOSafe("iccOpenLogicalChannel", appId));
		}
		
		/// <summary>
		/// Opens a logical channel to the ICC card. Input parameters equivalent to TS 27.007 AT+CCHO command.
		/// <uses-permission android:name="android.permission.MODIFY_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		/// <param name="appId">  Application id. See ETSI 102.221 and 101.220. </param>
		/// <param name="p2"> P2 parameter (described in ISO 7816-4). </param>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static IccOpenLogicalChannelResponse IccOpenLogicalChannel(string appId, int p2)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
			{
				return new IccOpenLogicalChannelResponse();
			}
			
			return new IccOpenLogicalChannelResponse(AGSystemService.TelephonyService.CallAJOSafe("iccOpenLogicalChannel", appId, p2));
		}
		
		/// <summary>
		/// Transmit an APDU to the ICC card over the basic channel. Input parameters equivalent to TS 27.007 AT+CSIM command.
		/// <uses-permission android:name="android.permission.MODIFY_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		/// <param name="cla"> Class of the APDU command. </param>
		/// <param name="instruction">  Instruction of the APDU command. </param>
		/// <param name="p1"> P1 value of the APDU command. </param>
		/// <param name="p2"> P2 value of the APDU command. </param>
		/// <param name="p3"> P3 value of the APDU command. If p3 is negative a 4 byte APDU is sent to the SIM. </param>
		/// <param name="data"> Data to be sent with the APDU. </param>
		/// <returns> The APDU response from the ICC card with the status appended at the end. </returns>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP)]
		public static string IccTransmitApduBasicChannel(int cla, int instruction, int p1, int p2, int p3, string data)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP))
			{
				return string.Empty;
			}
			
			return AGSystemService.TelephonyService.CallStrSafe("iccTransmitApduBasicChannel", cla, instruction, p1, p2, p3, data);
		}
		
		/// <summary>
		/// Transmit an APDU to the ICC card over a logical channel. Input parameters equivalent to TS 27.007 AT+CGLA command.
		/// <uses-permission android:name="android.permission.MODIFY_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		/// <param name="channelId"> Channel id to be closed as returned by a successful IccOpenLogicalChannel. </param>
		/// <param name="cla"> Class of the APDU command. </param>
		/// <param name="instruction">  Instruction of the APDU command. </param>
		/// <param name="p1"> P1 value of the APDU command. </param>
		/// <param name="p2"> P2 value of the APDU command. </param>
		/// <param name="p3"> P3 value of the APDU command. If p3 is negative a 4 byte APDU is sent to the SIM. </param>
		/// <param name="data"> Data to be sent with the APDU. </param>
		/// <returns> The APDU response from the ICC card with the status appended at the end. </returns>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP)]
		public static string IccTransmitApduLogicalChannel(int channelId, int cla, int instruction, int p1, int p2, int p3, string data)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP))
			{
				return string.Empty;
			}
			
			return AGSystemService.TelephonyService.CallStrSafe("iccTransmitApduLogicalChannel", channelId, cla, instruction, p1, p2, p3, data);
		}
		
		/// <summary>
		/// Whether the device is currently on a technology (e.g. UMTS or LTE) which can support voice and data simultaneously.
		/// This can change based on location or network condition.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static bool IsConcurrentVoiceAndDataSupported
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return false;
				}
				
				return AGSystemService.TelephonyService.CallBool("isConcurrentVoiceAndDataSupported");
			}
		}
		
		/// <summary>
		/// Returns whether mobile data is enabled or not per user setting.
		/// There are other factors that could disable mobile data, but they are not considered here.
		/// If this object has been created with createForSubscriptionId(int), applies to the given subId.
		/// Otherwise, applies to SubscriptionManager.getDefaultDataSubscriptionId()
		/// Requires one of the following permissions: ACCESS_NETWORK_STATE, MODIFY_PHONE_STATE, or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// Note that this does not take into account any data restrictions that may be present on the calling app.
		/// Such restrictions may be inspected with ConnectivityManager.getRestrictBackgroundStatus().
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static bool IsDataEnabled
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return false;
				}
				
				return AGSystemService.TelephonyService.CallBool("isDataEnabled");
			}
		}
		
		/// <summary>
		/// Whether the phone supports hearing aid compatibility.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.M)]
		public static bool IsHearingAidCompatibilitySupported
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
				{
					return false;
				}
				
				return AGSystemService.TelephonyService.CallBool("isHearingAidCompatibilitySupported");
			}
		}
		
		/// <summary>
		/// Returns true if the device is considered roaming on the current network, for GSM purposes.
		/// Availability: Only when user registered to a network.
		/// </summary>
		[PublicAPI]
		public static bool IsNetworkRoaming
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return false;
				}
				
				return AGSystemService.TelephonyService.CallBool("isNetworkRoaming");
			}
		}
		
		/// <summary>
		/// True if the current device supports sms service.
		/// If true, this means that the device supports both sending and receiving sms via the telephony network.
		/// Note: Voicemail waiting sms, cell broadcasting sms, and MMS are disabled when device doesn't support sms.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP)]
		public static bool IsSmsCapable
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP))
				{
					return false;
				}
				
				return AGSystemService.TelephonyService.CallBool("isSmsCapable");
			}
		}
		
		/// <summary>
		/// True if the device supports TTY mode, and false otherwise.
		/// </summary>
		[PublicAPI]
		[Obsolete("This method was deprecated in API level 28. Use TelecomManager.IsTtySupported() instead.")]
		[AndroidApi(AGDeviceInfo.VersionCodes.M)]
		public static bool IsTtyModeSupported
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
				{
					return false;
				}
				
				return AGSystemService.TelephonyService.CallBool("isTtyModeSupported");
			}
		}
		
		/// <summary>
		/// True if the current device is "voice capable".
		/// "Voice capable" means that this device supports circuit-switched (i.e. voice) phone calls over the telephony network,
		/// and is allowed to display the in-call UI while a cellular voice call is active.
		/// This will be false on "data only" devices which can't make voice calls and don't support any in-call UI.
		/// Note: the meaning of this flag is subtly different from the PackageManager.FEATURE_TELEPHONY system feature,
		/// which is available on any device with a telephony radio, even if the device is data-only.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP_MR1)]
		public static bool IsVoiceCapable
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP_MR1))
				{
					return false;
				}
				
				return AGSystemService.TelephonyService.CallBool("isVoiceCapable");
			}
		}
		
		/// <summary>
		/// Whether the device is a world phone.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.M)]
		public static bool IsWorldPhone
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
				{
					return false;
				}
				
				return AGSystemService.TelephonyService.CallBoolSafe("isWorldPhone");
			}
		}
		
		/// <summary>
		/// Send the special dialer code. The IPC caller must be the current default dialer or have carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static void SendDialerSpecialCode(string code)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
			{
				return;
			}
			
			AGSystemService.TelephonyService.Call("sendDialerSpecialCode", code);
		}
		
		/// <summary>
		/// Send ENVELOPE to the SIM and return the response.
		/// <uses-permission android:name="android.permission.MODIFY_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		/// <param name="content">
		/// String containing SAT/USAT response in hexadecimal format starting with command tag. See TS 102 223 for details.
		/// </param>
		/// <returns>
		/// The APDU response from the ICC card in hexadecimal format with the last 4 bytes being the status word.
		/// If the command fails, returns an empty string.
		/// </returns>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP)]
		public static string SendEnvelopeWithStatus(string content)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP))
			{
				return string.Empty;
			}
			
			return AGSystemService.TelephonyService.CallStrSafe("sendEnvelopeWithStatus", content);
		}

		/// <summary>
		/// Turns mobile data on or off.
		/// <uses-permission android:name="android.permission.MODIFY_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static void SetDataEnabled(bool isEnabled)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
			{
				return;
			}

			AGSystemService.TelephonyService.Call("setDataEnabled", isEnabled);
		}

		/// <summary>
		/// Set the line 1 phone number string and its alphatag for the current ICCID for display purpose only,
		/// for example, displayed in Phone Status. It won't change the actual MSISDN/MDN.
		/// To unset alphatag or number, pass in a null value.
		/// Requires that the calling app has carrier privileges.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP_MR1)]
		public static void SetLine1NumberForDisplay(string alphaTag, string number)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP_MR1))
			{
				return;
			}

			AGSystemService.TelephonyService.CallBoolSafe("setLine1NumberForDisplay", alphaTag, number);
		}
		
		/// <summary>
		/// Sets the network selection mode to automatic.
		/// <uses-permission android:name="android.permission.MODIFY_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public static void SetNetworkSelectionModeAutomatic()
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.P))
			{
				return;
			}

			AGSystemService.TelephonyService.Call("setLine1NumberForDisplay");
		}
		
		/// <summary>
		/// Ask the radio to connect to the input network and change selection mode to manual.
		/// <uses-permission android:name="android.permission.MODIFY_PHONE_STATE"/> or that the calling app has carrier privileges (see HasCarrierPrivileges()).
		/// </summary>
		/// <param name="operatorNumeric">
		/// The PLMN ID of the network to select.
		/// </param>
		/// <param name="persistSelection">
		/// Whether the selection will persist until reboot.
		/// If true, only allows attaching to the selected PLMN until reboot;
		/// otherwise, attach to the chosen PLMN and resume normal network selection next time.
		/// </param>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public static void SetNetworkSelectionModeManual(string operatorNumeric, bool persistSelection)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.P))
			{
				return;
			}

			AGSystemService.TelephonyService.CallBoolSafe("setNetworkSelectionModeManual", operatorNumeric, persistSelection);
		}
		
		/// <summary>
		/// Override the branding for the current ICCID. Once set, whenever the SIM is present in the device,
		/// the service provider name (SPN) and the operator name will both be replaced by the brand value input.
		/// To unset the value, the same function should be called with a null brand value.
		/// Requires that the calling app has carrier privileges.
		/// </summary>
		/// <param name="brand"></param>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP_MR1)]
		public static void SetOperatorBrandOverride(string brand)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP_MR1))
			{
				return;
			}

			AGSystemService.TelephonyService.CallBoolSafe("setOperatorBrandOverride", brand);
		}
		
		/// <summary>
		/// Set the preferred network type to global mode which includes LTE, CDMA, EvDo and GSM/WCDMA.
		/// Requires that the calling app has carrier privileges.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP_MR1)]
		public static void SetPreferredNetworkTypeToGlobal()
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP_MR1))
			{
				return;
			}

			AGSystemService.TelephonyService.CallBoolSafe("setPreferredNetworkTypeToGlobal");
		}

		/// <summary>
		/// Set the visual voicemail SMS filter settings for the subscription ID pinned to the TelephonyManager.
		/// Caller must be the default dialer, system dialer, or carrier visual voicemail app.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static void SetVisualVoicemailSmsFilterSettings(VisualVoicemailSmsFilterSettings settings)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
			{
				return;
			}

			AGSystemService.TelephonyService.Call("setVisualVoicemailSmsFilterSettings", settings.ajo);
		}
	}
}
#endif