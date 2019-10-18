#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using System.IO;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	[PublicAPI]
	public static class AGWallpaperManager
	{
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.N)]
		public enum WallpaperType
		{
			Unspecified = 0,
			System = 1,
			Lock = 2
		}

		const string ACTION_LIVE_WALLPAPER_CHOOSER = "android.service.wallpaper.LIVE_WALLPAPER_CHOOSER";

		const string SetBitmapMethodName = "setBitmap";

		static AndroidJavaObject WallpaperManager
		{
			get { return C.AndroidAppWallpaperManager.AJCCallStaticOnceAJO("getInstance", AGUtils.Activity); }
		}

		/// <summary>
		/// Remove any currently set system wallpaper, reverting to the system's built-in wallpaper. On success, the intent ACTION_WALLPAPER_CHANGED is broadcast.
		/// </summary>
		public static void Clear()
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			WallpaperManager.Call("clear");
		}

		/// <summary>
		/// Returns whether wallpapers are supported for the calling user.
		/// If this function returns <code>false</code>, any attempts to changing the wallpaper will have no effect,
		/// and any attempt to obtain of the wallpaper will return <code>null</code>.
		/// </summary>
		/// <returns>Whether wallpapers are supported for the calling user.</returns>
		public static bool IsWallpaperSupported()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			if (AGDeviceInfo.SDK_INT >= AGDeviceInfo.VersionCodes.M)
			{
				return WallpaperManager.CallBool("isWallpaperSupported");
			}

			return true;
		}

		/// <summary>
		/// Returns whether the calling package is allowed to set the wallpaper for the calling user.
		/// If this function returns <code>false</code>, any attempts to change the wallpaper will have no effect. 
		/// Always returns true for device owner and profile owner.
		/// </summary>
		/// <returns>Whether the calling package is allowed to set the wallpaper.</returns>
		public static bool IsSetWallpaperAllowed()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			if (Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
			{
				return WallpaperManager.CallBool("isSetWallpaperAllowed");
			}

			return true;
		}

		/// <summary>
		/// Sets provided texture as wallpaper
		/// </summary>
		/// <param name="wallpaperTexture"> A texture, that will supply the wallpaper imagery.</param>
		/// <param name="visibleCropHint">
		/// The rectangular subregion of fullImage that should be displayed as wallpaper. Passing null for this parameter means
		/// that the full image should be displayed if possible given the image's and device's aspect ratios, etc.
		/// </param>
		/// <param name="allowBackup">
		/// True if the OS is permitted to back up this wallpaper image for restore to a future device; false otherwise.
		/// </param>
		/// <param name="which">
		/// Which wallpaper to configure with the new imagery.
		/// </param>
		public static void SetWallpaper([NotNull] Texture2D wallpaperTexture,
			AndroidRect visibleCropHint = null, bool allowBackup = true, WallpaperType which = WallpaperType.Unspecified)
		{
			if (wallpaperTexture == null)
			{
				throw new ArgumentNullException("wallpaperTexture");
			}

			if (AGUtils.IsNotAndroid())
			{
				return;
			}
			
			var wallpaperPath = AndroidPersistanceUtilsInternal.SaveWallpaperImageToExternalStorage(wallpaperTexture);
			
			if (Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
			{
				SetWallpaperBitmap(AGUtils.DecodeBitmap(wallpaperPath), visibleCropHint, allowBackup, which);
				return;
			}
			
			SetWallpaperBitmap(AGUtils.DecodeBitmap(wallpaperPath));
		}

		/// <summary>
		/// Sets provided image on the provided filepath as wallpaper. File MUST exist.
		/// </summary>
		/// <param name="imagePath"> A path to the image file </param>
		/// <param name="visibleCropHint">
		/// The rectangular subregion of fullImage that should be displayed as wallpaper. Passing null for this parameter means
		/// that the full image should be displayed if possible given the image's and device's aspect ratios, etc.
		/// </param>
		/// <param name="allowBackup">
		/// True if the OS is permitted to back up this wallpaper image for restore to a future device; false otherwise.
		/// </param>
		/// <param name="which">
		/// Which wallpaper to configure with the new imagery.
		/// </param>
		public static void SetWallpaper([NotNull] string imagePath, 
			AndroidRect visibleCropHint = null, bool allowBackup = true, WallpaperType which = WallpaperType.Unspecified)
		{
			if (imagePath == null)
			{
				throw new ArgumentNullException("imagePath");
			}

			if (AGUtils.IsNotAndroid())
			{
				return;
			}
			
			CheckIfFileExists(imagePath);
			
			SetWallpaperBitmap(AGUtils.DecodeBitmap(imagePath), visibleCropHint, allowBackup, which);
		}

		static void SetWallpaperBitmap(AndroidJavaObject bitmapAjo, AndroidRect visibleCropHint = null, 
			bool allowBackup = true, WallpaperType which = WallpaperType.Unspecified)
		{
			if (Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
			{
				WallpaperManager.CallInt(SetBitmapMethodName, bitmapAjo,
					visibleCropHint != null ? visibleCropHint.ajo : null, allowBackup, (int) which);
			}
			else
			{
				WallpaperManager.Call(SetBitmapMethodName, bitmapAjo);
			}
		}

		/// <summary>
		/// <remarks>
		/// WARNING: This method works on my devices but always crashes on emulators and I can't find a way to fix it.
		/// It may be something with emulator but use this method with care and test carefully before using it.
		/// </remarks>
		///  Sets provided texture as wallpaper allowing user to crop beforehand
		/// </summary>
		/// <param name="wallpaperTexture"> A texture, that will supply the wallpaper imagery.</param>
		/// <param name="visibleCropHint">
		/// The rectangular subregion of fullImage that should be displayed as wallpaper. Passing null for this parameter means
		/// that the full image should be displayed if possible given the image's and device's aspect ratios, etc.
		/// </param>
		/// <param name="allowBackup">
		/// True if the OS is permitted to back up this wallpaper image for restore to a future device; false otherwise.
		/// </param>
		/// <param name="which">
		/// Which wallpaper to configure with the new imagery.
		/// </param>
		public static void ShowCropAndSetWallpaperChooser([NotNull] Texture2D wallpaperTexture, 
			AndroidRect visibleCropHint = null, bool allowBackup = true, WallpaperType which = WallpaperType.Unspecified)
		{
			if (wallpaperTexture == null)
			{
				throw new ArgumentNullException("wallpaperTexture");
			}

			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (AGDeviceInfo.SDK_INT < AGDeviceInfo.VersionCodes.KITKAT)
			{
				return;
			}

			var uri = AndroidPersistanceUtilsInternal.SaveWallpaperImageToExternalStorageUri(wallpaperTexture);
			StartCropAndSetWallpaperActivity(uri, visibleCropHint, allowBackup, which);
		}

		/// <summary>
		///  <remarks>
		/// WARNING: This method works on my devices but always crashes on emulators and I can't find a way to fix it.
		/// It may be something with emulator but use this method with care and test carefully before using it.
		/// </remarks>
		///  Sets provided image on the provided filepath as wallpaper allowing user to crop beforehand. File MUST exist.
		/// </summary>
		/// <param name="imagePath"> A path to the image file </param>
		/// <param name="visibleCropHint">
		/// The rectangular subregion of fullImage that should be displayed as wallpaper. Passing null for this parameter means
		/// that the full image should be displayed if possible given the image's and device's aspect ratios, etc.
		/// </param>
		/// <param name="allowBackup">
		/// True if the OS is permitted to back up this wallpaper image for restore to a future device; false otherwise.
		/// </param>
		/// <param name="which">
		/// Which wallpaper to configure with the new imagery.
		/// </param>
		public static void ShowCropAndSetWallpaperChooser([NotNull] string imagePath, 
			AndroidRect visibleCropHint = null, bool allowBackup = true, WallpaperType which = WallpaperType.Unspecified)
		{
			if (imagePath == null)
			{
				throw new ArgumentNullException("imagePath");
			}

			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (AGDeviceInfo.SDK_INT < AGDeviceInfo.VersionCodes.KITKAT)
			{
				return;
			}

			CheckIfFileExists(imagePath);
			var uri = AndroidPersistanceUtilsInternal.GetUriFromFilePath(imagePath);
			StartCropAndSetWallpaperActivity(uri, visibleCropHint, allowBackup, which);
		}

		static void StartCropAndSetWallpaperActivity(AndroidJavaObject uri, AndroidRect visibleCropHint, bool allowBackup, WallpaperType which)
		{
			try
			{
				var intent = WallpaperManager.CallAJO("getCropAndSetWallpaperIntent", uri);
				AGUtils.StartActivity(intent);
			}
			catch (Exception)
			{
				Debug.Log("Setting wallpaper with crop failed, falling back to setting just image");
				var bitmapAjo = C.AndroidProviderMediaStoreImagesMedia.AJCCallStaticOnceAJO("getBitmap", AGUtils.ContentResolver, uri);
				SetWallpaperBitmap(bitmapAjo, visibleCropHint, allowBackup, which);
			}
		}

		/// <summary>
		/// Launch an activity for the user to pick the current global live wallpaper.
		/// </summary>
		public static void ShowLiveWallpaperChooser()
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			var intent = new AndroidIntent(ACTION_LIVE_WALLPAPER_CHOOSER);
			AGUtils.StartActivity(intent.AJO);
		}

		static void CheckIfFileExists(string imagePath)
		{
			if (!File.Exists(imagePath))
			{
				Debug.LogError("File doesn't exist: " + imagePath);
			}
		}
	}
}
#endif