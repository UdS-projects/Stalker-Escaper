#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using JetBrains.Annotations;
	using Internal;
	using UnityEngine;

	/// <summary>
	/// Response to the AGTelephony.IccOpenLogicalChannel command.
	/// </summary>
	[PublicAPI]
	[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP)]
	public class IccOpenLogicalChannelResponse
	{
		public IccOpenLogicalChannelResponse()
		{
			
		}
		
		public IccOpenLogicalChannelResponse(AndroidJavaObject ajo)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP))
			{
				channelId = InvalidChannel;
				selectResponse = new byte[0];
				status = Status.UnknownError;
			}
			
			channelId = ajo.CallInt("getChannel");
			selectResponse = ajo.Call<byte[]>("getSelectResponse");
			status = (Status) ajo.CallInt("getStatus");
		}

		public const int InvalidChannel = -1;

		/// <summary>
		/// Possible status values returned by open channel command.
		/// </summary>
		[PublicAPI]
		public enum Status
		{
			/// <summary>
			/// No logical channels available. 
			/// </summary>
			MissingResource = 2,
			/// <summary>
			/// Open channel command returned successfully.
			/// </summary>
			NoError = 1,
			/// <summary>
			/// AID not found on UICC.
			/// </summary>
			NoSuchElement = 3,
			/// <summary>
			/// Unknown error in open channel command.
			/// </summary>
			UnknownError = 4
		}

		public int channelId;

		public byte[] selectResponse;

		public Status status;
	}
}
#endif