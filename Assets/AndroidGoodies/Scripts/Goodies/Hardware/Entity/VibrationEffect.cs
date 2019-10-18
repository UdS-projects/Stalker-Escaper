#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	/// A VibrationEffect describes a haptic effect to be performed by a Vibrator.
	/// These effects may be any number of things, from single shot vibrations to complex waveforms.
	/// </summary>
	[PublicAPI]
	public class VibrationEffect
	{
		/// <summary>
		/// The default vibration strength of the device.
		/// </summary>
		[PublicAPI]
		public const int DEFAULT_AMPLITUDE = -1;

		VibrationEffect(AndroidJavaObject ajo)
		{
			AJO = ajo;
		}

		public AndroidJavaObject AJO { get; set; }

		/// <summary>
		/// Create a one shot vibration. One shot vibrations will vibrate constantly
		/// for the specified period of time at the specified amplitude, and then stop.
		/// </summary>
		/// <param name="milliseconds">
		/// The number of milliseconds to vibrate. This must be a positive number.
		/// </param>
		/// <param name="amplitude">
		/// The strength of the vibration. This must be a value between 1 and 255,
		/// or DEFAULT_AMPLITUDE.
		/// </param>
		[PublicAPI]
		public static VibrationEffect CreateOneShot(long milliseconds, int amplitude)
		{			
			var ajo = C.AndroidOsVibrationEffect.AJCCallStaticOnceAJO("createOneShot", milliseconds, amplitude);
			return new VibrationEffect(ajo);
		}

		/// <summary>
		/// Create a waveform vibration. Waveform vibrations are a potentially repeating series of timing
		/// and amplitude pairs. For each pair, the value in the amplitude array determines the strength
		/// of the vibration and the value in the timing array determines how long it vibrates for.
		/// An amplitude of 0 implies no vibration (i.e. off), and any pairs with a timing value of 0 will be ignored.
		/// 
		/// The amplitude array of the generated waveform will be the same size as the given timing array with
		/// alternating values of 0 (i.e. off) and DEFAULT_AMPLITUDE, starting with 0. Therefore the first timing
		/// value will be the period to wait before turning the vibrator on, the second value will be how long
		/// to vibrate at DEFAULT_AMPLITUDE strength, etc.
		///
		/// To cause the pattern to repeat, pass the index into the timings array
		/// at which to start the repetition, or -1 to disable repeating. 
		/// </summary>
		/// <param name="timings">
		/// The timing values of the timing / amplitude pairs. Timing values of 0 will
		/// cause the pair to be ignored.
		/// </param>
		/// <param name="amplitudes">
		/// The amplitude values of the timing / amplitude pairs. Amplitude values
		/// must be between 0 and 255, or equal to DEFAULT_AMPLITUDE. An amplitude value of 0 implies
		/// the motor is off.
		/// </param>
		/// <param name="repeat">The index into the timings array at which to repeat,
		/// or -1 if you don't want to repeat.</param>
		[PublicAPI]
		public static VibrationEffect CreateWaveForm(long[] timings, int[] amplitudes, int repeat)
		{
			var ajo = C.AndroidOsVibrationEffect.AJCCallStaticOnceAJO("createWaveform", timings, amplitudes, repeat);
			return new VibrationEffect(ajo);
		}

		/// <summary>
		/// Create a waveform vibration. Waveform vibrations are a potentially repeating series of
		/// timing and amplitude pairs. For each pair, the value in the amplitude array determines
		/// the strength of the vibration and the value in the timing array determines how long
		/// it vibrates for.
		/// 
		/// To cause the pattern to repeat, pass the index into the timings array at which to start
		/// the repetition, or -1 to disable repeating.
		/// </summary>
		/// <param name="timings">
		/// The pattern of alternating on-off timings, starting with off.
		/// Timing values of 0 will cause the timing / amplitude pair to be ignored.
		/// </param>
		/// <param name="repeat">
		/// The index into the timings array at which to repeat,
		/// or -1 if you you don't want to repeat.
		/// </param>
		[PublicAPI]
		public static VibrationEffect CreateWaveForm(long[] timings, int repeat)
		{
			var ajo = C.AndroidOsVibrationEffect.AJCCallStaticOnceAJO("createWaveform", timings, repeat);
			return new VibrationEffect(ajo);
		}
	}
}
#endif