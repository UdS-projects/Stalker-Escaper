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
	/// ShortcutManager executes operations on an app's set of shortcuts,
	/// which represent specific tasks and actions that users can perform within your app.
	/// This page lists components of the ShortcutManager class that you can use to create and manage sets of shortcuts.
	/// </summary>
	[PublicAPI]
	[AndroidApi(AGDeviceInfo.VersionCodes.N_MR1)]
	public static class AGShortcutManager
	{
		/// <summary>
		/// Publish the list of dynamic shortcuts.
		/// If there are already dynamic or pinned shortcuts with the same IDs, each mutable shortcut is updated.
		/// This API will be rate-limited.
		/// </summary>
		[PublicAPI]
		public static void AddDynamicShortcuts(List<ShortcutInfo> list)
		{
			if (!AreShortCutsSupported)
			{
				return;
			}

			var javaList = list.ToJavaList(info => info.ajo);
			AGSystemService.ShortcutService.CallBool("addDynamicShortcuts", javaList);
		}

		/// <summary>
		/// Request to create a pinned shortcut.
		/// The default launcher will receive this request and ask the user for approval.
		/// If the user approves it, the shortcut will be created, and resultIntent will be sent.
		/// If a request is denied by the user, however, no response will be sent to the caller.
		/// Only apps with a foreground activity or a foreground service can call this method. Otherwise, it'll throw IllegalStateException.
		/// It's up to the launcher to decide how to handle previous pending requests when the same package calls this API multiple times in a row.
		/// One possible strategy is to ignore any previous requests.
		/// </summary>
		/// <param name="shortcut">
		/// New shortcut to pin. If an app wants to pin an existing (either dynamic or manifest) shortcut,
		/// then it only needs to have an ID, and other fields don't have to be set, in which case, the target shortcut must be enabled.
		/// If it's a new shortcut, all the mandatory fields, such as a short label, must be set. This value must never be null.
		/// </param>
		[PublicAPI]
		public static void RequestPinShortcut(ShortcutInfo shortcut)
		{
			if (!AreShortCutsSupported)
			{
				return;
			}

			var intent =
				AndroidIntent.Wrap(AGSystemService.ShortcutService.CallAJO("createShortcutResultIntent", shortcut.ajo));
			intent.SetAction(AndroidIntent.ActionCreateShortCut);
			var pendingIntentSender = AndroidPendingIntent.GetActivity(intent.AJO, AGUtils.RandomId()).CallAJO("getIntentSender");
			AGSystemService.ShortcutService.CallBool("requestPinShortcut", shortcut.ajo, pendingIntentSender);
		}

		/// <summary>
		/// Disable pinned shortcuts, showing the user a custom error message when they try to select the disabled shortcuts.
		/// </summary>
		[PublicAPI]
		public static void DisableShortcuts(List<string> shortcutIds, [CanBeNull]
			string disabledMessage)
		{
			if (!AreShortCutsSupported)
			{
				return;
			}

			var javaList = shortcutIds.ToJavaList(id => id);
			AGSystemService.ShortcutService.Call("disableShortcuts", javaList, disabledMessage);
		}

		/// <summary>
		/// Re-enable pinned shortcuts that were previously disabled. If the target shortcuts are already enabled, this method does nothing.
		/// </summary>
		/// <param name="shortcutIds"></param>
		[PublicAPI]
		public static void EnableShortcuts(List<string> shortcutIds)
		{
			if (!AreShortCutsSupported)
			{
				return;
			}

			var javaList = shortcutIds.ToJavaList(id => id);
			AGSystemService.ShortcutService.Call("enableShortcuts", javaList);
		}

		/// <summary>
		/// Return all dynamic shortcuts from the caller app.
		/// This API is intended to be used for examining what shortcuts are currently published.
		/// Re-publishing returned ShortcutInfos via APIs such as SetDynamicShortcuts(List) may cause loss of information such as icons.
		/// </summary>
		[PublicAPI]
		public static List<ShortcutInfo> DynamicShortcuts
		{
			get
			{
				if (!AreShortCutsSupported)
				{
					return new List<ShortcutInfo>();
				}

				var listAjo = AGSystemService.ShortcutService.CallAJO("getDynamicShortcuts");
				var result = new List<ShortcutInfo>();
				var ajos = listAjo.FromJavaList<AndroidJavaObject>();
				foreach (var ajo in ajos)
				{
					result.Add(new ShortcutInfo(ajo));
				}

				return result;
			}
		}

		/// <summary>
		/// The max height for icons, in pixels.
		/// </summary>
		[PublicAPI]
		public static int IconMaxHeight
		{
			get
			{
				if (!AreShortCutsSupported)
				{
					return 0;
				}

				return AGSystemService.ShortcutService.CallInt("getIconMaxHeight");
			}
		}

		/// <summary>
		/// The max width for icons, in pixels. Note that this method returns max width of icon's visible part.
		/// </summary>
		[PublicAPI]
		public static int IconMaxWidth
		{
			get
			{
				if (!AreShortCutsSupported)
				{
					return 0;
				}

				return AGSystemService.ShortcutService.CallInt("getIconMaxWidth");
			}
		}

		/// <summary>
		/// The maximum number of static and dynamic shortcuts that each launcher icon can have at a time.
		/// </summary>
		[PublicAPI]
		public static int MaxShortcutCountPerActivity
		{
			get
			{
				if (!AreShortCutsSupported)
				{
					return 0;
				}

				return AGSystemService.ShortcutService.CallInt("getMaxShortcutCountPerActivity");
			}
		}

		/// <summary>
		/// Returns all pinned shortcuts from the caller app.
		/// This API is intended to be used for examining what shortcuts are currently published.
		/// Re-publishing returned ShortcutInfos via APIs such as SetDynamicShortcuts(List) may cause loss of information such as icons.
		/// </summary>
		[PublicAPI]
		public static List<ShortcutInfo> PinnedShortcuts
		{
			get
			{
				if (!AreShortCutsSupported)
				{
					return new List<ShortcutInfo>();
				}

				var listAjo = AGSystemService.ShortcutService.CallAJO("getPinnedShortcuts");
				var result = new List<ShortcutInfo>();
				var ajos = listAjo.FromJavaList<AndroidJavaObject>();
				foreach (var ajo in ajos)
				{
					result.Add(new ShortcutInfo(ajo));
				}

				return result;
			}
		}

		/// <summary>
		/// Return true when rate-limiting is active for the caller app.
		/// When using the SetDynamicShortcuts(), AddDynamicShortcuts(), or UpdateShortcuts() methods,
		/// keep in mind that you might only be able to call these methods a specific number of times in a background app,
		/// an app with no activities or services currently in the foreground. The limit on the specific number of times
		/// you can call these methods is called rate limiting. This feature is used to prevent ShortcutManager from over-consuming device resources.
		/// </summary>
		[PublicAPI]
		public static bool IsRateLimitingActive
		{
			get
			{
				if (!AreShortCutsSupported)
				{
					return true;
				}

				return AGSystemService.ShortcutService.CallBool("isRateLimitingActive");
			}
		}

		/// <summary>
		/// Return TRUE if the app is running on a device whose default launcher supports requestPinShortcut(ShortcutInfo, IntentSender).
		/// The return value may change in subsequent calls if the user changes the default launcher app.
		/// </summary>
		[PublicAPI]
		public static bool IsRequestPinShortcutSupported
		{
			get
			{
				if (!AreShortCutsSupported)
				{
					return false;
				}

				return AGSystemService.ShortcutService.CallBool("isRequestPinShortcutSupported");
			}
		}

		/// <summary>
		/// Delete all dynamic shortcuts from the caller app.
		/// </summary>
		[PublicAPI]
		public static void RemoveAllDynamicShortcuts()
		{
			if (!AreShortCutsSupported)
			{
				return;
			}

			AGSystemService.ShortcutService.Call("removeAllDynamicShortcuts");
		}

		/// <summary>
		/// Delete dynamic shortcuts by ID.
		/// </summary>
		[PublicAPI]
		public static void RemoveDynamicShortcuts(List<string> shortcutIds)
		{
			if (!AreShortCutsSupported)
			{
				return;
			}

			var javaList = shortcutIds.ToJavaList(id => id);
			AGSystemService.ShortcutService.Call("removeDynamicShortcuts", javaList);
		}

		/// <summary>
		/// Publish the list of shortcuts. All existing dynamic shortcuts from the caller app will be replaced.
		/// If there are already pinned shortcuts with the same IDs, the mutable pinned shortcuts are updated.
		/// This API will be rate-limited.
		/// </summary>
		[PublicAPI]
		public static void SetDynamicShortcuts(List<ShortcutInfo> list)
		{
			if (!AreShortCutsSupported)
			{
				return;
			}

			var javaList = list.ToJavaList(info => info.ajo);
			AGSystemService.ShortcutService.CallBool("setDynamicShortcuts", javaList);
		}

		/// <summary>
		/// Update all existing shortcuts with the same IDs. Target shortcuts may be pinned and/or dynamic, but they must not be immutable.
		/// This API will be rate-limited.
		/// </summary>
		[PublicAPI]
		public static void UpdateShortcuts(List<ShortcutInfo> list)
		{
			if (!AreShortCutsSupported)
			{
				return;
			}

			var javaList = list.ToJavaList(info => info.ajo);
			AGSystemService.ShortcutService.CallBool("updateShortcuts", javaList);
		}

		/// <summary>
		///     Use this to get the data, passed through Shortcut with <see cref="ShortcutInfo.Builder.SetOpenThisAppIntent" />.
		///     If the app was opened by tapping the notification, returns the value of KeyValue pair,
		///     that was added to the <see cref="Dictionary{TKey,TValue}" /> and passed as parameter to <see cref="ShortcutInfo.Builder.SetOpenThisAppIntent" />.
		/// </summary>
		[PublicAPI]
		public static string GetShortcutParameter([NotNull]
			string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}

			if (AGUtils.IsNotAndroid())
			{
				return null;
			}

			return AGUtils.ActivityIntent.GetStringExtra(key);
		}

		public static bool AreShortCutsSupported
		{
			get { return !AGUtils.IsNotAndroid() && Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N_MR1); }
		}
	}
}

#endif