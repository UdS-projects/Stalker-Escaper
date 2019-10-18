namespace AndroidGoodiesExamples
{
	#if UNITY_ANDROID
	using DeadMosquito.AndroidGoodies;
#endif
	using JetBrains.Annotations;
	using UnityEngine;

	public class DialTest : MonoBehaviour
	{
#if UNITY_ANDROID
		const string PhoneNumber = "123456789";

		[UsedImplicitly]
		public void OnShowDialer()
		{
			AGDialer.OpenDialer(PhoneNumber);
		}

		[UsedImplicitly]
		public void OnPlaceCall()
		{
			AGPermissions.ExecuteIfHasPermission(AGPermissions.CALL_PHONE, () => AGDialer.PlacePhoneCall(PhoneNumber), 
				() => ExampleUtil.ShowPermissionErrorToast(AGPermissions.CALL_PHONE));
		}

#endif
	}
}
