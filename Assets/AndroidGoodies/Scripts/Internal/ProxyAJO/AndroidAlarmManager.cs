#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;

	public static class AndroidAlarmManager
	{
		const int RTC_WAKEUP = 0;

		public static void SetExact(AndroidIntent intent, DateTime when, int id)
		{
			AGSystemService.AlarmService.Call("setExact", RTC_WAKEUP, CalcMillis(when),
				AndroidPendingIntent.GetBroadcast(intent.AJO, id));
		}

		static long CalcMillis(DateTime when)
		{
			return AGUtils.CurrentTimeMillis + when.ToMillisSinceEpoch() - DateTime.Now.ToMillisSinceEpoch();
		}

		public static void Set(AndroidIntent intent, DateTime when, int id)
		{
			AGSystemService.AlarmService.Call("set", RTC_WAKEUP, CalcMillis(when),
				AndroidPendingIntent.GetBroadcast(intent.AJO, id));
		}

		public static void SetRepeating(AndroidIntent intent, DateTime when, long intervalMillis, int id)
		{
			AGSystemService.AlarmService.Call("setRepeating", RTC_WAKEUP, CalcMillis(when), intervalMillis,
				AndroidPendingIntent.GetBroadcast(intent.AJO, id));
		}

		public static void Cancel(AndroidIntent intent, int id)
		{
			AGSystemService.AlarmService.Call("cancel", AndroidPendingIntent.GetBroadcast(intent.AJO, id));
		}
	}
}
#endif