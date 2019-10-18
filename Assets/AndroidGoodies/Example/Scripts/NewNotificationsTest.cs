namespace AndroidGoodiesExamples
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
#if UNITY_ANDROID
	using DeadMosquito.AndroidGoodies;
	using DeadMosquito.AndroidGoodies.Internal;
#endif
	using JetBrains.Annotations;
	using UnityEngine;
	using UnityEngine.UI;
	using Random = UnityEngine.Random;

	public class NewNotificationsTest : MonoBehaviour
	{
		const string ParamKeyId = "ParamKeyId";
		const string ParamKeyDate = "ParamKeyDate";

		string _soundFilePath;

		[SerializeField]
		InputField _channelIdInputField;

		[SerializeField]
		InputField _groupIdInputField;

		[SerializeField]
		Text _resultText;

		[SerializeField]
		Sprite _goodiesSprite;

		[SerializeField]
		Sprite _bigPictureSprite;
#if UNITY_ANDROID

		int _notificationId;
		string _lastChannelId;
		string _lastGroupId;

		void Start()
		{
			_channelIdInputField.text = "default_channel_id";
			_groupIdInputField.text = "default_group_id";
			
			LogNotificationParams();
		}

		void OnApplicationPause(bool pauseStatus)
		{
			if (!pauseStatus)
			{
				LogNotificationParams();
			}
		}

		[UsedImplicitly]
		public void LogNotificationParams()
		{
			if (AGNotificationManager.IsAppOpenedViaNotification)
			{
				var text = string.Format("App was opened via notification with parameters: {0}, {1}",
					AGNotificationManager.GetNotificationParameter(ParamKeyId),
					AGNotificationManager.GetNotificationParameter(ParamKeyDate));
				_resultText.text = text;
				Debug.Log(text);
			}
			else
			{
				_resultText.text = "App was not open via notification";
			}
		}

		[UsedImplicitly]
		public void CreateNotificationChannel()
		{
			if (NotificationChannelsApiCheck())
			{
				return;
			}

			var group = new NotificationChannelGroup(_groupIdInputField.text, RandomName())
			{
				Description = "Android-Goodies test notification channel group"
			};
			AGNotificationManager.CreateNotificationChannelGroup(group);

			var channel = NewNotificationChannel(_channelIdInputField.text, RandomName(), group.Id);
			channel.ShowBadge = true;
			AGNotificationManager.CreateNotificationChannel(channel);

			var retrievedChannel = AGNotificationManager.GetNotificationChannel(channel.Id);
			var retrievedGroup = AGNotificationManager.GetNotificationChannelGroup(group.Id);

			var msg = "Channel created: " + retrievedChannel + ", with group: " + retrievedGroup;
			Debug.Log(msg);
			_resultText.text = msg;

			_lastChannelId = channel.Id;
		}

		[UsedImplicitly]
		public void PickSoundForNotificationChannel()
		{
			if (NotificationChannelsApiCheck())
			{
				return;
			}

			AGFilePicker.PickAudio(audioFile =>
				{
					_soundFilePath = AndroidUri.FromFile(audioFile.OriginalPath).CallStr("getPath");
					Debug.Log(_soundFilePath);
					AGUIMisc.ShowToast(_soundFilePath);
				},
				error => AGUIMisc.ShowToast("Cancelled picking audio file"));
		}

		[UsedImplicitly]
		public void CreateNotificationChannelGroup()
		{
			if (NotificationChannelsApiCheck())
			{
				return;
			}

			var group = new NotificationChannelGroup(_groupIdInputField.text, RandomName())
			{
				Description = "Android-Goodies test notification channel group"
			};
			AGNotificationManager.CreateNotificationChannelGroup(group);

			var channelGroup = AGNotificationManager.GetNotificationChannelGroup(group.Id);
			Debug.Log("Channel group: " + channelGroup);

			_lastGroupId = group.Id;
		}

		static AudioAttributes CreateAudioAttributes()
		{
			return new AudioAttributes.Builder()
				.SetContentType(AudioAttributes.ContentType.Music)
				.SetFlags(AudioAttributes.Flags.FlagAll)
				.SetUsage(AudioAttributes.Usage.Notification)
				.Build();
		}

		[UsedImplicitly]
		public void OpenChannelSettings()
		{
			if (NotificationChannelsApiCheck())
			{
				return;
			}

			AGNotificationManager.OpenNotificationChannelSettings(_channelIdInputField.text);
		}

		[UsedImplicitly]
		public void DeleteChannel()
		{
			if (NotificationChannelsApiCheck())
			{
				return;
			}

			AGNotificationManager.DeleteNotificationChannel(_channelIdInputField.text);
		}

		[UsedImplicitly]
		public void DeleteNotificationChannelGroup()
		{
			if (NotificationChannelsApiCheck())
			{
				return;
			}

			AGNotificationManager.DeleteNotificationChannelGroup(_groupIdInputField.text);
		}

		[UsedImplicitly]
		public void GetAllChannels()
		{
			if (NotificationChannelsApiCheck())
			{
				return;
			}

			_resultText.text = string.Empty;
			foreach (var channel in AGNotificationManager.NotificationChannels)
			{
				_resultText.text += channel;
				Debug.Log(channel);
			}
		}

		[UsedImplicitly]
		public void GetAllChannelGroups()
		{
			if (NotificationChannelsApiCheck())
			{
				return;
			}

			_resultText.text = string.Empty;
			foreach (var group in AGNotificationManager.NotificationChannelGroups)
			{
				_resultText.text += group;
				Debug.Log(group);
			}
		}

		NotificationChannel NewNotificationChannel(string channelId, string channelName, string group)
		{
			var channel = new NotificationChannel(channelId, channelName, AGNotificationManager.Importance.Low)
			{
				Group = group,
				Description = "Android-Goodies test notification channel",
				VibrationPattern = new long[] {0, 400, 1000, 600, 1000, 800, 1000, 1000},
				BypassDnd = true,
				ShowBadge = true,
				LightColor = Color.red,
				EnableLights = true,
				EnableVibration = true,
				LockscreenVisibility = Notification.Visibility.Public
			};

			if (_soundFilePath != null)
			{
				channel.SetSound(_soundFilePath, CreateAudioAttributes());
			}

			return channel;
		}

		static string RandomName()
		{
			return "Name is " + Guid.NewGuid();
		}

		[UsedImplicitly]
		public void TrySetNotificationPolicy()
		{
			if (AGPermissions.IsPermissionGranted(AGPermissions.ACCESS_NOTIFICATION_POLICY))
			{
				var policy = new AGNotificationManager.Policy(AGNotificationManager.Policy.PriorityCategory.Alarms,
					AGNotificationManager.Policy.PrioritySenders.Any, AGNotificationManager.Policy.PrioritySenders.Any);
				AGNotificationManager.SetNotificationPolicy(policy);
			}
			else
			{
				Debug.LogWarning("You do not have the rights to access Notification Policy");
				_resultText.text = "You do not have the rights to access Notification Policy";
			}
		}

		[UsedImplicitly]
		public void CreateSimpleNotification()
		{
			Notify(CreateBaseNotificationBuilder().SetContentTitle("Simplest one!"));
		}

		[UsedImplicitly]
		public void CreateProgressBarNotification()
		{
			_notificationId++;

			var builder = CreateBaseNotificationBuilder()
				.SetContentTitle("Progress Bar Notification, id:" + _notificationId)
				.SetContentText("So progressive :)");
			AGNotificationManager.Notify(_notificationId, builder.Build());

			StartCoroutine(UpdateProgressBar(builder));
		}

		[UsedImplicitly]
		public void CreateBigTextNotification()
		{
			var builder = CreateBaseNotificationBuilder()
				.SetContentTitle("Big text Notification!")
				.SetBigTextStyle(new Notification.BigTextStyle()
					.BigText(
						"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."));

			Notify(builder);
		}

		[UsedImplicitly]
		public void CreateScheduledNotification()
		{
			_notificationId++;
			
			var notification = CreateBaseNotificationBuilder()
				.SetContentTitle("Scheduled one!")
				.SetContentText("Id: " + _notificationId)
				.Build();
			var time = DateTime.Now;
			time = time.AddSeconds(5);

			Debug.Log("Scheduled time is " + time + " with id: " + _notificationId);
			AGNotificationManager.Notify(_notificationId, notification, time);
		}

		[UsedImplicitly]
		public void CreateRepeatingNotification()
		{
			_notificationId++;
			
			var notification = CreateBaseNotificationBuilder()
				.SetContentTitle("Repeating one!")
				.SetContentText("I will repeat!")
				.Build();

			var time = DateTime.Now.AddSeconds(5);
			Debug.Log("NotificationsTest: scheduled time is " + time);
			// NOTE: as of API 19, all repeating alarms are inexact. If your
			// application needs precise delivery times then it must use one-time
			//	exact alarms, rescheduling each time.
			const long intervalMillis = 10 * 1000L; // every 10 seconds

			AGNotificationManager.NotifyRepeating(null, _notificationId, notification, intervalMillis, time);
		}

		[UsedImplicitly]
		public void CancelScheduledNotification()
		{
			Debug.Log("Cancelling scheduled notification with id: " + _notificationId);
			AGNotificationManager.CancelScheduledNotification(_notificationId);
		}

		[UsedImplicitly]
		public void CreateMessagingNotification()
		{
			if (AGDeviceInfo.SDK_INT < AGDeviceInfo.VersionCodes.N)
			{
				Debug.Log("Messaging styles not available on this API level. (Android N and higher)");
				return;
			}
			
			var builder = CreateBaseNotificationBuilder().SetMessagingStyle(new Notification.MessagingStyle("You:")
				.SetConversationTitle("Conversation Title")
				.AddMessage(NewMessage("Hello!", null))
				.AddMessage(NewMessage("Sup?", null))
				.AddMessage(NewMessage("Hi! Not bad, you?", "Ella"))
			);

			Notify(builder);
		}

		static Notification.MessagingStyle.Message NewMessage(string text, string sender)
		{
			return new Notification.MessagingStyle.Message(text, DateTime.Now.ToMillisSinceEpoch(), sender);
		}

		[UsedImplicitly]
		public void CancelLastNotification()
		{
			AGNotificationManager.Cancel(_notificationId);
		}

		[UsedImplicitly]
		public void CancelAllNotifications()
		{
			AGNotificationManager.CancelAll();
		}

		Notification.Builder CreateBaseNotificationBuilder()
		{
			if (AGNotificationManager.AreNotificationChannelsSupported)
			{
				CreateNotificationChannel();
				CreateNotificationChannelGroup();
			}

			var channelId = AGNotificationManager.AreNotificationChannelsSupported ? _lastChannelId : null;
			var groupId = AGNotificationManager.AreNotificationChannelsSupported ? _lastGroupId : null;
#pragma warning disable 0618
			var builder = new Notification.Builder(channelId, new Dictionary<string, string>
				{
					{ParamKeyId, "id: " + _notificationId},
					{ParamKeyDate, "date: " + DateTime.Now}
				})
				.SetGroup(groupId)
				.SetGroupAlertBehavior(Notification.GroupAlert.All)
				.SetChannelId(channelId)
				.SetSmallIcon("notify_icon_small")
				.SetLocalOnly(true)
				.SetBadgeIconType(Notification.BadgeIcon.Small)
				.SetContentTitle("Android Goodies")
				.SetContentInfo("Content info")
				.SetContentText("Test notification")
				.SetPriority(Notification.Priority.High)
				.SetVibrate(new long[] {1000, 100, 1000, 100, 100})
				.SetLights(Color.yellow, 1000, 1000)
				.SetColor(new Color(Random.value, Random.value, Random.value))
				.SetColorized(true)
				.SetDefaults(Notification.Default.All)
				.SetWhen(DateTime.Now.ToMillisSinceEpoch())
				.SetShowWhen(true)
				.SetSortKey("xyz")
				.SetAutoCancel(true)
				.SetVisibility(Notification.Visibility.Public);
#pragma warning restore 0618

			if (_soundFilePath != null)
			{
				builder.SetSound(_soundFilePath);
			}
			
			return builder;
		}

		[UsedImplicitly]
		public void CreateDifferentNotifications()
		{
			//Basic notification
			var builder = CreateBaseNotificationBuilder().SetContentTitle("Very basic one");
			Notify(builder);

			//Notification with large icon
			var iconTexture = _goodiesSprite.texture;
			builder = CreateBaseNotificationBuilder()
				.SetContentTitle("With large icon!")
				.SetBadgeIconType(Notification.BadgeIcon.Large)
				.SetLargeIcon(iconTexture)
				.SetCategory(Notification.CategoryMessage);
			Notify(builder);

			//Notification with number (shown as badge on app icon)
			builder = CreateBaseNotificationBuilder()
				.SetContentTitle("With number")
				.SetNumber(20);
			Notify(builder);

			//Chronometer notification
			builder = CreateBaseNotificationBuilder()
				.SetContentTitle("UsesChronometer")
				.SetUsesChronometer(true);
			Notify(builder);

			//Notification with ticker and subtext
			builder = CreateBaseNotificationBuilder()
				.SetContentTitle("With subtext and ticker")
				.SetSubText("Some subtext").SetTicker("Accessibility text");
			Notify(builder);

			//Auto-canceling notification (20s)
			builder = CreateBaseNotificationBuilder()
				.SetContentTitle("With timeout")
				.SetTimeoutAfter(20L * 1000L);
			Notify(builder);

			//Ongoing notification
			builder = CreateBaseNotificationBuilder()
				.SetContentTitle("Ongoing one")
				.SetOngoing(true);
			Notify(builder);

			//One-time alerting scheduled repeating notification
			builder = CreateBaseNotificationBuilder()
				.SetContentTitle("Alert once")
				.SetOnlyAlertOnce(true);
			var time = DateTime.Now.AddSeconds(5);
			const long intervalMillis = 2000;
			_notificationId++;
			AGNotificationManager.NotifyRepeating(null, _notificationId, builder.Build(), intervalMillis, time);

			//Notification with button - available in KitKat (20) or higher
			if (AGDeviceInfo.SDK_INT >= AGDeviceInfo.VersionCodes.KITKAT_WATCH)
			{
				builder = CreateBaseNotificationBuilder()
					.SetContentTitle("With button")
					.AddAction(Notification.CreateOpenUrlAction("https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki", "notify_icon_small", "Open URL"));
				Notify(builder);
			}

			builder = CreateBaseNotificationBuilder().SetGroupSummary(true);
			Notify(builder);
		}

		[UsedImplicitly]
		public void CreateBigPictureNotification()
		{
			var iconTexture = _goodiesSprite.texture;
			var bigPictureTexture = _bigPictureSprite.texture;
			var builder = CreateBaseNotificationBuilder().SetBigPictureStyle(
				new Notification.BigPictureStyle()
					.SetSummaryText("Summary Text")
					.SetBigContentTitle("Big Picture Title")
					.SetBigLargeIcon(iconTexture)
					.SetBigPicture(bigPictureTexture));
			Notify(builder);
			Debug.Log("NotificationTest: methods were performed!");
		}

		[UsedImplicitly]
		public void CreateInboxStyleNotification()
		{
			var builder = CreateBaseNotificationBuilder()
					.SetInboxStyle(new Notification.InboxStyle()
					.SetBigContentTitle("5 new messages from Julia:")
					.AddLine("Message 1")
					.AddLine("Message 2")
					.SetSummaryText("and +3 more"));
			Notify(builder);
		}

		[UsedImplicitly]
		public void AddPersonToNotification()
		{
			const string uri = "content://com.android.contacts/contacts/lookup/1911i5fe00f688ee1bc6d/1";
#pragma warning disable 0618
			var builder = CreateBaseNotificationBuilder().AddPerson(uri);
#pragma warning restore 0618
			Notify(builder);
		}

		[UsedImplicitly]
		public void CreateSimplestNotification()
		{
			CreateNotificationChannel();
			var builder = new Notification.Builder(_lastChannelId);
			builder.SetContentTitle("Simple");
			builder.SetContentText("Notification");
			builder.SetSmallIcon("notify_icon_small");
			Notify(builder);
		}

		void Notify(Notification.Builder builder)
		{
			_notificationId++;
			AGNotificationManager.Notify(_notificationId, builder.Build());
		}

		IEnumerator UpdateProgressBar(Notification.Builder builder)
		{
			var currentProgress = 0f;
			const float durationTime = 5f;
			const int maxProgress = 100;

			while (currentProgress < durationTime)
			{
				var progress = Mathf.CeilToInt(currentProgress / 5f * maxProgress);
				currentProgress += Time.deltaTime;
				builder.SetProgress(maxProgress, progress, false);
				AGNotificationManager.Notify(_notificationId, builder.Build());
				yield return null;
			}

			AGNotificationManager.Notify(_notificationId, builder.Build());
		}

		static bool NotificationChannelsApiCheck()
		{
			if (AGNotificationManager.AreNotificationChannelsSupported)
			{
				return false;
			}

			Debug.Log("Notification channels are not supported on this API version");
			return true;
		}
#endif
	}
}