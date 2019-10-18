#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using System.Collections.Generic;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	///     Class to control local notifications
	/// </summary>
	[PublicAPI]
	[AndroidApi(AGDeviceInfo.VersionCodes.BASE)]
	public class AGNotificationManager
	{
		/// <summary>
		///     Value signifying that the user has not expressed a per-app visibility override value.
		/// </summary>
		[PublicAPI]
		public enum Importance
		{
			/// <summary>
			///     A notification with no importance: does not show in the shade.
			/// </summary>
			[PublicAPI] None = 0,

			/// <summary>
			///     Min notification importance: only shows in the shade, below the fold.
			///     This should not be used with Service.startForeground since a foreground service is supposed to be
			///     something the user cares about so it does not make semantic sense to mark its notification
			///     as minimum importance. If you do this as of Android version Build.VERSION_CODES.O,
			///     the system will show a higher-priority notification about your app running in the background.
			/// </summary>
			[PublicAPI] Min = 1,

			/// <summary>
			///     Low notification importance: shows everywhere, but is not intrusive.
			/// </summary>
			[PublicAPI] Low = 2,

			/// <summary>
			///     Default notification importance: shows everywhere, makes noise, but does not visually intrude.
			/// </summary>
			[PublicAPI] Default = 3,

			/// <summary>
			///     Higher notification importance: shows everywhere, makes noise and peeks. May use full screen intents.
			/// </summary>
			[PublicAPI] High = 4,

			/// <summary>
			///     Unused.
			/// </summary>
			[PublicAPI] Max = 5,

			/// <summary>
			///     Value signifying that the user has not expressed an importance.
			///     This value is for persisting preferences, and should never be associated with an actual notification.
			/// </summary>
			[PublicAPI] Unspecified = -1000
		}

		[PublicAPI]
		public enum InterruptionFilter
		{
			/// <summary>
			///     Normal interruption filter - no notifications are suppressed.
			/// </summary>
			[PublicAPI] All = 1,

			/// <summary>
			///     Priority interruption filter - all notifications are suppressed except those that match the priority criteria.
			///     Some audio streams are muted. Users can additionally specify packages that can bypass this interruption filter.
			/// </summary>
			[PublicAPI] Priority = 2,

			/// <summary>
			///     No interruptions filter - all notifications are suppressed and all audio streams
			///     (except those used for phone calls) and vibrations are muted.
			/// </summary>
			[PublicAPI] None = 3,

			/// <summary>
			///     Alarms only interruption filter - all notifications except those
			///     of category Notification.CATEGORY_ALARM are suppressed. Some audio streams are muted.
			/// </summary>
			[PublicAPI] Alarms = 4,

			/// <summary>
			///     Returned when the value is unavailable for any reason.
			/// </summary>
			[PublicAPI] Unknown = 0
		}

		public const string OpenedWithNotificationParamKey = "openedWithNotification";

		/// <summary>
		///     Convenience property to check if the app was opened from notification
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.BASE)]
		public static bool IsAppOpenedViaNotification
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return false;
				}

				return AGUtils.NotificationsActivityIntent != null && AGUtils.NotificationsActivityIntent.GetBoolExtra(OpenedWithNotificationParamKey, false);
			}
		}

		/// <summary>
		///     Returns whether notifications from the calling package are blocked.
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.N)]
		public static bool AreNotificationsEnabled
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return false;
				}

				if (Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return AGSystemService.NotificationServiceCompat.CallBool("areNotificationsEnabled");
				}

				return true;
			}
		}

		/// <summary>
		///     Returns all notification channels belonging to the calling package.
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static List<NotificationChannel> NotificationChannels
		{
			get
			{
				if (AGUtils.IsNotAndroid() || HasNoNewNotificationsApi)
				{
					return new List<NotificationChannel>();
				}

				var result = new List<NotificationChannel>();
				var channelsListAJO = AGSystemService.NotificationService.CallAJO("getNotificationChannels");
				var ajos = channelsListAJO.FromJavaList<AndroidJavaObject>();
				foreach (var ajo in ajos)
				{
					result.Add(new NotificationChannel(ajo));
				}

				return result;
			}
		}

		/// <summary>
		///     Returns all notification channel groups belonging to the calling app.
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static List<NotificationChannelGroup> NotificationChannelGroups
		{
			get
			{
				if (AGUtils.IsNotAndroid() || HasNoNewNotificationsApi)
				{
					return new List<NotificationChannelGroup>();
				}

				var result = new List<NotificationChannelGroup>();
				var ajos = AGSystemService.NotificationService.CallAJO("getNotificationChannelGroups")
					.FromJavaList<AndroidJavaObject>();
				foreach (var ajo in ajos)
				{
					result.Add(new NotificationChannelGroup(ajo));
				}

				return result;
			}
		}

		/// <summary>
		///     Gets the current notification interruption filter. The interruption filter defines which notifications
		///     are allowed to interrupt the user (e.g. via sound & vibration) and is applied globally.
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.M)]
		public static InterruptionFilter CurrentInterruptionFilter
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return InterruptionFilter.Unknown;
				}

				if (Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
				{
					return (InterruptionFilter) AGSystemService.NotificationService.CallInt(
						"getCurrentInterruptionFilter");
				}

				return InterruptionFilter.Unknown;
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}

				if (Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
				{
					AGSystemService.NotificationService.Call("setInterruptionFilter", (int) value);
				}
			}
		}

		/// <summary>
		///     Returns the user specified importance for notifications from the calling package.
		/// </summary>
		/// <value></value>
		[AndroidApi(AGDeviceInfo.VersionCodes.N)]
		public static Importance CurrentImportance
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return Importance.Unspecified;
				}

				if (Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return (Importance) AGSystemService.NotificationServiceCompat.CallInt("getImportance");
				}

				return Importance.Unspecified;
			}
		}

		/// <summary>
		///     Checks the ability to modify notification do not disturb policy for the calling package.
		///     Returns true if the calling package can modify notification policy.
		///     Apps can request policy access by sending the user to the activity that matches
		///     the system intent action Settings.ACTION_NOTIFICATION_POLICY_ACCESS_SETTINGS.
		///     Use ACTION_NOTIFICATION_POLICY_ACCESS_GRANTED_CHANGED to listen for user grant or denial of this access.
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.M)]
		public static bool IsNotificationPolicyAccessGranted
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return false;
				}

				if (Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
				{
					return AGSystemService.NotificationService.CallBool("isNotificationPolicyAccessGranted");
				}

				return true;
			}
		}

		/// <summary>
		///     Property to check if the new notification channels API is supported on the current device
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.BASE)]
		public static bool AreNotificationChannelsSupported
		{
			get { return Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O); }
		}

		internal static bool HasNoNewNotificationsApi
		{
			get { return !AreNotificationChannelsSupported; }
		}

		/// <summary>
		///     Cancel a previously shown notification. If it's transient, the view will be hidden.
		///     If it's persistent, it will be removed from the status bar.
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.BASE)]
		public static void Cancel(int id)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			AGSystemService.NotificationServiceCompat.Call("cancel", id);
		}

		/// <summary>
		///     Cancel a previously shown notification. If it's transient, the view will be hidden.
		///     If it's persistent, it will be removed from the status bar.
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.ECLAIR)]
		public static void Cancel([CanBeNull] string tag, int id)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			AGSystemService.NotificationServiceCompat.Call("cancel", tag, id);
		}

		/// <summary>
		///     Cancel all previously shown notifications.
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.BASE)]
		public static void CancelAll()
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			AGSystemService.NotificationServiceCompat.Call("cancelAll");
		}

		/// <summary>
		///     Creates a notification channel that notifications can be posted to.
		///     This can also be used to restore a deleted channel and to update an existing channel's name,
		///     description, group, and/or importance. The name and description should only be changed
		///     if the locale changes or in response to the user renaming this channel.
		///     For example, if a user has a channel named 'John Doe' that represents messages from a 'John Doe',
		///     and 'John Doe' changes his name to 'John Smith,' the channel can be renamed to match.
		///     The importance of an existing channel will only be changed if the new importance is lower
		///     than the current value and the user has not altered any settings on this channel.
		///     The group an existing channel will only be changed if the channel does not already
		///     belong to a group. All other fields are ignored for channels that already exist.
		/// </summary>
		/// <param name="channel">
		///     The channel to create. Note that the created channel may differ from this value.
		///     If the provided channel is malformed, a RemoteException will be thrown.
		///     This value must never be null.
		/// </param>
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static void CreateNotificationChannel([NotNull] NotificationChannel channel)
		{
			if (channel == null)
			{
				throw new ArgumentNullException("channel");
			}

			if (AGUtils.IsNotAndroid() || HasNoNewNotificationsApi)
			{
				return;
			}

			CreateChannel(channel);
		}

		static void CreateChannel(NotificationChannel channel)
		{
			AGSystemService.NotificationService.Call("createNotificationChannel", channel.AJO);
		}

		/// <summary>
		///     Creates multiple notification channels that different notifications can be posted to.
		///     See createNotificationChannel(NotificationChannel).
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static void CreateNotificationChannels(List<NotificationChannel> channels)
		{
			if (channels == null)
			{
				throw new ArgumentNullException("channels");
			}

			if (AGUtils.IsNotAndroid() || HasNoNewNotificationsApi)
			{
				return;
			}

			foreach (var channel in channels)
			{
				CreateChannel(channel);
			}
		}

		/// <summary>
		///     Deletes the given notification channel.
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static void DeleteNotificationChannel([NotNull] string channelId)
		{
			if (AGUtils.IsNotAndroid() || HasNoNewNotificationsApi)
			{
				return;
			}

			AGSystemService.NotificationService.Call("deleteNotificationChannel", channelId);
		}

		/// <summary>
		///     Returns the notification channel settings for a given channel id.
		///     The channel must belong to your package, or it will not be returned.
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static NotificationChannel GetNotificationChannel([NotNull] string channelId)
		{
			if (AGUtils.IsNotAndroid() || HasNoNewNotificationsApi)
			{
				return new NotificationChannel("DUMMY_ID", "DUMMY_NAME");
			}

			return new NotificationChannel(
				AGSystemService.NotificationService.CallAJO("getNotificationChannel", channelId));
		}

		/// <summary>
		///     Sets the current notification policy.
		///     Only available if policy access is granted to this package. See IsNotificationPolicyAccessGranted().
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.M)]
		public static void SetNotificationPolicy([NotNull] Policy policy)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
			{
				AGSystemService.NotificationService.Call("setNotificationPolicy", policy.AJO);
			}
		}

		/// <summary>
		///     Post a notification to be shown in the status bar, stream, etc.
		/// </summary>
		/// <param name="id">
		///     The ID of the notification
		/// </param>
		/// <param name="notification">
		///     The notification to post to the system
		/// </param>
		/// <param name="when">When to trigger the notification. If <code>null</code> shows the notification immediately.</param>
		[AndroidApi(AGDeviceInfo.VersionCodes.BASE)]
		public static void Notify(int id, [NotNull] Notification notification, DateTime? when = null)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (when == null)
			{
				AGSystemService.NotificationServiceCompat.Call("notify", id, notification.AJO);
			}
			else
			{
				Schedule(notification, when.Value, id, null);
			}
		}

		static void Schedule(Notification notification, DateTime when, int id, string tag)
		{
			var intent = CreateScheduleNotificationIntent(notification, id, tag);

			if (AGDeviceInfo.SDK_INT >= AGDeviceInfo.VersionCodes.M)
			{
				AndroidAlarmManager.SetExact(intent, when, id);
			}
			else
			{
				AndroidAlarmManager.Set(intent, when, id);
			}
		}

		static void ScheduleRepeating(Notification notification, DateTime when, int id, string tag, long intervalMillis)
		{
			var intent = CreateScheduleNotificationIntent(notification, id, tag);

			AndroidAlarmManager.SetRepeating(intent, when, intervalMillis, id);
		}

		/// <summary>
		/// 	Cancel a scheduled notification.
		/// </summary>
		/// <param name="id">
		///		The ID of the notification
		/// </param>
		public static void CancelScheduledNotification(int id)
		{
			var intent = CreateScheduleNotificationIntent(new Notification(), id, string.Empty);

			AndroidAlarmManager.Cancel(intent, id);
		}

		/// <summary>
		///     Post a notification to be shown in the status bar, stream, etc.
		/// </summary>
		/// <param name="tag">
		///     The string identifier for a notification. Can be null.
		/// </param>
		/// <param name="id">
		///     The ID of the notification. The pair (tag, id) must be unique within your app.
		/// </param>
		/// <param name="notification">
		///     The notification to post to the system
		/// </param>
		/// <param name="when">When to trigger the notification. If <code>null</code> shows the notification immediately.</param>
		[AndroidApi(AGDeviceInfo.VersionCodes.ECLAIR)]
		public static void Notify(string tag, int id, [NotNull] Notification notification, DateTime? when = null)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (when == null)
			{
				AGSystemService.NotificationServiceCompat.Call("notify", tag, id, notification.AJO);
			}
			else
			{
				Schedule(notification, when.Value, id, tag);
			}
		}

		/// <summary>
		///     Shows the repeating local notification. Upon click it will open the game if the game is not in foreground.
		///     NOTE: as of API 19, all repeating alarms are inexact.  If your
		///     application needs precise delivery times then it must use one-time
		///     exact alarms, rescheduling each time. Legacy applications
		///     whose SDK version is earlier than API 19 will continue to have all
		///     of their alarms, including repeating alarms, treated as exact.
		/// </summary>
		/// <param name="tag">
		///     The string identifier for a notification. Can be null.
		/// </param>
		/// <param name="id">
		///     The ID of the notification. The pair (tag, id) must be unique within your app.
		/// </param>
		/// <param name="notification">
		///     The notification to post to the system
		/// </param>
		/// <param name="intervalMillis">A period at which the alarm will automatically repeat.</param>
		/// <param name="when">The first time when first notification should fire up.</param>
		[AndroidApi(AGDeviceInfo.VersionCodes.ECLAIR)]
		public static void NotifyRepeating(string tag, int id, [NotNull] Notification notification, long intervalMillis, DateTime? when)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (when == null)
			{
				when = DateTime.Now;
			}

			ScheduleRepeating(notification, when.Value, id, null, intervalMillis);
		}

		/// <summary>
		///     Opens system settings window for the selected channel
		/// </summary>
		/// <param name="channelId">
		///     ID of the selected channel
		/// </param>
		[PublicAPI]
		public static void OpenNotificationChannelSettings([NotNull] string channelId)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			AGSettings.OpenNotificationChannelSettings(channelId);
		}

		/// <summary>
		///     Creates a group container for NotificationChannel objects. This can be used to rename an existing group.
		///     Group information is only used for presentation, not for behavior. Groups are optional for channels,
		///     and you can have a mix of channels that belong to groups and channels that do not.
		///     For example, if your application supports multiple accounts, and those accounts will have similar channels,
		///     you can create a group for each account with account specific labels instead of appending account
		///     information to each channel's label.
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static void CreateNotificationChannelGroup(NotificationChannelGroup group)
		{
			if (AGUtils.IsNotAndroid() || HasNoNewNotificationsApi)
			{
				return;
			}

			CreateGroup(group);
		}

		/// <summary>
		///     Creates multiple notification channel groups.
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static void CreateNotificationChannelGroups(List<NotificationChannelGroup> groups)
		{
			if (AGUtils.IsNotAndroid() || HasNoNewNotificationsApi)
			{
				return;
			}

			foreach (var group in groups)
			{
				CreateGroup(group);
			}
		}

		static void CreateGroup(NotificationChannelGroup group)
		{
			AGSystemService.NotificationService.Call("createNotificationChannelGroup", group.AJO);
		}

		/// <summary>
		///     Deletes the given notification channel group, and all notification channels that belong to it.
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static void DeleteNotificationChannelGroup(string groupId)
		{
			if (AGUtils.IsNotAndroid() || HasNoNewNotificationsApi)
			{
				return;
			}

			AGSystemService.NotificationService.Call("deleteNotificationChannelGroup", groupId);
		}

		/// <summary>
		///     Returns the notification channel group settings for a given channel group id.
		///     The channel group must belong to your package, or null will be returned.
		/// </summary>
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public static NotificationChannelGroup GetNotificationChannelGroup(string channelGroupId)
		{
			if (AGUtils.IsNotAndroid())
			{
				return null;
			}

			if (Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.P))
			{
				return new NotificationChannelGroup(
					AGSystemService.NotificationService.CallAJO("getNotificationChannelGroup", channelGroupId));
			}

			return null;
		}

		/// <summary>
		///     Use this to get the data, passed through Notification with <see cref="Notification.Builder" />.
		///     If the app was opened by tapping the notification, returns the value of KeyValue pair,
		///     that was added to the <see cref="Dictionary{TKey,TValue}" /> and passed as parameter to <see cref="Notification.Builder" />.
		/// </summary>
		[PublicAPI]
		public static string GetNotificationParameter([NotNull] string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}

			if (AGUtils.IsNotAndroid())
			{
				return null;
			}

			return AGUtils.NotificationsActivityIntent == null ? null : AGUtils.NotificationsActivityIntent.GetStringExtra(key);
		}

		[AndroidApi(AGDeviceInfo.VersionCodes.M)]
		public class Policy
		{
			/// <summary>
			///     Notification categories to prioritize.
			/// </summary>
			[PublicAPI]
			public enum PriorityCategory
			{
				/// <summary>
				///     Alarms are prioritized.
				///     API level 28
				/// </summary>
				[PublicAPI] Alarms = 32,

				/// <summary>
				///     Calls are prioritized.
				///     API level 23
				/// </summary>
				[PublicAPI] Calls = 8,

				/// <summary>
				///     Event notifications are prioritized.
				///     API level 23
				/// </summary>
				[PublicAPI] Events = 2,

				/// <summary>
				///     Media, game, voice navigation are prioritized.
				///     API level 28
				/// </summary>
				[PublicAPI] Media = 64,

				/// <summary>
				///     Message notifications are prioritized.
				///     API level 23
				/// </summary>
				[PublicAPI] Messages = 4,

				/// <summary>
				///     Reminder notifications are prioritized.
				///     API level 23
				/// </summary>
				[PublicAPI] Reminders = 1,

				/// <summary>
				///     Calls from repeat callers are prioritized.
				///     API level 23
				/// </summary>
				[PublicAPI] RepeatCallers = 16,

				/// <summary>
				///     System (catch-all for non-never suppressible sounds) are prioritized.
				///     API level 28
				/// </summary>
				[PublicAPI] System = 128
			}

			/// <summary>
			///     Notification senders to prioritize for messages.
			/// </summary>
			[PublicAPI]
			public enum PrioritySenders
			{
				/// <summary>
				///     Any sender is prioritized.
				/// </summary>
				[PublicAPI] Any = 0,

				/// <summary>
				///     Saved contacts are prioritized.
				/// </summary>
				[PublicAPI] Contacts = 1,

				/// <summary>
				///     Only starred contacts are prioritized.
				/// </summary>
				[PublicAPI] Starred = 2
			}

			/// <summary>
			///     Visual effects to suppress for a notification that is filtered by Do Not Disturb mode.
			/// </summary>
			[PublicAPI]
			[Flags]
			public enum SuppressedEffect
			{
				[PublicAPI] Unset = -1,

				/// <summary>
				///     Whether notification intercepted by DND are prevented from appearing on ambient displays
				///     on devices that support ambient display.
				/// </summary>
				[PublicAPI] Ambient = 128,

				/// <summary>
				///     Whether badges from notifications intercepted by DND are blocked on devices that support badging.
				/// </summary>
				[PublicAPI] Badge = 64,

				/// <summary>
				///     Whether full screen intents from notifications intercepted by DND are blocked.
				/// </summary>
				[PublicAPI] FullScreenIntent = 4,

				/// <summary>
				///     Whether notification lights from notifications intercepted by DND are blocked.
				/// </summary>
				[PublicAPI] Lights = 8,

				/// <summary>
				///     Whether notification intercepted by DND are prevented from appearing in notification list
				///     views like the notification shade or lockscreen on devices that support those views.
				/// </summary>
				[PublicAPI] NotificationList = 256,

				/// <summary>
				///     Whether notifications intercepted by DND are prevented from peeking.
				/// </summary>
				[PublicAPI] Peek = 16,

				/// <summary>
				///     Whether notifications suppressed by DND should not interrupt visually
				///     (e.g. with notification lights or by turning the screen on) when the screen is off.
				/// </summary>
				[PublicAPI] [Obsolete] ScreenOff = 1,

				/// <summary>
				///     Whether notifications suppressed by DND should not interrupt visually when the screen is on
				///     (e.g. by peeking onto the screen).
				/// </summary>
				[PublicAPI] [Obsolete] ScreenOn = 2,

				/// <summary>
				///     Whether notifications intercepted by DND are prevented from appearing in the status bar,
				///     on devices that support status bars.
				/// </summary>
				[PublicAPI] [Obsolete] StatusBar = 32
			}

			Policy(AndroidJavaObject ajo)
			{
				AJO = ajo;
			}

			/// <summary>
			///     Constructs a policy for Do Not Disturb priority mode behavior.
			/// </summary>
			/// <param name="priorityCategories">
			///     Bitmask of categories of notifications that can bypass DND.
			/// </param>
			/// <param name="priorityCallSenders">
			///     Which callers can bypass DND.
			/// </param>
			/// <param name="priorityMessageSenders">
			///     Which message senders can bypass DND.
			/// </param>
			/// <param name="suppressedVisualEffects">
			///     Which visual interruptions should be suppressed from notifications that are filtered by DND.
			/// </param>
			[PublicAPI]
			public Policy(PriorityCategory priorityCategories, PrioritySenders priorityCallSenders,
				PrioritySenders priorityMessageSenders, SuppressedEffect suppressedVisualEffects)
			{
				AJO = new AndroidJavaObject(C.AndroidAppNotificationManagerPolicy,
					(int) priorityCategories, (int) priorityCallSenders, (int) priorityMessageSenders,
					(int) suppressedVisualEffects);
			}

			/// <summary>
			///     Constructs a policy for Do Not Disturb priority mode behavior.
			///     Apps that target API levels below Build.VERSION_CODES.P cannot change user-designated values
			///     to allow or disallow PRIORITY_CATEGORY_ALARMS, PRIORITY_CATEGORY_SYSTEM, and PRIORITY_CATEGORY_MEDIA
			///     from bypassing dnd.
			/// </summary>
			/// <param name="priorityCategories">
			///     Bitmask of categories of notifications that can bypass DND.
			/// </param>
			/// <param name="priorityCallSenders">
			///     Which callers can bypass DND.
			/// </param>
			/// <param name="priorityMessageSenders">
			///     Which message senders can bypass DND.
			/// </param>
			[PublicAPI]
			public Policy(PriorityCategory priorityCategories, PrioritySenders priorityCallSenders,
				PrioritySenders priorityMessageSenders)
			{
				AJO = new AndroidJavaObject(C.AndroidAppNotificationManagerPolicy, (int) priorityCategories,
					(int) priorityCallSenders, (int) priorityMessageSenders);
			}

			public AndroidJavaObject AJO { get; private set; }
		}

		static AndroidIntent CreateScheduleNotificationIntent(Notification notification, int id, string tag)
		{
			var intent = new AndroidIntent(AGUtils.ClassForName(C.GoodiesNotificationManagerClassModern));
			intent.PutExtra("notification", notification.AJO);
			intent.PutExtra("id", id);
			intent.PutExtra("tag", tag);
			return intent;
		}
	}
}
#endif