#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using System.Collections.Generic;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	///     Notification object
	/// </summary>
	[PublicAPI]
	public class Notification
	{
		/// <summary>
		///     What icon should be shown for this notification if it is being displayed in a Launcher that supports badging.
		/// </summary>
		[PublicAPI]
		public enum BadgeIcon
		{
			/// <summary>
			///     If this notification is being shown as a badge, always show as a number.
			/// </summary>
			[PublicAPI] None = 0,

			/// <summary>
			///     If this notification is being shown as a badge, use the GetSmallIcon() to represent this notification.
			/// </summary>
			[PublicAPI] Small = 1,

			/// <summary>
			///     If this notification is being shown as a badge, use the GetLargeIcon() to represent this notification.
			/// </summary>
			[PublicAPI] Large = 2
		}

		[PublicAPI]
		public enum Default
		{
			/// <summary>
			///     Use all default values (where applicable).
			/// </summary>
			[PublicAPI] All = -1,

			/// <summary>
			///     Use the default notification lights. This will ignore the FLAG_SHOW_LIGHTS bit, and ledARGB, ledOffMS, or ledOnMS.
			/// </summary>
			[PublicAPI] Lights = 4,

			/// <summary>
			///     Use the default notification sound. This will ignore any given sound.
			///     A notification that is noisy is more likely to be presented as a heads-up notification.
			/// </summary>
			[PublicAPI] Sound = 1,

			/// <summary>
			///     Use the default notification vibrate. This will ignore any given vibrate.
			///     Using phone vibration requires the VIBRATE permission.
			///     A notification that vibrates is more likely to be presented as a heads-up notification.
			/// </summary>
			[PublicAPI] Vibrate = 2
		}

		[PublicAPI]
		public enum Flags
		{
			/// <summary>
			///     Bit to be bitwise-ored into the flags field that should be set if the notification
			///     should be canceled when it is clicked by the user.
			/// </summary>
			[PublicAPI] AutoCancel = 16,

			/// <summary>
			///     Bit to be bitwise-ored into the flags field that should be set if this notification represents
			///     a currently running service. This will normally be set for you by Service.StartForeground(int, Notification).
			/// </summary>
			[PublicAPI] ForegroundService = 64,

			/// <summary>
			///     Bit to be bitwise-ored into the flags field that should be set if this notification is the group summary
			///     for a group of notifications. Grouped notifications may display in a cluster or stack on devices
			///     which support such rendering. Requires a group key also be set using Notification.Builder.SetGroup(string).
			/// </summary>
			[PublicAPI] GroupSummary = 512,

			/// <summary>
			///     Obsolete flag indicating high-priority notifications; use the priority field instead.
			/// </summary>
			[Obsolete] [PublicAPI] HighPriority = 128,

			/// <summary>
			///     Bit to be bitwise-ored into the flags field that if set, the audio will be repeated until the notification
			///     is cancelled or the notification window is opened.
			/// </summary>
			[PublicAPI] Insistent = 4,

			/// <summary>
			///     Bit to be bitwise-ored into the flags field that should be set if this notification is relevant
			///     to the current device only and it is not recommended that it bridge to other devices.
			/// </summary>
			[PublicAPI] LocalOnly = 256,

			/// <summary>
			///     Bit to be bitwise-ored into the flags field that should be set if the notification should not be canceled
			///     when the user clicks the Clear all button.
			/// </summary>
			[PublicAPI] NoClear = 32,

			/// <summary>
			///     Bit to be bitwise-ored into the flags field that should be set if this notification is in reference to
			///     something that is ongoing, like a phone call. It should not be set if this notification is in reference
			///     to something that happened at a particular point in time, like a missed phone call.
			/// </summary>
			[PublicAPI] OngoingEvent = 2,

			/// <summary>
			///     Bit to be bitwise-ored into the flags field that should be set if you would only like the sound,
			///     vibrate and ticker to be played if the notification was not already showing.
			/// </summary>
			[PublicAPI] OnlyAlertOnce = 8,

			/// <summary>
			///     Bit to be bitwise-ored into the flags field that should be set if you want the LED on for this notification.
			///     To turn the LED off, pass 0 in the alpha channel for colorARGB or 0 for both ledOnMS and ledOffMS.
			///     To turn the LED on, pass 1 for ledOnMS and 0 for ledOffMS.
			///     To flash the LED, pass the number of milliseconds that it should be on and off to ledOnMS and ledOffMS.
			///     Since hardware varies, you are not guaranteed that any of the values you pass are honored exactly.
			///     Use the system defaults if possible because they will be set to values that work on any given hardware.
			///     The alpha channel must be set for forward compatibility.
			///     Deprecated in API level 26, use NotificationChannel.ShouldShowLights().
			/// </summary>
			[PublicAPI] ShowLights = 1
		}

		/// <summary>
		///     Which type of notifications in a group are responsible for audibly alerting the user.
		/// </summary>
		[PublicAPI]
		public enum GroupAlert
		{
			/// <summary>
			///     Constant for Notification.Builder.SetGroupAlertBehavior(int), meaning that all notifications in a group
			///     with sound or vibration ought to make sound or vibrate (respectively),
			///     so this notification will not be muted when it is in a group.
			/// </summary>
			[PublicAPI] All = 0,

			/// <summary>
			///     Constant for Notification.Builder.setGroupAlertBehavior(int), meaning that the summary notification
			///     in a group should be silenced (no sound or vibration) even if they are posted to a NotificationChannel
			///     that has sound and/or vibration. Use this constant to mute this notification if this notification
			///     is a group summary.
			///     For example, you might want to use this constant if only the children notifications in your group
			///     have content and the summary is only used to visually group notifications rather than to alert the
			///     user that new information is available.
			/// </summary>
			[PublicAPI] Children = 2,

			/// <summary>
			///     Constant for Notification.Builder.setGroupAlertBehavior(int), meaning that all children notification
			///     in a group should be silenced (no sound or vibration) even if they are posted to a NotificationChannel
			///     that has sound and/or vibration. Use this constant to mute this notification if this notification
			///     is a group child. This must be applied to all children notifications you want to mute.
			///     For example, you might want to use this constant if you post a number of children notifications at once
			///     (say, after a periodic sync), and only need to notify the user audibly once.
			/// </summary>
			[PublicAPI] Summary = 1
		}

		[PublicAPI]
		public enum Priority
		{
			Min = -2,
			Low = -1,
			Default = 0,
			High = 1,
			Max = 2
		}

		/// <summary>
		///     Visibility of notification on lockscreen
		/// </summary>
		[PublicAPI]
		public enum Visibility
		{
			/// <summary>
			///     Show this notification on all lockscreens, but conceal sensitive or private information on secure lockscreens.
			/// </summary>
			[PublicAPI] Private = 0,

			/// <summary>
			///     Notification visibility: Show this notification in its entirety on all lockscreens.
			/// </summary>
			[PublicAPI] Public = 1,

			/// <summary>
			///     Notification visibility: Do not reveal any part of this notification on a secure lockscreen.
			/// </summary>
			[PublicAPI] Secret = -1
		}

		/// <summary>
		///     Notification category: alarm or timer.
		/// </summary>
		[PublicAPI] public const string CategoryAlarm = "alarm";

		/// <summary>
		///     Notification category: incoming call (voice or video) or similar synchronous communication request.
		/// </summary>
		[PublicAPI] public const string CategoryCall = "call";

		/// <summary>
		///     Notification category: asynchronous bulk message (email).
		/// </summary>
		[PublicAPI] public const string CategoryEmail = "email";

		/// <summary>
		///     Notification category: error in background operation or authentication status.
		/// </summary>
		[PublicAPI] public const string CategoryError = "error";

		/// <summary>
		///     Notification category: calendar event.
		/// </summary>
		[PublicAPI] public const string CategoryEvent = "event";

		/// <summary>
		///     Notification category: incoming direct message (SMS, instant message, etc.).
		/// </summary>
		[PublicAPI] public const string CategoryMessage = "msg";

		/// <summary>
		///     Notification category: map turn-by-turn navigation.
		/// </summary>
		[PublicAPI] public const string CategoryNavigation = "navigation";

		/// <summary>
		///     Notification category: progress of a long-running background operation.
		/// </summary>
		[PublicAPI] public const string CategoryProgress = "progress";

		/// <summary>
		///     Notification category: promotion or advertisement.
		/// </summary>
		[PublicAPI] public const string CategoryPromo = "promo";

		/// <summary>
		///     Notification category: a specific, timely recommendation for a single thing.
		///     For example, a news app might want to recommend a news story it believes the user will want to read next.
		/// </summary>
		[PublicAPI] public const string CategoryRecommendation = "recommendation";

		/// <summary>
		///     Notification category: user-scheduled reminder.
		/// </summary>
		[PublicAPI] public const string CategoryReminder = "reminder";

		/// <summary>
		///     Notification category: indication of running background service.
		/// </summary>
		[PublicAPI] public const string CategoryService = "service";

		/// <summary>
		///     Notification category: social network or sharing update.
		/// </summary>
		[PublicAPI] public const string CategorySocial = "social";

		/// <summary>
		///     Notification category: ongoing information about device or contextual status.
		/// </summary>
		[PublicAPI] public const string CategoryStatus = "status";

		/// <summary>
		///     Notification category: system or device status update. Reserved for system use.
		/// </summary>
		[PublicAPI] public const string CategorySystem = "system";

		/// <summary>
		///     Notification category: media transport control for playback.
		/// </summary>
		[PublicAPI] public const string CategoryTransport = "transport";

		public Notification(AndroidJavaObject ajo)
		{
			AJO = ajo;
		}

		public Notification()
		{
			AJO = new AndroidJavaObject(C.AndroidAppNotification);
		}

		public AndroidJavaObject AJO { get; set; }

		/// <summary>
		///     What icon should be shown for this notification if it is being displayed in a Launcher that supports badging.
		///     Will be one of BadgeIcon.None, BadgeIcon.Small, or BadgeIcon.Large.
		/// </summary>
		public BadgeIcon BadgeIconType
		{
			get { return (BadgeIcon) AJO.CallInt("getBadgeIconType"); }
		}

		/// <summary>
		///     The id of the channel this notification posts to.
		/// </summary>
		public string ChannelId
		{
			get { return AJO.CallStr("getChannelId"); }
		}

		/// <summary>
		///     The key used to group this notification into a cluster or stack with other notifications on devices which support
		///     such rendering.
		/// </summary>
		public string Group
		{
			get { return AJO.CallStr("getGroup"); }
		}

		/// <summary>
		///     Returns which type of notifications in a group are responsible for audibly alerting the user.
		/// </summary>
		public GroupAlert GroupAlertBehaviour
		{
			get { return (GroupAlert) AJO.CallInt("getGroupAlertBehaviour"); }
		}

		/// <summary>
		///     The large icon shown in this notification's content view.
		/// </summary>
		[PublicAPI]
		public Texture2D LargeIcon
		{
			get
			{
				var iconAJO = AJO.CallAJO("getLargeIcon");
				var uri = iconAJO.CallAJO("getUri").JavaToString();
				return AGFileUtils.ImageUriToTexture2D(uri);
			}
		}

		/// <summary>
		///     The small icon shown in the status bar and notification's content view.
		/// </summary>
		[PublicAPI]
		public Texture2D SmallIcon
		{
			get
			{
				var iconAJO = AJO.CallAJO("getSmallIcon");
				var uri = iconAJO.CallAJO("getUri").JavaToString();
				return AGFileUtils.ImageUriToTexture2D(uri);
			}
		}

		/// <summary>
		///     A sort key that orders this notification among other notifications from the same package.
		///     This can be useful if an external sort was already applied and an app would like to preserve this.
		///     Notifications will be sorted lexicographically using this value, although providing different
		///     priorities in addition to providing sort key may cause this value to be ignored.
		///     This sort key can also be used to order members of a notification group. See Notification.Builder.setGroup(String).
		/// </summary>
		[PublicAPI]
		public string SortKey
		{
			get { return AJO.CallStr("getSortKey"); }
		}

		/// <summary>
		///     The duration from posting after which this notification should be canceled by the system, if it's not canceled
		///     already.
		/// </summary>
		[PublicAPI]
		public long TimeoutAfter
		{
			get { return AJO.CallLong("getTimeoutAfter"); }
		}

		/// <summary>
		///     Creates an action to open an URL in browser, that can be attached to a notification
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.KITKAT)]
		public static Action CreateOpenUrlAction(string url, string iconName, string buttonName)
		{
			var uri = AndroidUri.Parse(url);
			var intent = new AndroidIntent(AndroidIntent.ActionView);
			intent.SetData(uri);

			return new Action.Builder(R.GetAppDrawableId(iconName), buttonName, intent).Build();
		}

		/// <summary>
		///     Builder class for <see cref="Notification" /> objects]
		///     On platform versions that don't offer expanded notifications, methods that depend on expanded notifications have no
		///     effect.
		///     For example, action buttons won't appear on platforms prior to Android 4.1. Action buttons depend on expanded
		///     notifications, which are only available in Android 4.1 and later.
		/// </summary>
		[PublicAPI]
		public class Builder
		{
			readonly AndroidJavaObject _builderAjo;
			string _contentText;
			string _contentTitle;

			int _iconId;
			Priority _priority;

			/// <summary>
			///     Constructor.
			/// </summary>
			/// <param name="channelId">The constructed Notification will be posted on this <see cref="NotificationChannel" />.</param>
			/// <param name="customData">
			///     Custom user notification data to be later retrieved with
			///     <see cref="AGNotificationManager.GetNotificationParameter" /> if app was launched from notification.
			/// </param>
			public Builder(string channelId, [CanBeNull] Dictionary<string, string> customData = null)
			{
				if (AGUtils.IsNotAndroid())
				{
					Debug.LogError("This class should be used only on Android, errors ahead");
					return;
				}

				_builderAjo = new AndroidJavaObject(C.AndroidAppNotificationCompatBuilder, AGUtils.Activity, channelId);
				SetOpenThisApp(customData);
			}

			void SetOpenThisApp(Dictionary<string, string> customData)
			{
				var androidIntent = AndroidIntent.Wrap(AGUtils.NotificationIntermediateActivityIntent);
				androidIntent.PutExtra(AGNotificationManager.OpenedWithNotificationParamKey, true);
				if (customData != null)
				{
					foreach (var entry in customData)
					{
						var entryKey = entry.Key;
						var entryValue = entry.Value;
						androidIntent.PutExtra(entryKey, entryValue);
					}
				}

				androidIntent.SetFlags(AndroidIntent.Flags.ActivityNewTask | AndroidIntent.Flags.ActivityClearTask);

				var contentIntent = AndroidPendingIntent.GetActivity(androidIntent.AJO, AGUtils.RandomId(), AndroidPendingIntent.FLAG_UPDATE_CURRENT);
				_builderAjo.CallAJO("setContentIntent", contentIntent);
			}

			/// <summary>
			///     Add an action to this notification. Actions are typically displayed by the system as a button adjacent to the
			///     notification content.
			///     Every action must have an icon (32dp square and matching the Holo Dark action bar visual style), a textual label,
			///     and a PendingIntent.
			///     A notification in its expanded form can display up to 3 actions, from left to right in the order they were added.
			///     Actions will not be displayed when the notification is collapsed, however,
			///     so be sure that any essential functions may be accessed by the user in some other way
			///     (for example, in the Activity pointed to by Notification.contentIntent).
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[AndroidApi(AGDeviceInfo.VersionCodes.KITKAT)]
			public Builder AddAction(Action action)
			{
				_builderAjo.CallAJO("addAction", action.AJO);
				return this;
			}

			/// <summary>
			///     Depending on user preferences, this annotation may allow the notification to pass through interruption filters,
			///     if this notification is of category Notification.CATEGORY_CALL or Notification.CATEGORY_MESSAGE.
			///     The addition of people may also cause this notification to appear more prominently in the user interface.
			///     The person should be specified by the String representation of a ContactsContract.Contacts.CONTENT_LOOKUP_URI.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[Obsolete("This method was deprecated in API level 28. Use AddPerson(Person)")]
			[PublicAPI]
			public Builder AddPerson(string uri)
			{
				_builderAjo.CallAJO("addPerson", uri);
				return this;
			}

			/// <summary>
			///     Combine all of the options that have been set and return a new <see cref="Notification" /> object.
			/// </summary>
			/// <returns><see cref="Notification" /> object.</returns>
			public Notification Build()
			{
				return new Notification(_builderAjo.CallAJO("build"));
			}

			/// <summary>
			///     Setting this flag will make it so the notification is automatically canceled
			///     when the user clicks it in the panel. The PendingIntent set with setDeleteIntent(PendingIntent)
			///     will be broadcast when the notification is canceled.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetAutoCancel(bool autoCancel)
			{
				_builderAjo.CallAJO("setAutoCancel", autoCancel);
				return this;
			}

			/// <summary>
			///     Sets which icon to display as a badge for this notification.
			///     Must be one of BADGE_ICON_NONE, BADGE_ICON_SMALL, BADGE_ICON_LARGE.
			///     Note: This value might be ignored, for launchers that don't support badge icons.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetBadgeIconType(BadgeIcon icon)
			{
				_builderAjo.CallAJO("setBadgeIconType", (int) icon);
				return this;
			}

			/// <summary>
			///     Set the notification category.
			///     Must be one of the predefined notification categories (see the Category* constants in <see cref="Notification" />
			///     that best describes this notification. May be used by the system for ranking and filtering.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetCategory(string category)
			{
				_builderAjo.CallAJO("setCategory", category);
				return this;
			}

			/// <summary>
			///     Specifies the channel the notification should be delivered on. No-op on versions prior to O (API level 26).
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[AndroidApi(AGDeviceInfo.VersionCodes.O)]
			public Builder SetChannelId(string channelId)
			{
				_builderAjo.CallAJO("setChannelId", channelId);
				return this;
			}

			/// <summary>
			///     Sets notification color
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetColor(Color color)
			{
				_builderAjo.CallAJO("setColor", color.ToAndroidColor());
				return this;
			}

			/// <summary>
			///     Set whether this notification should be colorized.
			///     When set, the color set with SetColor will be used as the background color of this notification.
			///     This should only be used for high priority ongoing tasks
			///     like navigation, an ongoing call, or other similarly high-priority events for the user.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetColorized(bool isColorized)
			{
				_builderAjo.CallAJO("setColorized", isColorized);
				return this;
			}

			/// <summary>
			///     Set the large text at the right-hand side of the notification.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetContentInfo(string info)
			{
				_builderAjo.CallAJO("setContentInfo", info);
				return this;
			}

			/// <summary>
			///     Set the title (first row) of the notification, in a standard notification.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetContentTitle(string text)
			{
				_builderAjo.CallAJO("setContentTitle", text);
				return this;
			}

			/// <summary>
			///     Set the text (second row) of the notification, in a standard notification.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetContentText(string text)
			{
				_builderAjo.CallAJO("setContentText", text);
				return this;
			}

			/// <summary>
			///     Set the default notification options that will be used.
			///     The value should be one or more of the following fields combined with bitwise-or:
			///     DEFAULT_SOUND, DEFAULT_VIBRATE, DEFAULT_LIGHTS.
			///     For all default values, use DEFAULT_ALL.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetDefaults(Default def)
			{
				_builderAjo.CallAJO("setDefaults", (int) def);
				return this;
			}

			/// <summary>
			///     Set this notification to be part of a group of notifications sharing the same key.
			///     Grouped notifications may display in a cluster or stack on devices which support such rendering.
			///     To make this notification the summary for its group, also call SetGroupSummary(bool).
			///     A sort order can be specified for group members by using SetSortKey(string).
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetGroup(string groupKey)
			{
				_builderAjo.CallAJO("setGroup", groupKey);
				return this;
			}

			/// <summary>
			///     Sets the group alert behavior for this notification.
			///     Use this method to mute this notification if alerts for this notification's group should be
			///     handled by a different notification. This is only applicable for notifications that belong to a group.
			///     This must be called on all notifications you want to mute.
			///     For example, if you want only the summary of your group to make noise,
			///     all children in the group should have the group alert behavior GROUP_ALERT_SUMMARY.
			///     The default value is GROUP_ALERT_ALL.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetGroupAlertBehavior(GroupAlert alert)
			{
				_builderAjo.CallAJO("setGroupAlertBehavior", (int) alert);
				return this;
			}

			/// <summary>
			///     Set this notification to be the group summary for a group of notifications.
			///     Grouped notifications may display in a cluster or stack on devices which support such rendering.
			///     Requires a group key also be set using SetGroup(string).
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetGroupSummary(bool isGroupSummary)
			{
				_builderAjo.CallAJO("setGroupSummary", isGroupSummary);
				return this;
			}

			/// <summary>
			///     Set the large icon that is shown in the ticker and notification.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetLargeIcon(Texture2D tex)
			{
				_builderAjo.CallAJO("setLargeIcon", AGUtils.Texture2DToAndroidBitmap(tex));
				return this;
			}

			/// <summary>
			///     Set the desired color for the indicator LED on the device, as well as the blink duty cycle (specified in
			///     milliseconds).
			///     Not all devices will honor all (or even any) of these values.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[Obsolete("Was deprecated in API level 26. Use NotificationChannel.EnableLights(boolean) instead.")]
			[PublicAPI]
			public Builder SetLights(Color color, int onMs, int offMs)
			{
				_builderAjo.CallAJO("setLights", color.ToAndroidColor(), onMs, offMs);
				return this;
			}

			/// <summary>
			///     Set whether or not this notification is only relevant to the current device.
			///     Some notifications can be bridged to other devices for remote display.
			///     his hint can be set to recommend this notification not be bridged.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetLocalOnly(bool localOnly)
			{
				_builderAjo.CallAJO("setLocalOnly", localOnly);
				return this;
			}

			/// <summary>
			///     Set the large number at the right-hand side of the notification.
			///     This is equivalent to setContentInfo, although it might show the number
			///     in a different font size for readability.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetNumber(int number)
			{
				_builderAjo.CallAJO("setNumber", number);
				return this;
			}

			/// <summary>
			///     Set whether this is an ongoing notification.
			///     Ongoing notifications differ from regular notifications in the following ways:
			///     Ongoing notifications are sorted above the regular notifications in the notification panel.
			///     Ongoing notifications do not have an 'X' close button, and are not affected by the "Clear all" button.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetOngoing(bool ongoing)
			{
				_builderAjo.CallAJO("setOngoing", ongoing);
				return this;
			}

			/// <summary>
			///     Set this flag if you would only like the sound, vibrate and ticker to be played if the notification is not already
			///     showing.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetOnlyAlertOnce(bool alertOnce)
			{
				_builderAjo.CallAJO("setOnlyAlertOnce", alertOnce);
				return this;
			}

			/// <summary>
			///     Set the relative priority for this notification. Priority is an indication of how much of the user's
			///     valuable attention should be consumed by this notification.
			///     Low-priority notifications may be hidden from the user in certain situations,
			///     while the user might be interrupted for a higher-priority notification.
			///     The system sets a notification's priority based on various factors including the setPriority value.
			///     The effect may differ slightly on different platforms.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetPriority(Priority priority)
			{
				_builderAjo.CallAJO("setPriority", (int) priority);
				return this;
			}

			/// <summary>
			///     Set the progress this notification represents, which may be represented as a ProgressBar.
			///     Use indeterminate mode for the progress bar when you do not know how long an operation will take.
			///     Indeterminate mode is the default for progress bar and shows a cyclic animation without
			///     a specific amount of progress indicated.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetProgress(int max, int progress, bool indeterminate)
			{
				_builderAjo.CallAJO("setProgress", max, progress, indeterminate);
				return this;
			}

			/// <summary>
			///     Supply a replacement Notification whose contents should be shown in insecure contexts
			///     (i.e. atop the secure lockscreen). See visibility and VISIBILITY_PUBLIC.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetPublicVersion(Notification n)
			{
				_builderAjo.CallAJO("setPublicVersion", n.AJO);
				return this;
			}

			/// <summary>
			///     If this notification is duplicative of a Launcher shortcut, sets the id of the shortcut,
			///     in case the Launcher wants to hide the shortcut.
			///     Note:This field will be ignored by Launchers that don't support badging or shortcuts.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetShortcutId(string id)
			{
				_builderAjo.CallAJO("setShortcutId", id);
				return this;
			}

			/// <summary>
			///     Control whether the timestamp set with setWhen is shown in the content view.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetShowWhen(bool show)
			{
				_builderAjo.CallAJO("setShowWhen", show);
				return this;
			}

			/// <summary>
			///     Set the small icon to use in the notification layouts.
			///     Different classes of devices may return different sizes.
			///     See the UX guidelines for more information on how to design these icons.
			/// </summary>
			/// <param name="iconName">
			///     Filename of an icon in the project
			/// </param>
			/// <returns> The same Builder instance </returns>
			public Builder SetSmallIcon(string iconName)
			{
				_builderAjo.CallAJO("setSmallIcon", R.GetAppDrawableId(iconName));
				return this;
			}

			/// <summary>
			///     Set a sort key that orders this notification among other notifications from the same package.
			///     This can be useful if an external sort was already applied and an app would like to preserve this.
			///     Notifications will be sorted lexicographically using this value,
			///     although providing different priorities in addition to providing sort key may cause this value to be ignored.
			///     This sort key can also be used to order members of a notification group.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetSortKey(string key)
			{
				_builderAjo.CallAJO("setSortKey", key);
				return this;
			}

			/// <summary>
			///     Set the sound to play. It will play on the default stream.
			///     On some platforms, a notification that is noisy is more likely to be presented as a heads-up notification.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetSound(string soundFilePath)
			{
				if (soundFilePath == null)
				{
					throw new ArgumentNullException("soundFilePath");
				}

				_builderAjo.CallAJO("setSound", AndroidUri.FromFile(soundFilePath));
				return this;
			}

			/// <summary>
			///     Add a rich notification style to be applied at build time.
			/// </summary>
			public Builder SetBigPictureStyle(BigPictureStyle style)
			{
				_builderAjo.CallAJO("setStyle", style.AJO);
				return this;
			}

			/// <summary>
			///     Add a rich notification style to be applied at build time.
			/// </summary>
			public Builder SetBigTextStyle(BigTextStyle style)
			{
				_builderAjo.CallAJO("setStyle", style.AJO);
				return this;
			}

			/// <summary>
			///     Generate a large-format notification that include a list of (up to 5) strings.
			/// </summary>
			[AndroidApi(AGDeviceInfo.VersionCodes.JELLY_BEAN)]
			public Builder SetInboxStyle(InboxStyle style)
			{
				_builderAjo.CallAJO("setStyle", style.AJO);
				return this;
			}

			/// <summary>
			///     Use this for generating large-format notifications that include multiple back-and-forth messages of varying types
			///     between any number of people.
			/// </summary>
			[AndroidApi(AGDeviceInfo.VersionCodes.N)]
			public Builder SetMessagingStyle(MessagingStyle style)
			{
				_builderAjo.CallAJO("setStyle", style.AJO);
				return this;
			}

			/// <summary>
			///     Set the third line of text in the platform notification template.
			///     Don't use if you're also using SetProgress(int, int, bool);
			///     they occupy the same location in the standard template.
			///     If the platform does not provide large-format notifications,
			///     this method has no effect. The third line of text only appears in expanded view.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetSubText(string text)
			{
				_builderAjo.CallAJO("setSubText", text);
				return this;
			}

			/// <summary>
			///     Sets the "ticker" text which is sent to accessibility services.
			///     Prior to LOLLIPOP, sets the text that is displayed in the status bar when the notification first arrives.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetTicker(string text)
			{
				_builderAjo.CallAJO("setTicker", text);
				return this;
			}

			/// <summary>
			///     Specifies the time at which this notification should be canceled, if it is not already canceled.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetTimeoutAfter(long durationMs)
			{
				_builderAjo.CallAJO("setTimeoutAfter", durationMs);
				return this;
			}

			/// <summary>
			///     Show the when field as a stopwatch. Instead of presenting when as a timestamp, the notification will show
			///     an automatically updating display of the minutes and seconds since when.
			///     Useful when showing an elapsed time (like an ongoing phone call).
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetUsesChronometer(bool usesChronometer)
			{
				_builderAjo.CallAJO("setUsesChronometer", usesChronometer);
				return this;
			}

			/// <summary>
			///     Set the vibration pattern to use.
			///     On some platforms, a notification that vibrates is more likely to be presented as a heads-up notification.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetVibrate(long[] pattern)
			{
				_builderAjo.CallAJO("setVibrate", pattern);
				return this;
			}

			/// <summary>
			///     Sets visibility. One of VISIBILITY_PRIVATE (the default), VISIBILITY_PUBLIC, or VISIBILITY_SECRET.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetVisibility(Visibility visibility)
			{
				_builderAjo.CallAJO("setVisibility", (int) visibility);
				return this;
			}

			/// <summary>
			///     Set the time that the event occurred. Notifications in the panel are sorted by this time.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetWhen(long when)
			{
				_builderAjo.CallAJO("setWhen", when);
				return this;
			}

			/// <summary>
			///     Set the default Unity small icon to use in the notification layouts.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetDefaultSmallIcon()
			{
				_builderAjo.CallAJO("setSmallIcon", R.UnityLauncherIcon);
				return this;
			}
		}

		/// <summary>
		///     Structure to encapsulate a named action that can be shown as part of this notification.
		///     It must include an icon, a label, and a PendingIntent to be fired when the action is selected by the user.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.KITKAT)]
		public class Action
		{
			[PublicAPI]
			public enum SemanticAction
			{
				/// <summary>
				///     Archive the content associated with the notification. This could mean archiving an email, message, etc.
				/// </summary>
				[PublicAPI] Archive = 5,

				/// <summary>
				///     Call a contact, group, etc.
				/// </summary>
				[PublicAPI] Call = 10,

				/// <summary>
				///     Delete the content associated with the notification. This could mean deleting an email, message, etc.
				/// </summary>
				[PublicAPI] Delete = 4,

				/// <summary>
				///     Mark content as read.
				/// </summary>
				[PublicAPI] MarkAsRead = 2,

				/// <summary>
				///     Mark content as unread.
				/// </summary>
				[PublicAPI] MarkAsUnread = 3,

				/// <summary>
				///     Mute the content associated with the notification. This could mean silencing a conversation or currently playing
				///     media.
				/// </summary>
				[PublicAPI] Mute = 6,

				/// <summary>
				///     No semantic action defined.
				/// </summary>
				[PublicAPI] None = 0,

				/// <summary>
				///     Reply to a conversation, chat, group, or wherever replies may be appropriate.
				/// </summary>
				[PublicAPI] Reply = 1,

				/// <summary>
				///     Mark content with a thumbs down.
				/// </summary>
				[PublicAPI] ThumbsDown = 9,

				/// <summary>
				///     Mark content with a thumbs up.
				/// </summary>
				[PublicAPI] ThumbsUp = 8,

				/// <summary>
				///     Unmute the content associated with the notification. This could mean un-silencing a conversation or currently
				///     playing media.
				/// </summary>
				[PublicAPI] Unmute = 7
			}

			public Action(AndroidJavaObject ajo)
			{
				AJO = ajo;
			}

			public AndroidJavaObject AJO { get; set; }

			/// <summary>
			///     Constructs an <see cref="Notification.Action" /> instance.
			/// </summary>
			[PublicAPI]
			public class Builder
			{
				Builder(AndroidJavaObject ajo)
				{
					AJO = ajo;
				}

				public Builder(int icon, string title, AndroidIntent intent)
				{
					if (AGUtils.IsNotAndroid())
					{
						Debug.LogError("This class should be used only on Android, errors ahead");
						return;
					}

					if (Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.KITKAT_WATCH))
					{
						Debug.LogError("This class should be used on API level 20 or above, errors ahead");
						return;
					}

					var pendingIntent = AndroidPendingIntent.GetActivity(intent.AJO, AGUtils.RandomId());
					AJO = new AndroidJavaObject(C.AndroidAppNotificationCompatActionBuilder, icon, title,
						pendingIntent);
				}

				public AndroidJavaObject AJO { get; set; }

				/// <summary>
				///     Sets the SemanticAction for this Notification.Action. A SemanticAction denotes what
				///     an Notification.Action's PendingIntent will do (eg. reply, mark as read, delete, etc).
				/// </summary>
				[PublicAPI]
				public Builder SetSemanticAction(SemanticAction semanticAction)
				{
					AJO.CallAJO("setSemanticAction", (int) semanticAction);
					return this;
				}

				/// <summary>
				///     Set whether the platform should automatically generate possible replies to add to RemoteInput.getChoices().
				///     If the Notification.Action doesn't have a RemoteInput, this has no effect.
				/// </summary>
				[PublicAPI]
				public Builder SetAllowGeneratedReplies(bool allowGeneratedReplies)
				{
					AJO.CallAJO("setAllowGeneratedReplies", allowGeneratedReplies);
					return this;
				}

				/// <summary>
				///     Combine all of the options that have been set and return a new Notification.Action object.
				/// </summary>
				[PublicAPI]
				public Action Build()
				{
					return new Action(AJO.CallAJO("build"));
				}
			}
		}

		/// <summary>
		///     Helper class for generating large-format notifications that include a lot of text.
		///     If the platform does not provide large-format notifications, this method has no effect.
		///     The user will always see the normal notification view.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.JELLY_BEAN)]
		public class BigTextStyle
		{
			public AndroidJavaObject AJO;

			public BigTextStyle()
			{
				if (AGUtils.IsNotAndroid())
				{
					Debug.LogError("This class should be used only on Android, errors ahead");
					return;
				}

				if (Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.JELLY_BEAN))
				{
					Debug.LogError("This class should be used on API level 16 or above, errors ahead");
					return;
				}

				AJO = new AndroidJavaObject(C.AndroidAppNotificationCompatBigTextStyle);
			}

			public BigTextStyle(AndroidJavaObject ajo)
			{
				AJO = ajo;
			}

			/// <summary>
			///     Provide the longer text to be displayed in the big form of the template in place of the content text.
			/// </summary>
			/// <returns> The same instance of BigTextStyle </returns>
			[PublicAPI]
			public BigTextStyle BigText(string text)
			{
				AJO.CallAJO("bigText", text);
				return this;
			}

			/// <summary>
			///     Overrides ContentTitle in the big form of the template. This defaults to the value passed to SetContentTitle().
			/// </summary>
			/// <returns> The same instance of BigTextStyle </returns>
			[PublicAPI]
			public BigTextStyle SetBigContentTitle(string text)
			{
				AJO.CallAJO("setBigContentTitle", text);
				return this;
			}

			/// <summary>
			///     Set the first line of text after the detail section in the big form of the template.
			/// </summary>
			/// <returns> The same instance of BigTextStyle </returns>
			[PublicAPI]
			public BigTextStyle SetSummaryText(string text)
			{
				AJO.CallAJO("setSummaryText", text);
				return this;
			}
		}

		/// <summary>
		///     Helper class for generating large-format notifications that include multiple back-and-forth messages of varying
		///     types between any number of people.
		///     This could be the user-created name of the group or, if it doesn't have a specific name, a list of the participants
		///     in the conversation.
		///     Do not set a conversation title for one-on-one chats, since platforms use the existence of this field as a hint
		///     that the conversation is a group.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.N)]
		public class MessagingStyle
		{
			public AndroidJavaObject AJO;

			public MessagingStyle(string userDisplayName)
			{
				if (AGUtils.IsNotAndroid())
				{
					Debug.LogError("This class should be used only on Android, errors ahead");
					return;
				}

				if (Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					Debug.LogError("This class should be used on API level 24 or above, errors ahead");
					return;
				}

				AJO = new AndroidJavaObject(C.AndroidAppNotificationCompatMessagingStyle, userDisplayName);
			}

			/// <summary>
			///     Adds a message for display by this notification. Convenience call for a simple MessagingStyle.Message
			/// </summary>
			[PublicAPI]
			public MessagingStyle AddMessage(string text, long timestamp, string sender)
			{
				AJO.CallAJO("addMessage", text, timestamp, sender);
				return this;
			}

			/// <summary>
			///     Adds a MessagingStyle.Message for display in this notification.
			/// </summary>
			[PublicAPI]
			[AndroidApi(AGDeviceInfo.VersionCodes.N)]
			public MessagingStyle AddMessage(Message message)
			{
				AJO.CallAJO("addMessage", message.AJO);
				return this;
			}

			/// <summary>
			///     Return the title to be displayed on this conversation. Can be null.
			/// </summary>
			[PublicAPI]
			public string GetConversationTitle()
			{
				return AJO.CallStr("getConversationTitle");
			}

			/// <summary>
			///     Gets the list of Message objects that represent the notification
			/// </summary>
			[PublicAPI]
			public List<Message> GetMessages()
			{
				var result = new List<Message>();
				var messageListAjo = AJO.CallAJO("getMessages");
				var ajos = messageListAjo.FromJavaList<AndroidJavaObject>();
				foreach (var ajo in ajos)
				{
					result.Add(new Message(ajo));
				}

				return result;
			}

			/// <summary>
			///     Returns the name to be displayed for any replies sent by the user
			/// </summary>
			[PublicAPI]
			public string GetUserDisplayName()
			{
				return AJO.CallStr("getUserDisplayName");
			}

			/// <summary>
			///     Returns true if this notification represents a group conversation, otherwise false.
			///     If the application that generated this NotificationCompat.MessagingStyle targets an SDK version less than P,
			///     this method becomes dependent on whether or not the conversation title is set; returning true
			///     if the conversation title is a non-null value, or false otherwise. From P forward, this method returns
			///     what's set by setGroupConversation(boolean) allowing for named, non-group conversations.
			/// </summary>
			[PublicAPI]
			public bool IsGroupConversation()
			{
				return AJO.CallBool("isGroupConversation");
			}

			/// <summary>
			///     Sets the title to be displayed on this conversation. May be set to null.
			///     This API's behavior was changed in SDK version P. If your application's target version is less than P,
			///     setting a conversation title to a non-null value will make isGroupConversation() return true and passing
			///     null will make it return false. In P and beyond, use setGroupConversation(boolean) to set group conversation
			///     status.
			/// </summary>
			[PublicAPI]
			public MessagingStyle SetConversationTitle(string conversationTitle)
			{
				AJO.CallAJO("setConversationTitle", conversationTitle);
				return this;
			}

			/// <summary>
			///     Sets whether this conversation notification represents a group.
			/// </summary>
			[PublicAPI]
			public MessagingStyle SetGroupConversation(bool isGroupConversation)
			{
				AJO.CallAJO("setGroupConversation", isGroupConversation);
				return this;
			}

			/// <summary>
			///     Message object to be used by MessagingStyle Notifications
			/// </summary>
			[PublicAPI]
			public class Message
			{
				public AndroidJavaObject AJO;

				[PublicAPI]
				public Message(string text, long time, string sender = null)
				{
					if (AGUtils.IsNotAndroid())
					{
						Debug.LogError("This class should be used only on Android, errors ahead");
						return;
					}

					if (Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
					{
						Debug.LogError("This class should be used on API level 16 or above, errors ahead");
						return;
					}

					AJO = new AndroidJavaObject(C.AndroidAppNotificationCompatMessagingStyleMessage, text, time,
						sender);
				}

				public Message(AndroidJavaObject ajo)
				{
					AJO = ajo;
				}

				/// <summary>
				///     Get the text used to display the contact's name in the messaging experience
				/// </summary>
				[PublicAPI]
				public string GetSender()
				{
					return AJO.CallStr("getSender");
				}

				/// <summary>
				///     Get the text to be used for this message, or the fallback text if a type and content Uri have been set
				/// </summary>
				[PublicAPI]
				public string GetText()
				{
					return AJO.CallStr("getText");
				}

				/// <summary>
				///     Get the time at which this message arrived in ms since Unix epoch
				/// </summary>
				[PublicAPI]
				public long GetTimestamp()
				{
					return AJO.CallLong("getTimestamp");
				}
			}
		}

		/// <summary>
		///     Helper class for generating large-format notifications that include a large image attachment.
		///     If the platform does not provide large-format notifications, this method has no effect,
		///     the user will always see the normal notification view.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.JELLY_BEAN)]
		public class BigPictureStyle
		{
			public AndroidJavaObject AJO;

			public BigPictureStyle()
			{
				if (AGUtils.IsNotAndroid())
				{
					Debug.LogError("This class should be used only on Android, errors ahead");
					return;
				}

				if (Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.JELLY_BEAN))
				{
					Debug.LogError("This class should be used on API level 16 or above, errors ahead");
					return;
				}

				AJO = new AndroidJavaObject(C.AndroidAppNotificationCompatBigPictureStyle);
			}

			/// <summary>
			///     Override the large icon when the big notification is shown.
			/// </summary>
			[PublicAPI]
			public BigPictureStyle SetBigLargeIcon(Texture2D tex)
			{
				AJO.CallAJO("bigLargeIcon", AGUtils.Texture2DToAndroidBitmap(tex));
				return this;
			}

			/// <summary>
			///     Provide the texture to be used as the payload for the BigPicture notification.
			/// </summary>
			[PublicAPI]
			public BigPictureStyle SetBigPicture(Texture2D tex)
			{
				AJO.CallAJO("bigPicture", AGUtils.Texture2DToAndroidBitmap(tex));
				return this;
			}

			/// <summary>
			///     Overrides ContentTitle in the big form of the template. This defaults to the value passed to SetContentTitle().
			/// </summary>
			[PublicAPI]
			public BigPictureStyle SetBigContentTitle(string title)
			{
				AJO.CallAJO("setBigContentTitle", title);
				return this;
			}

			/// <summary>
			///     Set the first line of text after the detail section in the big form of the template.
			/// </summary>
			[PublicAPI]
			public BigPictureStyle SetSummaryText(string text)
			{
				AJO.CallAJO("setSummaryText", text);
				return this;
			}
		}

		/// <summary>
		///     Helper class for generating large-format notifications that include a list of (up to 5) strings.
		///     If the platform does not provide large-format notifications, this method has no effect,
		///     the user will always see the normal notification view.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.JELLY_BEAN)]
		public class InboxStyle
		{
			public AndroidJavaObject AJO;

			public InboxStyle()
			{
				if (AGUtils.IsNotAndroid())
				{
					Debug.LogError("This class should be used only on Android, errors ahead");
					return;
				}

				if (Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.JELLY_BEAN))
				{
					Debug.LogError("This class should be used on API level 16 or above, errors ahead");
					return;
				}

				AJO = new AndroidJavaObject(C.AndroidAppNotificationCompatInboxStyle);
			}

			/// <summary>
			///     Append a line to the digest section of the Inbox notification.
			/// </summary>
			[PublicAPI]
			public InboxStyle AddLine(string line)
			{
				AJO.CallAJO("addLine", line);
				return this;
			}

			/// <summary>
			///     Overrides ContentTitle in the big form of the template. This defaults to the value passed to SetContentTitle().
			/// </summary>
			[PublicAPI]
			public InboxStyle SetBigContentTitle(string title)
			{
				AJO.CallAJO("setBigContentTitle", title);
				return this;
			}

			/// <summary>
			///     Set the first line of text after the detail section in the big form of the template.
			/// </summary>
			[PublicAPI]
			public InboxStyle SetSummaryText(string text)
			{
				AJO.CallAJO("setSummaryText", text);
				return this;
			}
		}
	}
}
#endif