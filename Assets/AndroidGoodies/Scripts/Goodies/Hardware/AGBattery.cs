// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGBattery.cs
//


#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	[PublicAPI]
	public static class AGBattery
	{
		/// <summary>
		/// Sent when the device's battery has started charging (or has reached full charge and the device is on power).
		/// This is a good time to do work that you would like to avoid doing while on battery
		/// (that is to avoid draining the user's battery due to things they don't care enough about).
		/// This is paired with ActionDischarging. The current state can always be retrieved with IsCharging().
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.M)]
		public const string ActionCharging = "android.os.action.CHARGING";

		/// <summary>
		/// Sent when the device's battery may be discharging, so apps should avoid doing extraneous work
		/// that would cause it to discharge faster. This is paired with ActionCharging.
		/// The current state can always be retrieved with IsCharging().
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.M)]
		public const string ActionDischarging = "android.os.action.DISCHARGING";

		/// <summary>
		/// Constants indicating battery health
		/// </summary>
		[PublicAPI]
		public enum BatteryHealth
		{
			[AndroidApi(AGDeviceInfo.VersionCodes.HONEYCOMB)]
			Cold = 7,
			Dead = 4,
			Good = 2,
			Overheat = 3,
			OverVoltage = 5,
			Unknown = 1,
			UnspecifiedFailure = 6
		}

		/// <summary>
		/// Power source types
		/// </summary>
		[PublicAPI]
		public enum BatteryPlugged
		{
			/// <summary>
			/// Power source is device's battery.
			/// </summary>
			OnBattery = 0,
			/// <summary>
			/// Power source is an AC charger.
			/// </summary>
			Ac =1,
			/// <summary>
			/// Power source is a USB port.
			/// </summary>
			Usb = 2,
			/// <summary>
			/// Power source is wireless.
			/// </summary>
			[AndroidApi(AGDeviceInfo.VersionCodes.JELLY_BEAN_MR1)]
			Wireless = 4
		}

		enum BatteryPropertyId
		{
			Capacity = 4,
			ChargeCounter = 1,
			CurrentAverage = 3,
			CurrentNow = 2,
			EnergyCounter = 5,
			Status = 6
		}

		/// <summary>
		/// Constants describing possible battery statuses
		/// </summary>
		[PublicAPI]
		public enum BatteryStatus
		{
			Charging = 2,
			Discharging = 3,
			Full = 5,
			NotCharging = 4,
			Unknown = 1
		}

		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		const string ExtraBatteryLow = "battery_low";
		const string ExtraHealth = "health";
		const string ExtraIconSmall = "icon-small";
		const string ExtraLevel = "level";
		const string ExtraPlugged = "plugged";
		const string ExtraPresent = "present";
		const string ExtraScale = "scale";
		const string ExtraStatus = "status";
		const string ExtraTechnology = "technology";
		const string ExtraTemperature = "temperature";
		const string ExtraVoltage = "voltage";

		const string GetIntPropertyMethodName = "getIntProperty";
		const string GetIntExtraMethod = "getIntExtra";
		const string GetBoolExtraMethod = "getBooleanExtra";
		
		static AndroidJavaObject BatteryChangeIntentFilter
		{
			get
			{
				return new AndroidJavaObject("android.content.IntentFilter", AndroidIntent.ActionBatteryChanged);
			}
		}

		static AndroidJavaObject BatteryChangeIntentReceiver
		{
			get { return AGUtils.Activity.CallAJO("registerReceiver", null, BatteryChangeIntentFilter); }
		}

		/// <summary>
		/// Gets the battery charge level from 1-100.
		/// </summary>
		/// <returns>The battery charge level from 1-100.</returns>
		[PublicAPI]
		public static float GetBatteryChargeLevel()
		{
			if (AGUtils.IsNotAndroid())
			{
				return 0f;
			}

			using (BatteryChangeIntentFilter)
			{
				using (BatteryChangeIntentReceiver)
				{
					var level = BatteryChangeIntentReceiver.CallInt(GetIntExtraMethod, ExtraLevel, -1);
					var scale = BatteryChangeIntentReceiver.CallInt(GetIntExtraMethod, ExtraScale, -1);
					
					if (level == -1 || scale == -1)
					{
						return 50.0f;
					}

					return level / (float) scale * 100.0f;
				}
			}
		}

		/// <summary>
		/// An approximation for how much time (in milliseconds) remains until the battery is fully charged.
		/// Returns -1 if no time can be computed: either there is not enough current data to make a decision or the battery is currently discharging.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public static long ChargeTimeRemaining
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.P))
				{
					return 0;
				}

				return AGSystemService.BatteryService.CallLong("computeChargeTimeRemaining");
			}
		}

		/// <summary>
		/// Remaining battery capacity as an integer percentage of total capacity (with no fractional part).
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP)]
		public static int Capacity
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP))
				{
					return 0;
				}
				
				return AGSystemService.BatteryService.CallInt(GetIntPropertyMethodName, (int) BatteryPropertyId.Capacity);
			}
		}
		
		/// <summary>
		/// Battery capacity in micro ampere-hours, as an integer.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP)]
		public static int ChargeCounter
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP))
				{
					return 0;
				}
				
				return AGSystemService.BatteryService.CallInt(GetIntPropertyMethodName, (int) BatteryPropertyId.ChargeCounter);
			}
		}
		
		/// <summary>
		/// Average battery current in micro amperes, as an integer.
		/// Positive values indicate net current entering the battery from a charge source,
		/// negative values indicate net current discharging from the battery.
		/// The time period over which the average is computed may depend on the fuel gauge hardware and its configuration.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP)]
		public static int CurrentAverage
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP))
				{
					return 0;
				}
				
				return AGSystemService.BatteryService.CallInt(GetIntPropertyMethodName, (int) BatteryPropertyId.CurrentAverage);
			}
		}
		
		/// <summary>
		/// Instantaneous battery current in micro amperes, as an integer.
		/// Positive values indicate net current entering the battery from a charge source,
		/// negative values indicate net current discharging from the battery.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP)]
		public static int CurrentNow
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP))
				{
					return 0;
				}
				
				return AGSystemService.BatteryService.CallInt(GetIntPropertyMethodName, (int) BatteryPropertyId.CurrentNow);
			}
		}
		
		/// <summary>
		/// Battery remaining energy in nano watt-hours, as a long integer.
		/// Long.MIN_VALUE if not supported.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP)]
		public static long EnergyCounter
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP))
				{
					return 0;
				}
				
				return AGSystemService.BatteryService.CallLong("getLongProperty", (int) BatteryPropertyId.EnergyCounter);
			}
		}
		
		/// <summary>
		/// Battery charge status, one from <see cref="BatteryStatus"> values.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP)]
		public static BatteryStatus Status
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return BatteryStatus.Unknown;
				}

				if (Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP))
				{
					return (BatteryStatus) AGSystemService.BatteryService.CallInt(GetIntPropertyMethodName, (int) BatteryPropertyId.Status);
				}
				
				using (BatteryChangeIntentFilter)
				{
					using (BatteryChangeIntentReceiver)
					{
						return (BatteryStatus) BatteryChangeIntentReceiver.CallInt(GetIntExtraMethod, ExtraStatus, 1);
					}
				}
			}
		}

		/// <summary>
		/// Whether the battery is currently considered to be low,
		/// that is whether a Intent.ActionBatteryLow broadcast has been sent.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public static bool IsBatteryLow
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.P))
				{
					return false;
				}

				using (BatteryChangeIntentFilter)
				{
					using (BatteryChangeIntentReceiver)
					{
						return BatteryChangeIntentReceiver.CallBool(GetBoolExtraMethod, ExtraBatteryLow, false);
					}
				}
			}
		}

		/// <summary>
		/// Current health constant, one of BatteryHealth values.
		/// </summary>
		[PublicAPI]
		public static BatteryHealth Health
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return BatteryHealth.Unknown;
				}

				using (BatteryChangeIntentFilter)
				{
					using (BatteryChangeIntentReceiver)
					{
						return (BatteryHealth) BatteryChangeIntentReceiver.CallInt(GetIntExtraMethod, ExtraHealth, 1);
					}
				}
			}
		}
		
		/// <summary>
		/// The resource ID of a small status bar icon indicating the current battery state.
		/// </summary>
		[PublicAPI]
		public static int IconSmall
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return R.UnityLauncherIcon;
				}

				using (BatteryChangeIntentFilter)
				{
					using (BatteryChangeIntentReceiver)
					{
						return BatteryChangeIntentReceiver.CallInt(GetIntExtraMethod, ExtraIconSmall, R.UnityLauncherIcon);
					}
				}
			}
		}
		
		/// <summary>
		/// The current battery level, from 0 to Scale.
		/// Value is -1 if something went wrong.
		/// </summary>
		[PublicAPI]
		public static int Level
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return -1;
				}

				using (BatteryChangeIntentFilter)
				{
					using (BatteryChangeIntentReceiver)
					{
						return BatteryChangeIntentReceiver.CallInt(GetIntExtraMethod, ExtraLevel, -1);
					}
				}
			}
		}

		/// <summary>
		/// One of the BatteryPlugged values indicating whether the device is plugged in to a power source.
		/// </summary>
		[PublicAPI]
		public static BatteryPlugged PluggedState
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return BatteryPlugged.OnBattery;
				}

				using (BatteryChangeIntentFilter)
				{
					using (BatteryChangeIntentReceiver)
					{
						return (BatteryPlugged) BatteryChangeIntentReceiver.CallInt(GetIntExtraMethod, ExtraPlugged, 0);
					}
				}
			}
		}
		
		/// <summary>
		/// Whether a battery is present.
		/// </summary>
		[PublicAPI]
		public static bool IsBatteryPresent
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return false;
				}

				using (BatteryChangeIntentFilter)
				{
					using (BatteryChangeIntentReceiver)
					{
						return BatteryChangeIntentReceiver.CallBool(GetBoolExtraMethod, ExtraPresent, false);
					}
				}
			}
		}
		
		/// <summary>
		/// The maximum battery level. -1 if something went wrong.
		/// </summary>
		[PublicAPI]
		public static int Scale
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return -1;
				}

				using (BatteryChangeIntentFilter)
				{
					using (BatteryChangeIntentReceiver)
					{
						return BatteryChangeIntentReceiver.CallInt(GetIntExtraMethod, ExtraScale, -1);
					}
				}
			}
		}
		
		/// <summary>
		/// The technology of the current battery.
		/// </summary>
		[PublicAPI]
		public static string Technology
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}

				using (BatteryChangeIntentFilter)
				{
					using (BatteryChangeIntentReceiver)
					{
						return BatteryChangeIntentReceiver.CallStr("getStringExtra", ExtraTechnology);
					}
				}
			}
		}
		
		/// <summary>
		/// The current battery temperature.
		/// </summary>
		[PublicAPI]
		public static int Temperature
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return int.MinValue;
				}

				using (BatteryChangeIntentFilter)
				{
					using (BatteryChangeIntentReceiver)
					{
						return BatteryChangeIntentReceiver.CallInt(GetIntExtraMethod, ExtraTemperature, int.MinValue);
					}
				}
			}
		}
		
		/// <summary>
		/// The current battery voltage level in mV.
		/// </summary>
		[PublicAPI]
		public static int Voltage
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return 0;
				}

				using (BatteryChangeIntentFilter)
				{
					using (BatteryChangeIntentReceiver)
					{
						return BatteryChangeIntentReceiver.CallInt(GetIntExtraMethod, ExtraVoltage, 0);
					}
				}
			}
		}
	}
}
#endif