#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using UnityEngine;

	/// <summary>
	/// Android pending intent.
	/// </summary>
	static class AndroidPendingIntent
	{
		public const int FLAG_UPDATE_CURRENT = 1 << 27;
		public const int FLAG_ONE_SHOT = 1073741824;
		public const int FLAG_CANCEL_CURRENT = 268435456;

		public static AndroidJavaObject GetActivity(AndroidJavaObject intent, int id, int flag = FLAG_UPDATE_CURRENT)
		{
			using (var pic = new AndroidJavaClass(C.AndroidAppPendingIntent))
			{
				return pic.CallStaticAJO("getActivity", AGUtils.Activity, id, intent, flag);
			}
		}
		
		public static AndroidJavaObject GetBroadcast(AndroidJavaObject intent, int id)
		{
			using (var pic = new AndroidJavaClass(C.AndroidAppPendingIntent))
			{
				return pic.CallStaticAJO("getBroadcast", AGUtils.Activity, id, intent, FLAG_UPDATE_CURRENT);
			}
		}
	}
}
#endif