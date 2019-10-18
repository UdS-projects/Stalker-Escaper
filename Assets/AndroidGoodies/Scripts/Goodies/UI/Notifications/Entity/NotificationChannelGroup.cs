#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using System.Collections.Generic;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	[PublicAPI]
	[AndroidApi(AGDeviceInfo.VersionCodes.O)]
	public class NotificationChannelGroup
	{
		public NotificationChannelGroup(AndroidJavaObject ajo)
		{
			AJO = ajo;
		}

		/// <summary>
		///     Creates a notification channel group.
		/// </summary>
		/// <param name="id">
		///     The id of the group. Must be unique per package. the value may be truncated if it is too long.
		/// </param>
		/// <param name="name">
		///     The recommended maximum length is 40 characters; the value may be truncated if it is too long.
		/// </param>
		[PublicAPI]
		public NotificationChannelGroup([NotNull] string id, string name)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}

			if (ApiCheck)
			{
				return;
			}

			AJO = new AndroidJavaObject(C.AndroidAppNotificationChannelGroup, id, name);
		}

		public AndroidJavaObject AJO { get; set; }

		/// <summary>
		///     Returns the id of this group.
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
		///     Returns the user visible name of this group.
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
		///     The user visible description of this group.
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

				if (AGDeviceInfo.SDK_INT >= AGDeviceInfo.VersionCodes.P)
				{
					return AJO.CallStr("getDescription");
				}

				return string.Empty;
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}

				if (AGDeviceInfo.SDK_INT >= AGDeviceInfo.VersionCodes.P)
				{
					AJO.Call("setDescription", value);
				}
			}
		}

		/// <summary>
		///     Returns whether or not notifications posted to channels belonging to this group are blocked.
		///     This value is independent of <see cref="AGNotificationManager.AreNotificationsEnabled()" />. and
		///     <see cref="AGNotificationManager.Importance()" />.
		/// </summary>
		[PublicAPI]
		public bool IsBlocked
		{
			get
			{
				if (ApiCheck)
				{
					return false;
				}

				if (AGDeviceInfo.SDK_INT >= AGDeviceInfo.VersionCodes.P)
				{
					return AJO.CallBool("isBlocked");
				}

				return false;
			}
		}

		/// <summary>
		///     Returns the list of channels that belong to this group
		/// </summary>
		[PublicAPI]
		public List<NotificationChannel> Channels
		{
			get
			{
				if (ApiCheck)
				{
					return new List<NotificationChannel>();
				}

				var result = new List<NotificationChannel>();
				var ajos = AJO.CallAJO("getChannels").FromJavaList<AndroidJavaObject>();
				foreach (var ajo in ajos)
				{
					result.Add(new NotificationChannel(ajo));
				}

				return result;
			}
		}

		static bool ApiCheck
		{
			get { return AGUtils.IsNotAndroid() || AGNotificationManager.HasNoNewNotificationsApi; }
		}

		public override string ToString()
		{
			return string.Format("Id: {0}, Name: {1}, Description: {2}, Channels: {3}, IsBlocked: {4}", Id, Name,
				Description, Channels, IsBlocked);
		}
	}
}
#endif