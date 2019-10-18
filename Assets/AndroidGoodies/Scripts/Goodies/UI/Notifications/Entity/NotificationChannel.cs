#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	///     A representation of settings that apply to a collection of similarly themed notifications.
	/// </summary>
	[PublicAPI]
	[AndroidApi(AGDeviceInfo.VersionCodes.O)]
	public class NotificationChannel
	{
		/// <summary>
		///     The id of the default channel for an app.
		///     This id is reserved by the system.
		/// </summary>
		[PublicAPI]
		public const string DEFAULT_CHANNEL_ID = "miscellaneous";

		internal NotificationChannel(AndroidJavaObject ajo)
		{
			AJO = ajo;
		}

		/// <summary>
		///     Creates a notification channel.
		/// </summary>
		/// <param name="id">
		///     The id of the channel. Must be unique per package. The value may be truncated if it is too long.
		/// </param>
		/// <param name="name">
		///     The user visible name of the channel.
		///     You can rename this channel when the system locale changes by listening for the
		///     Intent.ACTION_LOCALE_CHANGED broadcast. The recommended maximum length is 40 characters;
		///     the value may be truncated if it is too long.
		/// </param>
		/// <param name="importance">
		///     The importance of the channel. This controls how interruptive notifications posted to this channel are.
		///     Value is IMPORTANCE_UNSPECIFIED, IMPORTANCE_NONE, IMPORTANCE_MIN, IMPORTANCE_LOW,
		///     IMPORTANCE_DEFAULT or IMPORTANCE_HIGH.
		/// </param>
		[PublicAPI]
		public NotificationChannel(string id, string name,
			AGNotificationManager.Importance importance = AGNotificationManager.Importance.Default)
		{
			if (ApiCheck)
			{
				return;
			}

			AJO = new AndroidJavaObject(C.AndroidAppNotificationChannel, id, name, (int) importance);
		}

		static bool ApiCheck
		{
			get { return AGUtils.IsNotAndroid() || AGNotificationManager.HasNoNewNotificationsApi; }
		}

		internal AndroidJavaObject AJO { get; private set; }

		/// <summary>
		///     Returns the id of this channel.
		/// </summary>
		[PublicAPI]
		public string Id
		{
			get
			{
				if (ApiCheck)
				{
					return string.Empty;
				}

				return AJO.CallStr("getId");
			}
		}

		/// <summary>
		///     Gets/sets whether or not notifications posted to this channel can bypass the
		///     Do Not Disturb NotificationManager.INTERRUPTION_FILTER_PRIORITY mode.
		/// </summary>
		[PublicAPI]
		public bool BypassDnd
		{
			get
			{
				if (ApiCheck)
				{
					return false;
				}

				return AJO.CallBool("canBypassDnd");
			}
			set
			{
				if (ApiCheck)
				{
					return;
				}

				AJO.Call("setBypassDnd", value);
			}
		}

		/// <summary>
		///     Returns whether notifications posted to this channel can appear as badges in a Launcher application.
		///     Note that badging may be disabled for other reasons.
		/// </summary>
		[PublicAPI]
		public bool ShowBadge
		{
			get
			{
				if (ApiCheck)
				{
					return false;
				}

				return AJO.CallBool("canShowBadge");
			}
			set
			{
				if (ApiCheck)
				{
					return;
				}

				AJO.Call("setShowBadge", value);
			}
		}

		/// <summary>
		///     Sets whether notifications posted to this channel should display notification lights,
		///     on devices that support that feature. Only modifiable before the channel is submitted to
		///     NotificationManager.createNotificationChannel(NotificationChannel).
		/// </summary>
		/// <value>
		///     <c>true</c> if notifications posted to this channel should display
		///     notification lights, otherwise, <c>false</c>.
		/// </value>
		[PublicAPI]
		public bool EnableLights
		{
			set
			{
				if (ApiCheck)
				{
					return;
				}

				AJO.Call("enableLights", value);
			}
			get
			{
				if (ApiCheck)
				{
					return false;
				}

				return AJO.CallBool("shouldShowLights");
			}
		}

		/// <summary>
		///     Sets whether notification posted to this channel should vibrate.
		///     Only modifiable before the channel is submitted to
		///     NotificationManager.createNotificationChannel(NotificationChannel).
		/// </summary>
		/// <value>
		///     <c>true</c> notification posted to this channel should vibrate, otherwise, <c>false</c>.
		/// </value>
		[PublicAPI]
		public bool EnableVibration
		{
			set
			{
				if (ApiCheck)
				{
					return;
				}

				AJO.Call("enableVibration", value);
			}
			get
			{
				if (ApiCheck)
				{
					return false;
				}

				return AJO.CallBool("shouldVibrate");
			}
		}

		/// <summary>
		///     Returns the audio attributes for sound played by notifications posted to this channel.
		/// </summary>
		[PublicAPI]
		public AudioAttributes AudioAttributes
		{
			get
			{
				if (ApiCheck)
				{
					return null;
				}

				return new AudioAttributes(AJO.CallAJO("getAudioAttributes"));
			}
		}

		/// <summary>
		///     The user visible description of this channel.
		/// </summary>
		[PublicAPI]
		public string Description
		{
			get
			{
				if (ApiCheck)
				{
					return string.Empty;
				}

				return AJO.CallStr("getDescription");
			}
			set
			{
				if (ApiCheck)
				{
					return;
				}

				AJO.Call("setDescription", value);
			}
		}

		/// <summary>
		///     What group this channel belongs to.
		///     This is used only for visually grouping channels in the UI.
		/// </summary>
		[PublicAPI]
		public string Group
		{
			get
			{
				if (ApiCheck)
				{
					return string.Empty;
				}

				return AJO.CallStr("getGroup");
			}
			set
			{
				if (ApiCheck)
				{
					return;
				}

				AJO.Call("setGroup", value);
			}
		}

		/// <summary>
		///     Returns the user specified importance e.g. NotificationManager.IMPORTANCE_LOW for
		///     notifications posted to this channel. Note: This value might be >
		///     NotificationManager.IMPORTANCE_NONE, but notifications posted to this channel will not
		///     be shown to the user if the parent NotificationChannelGroup or app is blocked.
		///     See NotificationChannelGroup.IsBlocked() and NotificationManager.AreNotificationsEnabled().
		/// </summary>
		[PublicAPI]
		public AGNotificationManager.Importance Importance
		{
			get
			{
				if (ApiCheck)
				{
					return AGNotificationManager.Importance.Default;
				}

				return (AGNotificationManager.Importance) AJO.CallInt("getImportance");
			}
			set
			{
				if (ApiCheck)
				{
					return;
				}

				AJO.Call("setImportance", (int) value);
			}
		}

		/// <summary>
		///     Returns the notification light color for notifications posted to this channel.
		///     Irrelevant unless ShouldShowLights().
		/// </summary>
		[PublicAPI]
		public Color LightColor
		{
			get
			{
				if (ApiCheck)
				{
					return Color.magenta;
				}

				return AJO.CallInt("getLightColor").ToUnityColor();
			}
			set
			{
				if (ApiCheck)
				{
					return;
				}

				AJO.Call("setLightColor", value.ToAndroidColor());
			}
		}

		/// <summary>
		///     Returns whether or not notifications posted to this channel are shown
		///     on the lockscreen in full or redacted form.
		/// </summary>
		[PublicAPI]
		public Notification.Visibility LockscreenVisibility
		{
			get
			{
				if (ApiCheck)
				{
					return 0;
				}

				return (Notification.Visibility) AJO.CallInt("getLockscreenVisibility");
			}
			set
			{
				if (ApiCheck)
				{
					return;
				}

				AJO.Call("setLockscreenVisibility", (int) value);
			}
		}

		/// <summary>
		///     Returns the user visible name of this channel.
		/// </summary>
		[PublicAPI]
		public string Name
		{
			get
			{
				if (ApiCheck)
				{
					return string.Empty;
				}

				return AJO.CallStr("getName");
			}
		}

		/// <summary>
		///     Returns the notification sound for this channel.
		/// </summary>
		[PublicAPI]
		public string Sound
		{
			get
			{
				if (ApiCheck)
				{
					return null;
				}

				return AJO.CallAJO("getSound").CallStr("getPath");
			}
		}

		/// <summary>
		///     Returns the vibration pattern for notifications posted to this channel.
		///     Will be ignored if vibration is not enabled (ShouldVibrate().
		/// </summary>
		[PublicAPI]
		public long[] VibrationPattern
		{
			get
			{
				if (ApiCheck)
				{
					return null;
				}

				return AJO.Call<long[]>("getVibrationPattern");
			}
			set
			{
				if (ApiCheck)
				{
					return;
				}

				AJO.Call("setVibrationPattern", value);
			}
		}

		/// <summary>
		///     Sets the sound that should be played for notifications posted to this channel and its audio attributes.
		///     Notification channels with an importance of at least <see cref="AGNotificationManager.Importance.Default" />
		///     should have a sound. Only modifiable before the channel is submitted to
		///     <see cref="AGNotificationManager.CreateNotificationChannel(NotificationChannel)" />.
		/// </summary>
		[PublicAPI]
		public void SetSound([NotNull] string soundFilePath, AudioAttributes audioAttributes)
		{
			if (soundFilePath == null)
			{
				throw new ArgumentNullException("soundFilePath");
			}

			if (ApiCheck)
			{
				return;
			}

			AJO.Call("setSound", AndroidUri.FromFile(soundFilePath),
				audioAttributes == null ? null : audioAttributes.AJO);
		}

		public override string ToString()
		{
			return string.Format(
				"Id: {0}, BypassDnd: {1}, ShowBadge: {2}, EnableLights: {3}, EnableVibration: {4}, AudioAttributes: {5}, Description: {6}, Group: {7}, Importance: {8}, LightColor: {9}, LockscreenVisibility: {10}, Name: {11}, Sound: {12}, VibrationPattern: {13}",
				Id, BypassDnd, ShowBadge, EnableLights, EnableVibration, AudioAttributes, Description, Group,
				Importance, LightColor, LockscreenVisibility, Name, Sound, VibrationPattern);
		}
	}
}
#endif