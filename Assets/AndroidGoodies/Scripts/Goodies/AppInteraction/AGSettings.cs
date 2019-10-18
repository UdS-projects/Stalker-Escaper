// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGSettings.cs
//


#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	/// Allows to perform tasks on Android settings.
	/// <see href="https://developer.android.com/reference/android/provider/Settings.html">Android Settings Docs</see>
	/// </summary>
	[PublicAPI]
	public static class AGSettings
	{
		/// <summary>
		/// Show system settings.
		/// </summary>
		[PublicAPI]
		public const string ACTION_SETTINGS = "android.settings.SETTINGS";
		// API 1

		/// <summary>
		/// Show settings for accessibility modules.
		/// </summary>
		[PublicAPI]
		public const string ACTION_ACCESSIBILITY_SETTINGS = "android.settings.ACCESSIBILITY_SETTINGS";
		// API 5

		/// <summary>
		/// Show add account screen for creating a new account.
		/// </summary>
		[PublicAPI]
		public const string ACTION_ADD_ACCOUNT = "android.settings.ADD_ACCOUNT_SETTINGS";
		// API 5

		/// <summary>
		/// Show settings to allow entering/exiting airplane mode.
		/// </summary>
		[PublicAPI]
		public const string ACTION_AIRPLANE_MODE_SETTINGS = "android.settings.AIRPLANE_MODE_SETTINGS";
		// API 3

		/// <summary>
		/// Activity Action: Show settings to allow configuration of APNs.
		/// </summary>
		[PublicAPI]
		public const string ACTION_APN_SETTINGS = "android.settings.APN_SETTINGS";
		// API 1

		/// <summary>
		/// Show screen of details about a particular application.
		/// </summary>
		[PublicAPI]
		public const string ACTION_APPLICATION_DETAILS_SETTINGS = "android.settings.APPLICATION_DETAILS_SETTINGS";
		// API 9

		/// <summary>
		/// Show settings to allow configuration of application development-related settings. As of JELLY_BEAN_MR1 this action is a required part of the platform.
		/// </summary>
		[PublicAPI]
		public const string ACTION_APPLICATION_DEVELOPMENT_SETTINGS = "android.settings.APPLICATION_DEVELOPMENT_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings to allow configuration of application-related settings.
		/// </summary>
		[PublicAPI]
		public const string ACTION_APPLICATION_SETTINGS = "android.settings.APPLICATION_SETTINGS";
		// API 1

		/// <summary>
		/// Show notification settings for a single app.
		/// </summary>
		[PublicAPI]
		public const string ACTION_APP_NOTIFICATION_SETTINGS = "android.settings.APP_NOTIFICATION_SETTINGS";
		// API 26

		/// <summary>
		/// Show battery saver settings.
		/// </summary>
		[PublicAPI]
		public const string ACTION_BATTERY_SAVER_SETTINGS = "android.settings.BATTERY_SAVER_SETTINGS";
		// API 22

		/// <summary>
		/// Show settings to allow configuration of Bluetooth.
		/// </summary>
		[PublicAPI]
		public const string ACTION_BLUETOOTH_SETTINGS = "android.settings.BLUETOOTH_SETTINGS";
		// API 1

		/// <summary>
		/// Show settings for video captioning.
		/// </summary>
		[PublicAPI]
		public const string ACTION_CAPTIONING_SETTINGS = "android.settings.CAPTIONING_SETTINGS";
		// API 19

		/// <summary>
		/// Show settings to allow configuration of cast endpoints.
		/// </summary>
		[PublicAPI]
		public const string ACTION_CAST_SETTINGS = "android.settings.CAST_SETTINGS";
		// API 21

		/// <summary>
		/// Show notification settings for a single NotificationChannel.
		/// </summary>
		[PublicAPI]
		public const string ACTION_CHANNEL_NOTIFICATION_SETTINGS = "android.settings.CHANNEL_NOTIFICATION_SETTINGS";
		// API 21

		/// <summary>
		/// Show settings for selection of 2G/3G.
		/// </summary>
		[PublicAPI]
		public const string ACTION_DATA_ROAMING_SETTINGS = "android.settings.DATA_ROAMING_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings to allow configuration of data and view data usage.
		/// </summary>
		[PublicAPI]
		public const string ACTION_DATA_USAGE_SETTINGS = "android.settings.DATA_USAGE_SETTINGS";
		// API 28

		/// <summary>
		/// Show settings to allow configuration of date and time.
		/// </summary>
		[PublicAPI]
		public const string ACTION_DATE_SETTINGS = "android.settings.DATE_SETTINGS";
		// API 1

		/// <summary>
		/// Show general device information settings (serial number, software version, phone number, etc.).
		/// </summary>
		[PublicAPI]
		public const string ACTION_DEVICE_INFO_SETTINGS = "android.settings.DEVICE_INFO_SETTINGS";
		// API 8

		/// <summary>
		/// Show settings to allow configuration of display.
		/// </summary>
		[PublicAPI]
		public const string ACTION_DISPLAY_SETTINGS = "android.settings.DISPLAY_SETTINGS";
		// API 1

		/// <summary>
		/// Show Daydream settings.
		/// </summary>
		[PublicAPI]
		public const string ACTION_DREAM_SETTINGS = "android.settings.DREAM_SETTINGS";
		// API 18

		/// <summary>
		///  Show settings to enroll fingerprints, and setup PIN/Pattern/Pass if necessary.
		/// </summary>
		[PublicAPI]
		public const string ACTION_FINGERPRINT_ENROLL = "android.settings.FINGERPRINT_ENROLL";
		// API 28

		/// <summary>
		///  Show settings to configure the hardware keyboard.
		/// </summary>
		[PublicAPI]
		public const string ACTION_HARD_KEYBOARD_SETTINGS = "android.settings.HARD_KEYBOARD_SETTINGS";
		// API 24

		/// <summary>
		/// If there are multiple activities that can satisfy the CATEGORY_HOME intent, this screen allows you to pick your preferred activity.
		/// </summary>
		[PublicAPI]
		public const string ACTION_HOME_SETTINGS = "android.settings.HOME_SETTINGS";
		// API 21

		/// <summary>
		/// Show screen for controlling background data restrictions for a particular application.
		/// Input: Intent's data URI set with an application name, using the "package" schema (like "package:com.my.app").
		/// </summary>
		[PublicAPI]
		public const string ACTION_IGNORE_BACKGROUND_DATA_RESTRICTIONS_SETTINGS = "android.settings.IGNORE_BACKGROUND_DATA_RESTRICTIONS_SETTINGS";
		// API 24

		/// <summary>
		/// Show screen for controlling which apps can ignore battery optimizations.
		/// </summary>
		[PublicAPI]
		public const string ACTION_IGNORE_BATTERY_OPTIMIZATION_SETTINGS = "android.settings.IGNORE_BATTERY_OPTIMIZATION_SETTINGS";
		// API 23

		/// <summary>
		/// Show settings to configure input methods, in particular allowing the user to enable input methods.
		/// </summary>
		[PublicAPI]
		public const string ACTION_INPUT_METHOD_SETTINGS = "android.settings.INPUT_METHOD_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings to enable/disable input method subtypes.
		/// </summary>
		[PublicAPI]
		public const string ACTION_INPUT_METHOD_SUBTYPE_SETTINGS = "android.settings.INPUT_METHOD_SUBTYPE_SETTINGS";
		// API 11

		/// <summary>
		/// Show settings for internal storage.
		/// </summary>
		[PublicAPI]
		public const string ACTION_INTERNAL_STORAGE_SETTINGS = "android.settings.INTERNAL_STORAGE_SETTINGS";
		// API 3

		/// <summary>
		///  Show settings to allow configuration of locale.
		/// </summary>
		[PublicAPI]
		public const string ACTION_LOCALE_SETTINGS = "android.settings.LOCALE_SETTINGS";
		// API 1

		/// <summary>
		/// Show settings to allow configuration of current location sources.
		/// </summary>
		[PublicAPI]
		public const string ACTION_LOCATION_SOURCE_SETTINGS = "android.settings.LOCATION_SOURCE_SETTINGS";
		// API 1

		/// <summary>
		/// Show settings to manage all applications.
		/// </summary>
		[PublicAPI]
		public const string ACTION_MANAGE_ALL_APPLICATIONS_SETTINGS = "android.settings.MANAGE_ALL_APPLICATIONS_SETTINGS";
		// API 9

		/// <summary>
		/// Show settings to manage installed applications.
		/// </summary>
		[PublicAPI]
		public const string ACTION_MANAGE_APPLICATIONS_SETTINGS = "android.settings.MANAGE_APPLICATIONS_SETTINGS";
		// API 3

		/// <summary>
		/// Show Default apps settings.
		/// In some cases, a matching Activity may not exist, so ensure you safeguard against this.
		/// </summary>
		[PublicAPI]
		public const string ACTION_MANAGE_DEFAULT_APPS_SETTINGS = "android.settings.MANAGE_DEFAULT_APPS_SETTINGS";
		// API 24

		/// <summary>
		/// Show screen for controlling which apps can draw on top of other apps.
		/// </summary>
		[PublicAPI]
		public const string ACTION_MANAGE_OVERLAY_PERMISSION = "android.settings.MANAGE_APPLICATIONS_SETTINGS";
		// API 23

		/// <summary>
		/// Show settings to allow configuration of trusted external sources
		/// Input: Optionally, the Intent's data URI can specify the application package name to directly invoke
		/// the management GUI specific to the package name. For example "package:com.my.app".
		/// </summary>
		[PublicAPI]
		public const string ACTION_MANAGE_UNKNOWN_APP_SOURCES = "android.settings.MANAGE_UNKNOWN_APP_SOURCES";
		// API 26

		/// <summary>
		/// Show screen for controlling which apps are allowed to write/modify system settings.
		/// </summary>
		[PublicAPI]
		public const string ACTION_MANAGE_WRITE_SETTINGS = "android.settings.MANAGE_WRITE_SETTINGS";
		// API 23

		/// <summary>
		/// Show settings for memory card storage.
		/// </summary>
		[PublicAPI]
		public const string ACTION_MEMORY_CARD_SETTINGS = "android.settings.MEMORY_CARD_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings for selecting the network operator.
		/// </summary>
		[PublicAPI]
		public const string ACTION_NETWORK_OPERATOR_SETTINGS = "android.settings.NETWORK_OPERATOR_SETTINGS";
		// API 3

		/// <summary>
		/// Show NFC Sharing settings.
		/// </summary>
		[PublicAPI]
		public const string ACTION_NFCSHARING_SETTINGS = "android.settings.NFCSHARING_SETTINGS";
		// API 14

		/// <summary>
		/// Show NFC Tap & Pay settings
		/// </summary>
		[PublicAPI]
		public const string ACTION_NFC_PAYMENT_SETTINGS = "android.settings.NFC_PAYMENT_SETTINGS";
		// API 19

		/// <summary>
		/// Show NFC settings.
		/// </summary>
		[PublicAPI]
		public const string ACTION_NFC_SETTINGS = "android.settings.NFC_SETTINGS";
		// API 16

		/// <summary>
		/// Show settings to allow configuration of Night display.
		/// </summary>
		[PublicAPI]
		public const string ACTION_NIGHT_DISPLAY_SETTINGS = "android.settings.NIGHT_DISPLAY_SETTINGS";
		// API 26

		/// <summary>
		/// Show Notification listener settings.
		/// </summary>
		[PublicAPI]
		public const string ACTION_NOTIFICATION_LISTENER_SETTINGS = "android.settings.ACTION_NOTIFICATION_LISTENER_SETTINGS";
		// API 22

		/// <summary>
		/// Show Do Not Disturb access settings.
		/// </summary>
		[PublicAPI]
		public const string ACTION_NOTIFICATION_POLICY_ACCESS_SETTINGS = "android.settings.NOTIFICATION_POLICY_ACCESS_SETTINGS";
		// API 23

		/// <summary>
		/// Show the top level print settings.
		/// </summary>
		[PublicAPI]
		public const string ACTION_PRINT_SETTINGS = "android.settings.ACTION_PRINT_SETTINGS";
		// API 19

		/// <summary>
		/// Show settings to allow configuration of privacy options.
		/// </summary>
		[PublicAPI]
		public const string ACTION_PRIVACY_SETTINGS = "android.settings.PRIVACY_SETTINGS";
		// API 5

		/// <summary>
		/// Show settings to allow configuration of quick launch shortcuts.
		/// </summary>
		[PublicAPI]
		public const string ACTION_QUICK_LAUNCH_SETTINGS = "android.settings.QUICK_LAUNCH_SETTINGS";
		// API 3

		/// <summary>
		/// Ask the user to allow an app to ignore battery optimizations
		/// (that is, put them on the whitelist of apps shown by ACTION_IGNORE_BATTERY_OPTIMIZATION_SETTINGS).
		/// For an app to use this, it also must hold the REQUEST_IGNORE_BATTERY_OPTIMIZATIONS permission.
		/// </summary>
		[PublicAPI]
		public const string ACTION_REQUEST_IGNORE_BATTERY_OPTIMIZATIONS =
			"android.settings.REQUEST_IGNORE_BATTERY_OPTIMIZATIONS";
		// API 23

		/// <summary>
		/// Show screen that lets user select its Autofill Service.
		/// </summary>
		[PublicAPI]
		public const string ACTION_REQUEST_SET_AUTOFILL_SERVICE =
			"android.settings.REQUEST_SET_AUTOFILL_SERVICE";
		// API 26

		/// <summary>
		/// Show settings for global search.
		/// </summary>
		[PublicAPI]
		public const string ACTION_SEARCH_SETTINGS = "android.settings.SEARCH_SETTINGS";
		// API 8

		/// <summary>
		/// Show settings to allow configuration of security and location privacy.
		/// </summary>
		[PublicAPI]
		public const string ACTION_SECURITY_SETTINGS = "android.settings.SECURITY_SETTINGS";
		// API 1

		/// <summary>
		/// Show the regulatory information screen for the device.
		/// </summary>
		[PublicAPI]
		public const string ACTION_SHOW_REGULATORY_INFO = "android.settings.SHOW_REGULATORY_INFO";
		// API 1

		/// <summary>
		/// Show settings to allow configuration of sound and volume.
		/// </summary>
		[PublicAPI]
		public const string ACTION_SOUND_SETTINGS = "android.settings.SOUND_SETTINGS";
		// API 1

		/// <summary>
		/// Show screen for controlling which apps have access on volume directories.
		/// </summary>
		[PublicAPI]
		public const string ACTION_STORAGE_VOLUME_ACCESS_SETTINGS =
			"android.settings.STORAGE_VOLUME_ACCESS_SETTINGS";
		// API 1

		/// <summary>
		/// Show settings to allow configuration of sync settings.
		/// </summary>
		[PublicAPI]
		public const string ACTION_SYNC_SETTINGS = "android.settings.SYNC_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings to control access to usage information.
		/// </summary>
		[PublicAPI]
		public const string ACTION_USAGE_ACCESS_SETTINGS = "android.settings.USAGE_ACCESS_SETTINGS";
		// API 21

		/// <summary>
		/// Show settings to manage the user input dictionary.
		/// </summary>
		[PublicAPI]
		public const string ACTION_USER_DICTIONARY_SETTINGS = "android.settings.USER_DICTIONARY_SETTINGS";
		// API 3

		/// <summary>
		/// Modify Airplane mode settings using a voice command.
		/// </summary>
		[PublicAPI]
		public const string ACTION_VOICE_CONTROL_AIRPLANE_MODE = "android.settings.VOICE_CONTROL_AIRPLANE_MODE";
		// API 23

		/// <summary>
		///  Modify Battery Saver mode setting using a voice command.
		/// </summary>
		[PublicAPI]
		public const string ACTION_VOICE_CONTROL_BATTERY_SAVER_MODE = "android.settings.VOICE_CONTROL_BATTERY_SAVER_MODE";
		// API 23

		/// <summary>
		/// Modify do not disturb mode settings.
		/// </summary>
		[PublicAPI]
		public const string ACTION_VOICE_CONTROL_DO_NOT_DISTURB_MODE = "android.settings.VOICE_CONTROL_DO_NOT_DISTURB_MODE";
		// API 23

		/// <summary>
		/// Show settings to configure input methods, in particular allowing the user to enable input methods.
		/// </summary>
		[PublicAPI]
		public const string ACTION_VOICE_INPUT_SETTINGS = "android.settings.VOICE_INPUT_SETTINGS";
		// API 21

		/// <summary>
		/// Show settings to allow configuration of VPN.
		/// </summary>
		[PublicAPI]
		public const string ACTION_VPN_SETTINGS = "android.settings.VPN_SETTINGS";
		// API 24

		/// <summary>
		/// Show VR listener settings.
		/// </summary>
		[PublicAPI]
		public const string ACTION_VR_LISTENER_SETTINGS = "android.settings.VR_LISTENER_SETTINGS";
		// API 24

		/// <summary>
		/// Allows user to select current webview implementation.
		/// </summary>
		[PublicAPI]
		public const string ACTION_WEBVIEW_SETTINGS = "android.settings.WEBVIEW_SETTINGS";
		// API 24

		/// <summary>
		/// Show settings to allow configuration of a static IP address for Wi-Fi.
		/// </summary>
		[PublicAPI]
		public const string ACTION_WIFI_IP_SETTINGS = "android.settings.WIFI_IP_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings to allow configuration of Wi-Fi.
		/// </summary>
		[PublicAPI]
		public const string ACTION_WIFI_SETTINGS = "android.settings.WIFI_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings to allow configuration of wireless controls such as Wi-Fi, Bluetooth and Mobile networks.
		/// </summary>
		[PublicAPI]
		public const string ACTION_WIRELESS_SETTINGS = "android.settings.WIRELESS_SETTINGS";
		// API 1

		/// <summary>
		/// Show Zen Mode (aka Do Not Disturb) priority configuration settings.
		/// </summary>
		[PublicAPI]
		public const string ACTION_ZEN_MODE_PRIORITY_SETTINGS =
			"android.settings.ZEN_MODE_PRIORITY_SETTINGS";
		// API 1

		/// <summary>
		/// Limit available options in launched activity based on the given account types.
		/// </summary>
		[PublicAPI]
		public const string EXTRA_ACCOUNT_TYPES = "account_types";
		// API 18

		/// <summary>
		/// Enable or disable Airplane Mode.
		/// </summary>
		[PublicAPI]
		public const string EXTRA_AIRPLANE_MODE_ENABLED = "airplane_mode_enabled";
		// API 23

		/// <summary>
		/// Enable or disable Airplane Mode.
		/// </summary>
		[PublicAPI]
		public const string EXTRA_APP_PACKAGE = "android.provider.extra.APP_PACKAGE";
		// API 26
		
		/// <summary>
		/// Limit available options in launched activity based on the given authority.
		/// </summary>
		[PublicAPI]
		public const string EXTRA_AUTHORITIES = "authorities";
		// API 8
		
		/// <summary>
		/// Enable or disable Battery saver mode.
		/// </summary>
		[PublicAPI]
		public const string EXTRA_BATTERY_SAVER_MODE_ENABLED = "android.settings.extra.battery_saver_mode_enabled";
		// API 23
		
		/// <summary>
		/// The NotificationChannel.GetId() of the notification channel settings to display.
		/// </summary>
		[PublicAPI]
		public const string EXTRA_CHANNEL_ID = "android.provider.extra.CHANNEL_ID";
		// API 26
		
		/// <summary>
		/// Enable or disable Do Not Disturb mode.
		/// </summary>
		[PublicAPI]
		public const string EXTRA_DO_NOT_DISTURB_MODE_ENABLED = "android.settings.extra.do_not_disturb_mode_enabled";
		// API 23
		
		/// <summary>
		/// How many minutes to enable do not disturb mode for.
		/// </summary>
		[PublicAPI]
		public const string EXTRA_DO_NOT_DISTURB_MODE_MINUTES = "android.settings.extra.do_not_disturb_mode_minutes";
		// API 23
		
		[PublicAPI]
		public const string EXTRA_INPUT_METHOD_ID = "input_method_id";
		// API 11

		/// <summary>
		/// An int extra specifying a subscription ID.
		/// </summary>
		[PublicAPI]
		public const string EXTRA_SUB_ID = "android.provider.extra.SUB_ID";
		// API 28

		/// <summary>
		/// Check if the current device can open the specified settings screen.
		/// </summary>
		/// <returns><c>true</c> if the current device can open the specified settings screen; otherwise, <c>false</c>.</returns>
		/// <param name="action">Action.</param>
		public static bool CanOpenSettingsScreen(string action)
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			var intent = new AndroidIntent(action);
			return intent.ResolveActivity();
		}

		#region API

		/// <summary>
		/// Opens android main settings
		/// </summary>
		public static void OpenSettings()
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			OpenSettingsScreen(ACTION_SETTINGS);
		}

		#endregion

		/// <summary>
		/// Opens the provided settings screen
		/// </summary>
		/// <param name="action">
		/// Screen to open. Use on of actions provided as constants in this class. Check android.provider.Settings java class for more info
		/// </param>
		public static void OpenSettingsScreen(string action)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			var intent = new AndroidIntent(action);
			if (intent.ResolveActivity())
			{
				AGUtils.StartActivity(intent.AJO);
			}
			else
			{
				Debug.LogWarning("Could not launch " + action + " settings. Check the device API level");
			}
		}

		/// <summary>
		/// Open application details settings
		/// </summary>
		/// <param name="package">Package of the application to open settings</param>
		public static void OpenApplicationDetailsSettings(string package)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			var intent = new AndroidIntent(ACTION_APPLICATION_DETAILS_SETTINGS);
			intent.SetData(AndroidUri.Parse(string.Format("package:{0}", package)));
			if (intent.ResolveActivity())
			{
				AGUtils.StartActivity(intent.AJO);
			}
			else
			{
				Debug.LogWarning("Could not open application details settings for package " + package +
				                 ". Most likely application is not installed.");
			}
		}

		#region system_settings_general

		/// <summary>
		/// Determines if the application can write system settings.
		/// </summary>
		/// <returns><c>true</c> if the application can write system settings; otherwise, <c>false</c>.</returns>
		/// 
		/// See: http://stackoverflow.com/questions/32083410/cant-get-write-settings-permission
		public static bool CanWriteSystemSettings()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			return AndroidSettings.System.CanWrite();
		}

		/// <summary>
		/// Opens the activity to modify system settings activity.
		/// </summary>
		/// <param name="onFailure">Invoked if activity could not be started.</param>
		public static void OpenModifySystemSettingsActivity(Action onFailure)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			var intent = new AndroidIntent(AndroidSettings.ACTION_MANAGE_WRITE_SETTINGS).AJO;
			AGUtils.StartActivity(intent, onFailure);
		}

		/// <summary>
		/// Gets the system screen brightness. The value is between 0 and 1
		/// </summary>
		/// <returns>The system screen brightness.</returns>
		public static float GetSystemScreenBrightness()
		{
			if (AGUtils.IsNotAndroid())
			{
				return 0;
			}

			var brightnessInt = AndroidSettings.System.GetInt(AndroidSettings.System.SCREEN_BRIGHTNESS, 1);
			return brightnessInt / 255f;
		}

		#endregion

		#region screen_brightness

		/// <summary>
		/// Sets the system screen brightness. The vaue must be between 0 and 1 and will be clamped if it is not.
		/// 
		/// Before invoking the method you have to check with <see cref="CanWriteSystemSettings"/>  if user allowed to write system settings.
		/// If not prompt the user to open the screen where user can modify permissions for your app using <see cref="OpenModifySystemSettingsActivity"/>
		/// 
		/// REQUIRED PERMISSION:
		///      <uses-permission android:name="android.permission.WRITE_SETTINGS"/>
		/// </summary>
		/// <param name="newBrightness">New brightness to set.</param>
		public static void SetSystemScreenBrightness(float newBrightness)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			newBrightness = Mathf.Clamp01(newBrightness);

			if (!CanWriteSystemSettings())
			{
				Debug.LogError("The application does not have the permission to modify system settings." +
				               " Check before invoking this method by invoking 'CanWriteSystemSettings()' and use 'OpenModifySystemSettingsActivity()' to prompt the use to allow");
				return;
			}

			try
			{
				AndroidSettings.System.PutInt(AndroidSettings.System.SCREEN_BRIGHTNESS_MODE,
					AndroidSettings.System.SCREEN_BRIGHTNESS_MODE_MANUAL);
				int brightnessInt = (int) (newBrightness * 255);
				AndroidSettings.System.PutInt(AndroidSettings.System.SCREEN_BRIGHTNESS, brightnessInt);
			}
			catch (Exception e)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogException(e);
				}
			}
		}

		// TODO Window Brightness
		// WindowManager.LayoutParams layout = getWindow().getAttributes();
		// layout.screenBrightness = 1F;
		// getWindow().setAttributes(layout);
		static void SetWindowScreenBrightness(float newBrightness)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			newBrightness = Mathf.Clamp01(newBrightness);

			var window = AGUtils.Window;
			var layout = window.CallAJO("getAttributes");
			layout.SetStatic<float>("screenBrightness", newBrightness);
			window.Call("setAttributes", layout);
		}

		#endregion

		#region notification

		
		/// <summary>
		/// Opens systen settings window for the selected channel
		/// </summary>
		/// <param name="channelId">
		/// ID of the selected channel
		/// </param>
		public static void OpenNotificationChannelSettings(string channelId)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			var intent = new AndroidIntent(ACTION_CHANNEL_NOTIFICATION_SETTINGS);
			intent.PutExtra(EXTRA_APP_PACKAGE, AGDeviceInfo.GetApplicationPackage());
			intent.PutExtra(EXTRA_CHANNEL_ID, channelId);
			AGUtils.StartActivity(intent.AJO);
		}

		#endregion
	}
}

#endif