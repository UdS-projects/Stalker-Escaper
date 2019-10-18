// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGUIMisc.cs
//


using JetBrains.Annotations;

#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using Internal;
	using UnityEngine;

	[PublicAPI]
	public static class AGUIMisc
	{
		#region toast

		/// <summary>
		/// Toast length.
		/// </summary>
		public enum ToastLength
		{
			/// <summary>
			/// The short toast duration.
			/// </summary>
			Short = 0,

			/// <summary>
			/// The long toast duration.
			/// </summary>
			Long = 1
		}

		/// <summary>
		/// Shows the toast with specified text.
		/// </summary>
		/// <param name="text">Text to display on toast.</param>
		/// <param name="length">Duration to show.</param>
		public static void ShowToast(string text, ToastLength length = ToastLength.Short)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			AGUtils.RunOnUiThread(() =>
				{
					using (var toast = new AndroidJavaClass(C.AndroidWidgetToast))
					{
						var toastInstance = toast.CallStaticAJO("makeText", AGUtils.Activity, text, (int) length);
						toastInstance.Call("show");
					}
				}
			);
		}

		#endregion

		#region immersive_mode

		const int SYSTEM_UI_FLAG_LAYOUT_STABLE = 0x00000100;
		const int SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION = 0x00000200;
		const int SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN = 0x00000400;
		const int SYSTEM_UI_FLAG_HIDE_NAVIGATION = 0x00000002;
		const int SYSTEM_UI_FLAG_FULLSCREEN = 0x00000004;
		const int SYSTEM_UI_FLAG_IMMERSIVE = 0x00000800;
		const int SYSTEM_UI_FLAG_IMMERSIVE_STICKY = 0x00001000;
		const int SYSTEM_UI_FLAG_VISIBLE = 0;

		const int ImmersiveFlagNonSticky = SYSTEM_UI_FLAG_LAYOUT_STABLE
		                                   | SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
		                                   | SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
		                                   | SYSTEM_UI_FLAG_HIDE_NAVIGATION // hide nav bar
		                                   | SYSTEM_UI_FLAG_FULLSCREEN // hide status bar
		                                   | SYSTEM_UI_FLAG_IMMERSIVE;

		const int ImmersiveFlagSticky = SYSTEM_UI_FLAG_LAYOUT_STABLE
		                                | SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION
		                                | SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN
		                                | SYSTEM_UI_FLAG_HIDE_NAVIGATION // hide nav bar
		                                | SYSTEM_UI_FLAG_FULLSCREEN // hide status bar
		                                | SYSTEM_UI_FLAG_IMMERSIVE_STICKY;

		// Enables Immersive Full-Screen Mode on Android device
		// Unity 5 has added immersive mode by default, so if your using Unity 5 or above, this method is redundant.
		public static void EnableImmersiveMode(bool sticky = true)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			GoodiesSceneHelper.IsInImmersiveMode = true;
			int mode = sticky ? ImmersiveFlagSticky : ImmersiveFlagNonSticky;

			AGUtils.RunOnUiThread(
				() =>
				{
					using (var decorView = AGUtils.ActivityDecorView)
					{
						decorView.Call("setSystemUiVisibility", mode);
					}
				});
		}

		/// <summary>
		/// Show the status bar
		/// </summary>
		[PublicAPI]
		public static void ShowStatusBar()
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			AGUtils.RunOnUiThread(
				() =>
				{
					using (var window = AGUtils.Window)
					{
						window.Call("clearFlags", SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN);
					}
				});
		}

		/// <summary>
		/// Hide the status bar
		/// </summary>
		[PublicAPI]
		public static void HideStatusBar()
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			AGUtils.RunOnUiThread(
				() =>
				{
					using (var window = AGUtils.Window)
					{
						window.Call("addFlags", SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN);
					}
				});
		}

		#endregion

		[PublicAPI]
		public enum ComponentEnabledState
		{
			Default = 0,
			Disabled = 2,
			DisabledUntilUsed = 4,
			DisabledUser = 3,
			Enabled = 1
		}

		const int DoNotKillAppFlag = 1;

		/// <summary>
		/// Hides/shows application icon from Launcher.
		/// IMPORTANT: It's somehow tricky if you need to do something with main activity in your app when it's hidden.
		/// You will face an ActivityNotFoundException. To make it work, you should show icon
		/// before doing anything to your main activity and hide it again after you are finished.
		/// </summary>
		public static void ChangeApplicationIconState(bool show)
		{
			var componentName = new AndroidJavaObject(C.AndroidContentComponentName, AGUtils.Activity, AGUtils.GetMainActivityClass());
			using (AGUtils.PackageManager)
			{
				AGUtils.PackageManager.Call("setComponentEnabledSetting", componentName, show ? (int) ComponentEnabledState.Enabled : (int) ComponentEnabledState.Disabled,
					DoNotKillAppFlag);
			}
		}
	}
}
#endif