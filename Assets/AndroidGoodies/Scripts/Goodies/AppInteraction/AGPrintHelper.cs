#if UNITY_ANDROID

namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	/// Class for printing images and HTML pages.
	/// </summary>
	[PublicAPI]
	public static class AGPrintHelper
	{
		/// <summary>
		/// Color modes of printing
		/// </summary>
		[PublicAPI]
		public enum ColorModes
		{
			Monochrome = 1,
			Color = 2
		}

		/// <summary>
		/// Sheet orientations of printing
		/// </summary>
		[PublicAPI]
		public enum Orientations
		{
			Landscape = 1,
			Portrait = 2
		}

		/// <summary>
		/// Scale modes of printing
		/// </summary>
		[PublicAPI]
		public enum ScaleModes
		{
			Fit = 1,
			Fill = 2
		}

		static AndroidJavaObject _printHelper;
		static Action _onPrintSuccess;

		/// <summary>
		/// Gets whether the system supports printing.
		/// </summary>
		public static bool SystemSupportsPrint
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return false;
				}

				return new AndroidJavaClass(C.AndroidPrintHelper).CallStaticBool("systemSupportsPrint");
			}
		}

		/// <summary>
		/// Prints an image.
		/// </summary>
		/// <param name="image">Image to be printed</param>
		/// <param name="jobName">Name of the printing job</param>
		/// <param name="onFinishCallback">
		/// Action to perform after printing is finished.
		/// Is performed even if the printing was cancelled.
		/// </param>
		/// <param name="colorMode">Color mode of printing</param>
		/// <param name="orientation">Sheet orientation of printing</param>
		/// <param name="scaleMode">Scale mode of printing</param>
		/// <exception cref="ArgumentNullException"></exception>
		public static void PrintImage([NotNull] Texture2D image, [NotNull] string jobName, Action onFinishCallback = null,
			ColorModes colorMode = ColorModes.Color, Orientations orientation = Orientations.Portrait, ScaleModes scaleMode = ScaleModes.Fit)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (image == null)
			{
				throw new ArgumentNullException("image");
			}

			if (jobName == null)
			{
				throw new ArgumentNullException("jobName");
			}

			_printHelper = new AndroidJavaObject(C.AndroidPrintHelper, AGUtils.Activity);
			_printHelper.Call("setColorMode", (int) colorMode);
			_printHelper.Call("setOrientation", (int) orientation);
			_printHelper.Call("setScaleMode", (int) scaleMode);

			_onPrintSuccess = onFinishCallback;

			var helperClass = new AndroidJavaClass(C.PrintHelperUtilsClass);
			helperClass.CallStatic("printBitmap", jobName, AGUtils.Texture2DToAndroidBitmap(image), _printHelper);
		}

		/// <summary>
		/// Prints an HTML page from the source.
		/// </summary>
		/// <param name="htmlText">Text in HTML format.</param>
		/// <param name="jobName">The name of the print job.</param>
		public static void PrintHtmlPage([NotNull] string htmlText, [NotNull] string jobName)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (htmlText == null)
			{
				throw new ArgumentNullException("htmlText");
			}

			if (jobName == null)
			{
				throw new ArgumentNullException("jobName");
			}

			var webView = C.PrintHelperUtilsClass.AJCCallStaticOnceAJO("createWebView", AGUtils.Activity, jobName);
			webView.Call("loadDataWithBaseURL", string.Empty, htmlText, "text/HTML", "UTF-8", string.Empty);
		}

		/// <summary>
		/// Prints an HTML page from the URL.
		/// </summary>
		/// <param name="htmlUrl">URL of the HTML page.</param>
		/// <param name="jobName">The name of the print job.</param>
		public static void PrintHtmlPageFromUrl([NotNull] string htmlUrl, [NotNull] string jobName)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (htmlUrl == null)
			{
				throw new ArgumentNullException("htmlUrl");
			}

			if (jobName == null)
			{
				throw new ArgumentNullException("jobName");
			}

			var webView = C.PrintHelperUtilsClass.AJCCallStaticOnceAJO("createWebView", AGUtils.Activity, jobName);
			webView.Call("loadUrl", htmlUrl);
		}

		public static void OnPrintSuccess()
		{
			if (_onPrintSuccess != null)
			{
				_onPrintSuccess();
			}
		}
	}
}
#endif