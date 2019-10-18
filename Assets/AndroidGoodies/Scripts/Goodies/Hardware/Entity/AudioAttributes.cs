#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	[PublicAPI]
	[AndroidApi(AGDeviceInfo.VersionCodes.LOLLIPOP)]
	public class AudioAttributes
	{
		[PublicAPI]
		public class Builder
		{
			int _usage;
			int _contentType;
			int _flags;

			/// <summary>
			/// Sets the attribute describing what is the intended use of the the audio signal, such as alarm or ringtone.
			/// </summary>
			/// <param name="usage">
			/// Use, intended for the audio signal
			/// </param>
			/// <returns>
			/// The same Builder instance.
			/// </returns>
			[PublicAPI]
			public Builder SetUsage(Usage usage)
			{
				_usage = (int) usage;
				return this;
			}

			/// <summary>
			/// Sets the attribute describing the content type of the audio signal, such as speech, or music.
			/// </summary>
			/// <param name="contentType">
			/// Type of content
			/// </param>
			/// <returns>
			/// The same Builder instance.
			/// </returns>
			[PublicAPI]
			public Builder SetContentType(ContentType contentType)
			{
				_contentType = (int) contentType;
				return this;
			}

			/// <summary>
			/// Sets the combination of flags. This is a bitwise OR with the existing flags.
			/// </summary>
			/// <param name="flags">
			/// Chosen flags
			/// </param>
			/// <returns>
			/// The same Builder instance.
			/// </returns>
			[PublicAPI]
			public Builder SetFlags(Flags flags)
			{
				_flags = (int) flags;
				return this;
			}

			/// <summary>
			/// Combines all of the attributes that have been set and returns a new AudioAttributes object.
			/// </summary>
			[PublicAPI]
			public AudioAttributes Build()
			{
				return new AudioAttributes((Usage) _usage, (ContentType) _contentType, (Flags) _flags);
			}
		}

		/// <summary>
		/// Describes the content type of the audio signal, such as speech, or music.
		/// </summary>
		[PublicAPI]
		public enum ContentType
		{
			[PublicAPI]
			Unknown = 0,

			[PublicAPI]
			Speech = 1,

			[PublicAPI]
			Music = 2,

			[PublicAPI]
			Movie = 3,

			[PublicAPI]
			Sonification = 4
		}

		/// <summary>
		/// Describes what is the intended use of the the audio signal, such as alarm or ringtone.
		/// </summary>
		[PublicAPI]
		public enum Usage
		{
			[PublicAPI]
			Unknown = 0,

			[PublicAPI]
			Media = 1,

			[PublicAPI]
			VoiceCommunication = 2,

			[PublicAPI]
			VoiceCommunicationSignalling = 3,

			[PublicAPI]
			Alarm = 4,

			[PublicAPI]
			Notification = 5,

			[PublicAPI]
			NotificationRingtone = 6,

			[PublicAPI]
			NotificationCommunicationRequest = 7,

			[PublicAPI]
			NotificationCommunicationInstant = 8,

			[PublicAPI]
			NotificationCommunicationDelayed = 9,

			[PublicAPI]
			NotificationEvent = 10,

			[PublicAPI]
			AssistanceAccessibility = 11,

			[PublicAPI]
			AssistanceNavigationGuidance = 12,

			[PublicAPI]
			AssistanceSonification = 13,

			[PublicAPI]
			Game = 14,

			[PublicAPI]
			VirtualSource = 15,

			[PublicAPI]
			Assistant = 16
		}

		/// <summary>
		/// Options for the combination of flags
		/// </summary>
		[PublicAPI]
		[Flags]
		public enum Flags
		{
			[PublicAPI]
			FlagAudibilityEnforced = 0x1 << 0,

			[PublicAPI]
			FlagSecure = 0x1 << 1,

			[PublicAPI]
			FlagSco = 0x1 << 2,

			[PublicAPI]
			FlagBeacon = 0x1 << 3,

			[PublicAPI]
			FlagHwAvSync = 0x1 << 4,

			[PublicAPI]
			FlagHwHotWord = 0x1 << 5,

			[PublicAPI]
			FlagBypassInterruptionPolicy = 0x1 << 6,

			[PublicAPI]
			FlagBypassMute = 0x1 << 7,

			[PublicAPI]
			FlagLowLatency = 0x1 << 8,

			[PublicAPI]
			FlagDeepBuffer = 0x1 << 9,

			[PublicAPI]
			FlagAll = FlagAudibilityEnforced | FlagSecure | FlagSco | FlagBeacon | FlagHwAvSync | FlagHwHotWord |
			          FlagBypassInterruptionPolicy | FlagBypassMute | FlagLowLatency | FlagDeepBuffer,

			[PublicAPI]
			FlagAllPublic = FlagAudibilityEnforced | FlagHwAvSync | FlagLowLatency
		}

		int _usage;
		int _contentType;
		int _flags;

		public AndroidJavaObject AJO { get; set; }

		[PublicAPI]
		public AudioAttributes(AndroidJavaObject ajo)
		{
			AJO = ajo;
		}

		AudioAttributes(Usage usage, ContentType contentType, Flags flags)
		{
			if (AGUtils.IsNotAndroid())
			{
				Debug.LogError("This class should be used only on Android, errors ahead");
				return;
			}
			
			if(!Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.LOLLIPOP))
			{
				Debug.LogError("This class should be used on API level 21 and higher, errors ahead");
				return;
			}
			
			AJO = new AndroidJavaObject(C.AndroidMediaAudioAttributesBuilder)
				.CallAJO("setUsage", (int) usage)
				.CallAJO("setContentType", (int) contentType)
				.CallAJO("setFlags", (int) flags)
				.CallAJO("build");
		}
	}
}
#endif