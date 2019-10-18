#if UNITY_ANDROID
namespace AndroidGoodiesExamples
{
	using DeadMosquito.AndroidGoodies;

	public static class ExampleUtil
	{
		public static void ShowPermissionErrorToast(string permission)
		{
			var message = string.Format("{0} runtime permission missing in AndroidManifest.xml or user did not grant the permission.", permission);
			AGUIMisc.ShowToast(message);
		}
	}
}
#endif