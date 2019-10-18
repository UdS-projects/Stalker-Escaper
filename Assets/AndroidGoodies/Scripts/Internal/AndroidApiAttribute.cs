#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	[MeansImplicitUse]
	public sealed class AndroidApiAttribute : Attribute
	{
		public AndroidApiAttribute()
		{
			AddedInSdkVersion = AGDeviceInfo.VersionCodes.BASE;
		}

		public AndroidApiAttribute(int addedInSdkVersion)
		{
			AddedInSdkVersion = addedInSdkVersion;
		}

		public int AddedInSdkVersion { get; private set; }
	}
}
#endif
