// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGApps.cs
//


using System.IO;

#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	/// Class to open other apps on device and other application manipulations.
	/// </summary>
	public static class AGApps
	{
		/// <summary>
		/// Watch YouTube video. Opens video in YouTube app if its installed, falls back to browser.
		/// </summary>
		/// <param name="id">YouTube video id</param>
		[PublicAPI]
		public static void WatchYoutubeVideo(string id)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			var intent = new AndroidIntent(AndroidIntent.ActionView, AndroidUri.Parse("vnd.youtube:" + id));

			AGUtils.StartActivity(intent.AJO, () =>
			{
				var fallbackIntent = new AndroidIntent(AndroidIntent.ActionView,
					AndroidUri.Parse("http://www.youtube.com/watch?v=" + id));
				AGUtils.StartActivity(fallbackIntent.AJO);
			});
		}

		/// <summary>
		/// Opens the instagram profile in the app. Falls back to browser if instagram app is not installed.
		/// </summary>
		/// <param name="profileId">Profile id.</param>
		[PublicAPI]
		public static void OpenInstagramProfile(string profileId)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			Check.Argument.IsStrNotNullOrEmpty(profileId, "profileId");

			var formatUri = "http://instagram.com/_u/{0}";
			OpenProfileInternal("com.instagram.android", formatUri, profileId, formatUri);
		}

		/// <summary>
		/// Opens the twitter profile in the app. Falls back to browser if twitter app is not installed.
		/// </summary>
		/// <param name="profileId">Profile id.</param>
		[PublicAPI]
		public static void OpenTwitterProfile(string profileId)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			Check.Argument.IsStrNotNullOrEmpty(profileId, "profileId");

			OpenProfileInternal(null, "twitter://user?screen_name={0}", profileId, "https://twitter.com/{0}");
		}

		/// <summary>
		/// Opens the facebook profile in the app. Falls back to browser if facebook app is not installed.
		/// </summary>
		/// <param name="profileId">Profile id.</param>
		[PublicAPI]
		public static void OpenFacebookProfile(string profileId)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			Check.Argument.IsStrNotNullOrEmpty(profileId, "profileId");

			OpenProfileInternal(null, "fb://profile/{0}", profileId, "https://www.facebook.com/{0}");
		}

		[PublicAPI]
		static void OpenProfileInternal(string package, string formatUri, string profileId, string fallbackFormatUri)
		{
			var intent = GetViewProfileIntent(formatUri, profileId);
			if (package != null)
			{
				intent.SetPackage(package);
			}

			AGUtils.StartActivity(intent.AJO, () =>
			{
				var fallbackIntent = GetViewProfileIntent(fallbackFormatUri, profileId);
				AGUtils.StartActivity(fallbackIntent.AJO);
			});
		}

		static AndroidIntent GetViewProfileIntent(string uriFormat, string profileId)
		{
			var url = string.Format(uriFormat, profileId);
			return new AndroidIntent(AndroidIntent.ActionView, AndroidUri.Parse(url));
		}

		/// <summary>
		/// Opens the other app on device.
		/// </summary>
		/// <param name="package">Package of the app to open.</param>
		/// <param name="onAppNotInstalled">Invoked when the app with package is not installed</param>
		[PublicAPI]
		public static void OpenOtherAppOnDevice(string package, Action onAppNotInstalled = null)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			try
			{
				var launchIntent = AGUtils.GetLaunchIntent(package);
				launchIntent.CallAJO("addCategory", AndroidIntent.CATEGORY_LAUNCHER);
				AGUtils.StartActivity(launchIntent);
			}
			catch (Exception ex)
			{
				if (Debug.isDebugBuild)
				{
					Debug.Log("Could not find launch intent for package:" + package + ", Error: " + ex.StackTrace);
				}

				if (onAppNotInstalled != null)
				{
					onAppNotInstalled();
				}
			}
		}

		/// <summary>
		/// Displays the prompt to uninstall the app.
		/// </summary>
		/// <param name="package">Package to uninstall.</param>
		[PublicAPI]
		public static void UninstallApp(string package)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			try
			{
				var uri = AndroidUri.Parse(string.Format("package:{0}", package));
				var intent = new AndroidIntent(AndroidIntent.ActionDelete, uri);
				AGUtils.StartActivity(intent.AJO);
			}
			catch
			{
				// ignore
			}
		}

		/// <summary>
		/// Installs the apk file from SD card. The file MUST exist. Always check if file exists before invoking the method.
		/// </summary>
		/// <param name="apkPathOnDisc">APK path on disc.</param>
		[PublicAPI]
		public static void InstallApkFileFromSDCard(string apkPathOnDisc)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			try
			{
				var intent = new AndroidIntent(AndroidIntent.ActionView);
				intent.SetDataAndType(AndroidUri.FromFile(apkPathOnDisc), "application/vnd.android.package-archive");
				AGUtils.StartActivity(intent.AJO);
			}
			catch
			{
				if (Debug.isDebugBuild)
				{
					Debug.Log("Could not find apk file:" + apkPathOnDisc);
				}
			}
		}
		
		/// <summary>
		/// Opens pdf file from its full path.
		/// </summary>
		/// <param name="path"></param>
		[PublicAPI]
		public static void OpenPdf(string path)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}
			
			if (!File.Exists(path))
			{
				Debug.LogError("File doesn't exist: " + path);
				return;
			}

			var fileUri = AndroidPersistanceUtilsInternal.GetUriFromFilePath(path);

			var intent = new AndroidIntent(AndroidIntent.ActionView);
			intent.SetDataAndType(fileUri, AndroidIntent.MIMETypePdf);
			intent.SetFlags(AndroidIntent.Flags.GrantReadUriPermission | AndroidIntent.Flags.ActivityClearTop);
			
			AGUtils.StartActivity(intent.AJO);
		}
	}
}

#endif