#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System.Collections.Generic;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;
	
	/// <summary>
	/// Class to represent various settings for the visual voicemail SMS filter. When the filter is enabled, incoming SMS matching the generalized OMTP format:
	/// [clientPrefix]:[prefix]:([key]=[value];)*
	/// will be regarded as a visual voicemail SMS, and removed before reaching the SMS provider.
	/// </summary>
	[PublicAPI]
	[AndroidApi(AGDeviceInfo.VersionCodes.O)]
	public class VisualVoicemailSmsFilterSettings
	{
		[PublicAPI]
		public enum DestinationPort
		{
			/// <summary>
			/// The visual voicemail SMS message does not have to be a data SMS, and can be directed to any port.
			/// </summary>
			Any = -1,
			/// <summary>
			/// The visual voicemail SMS message can be directed to any port, but must be a data SMS.
			/// </summary>
			DataSMS = -2
		}
		
		public AndroidJavaObject ajo;
		
		public VisualVoicemailSmsFilterSettings()
		{
			
		}

		public VisualVoicemailSmsFilterSettings(AndroidJavaObject ajo)
		{
			if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
			{
				return;
			}
			
			this.ajo = ajo;

			clientPrefix = ajo.Get<string>("clientPrefix");
			destinationPort = (DestinationPort) ajo.Get<int>("destinationPort");
			originatingNumbers = ajo.Get<List<string>>("originatingNumbers");
		}

		public string clientPrefix;

		public DestinationPort destinationPort;

		public List<string> originatingNumbers;

		/// <summary>
		/// Builder class for VisualVoicemailSmsFilterSettings objects.
		/// </summary>
		[PublicAPI]
		public class Builder
		{
			public AndroidJavaObject builderAjo;

			public Builder()
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return;
				}
				
				builderAjo = new AndroidJavaObject(C.AndroidTelephonyVisualVoicemailSmsFilterSettingsBuider);
			}

			/// <summary>
			/// Sets the client prefix for the visual voicemail SMS filter. The client prefix will appear at the start of a visual voicemail SMS message, followed by a colon(:).
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetClientPrefix(string clientPrefix)
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return this;
				}
				
				builderAjo.CallAJO("setClientPrefix", clientPrefix);
				return this;
			}

			/// <summary>
			/// Sets the destination port for the visual voicemail SMS filter.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetDestinationPort(DestinationPort port)
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return this;
				}
				
				builderAjo.CallAJO("setDestinationPort", (int) port);
				return this;
			}

			/// <summary>
			/// Sets the originating number whitelist for the visual voicemail SMS filter.
			/// If the list is not null only the SMS messages from a number in the list can be considered
			/// as a visual voicemail SMS. Otherwise, messages from any address will be considered.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			public Builder SetOriginatingNumbers(List<string> originatingNumbers)
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return this;
				}
				
				builderAjo.CallAJO("setOriginatingNumbers", originatingNumbers);
				return this;
			}

			public VisualVoicemailSmsFilterSettings Build()
			{if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return new VisualVoicemailSmsFilterSettings();
				}
				
				return new VisualVoicemailSmsFilterSettings(builderAjo.CallAJO("build"));
			}
		}
	}
}
#endif
