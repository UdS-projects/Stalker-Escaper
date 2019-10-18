// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGNetwork.cs
//


using System.Collections.Generic;

#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	/// Retrieve information about device network connectivity.
	///
	/// Used permissions:
	/// <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
	/// <uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
	/// </summary>
	[PublicAPI]
	public static class AGNetwork
	{
		public class WifiInfo
		{
			/// <summary>
			/// the BSSID, in the form of a six-byte MAC address: XX:XX:XX:XX:XX:XX
			/// </summary>
			/// <value>the BSSID, in the form of a six-byte MAC address: XX:XX:XX:XX:XX:XX</value>
			public string BSSID { get; set; }

			/// <summary>
			/// Returns the service set identifier (SSID) of the current 802.11 network.
			/// If the SSID can be decoded as UTF-8, it will be returned surrounded by double quotation marks.
			/// Otherwise, it is returned as a string of hex digits.
			/// The SSID may be <unknown ssid> if there is no network currently connected.
			/// </summary>
			/// <value>The SSI.</value>
			public string SSID { get; set; }

			/// <summary>
			/// Mac Address.
			/// </summary>
			/// <value>Mac Address.</value>
			public string MacAddress { get; set; }

			/// <summary>
			/// Returns the current link speed in Mbps.
			/// </summary>
			/// <value>The current link speed in Mbps.</value>
			public int LinkSpeed { get; set; }

			/// <summary>
			/// Returns the service set identifier (SSID) of the current 802.11 network. If the SSID can be decoded as UTF-8, it will be returned surrounded by double quotation marks. Otherwise, it is returned as a string of hex digits. The SSID may be <unknown ssid> if there is no network currently connected.
			/// </summary>
			/// <value>Returns the service set identifier (SSID) of the current 802.11 network.</value>
			public int IpAddress { get; set; }

			/// <summary>
			/// Each configured network has a unique small integer ID, used to identify the network when performing operations on the supplicant.
			///  This property returns the ID for the currently connected network.
			/// </summary>
			/// <value>The network identifier.</value>
			public int NetworkId { get; set; }

			/// <summary>
			/// Returns the received signal strength indicator of the current 802.11 network, in dBm.
			/// </summary>
			/// <value>The rssi signal strength.</value>
			public int Rssi { get; set; }

			/// <summary>
			/// Returns a <see cref="System.String"/> that represents the current <see cref="AGNetwork"/>.
			/// </summary>
			/// <returns>A <see cref="System.String"/> that represents the current <see cref="AGNetwork"/>.</returns>
			public override string ToString()
			{
				return string.Format("[WifiInfo: BSSID={0}, SSID={1}, MacAddress={2}, LinkSpeed={3} Mbps, IpAddress={4}, NetworkId={5}, Rssi={6}]",
					BSSID, SSID, MacAddress, LinkSpeed, IpAddress, NetworkId, Rssi);
			}
		}

		/// <summary>
		/// A class representing a configured Wi-Fi network, including the security configuration.
		/// </summary>
		[PublicAPI]
		public class WifiConfiguration
		{
			public AndroidJavaObject ajo;

			public WifiConfiguration()
			{
				ajo = new AndroidJavaObject(C.AndroidNetWifiConfiguration);
			}
			
			public WifiConfiguration(AndroidJavaObject obj)
			{
				ajo = obj;
			}

			/// <summary>
			/// When set, this network configuration entry should only be used when associating with the AP having the specified BSSID.
			/// The value is a string in the format of an Ethernet MAC address, e.g., XX:XX:XX:XX:XX:XX where each X is a hex digit.
			/// </summary>
			[PublicAPI]
			public string BSSID
			{
				get
				{
					return ajo.Get<string>("BSSID");
				}
				set
				{
					ajo.Set("BSSID", value);
				}
			}
			
			/// <summary>
			/// Fully qualified domain name of a Passpoint configuration
			/// </summary>
			[PublicAPI]
			public string FQDN
			{
				get
				{
					return ajo.Get<string>("FQDN");
				}
				set
				{
					ajo.Set("FQDN", value);
				}
			}
			
			/// <summary>
			/// The network's SSID. Can either be a UTF-8 string, which must be enclosed in double quotation marks
			/// (e.g., "MyNetwork"), or a string of hex digits, which are not enclosed in quotes (e.g., 01a243f405).
			/// </summary>
			[PublicAPI]
			public string SSID
			{
				get
				{
					return ajo.Get<string>("SSID");
				}
				set
				{
					ajo.Set("SSID", value);
				}
			}
			
			/// <summary>
			/// The ID number that the supplicant uses to identify this network configuration entry.
			/// This must be passed as an argument to most calls into the supplicant.
			/// </summary>
			[PublicAPI]
			public int NetworkId
			{
				get
				{
					return ajo.Get<int>("networkId");
				}
				set
				{
					ajo.Set("networkId", value);
				}
			}
			
			/// <summary>
			/// Priority determines the preference given to a network when choosing an access point with which to associate.
			/// </summary>
			[PublicAPI]
			[Obsolete("Deprecated in API level 26. This field does not exist anymore.")]
			public int Priority
			{
				get
				{
					return ajo.Get<int>("priority");
				}
				set
				{
					ajo.Set("priority", value);
				}
			}

			[PublicAPI]
			public override string ToString()
			{
				return string.Format("Network ID: {0}, SSID: {1}, FQDN: {2}, BSSID: {3}.\n", NetworkId, SSID, FQDN, BSSID);
			}
		}

		const int TYPE_MOBILE = 0x00000000;
		const int TYPE_WIFI = 0x00000001;

		/// <summary>
		/// Required permission:
		/// <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
		/// Check if device is connected to the internet
		/// </summary>
		/// <returns>Whether device is connected to the internet</returns>
		public static bool IsInternetAvailable()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			AndroidJavaObject networkInfo;
			try
			{
				networkInfo = AGSystemService.ConnectivityService.CallAJO("getActiveNetworkInfo");
			}
			catch ( /* Null */ Exception)
			{
				return false;
			}

			if (networkInfo.IsJavaNull())
			{
				return false;
			}
			
			return networkInfo.Call<bool>("isConnected");
		}

		/// <summary>
		/// Gets the Mac Address for the ethernet if available, <code>null</code> otherwise.
		/// </summary>
		[PublicAPI]
		public static string EthernetMacAddress
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return null;
				}

				try
				{
					return C.UnityHelperUtilsClass.AJCCallStaticOnce<string>("getMacAddress");
				}
				catch (Exception)
				{
					return null;
				}
			}
		}
		
		/// <summary>
		/// Required permission: <uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
		/// </summary>
		public static bool IsWifiEnabled()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			try
			{
				return AGSystemService.WifiService.Call<bool>("isWifiEnabled");
			}
			catch ( /* Null */ Exception e)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("Failed to check if wi-fi is enabled. Error: " + e.Message);
				}
				return false;
			}
		}

		/// <summary>
		/// Determines if wifi is connected.
		/// </summary>
		/// <returns><c>true</c> if wifi is connected; otherwise, <c>false</c>.</returns>
		public static bool IsWifiConnected()
		{
			return IsNetworkConnected(TYPE_WIFI);
		}

		/// <summary>
		/// Determines if mobile data internet is connected.
		/// </summary>
		/// <returns><c>true</c> if mobile data internet is connected; otherwise, <c>false</c>.</returns>
		public static bool IsMobileConnected()
		{
			return IsNetworkConnected(TYPE_MOBILE);
		}

		static bool IsNetworkConnected(int networkType)
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			AndroidJavaObject networkInfo;
			try
			{
				networkInfo = AGSystemService.ConnectivityService.CallAJO("getNetworkInfo", networkType);
				if (networkInfo.IsJavaNull())
				{
					return false;
				}
			}
			catch ( /* Null */ Exception)
			{
				return false;
			}
			return networkInfo.Call<bool>("isConnected");
		}

		/// <summary>
		/// Return dynamic information about the current Wi-Fi connection, if any is active.
		/// </summary>
		/// <returns>   the Wi-Fi information, contained in WifiInfo.</returns>
		public static WifiInfo GetWifiConnectionInfo()
		{
			if (AGUtils.IsNotAndroid())
			{
				return null;
			}

			using (var wifiInfoAJO = GetWifiInfoAJO())
			{
				if (wifiInfoAJO.IsJavaNull())
				{
					return null;
				}

				var result = new WifiInfo
				{
					BSSID = wifiInfoAJO.CallStr("getBSSID"),
					SSID = wifiInfoAJO.CallStr("getSSID"),
					MacAddress = wifiInfoAJO.CallStr("getMacAddress"),
					LinkSpeed = wifiInfoAJO.CallInt("getLinkSpeed"),
					NetworkId = wifiInfoAJO.CallInt("getNetworkId"),
					IpAddress = wifiInfoAJO.CallInt("getIpAddress"),
					Rssi = wifiInfoAJO.CallInt("getRssi")
				};

				return result;
			}
		}

		/// <summary>
		/// Gets the wifi signal level out of 100.
		/// </summary>
		/// <returns>The wifi signal level out of 100.</returns>
		public static int GetWifiSignalLevel()
		{
			if (AGUtils.IsNotAndroid())
			{
				return 0;
			}

			var wifiInfo = GetWifiConnectionInfo();
			return wifiInfo == null ? 0 : AGSystemService.WifiService.CallStatic<int>("calculateSignalLevel", wifiInfo.Rssi, 100);
		}
		
		/// <summary>
		/// Opens system dialog for picking wifi-network
		/// </summary>
		[PublicAPI]
		public static void ShowAvailableWifiNetworks()
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			using (var intent = new AndroidIntent(AndroidIntent.ActionPickWifiNetwork))
			{
				AGUtils.StartActivity(intent.AJO);
			}
		}

		/// <summary>
		/// A list of all the networks configured for the current foreground user.
		/// Not all fields of WifiConfiguration are returned.
		/// </summary>
		[PublicAPI]
		public static List<WifiConfiguration> ConfiguredNetworks
		{
			get
			{
				var listAjo = AGSystemService.WifiService.CallAJO("getConfiguredNetworks");
				var result = new List<WifiConfiguration>();
				var ajos = listAjo.FromJavaList<AndroidJavaObject>();
				foreach (var ajo in ajos)
				{
					result.Add(new WifiConfiguration(ajo));
				}

				return result;
			}
		}

		/// <summary>
		/// Add a new network description to the set of configured networks. The networkId field of the supplied configuration object is ignored.
		/// The new network will be marked DISABLED by default. To enable it, called enableNetwork(int, boolean).
		/// </summary>
		[PublicAPI]
		public static void AddNetwork(WifiConfiguration config)
		{
			AGSystemService.WifiService.CallInt("addNetwork", config.ajo);
		}

		/// <summary>
		/// Disable a configured network. The specified network will not be a candidate for associating.
		/// This may result in the asynchronous delivery of state change events.
		/// Applications are not allowed to disable networks created by other applications.
		/// </summary>
		/// <param name="networkId"></param>
		[PublicAPI]
		public static void DisableNetwork(int networkId)
		{
			AGSystemService.WifiService.CallBool("disableNetwork", networkId);
		}

		/// <summary>
		/// Enable or disable Wi-Fi.
		/// Applications must have the Manifest.permission.CHANGE_WIFI_STATE permission to toggle wifi.
		/// </summary>
		[PublicAPI]
		public static void SetWifiEnabled(bool isEnabled)
		{
			AGSystemService.WifiService.CallBool("setWifiEnabled", isEnabled);
		}

		/// <summary>
		/// Disassociate from the currently active access point.
		/// This may result in the asynchronous delivery of state change events.
		/// Note: if network has Auto-reconnect enabled, this will take no visible effect!
		/// </summary>
		[PublicAPI]
		public static void Disconnect()
		{
			AGSystemService.WifiService.CallBool("disconnect");
		}
		
		/// <summary>
		/// Allow a previously configured network to be associated with.
		/// If attemptConnection is true, an attempt to connect to the selected network is initiated.
		/// This may result in the asynchronous delivery of state change events.
		/// Applications are not allowed to enable networks created by other applications.
		/// </summary>
		[PublicAPI]
		public static void EnableNetwork(int networkId, bool attemptConnection)
		{
			AGSystemService.WifiService.CallBool("enableNetwork", networkId, attemptConnection);
		}

		static AndroidJavaObject GetWifiInfoAJO()
		{
			try
			{
				return AGSystemService.WifiService.CallAJO("getConnectionInfo");
			}
			catch ( /* Null */ Exception e)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("Failed to get wifi info. Error: " + e.Message);
				}
				return null;
			}
		}
	}
}
#endif