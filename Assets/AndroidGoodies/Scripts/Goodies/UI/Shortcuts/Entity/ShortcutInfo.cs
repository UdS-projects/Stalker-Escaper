#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using System.Collections.Generic;
	using AndroidGoodies;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	/// Represents a shortcut that can be published via <see cref="AGShortcutManager"/>>.
	/// </summary>
	[PublicAPI]
	[AndroidApi(AGDeviceInfo.VersionCodes.N_MR1)]
	public class ShortcutInfo
	{
		/// <summary>
		/// Builder class for ShortcutInfo objects.
		/// </summary>
		public class Builder
		{
			readonly AndroidJavaObject _ajo;

			/// <summary>
			/// Constructor.
			/// </summary>
			[PublicAPI]
			public Builder(string shortcutId)
			{
				if (!AGShortcutManager.AreShortCutsSupported)
				{
					Debug.LogError(
						"This class should be used only on Android with API level 25 or higher, errors ahead");
					return;
				}

				_ajo = new AndroidJavaObject(C.AndroidContentPmShortcutInfoBuilder, AGUtils.Activity, shortcutId);
			}

			/// <summary>
			/// Creates a ShortcutInfo instance.
			/// </summary>
			[PublicAPI]
			public ShortcutInfo Build()
			{
				return new ShortcutInfo(_ajo.CallAJO("build"));
			}

			/// <summary>
			/// Sets the message that should be shown when the user attempts to start a shortcut that is disabled.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetDisabledMessage(string message)
			{
				_ajo.Call("setDisabledMessage", message);
				return this;
			}

			/// <summary>
			/// Sets an icon of a shortcut.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetIcon(Texture2D tex)
			{
				var iconAjo =
					C.AndroidGraphicsDrawableIcon.AJCCallStaticOnceAJO("createWithBitmap",
						AGUtils.Texture2DToAndroidBitmap(tex));
				_ajo.CallAJO("setIcon", iconAjo);
				return this;
			}

			/// <summary>
			/// Sets the open url intent of a shortcut.
			///  A shortcut can launch any intent that the publisher app has permission to launch.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetOpenUrlIntent(string url)
			{
				var uri = AndroidUri.Parse(url);
				var intent = new AndroidIntent(AndroidIntent.ActionView);
				intent.SetData(uri);

				_ajo.CallAJO("setIntent", intent.AJO);
				return this;
			}

			/// <summary>
			/// Sets the open current app intent of a shortcut.
			/// A shortcut can launch any intent that the publisher app has permission to launch.
			/// </summary>
			/// <param name="customData">
			/// Custom user notification data to be later retrieved.
			/// </param>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetOpenThisAppIntent(Dictionary<string, string> customData)
			{
				var androidIntent = AndroidIntent.Wrap(AGUtils.CurrentAppLaunchIntent);
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

				_ajo.CallAJO("setIntent", androidIntent.AJO);
				return this;
			}

			/// <summary>
			/// Sets the text of a shortcut.
			/// This field is intended to be more descriptive than the shortcut title.
			/// The launcher shows this instead of the short title when it has enough space.
			/// The recommend maximum length is 25 characters.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetLongLabel(string longLabel)
			{
				_ajo.CallAJO("setLongLabel", longLabel);
				return this;
			}

			/// <summary>
			/// "Rank" of a shortcut, which is a non-negative value that's used by the launcher app to sort shortcuts.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetRank(int rank)
			{
				_ajo.CallAJO("setRank", rank);
				return this;
			}

			/// <summary>
			/// Sets the short title of a shortcut.
			/// This field is intended to be a concise description of a shortcut.
			/// The recommended maximum length is 10 characters.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetShortLabel(string shortLabel)
			{
				_ajo.CallAJO("setShortLabel", shortLabel);
				return this;
			}
		}

		public AndroidJavaObject ajo;

		public ShortcutInfo(AndroidJavaObject ajo)
		{
			this.ajo = ajo;
		}

		[PublicAPI]
		public enum DisabledReasons
		{
			/// <summary>
			/// Shortcut has been disabled due to changes to the publisher app. (e.g. a manifest shortcut no longer exists.)
			/// </summary>
			AppChanged = 2,

			/// <summary>
			/// Shortcut has not been restored because the publisher app does not support backup and restore.
			/// </summary>
			BackupNotSupported = 101,

			/// <summary>
			/// Shortcut has been disabled by the publisher app with the AGShortcutManager.DisableShortcuts(List) API.
			/// </summary>
			ByApp = 1,

			/// <summary>
			/// Shortcut is not disabled.
			/// </summary>
			NotDisabled = 0,

			/// <summary>
			/// Shortcut has not been restored for unknown reason.
			/// </summary>
			OtherRestoreIssue = 103,

			/// <summary>
			/// Shortcut has not been restored because the publisher app's signature has changed.
			/// </summary>
			SignatureMismatch = 102,

			/// <summary>
			/// Shortcut is disabled for an unknown reason.
			/// </summary>
			Unknown = 3,

			/// <summary>
			/// Shortcut has been restored from the previous device, but the publisher app on the current device is of a lower version.
			/// The shortcut will not be usable until the app is upgraded to the same version or higher.
			/// </summary>
			VersionLower = 100
		}

		public const string ShortcutCategoryConversation = "android.shortcut.conversation";

		/// <summary>
		/// The message that should be shown when the user attempts to start a shortcut that is disabled.
		/// </summary>
		[PublicAPI]
		public string DisabledMessage
		{
			get
			{
				if (!AGShortcutManager.AreShortCutsSupported)
				{
					return string.Empty;

				}

				return ajo.CallStr("getDisabledMessage");
			}
		}

		/// <summary>
		/// Why a shortcut has been disabled.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		public DisabledReasons DisabledReason
		{
			get
			{
				if (Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.P))
				{
					return (DisabledReasons) ajo.CallInt("getDisabledReason");
				}

				return DisabledReasons.Unknown;
			}
		}

		/// <summary>
		/// The ID of a shortcut. Shortcut IDs are unique within each publisher app and must be stable across devices
		/// so that shortcuts will still be valid when restored on a different device. <see cref="AGShortcutManager"/> for details.
		/// </summary>
		/// <returns></returns>
		[PublicAPI]
		public string Id
		{
			get
			{
				if (!AGShortcutManager.AreShortCutsSupported)
				{
					return string.Empty;

				}

				return ajo.CallStr("getId");
			}
		}

		/// <summary>
		/// Last time when any of the fields was updated.
		/// </summary>
		public DateTime LastChangedTimestamp
		{
			get
			{
				if (!AGShortcutManager.AreShortCutsSupported)
				{
					return DateTime.MinValue;
				}

				var timeStamp = ajo.CallLong("getLastChangedTimestamp");
				var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				return start.AddMilliseconds(timeStamp).ToLocalTime();
			}
		}

		/// <summary>
		/// The long description of a shortcut.
		/// </summary>
		[PublicAPI]
		public string LongLabel
		{
			get
			{
				if (!AGShortcutManager.AreShortCutsSupported)
				{
					return string.Empty;

				}

				return ajo.CallStr("getLongLabel");
			}
		}

		/// <summary>
		/// The package name of the publisher app.
		/// </summary>
		[PublicAPI]
		public string Package
		{
			get
			{
				if (!AGShortcutManager.AreShortCutsSupported)
				{
					return string.Empty;

				}

				return ajo.CallStr("getPackage");
			}
		}

		/// <summary>
		/// "Rank" of a shortcut, which is a non-negative, sequential value that's unique for each getActivity() for each of the two types of shortcuts (static and dynamic).
		/// Because static shortcuts and dynamic shortcuts have overlapping ranks, when a launcher app shows shortcuts for an activity,
		/// it should first show the static shortcuts, followed by the dynamic shortcuts. Within each of those categories, shortcuts should be sorted by rank in ascending order.
		/// Floating shortcuts, or shortcuts that are neither static nor dynamic, will all have rank 0, because they aren't sorted.
		/// </summary>
		[PublicAPI]
		public int Rank
		{
			get
			{
				if (!AGShortcutManager.AreShortCutsSupported)
				{
					return 0;

				}

				return ajo.CallInt("getRank");
			}
		}

		/// <summary>
		/// The short description of a shortcut.
		/// </summary>
		[PublicAPI]
		public string ShortLabel
		{
			get
			{
				if (!AGShortcutManager.AreShortCutsSupported)
				{
					return string.Empty;

				}

				return ajo.CallStr("getShortLabel");
			}
		}

		/// <summary>
		/// Whether a shortcut only contains "key" information only or not. If true, only the following fields are available:
		/// Id, Package, LastChangedTimestamp, IsDynamic, IsPinned, IsDeclaredInManifest, IsImmutable, IsEnabled
		/// </summary>
		[PublicAPI]
		public bool HasKeyFieldsOnly
		{
			get
			{
				if (!AGShortcutManager.AreShortCutsSupported)
				{
					return false;

				}

				return ajo.CallBool("hasKeyFieldsOnly");
			}
		}

		/// <summary>
		/// Whether a shortcut is static; that is, whether a shortcut is published from AndroidManifest.xml.
		/// If true, the shortcut is also IsImmutable().
		/// When an app is upgraded and a shortcut is no longer published from AndroidManifest.xml, this will be set to false.
		/// If the shortcut is not pinned, then it'll disappear. However, if it's pinned,
		/// it will still be visible, IsEnabled() will be false and IsImmutable() will be true.
		/// </summary>
		[PublicAPI]
		public bool IsDeclaredInManifest
		{
			get
			{
				if (!AGShortcutManager.AreShortCutsSupported)
				{
					return false;

				}

				return ajo.CallBool("isDeclaredInManifest");
			}
		}

		/// <summary>
		/// Whether a shortcut is dynamic.
		/// </summary>
		[PublicAPI]
		public bool IsDynamic
		{
			get { return ajo.CallBool("isDynamic"); }
		}

		/// <summary>
		/// False if a shortcut is disabled with AGShortcutManager.DisableShortcuts(List).
		/// </summary>
		[PublicAPI]
		public bool IsEnabled
		{
			get
			{
				if (!AGShortcutManager.AreShortCutsSupported)
				{
					return false;
				}

				return ajo.CallBool("isEnabled");
			}
		}

		/// <summary>
		/// Whether a shortcut is immutable, in which case it cannot be modified with any of ShortcutManager APIs.
		/// All static shortcuts are immutable. When a static shortcut is pinned and is then disabled
		/// because it doesn't appear in AndroidManifest.xml for a newer version of the app, isDeclaredInManifest() returns false,
		/// but the shortcut is still immutable. All shortcuts originally published via the ShortcutManager APIs are all mutable.
		/// </summary>
		[PublicAPI]
		public bool IsImmutable
		{
			get
			{
				if (!AGShortcutManager.AreShortCutsSupported)
				{
					return false;
				}

				return ajo.CallBool("isImmutable");
			}
		}

		/// <summary>
		/// Whether a shortcut is pinned.
		/// </summary>
		[PublicAPI]
		public bool IsPinned
		{
			get
			{
				if (!AGShortcutManager.AreShortCutsSupported)
				{
					return false;
				}

				return ajo.CallBool("isPinned");
			}
		}

		public override string ToString()
		{
			return string.Format(
				"DisabledMessage: {0}, DisabledReason: {1}, Id: {2}, LastChangedTimestamp: {3}, LongLabel: {4}, Package: {5}, Rank: {6}, ShortLabel: {7}, HasKeyFieldsOnly: {8}, IsDeclaredInManifest: {9}, IsDynamic: {10}, IsEnabled: {11}, IsImmutable: {12}, IsPinned: {13}",
				DisabledMessage, DisabledReason, Id, LastChangedTimestamp, LongLabel, Package, Rank, ShortLabel,
				HasKeyFieldsOnly, IsDeclaredInManifest, IsDynamic, IsEnabled, IsImmutable, IsPinned);
		}
	}
}

#endif