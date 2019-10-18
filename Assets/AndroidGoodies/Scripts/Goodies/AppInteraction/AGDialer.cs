// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGDialer.cs
//


using JetBrains.Annotations;

#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using Internal;
	using UnityEngine;

	/// <summary>
	/// Android class to place phone calls.
	/// </summary>
	[PublicAPI]
	public static class AGDialer
	{
		/// <summary>
		/// Indicates whether the device has the app installed which can place phone calls
		/// </summary>
		/// <returns><c>true</c>, if user has any phone app installed, <c>false</c> otherwise.</returns>
		public static bool UserHasPhoneApp()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			using (var i = new AndroidIntent(AndroidIntent.ActionDial))
			{
				return i.ResolveActivity();
			}
		}

		/// <summary>
		/// Opens the dialer with the number provided.
		/// </summary>
		/// <param name="phoneNumber">Phone number.</param>
		public static void OpenDialer(string phoneNumber)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			using (var i = new AndroidIntent(AndroidIntent.ActionDial))
			{
				i.SetData(ParsePhoneNumber(phoneNumber));
				AGUtils.StartActivity(i.AJO);
			}
		}

		/// <summary>
		/// Places the phone call immediately.
		/// 
		/// You need <uses-permission android:name="android.permission.CALL_PHONE" /> to use this method!
		/// </summary>
		/// <param name="phoneNumber">Phone number.</param>
		public static void PlacePhoneCall(string phoneNumber)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (!AGPermissions.IsPermissionGranted(AGPermissions.CALL_PHONE))
			{
				Debug.LogError(AGUtils.GetPermissionErrorMessage(AGPermissions.CALL_PHONE));
				return;
			}
			
			using (var i = new AndroidIntent(AndroidIntent.ActionCall))
			{
				i.SetData(ParsePhoneNumber(phoneNumber));
				AGUtils.StartActivity(i.AJO);
			}
		}

		static AndroidJavaObject ParsePhoneNumber(string phoneNumber)
		{
			return AndroidUri.Parse("tel:" + phoneNumber);
		}
	}
}
#endif