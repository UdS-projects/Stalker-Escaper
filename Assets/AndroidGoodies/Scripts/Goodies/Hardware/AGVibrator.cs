// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGVibrator.cs
//


#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using JetBrains.Annotations;

	/// <summary>
	/// Class to deal with vibrator functionality, for android API see https://developer.android.com/reference/android/os/Vibrator
	/// </summary>
	[PublicAPI]
	public static class AGVibrator
	{
		/// <summary>
		/// Check whether the hardware has a vibrator.
		/// </summary>
		/// <returns><c>true</c> if the hardware has a vibrator; otherwise, <c>false</c>.</returns>
		[PublicAPI]
		public static bool HasVibrator()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}
			
			try
			{
				return AGSystemService.VibratorService.Call<bool>("hasVibrator");
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Check whether the vibrator has amplitude control.
		/// </summary>
		/// ///
		/// <value>
		///     <c>true</c> if the vibrator has amplitude control, otherwise, <c>false</c>.
		/// </value>
		[PublicAPI]
		public static bool HasAmplitudeControl
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return false;
				}

				try
				{
					return AGSystemService.VibratorService.Call<bool>("hasAmplitudeControl");
				}
				catch (Exception)
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Vibrate constantly for the specified period of time.
		/// 
		/// You must specify <uses-permission android:name="android.permission.VIBRATE"/> permission in order for this method to work.
		/// </summary>
		/// <param name="durationInMillisec">Vibration duration in millisec.</param>
		[PublicAPI]
		public static void Vibrate(long durationInMillisec)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			AGSystemService.VibratorService.Call("vibrate", durationInMillisec);
		}

		const int REPEAT = -1;
		// Do not repeat for now

		/// <summary>
		/// Vibrate with a given pattern.
		/// </summary>
		/// <param name="pattern">
		/// Pass in an array of ints that are the durations for which to turn on or off the vibrator in milliseconds. 
		/// The first value indicates the number of milliseconds to wait before turning the vibrator on. 
		/// The next value indicates the number of milliseconds for which to keep the vibrator on before turning it off. 
		/// Subsequent values alternate between durations in milliseconds to turn the vibrator off or to turn the vibrator on.
		/// </param>
		[PublicAPI]
		public static void VibratePattern(long[] pattern)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			AGSystemService.VibratorService.Call("vibrate", pattern, REPEAT);
		}

		/// <summary>
		/// Check if device's OS supports new vibration effects
		/// </summary>
		/// <value>
		///     <c>true</c> if the device's operating system is Android 8.0(O) or higher, otherwise, <c>false</c>.
		/// </value>
		public static bool AreVibrationEffectsSupported
		{
			get { return AGDeviceInfo.SDK_INT >= AGDeviceInfo.VersionCodes.O; }
		}

		/// <summary>
		/// Vibrates with custom vibration effect
		/// </summary>
		/// <param name="vibe">
		/// A VibrationEffect describes a haptic effect to be performed by a Vibrator.
		/// These effects may be any number of things, from single shot vibrations to complex waveforms.
		/// </param>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static void Vibrate(VibrationEffect vibe)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (!AreVibrationEffectsSupported)
			{
				return;
			}
			
			AGSystemService.VibratorService.Call("vibrate", vibe.AJO);
		}

		/// <summary>
		/// Vibrates with custom vibration effect and preset audio attributes
		/// </summary>
		/// <param name="vibe">
		/// A VibrationEffect describes a haptic effect to be performed by a Vibrator.
		/// These effects may be any number of things, from single shot vibrations to complex waveforms.
		///</param>
		/// <param name="attributes">
		/// AudioAttributes supersede the notion of stream types for defining the behavior of audio playback.
		/// Attributes allow an application to specify more information than is conveyed in a stream type
		/// by allowing the application to define usage, content type and flags.
		/// </param>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public static void Vibrate(VibrationEffect vibe, AudioAttributes attributes)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}
			
			if (!AreVibrationEffectsSupported)
			{
				return;
			}
			
			AGSystemService.VibratorService.Call("vibrate", vibe.AJO, attributes.AJO);
		}
		
		/// <summary>
		/// Cancels current vibration
		/// </summary>
		[PublicAPI]
		public static void Cancel()
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}
			
			AGSystemService.VibratorService.Call("cancel");
		}
	}
}

#endif