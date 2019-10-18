#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using JetBrains.Annotations;
	using UnityEngine;
	using Internal;
	/// <summary>
	/// Immutable cell information from a point in time.
	/// </summary>
	[AndroidApi(AGDeviceInfo.VersionCodes.JELLY_BEAN_MR1)]
	[PublicAPI]
	public class CellInfo
	{
		[AndroidApi(AGDeviceInfo.VersionCodes.P)]
		[PublicAPI]
		public enum Connection
		{
			/// <summary>
			/// Cell is not a serving cell.
			/// The cell has been measured but is neither a camped nor serving cell (3GPP 36.304).
			/// </summary>
			None = 0,
			/// <summary>
			/// UE is connected to cell for signalling and possibly data (3GPP 36.331, 25.331).
			/// </summary>
			PrimaryServing = 1,
			/// <summary>
			/// UE is connected to cell for data (3GPP 36.331, 25.331).
			/// </summary>
			SecondaryServing = 2,
			/// <summary>
			/// Connection status is unknown.
			/// </summary>
			Unknown = 2147483647
		}

		public CellInfo(AndroidJavaObject ajo)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.JELLY_BEAN_MR1))
			{
				return;
			}

			if (!Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.P))
			{
				cellConnectionStatus = Connection.Unknown;
			}
			else
			{
				cellConnectionStatus = (Connection) ajo.CallInt("getCellConnectionStatus");
			}
			
			timeStamp = ajo.CallLong("getTimeStamp");
			isRegistered = ajo.CallBool("isRegistered");
		}

		/// <summary>
		/// Gets the connection status of this cell.
		/// </summary>
		public Connection cellConnectionStatus;

		/// <summary>
		/// Approximate time of this cell information in nanos since boot
		/// </summary>
		public long timeStamp;

		/// <summary>
		/// True if this cell is registered to the mobile network
		/// </summary>
		public bool isRegistered;

		public override string ToString()
		{
			return string.Format("Cell connection status: {0}, Time stamp: {1}, is registered - {2}",
				cellConnectionStatus, timeStamp, isRegistered);
		}
	}
}
#endif
