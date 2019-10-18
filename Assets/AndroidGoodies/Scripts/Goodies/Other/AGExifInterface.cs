#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using UnityEngine;
	using Internal;
	using JetBrains.Annotations;
	using System.Globalization;

	/// <summary>
	/// This is a class for reading and writing Exif tags in a JPEG file or a RAW image file.
	/// Supported formats are: JPEG, DNG, CR2, NEF, NRW, ARW, RW2, ORF, PEF, SRW, RAF and HEIF.
	/// Attribute mutation is supported for JPEG image files.
	/// </summary>
	[PublicAPI]
	public class AGExifInterface
	{
		/// <summary>
		/// Possible orientations
		/// </summary>
		[PublicAPI]
		public enum Orientations
		{
			FlipHorizontal = 2,
			FlipVertical = 4,
			Normal = 1,
			Rotate180 = 3,
			Rotate270 = 8,
			Rotate90 = 6,
			Transpose = 5,
			Transverse = 7,
			Undefined = 0
		}

		/// <summary>
		/// White balance setting options
		/// </summary>
		[PublicAPI]
		public enum WhiteBalances
		{
			Auto = 0,
			Manual = 1
		}
		
		[PublicAPI]
		public enum ColorSpaces
		{
			SRgb = 1,
			Uncalibrated = 65535
		}

		[PublicAPI]
		public enum CompressionTypes
		{
			DeflateZip = 8,
			HuffmanCompressed = 2,
			Jpeg = 6,
			JpegCompressed = 7,
			LossyJpeg = 34892,
			PackBitsCompressed = 32773,
			Uncompressed = 1
		}

		[PublicAPI]
		public enum ExposureModes
		{
			Auto = 0,
			AutoBracket = 2,
			Manual = 1
		}

		[PublicAPI]
		public enum ExposureProgrammes
		{
			Action = 6,
			AperturePriority = 3,
			Creative = 5,
			LandscapeMode = 8,
			Manual = 1,
			Normal = 2,
			NotDefined = 0,
			PortraitMode = 7,
			ShutterPriority = 4
		}

		[PublicAPI]
		public enum GainControlType
		{
			HighGainDown = 4,
			HighGainUp = 2,
			LowGainDown = 3,
			LowGainUp = 1,
			None = 0
		}
		
		[PublicAPI]
		public enum LightSourceType
		{
			CloudyWater = 10,
			CoolWhiteFluorescent = 14,
			D50 = 23,
			D55 = 20,
			D65 = 21,
			D75 = 22,
			Daylight = 1,
			DaylightFluorescent = 12,
			DayWhiteFluorescent = 12,
			FineWeather = 9,
			Flash = 4,
			Fluorescent = 2,
			IsoStudioTungsten = 24,
			Other = 255,
			Shade = 11,
			StandardLightA = 17,
			StandardLightB = 18,
			StandardLightC = 19,
			Tungsten = 3,
			Unknown = 0,
			WarmWhiteFluorescent = 16,
			WhiteFluorescent = 15
		}

		/// <summary>
		/// The constants used by AltitudeRef to denote the altitude relativity to sea level.
		/// </summary>
		[PublicAPI]
		public enum AltitudesRef
		{
			AboveSeaLevel = 0,
			BelowSeaLevel = 1
		}
		
		/// <summary>
		/// The constants used by AltitudeRef to denote the altitude relativity to sea level.
		/// </summary>
		[PublicAPI]
		public enum Contrasts
		{
			Hard = 2,
			Normal = 0,
			Soft = 1
		}

		[PublicAPI]
		public enum MeteringModes
		{
			Average = 1,
			CenterWeightAverage = 2,
			MultiSpot = 4,
			Other = 255,
			Partial = 6,
			Pattern = 5,
			Spot = 3,
			Unknown = 0
		}

		[PublicAPI]
		public enum PhotometricInterpretations
		{
			BlackIsZero = 1,
			Rgb = 2,
			WhiteIsZero = 0,
			Ycbcr = 6
		}

		[PublicAPI]
		public enum SceneCaptureTypes
		{
			Landscape = 1,
			Night = 3,
			Portrait = 2,
			Standard = 0
		}

		[PublicAPI]
		public enum SensorType
		{
			ColorSequential = 5,
			ColorSequentialLinear = 8,
			NotDefined = 1,
			OneChip = 2,
			ThreeChip = 4,
			TriLinear = 7,
			TwoChip = 3
		}

		[PublicAPI]
		public enum SubjectDistanceRangeType
		{
			CloseView = 2,
			DistantView = 3,
			Macro = 1,
			Unknown = 0
		}

		public const string TagAperture = "FNumber";
		public const string TagApertureValue = "ApertureValue";
		public const string TagArtist = "Artist";
		public const string TagBitsPerSample = "BitsPerSample";
		public const string TagBrightnessValue = "BrightnessValue";
		public const string TagCfaPattern = "CFAPattern";
		public const string TagColorSpace = "ColorSpace";
		public const string TagComponentsConfiguration = "ComponentsConfiguration";
		public const string TagCompressedBitsPerPixel = "CompressedBitsPerPixel";
		public const string TagCompression = "Compression";
		public const string TagContrast = "Contrast";
		public const string TagCopyright = "Copyright";
		public const string TagCustomRendered = "CustomRendered";
		public const string TagDateTime = "DateTime";
		public const string TagDateTimeDigitized = "DateTimeDigitized";
		public const string TagDateTimeOriginal = "DateTimeOriginal";
		public const string TagDefaultCropSize = "DefaultCropSize";
		public const string TagDeviceSettingDescription = "DeviceSettingDescription";
		public const string TagDigitalZoomRatio = "DigitalZoomRatio";
		public const string TagDngVersion = "DNGVersion";
		public const string TagExifVersion = "ExifVersion";
		public const string TagExposureBiasValue = "ExposureBiasValue";
		public const string TagExposureIndex = "ExposureIndex";
		public const string TagExposureMode = "ExposureMode";
		public const string TagExposureProgram = "ExposureProgram";
		public const string TagExposureTime = "ExposureTime";
		public const string TagFileSource = "FileSource";
		public const string TagFlash = "Flash";
		public const string TagFlashPixVersion = "FlashpixVersion";
		public const string TagFlashEnergy = "FlashEnergy";
		public const string TagFocalLength = "FocalLength";
		public const string TagFocalLengthIn35MmFilm = "FocalLengthIn35mmFilm";
		public const string TagFocalPlaneResolutionUnit = "FocalPlaneResolutionUnit";
		public const string TagFocalPlaneXResolution = "FocalPlaneXResolution";
		public const string TagFocalPlaneYResolution = "FocalPlaneYResolution";
		public const string TagGainControl = "GainControl";
		public const string TagGpsAltitude = "GPSAltitude";
		public const string TagGpsAltitudeRef = "GPSAltitudeRef";
		public const string TagGpsAreaInformation = "GPSAreaInformation";
		public const string TagGpsDateStamp = "GPSDateStamp";
		public const string TagGpsDestinationBearing = "GPSDestBearing";
		public const string TagGpsDestinationBearingRef = "GPSDestBearingRef";
		public const string TagGpsDestinationDistance = "GPSDestDistance";
		public const string TagGpsDestinationDistanceRef = "GPSDestDistanceRef";
		public const string TagGpsDestinationLatitude = "GPSDestLatitude";
		public const string TagGpsDestinationLatitudeRef = "GPSDestLatitudeRef";
		public const string TagGpsDestinationLongitude = "GPSDestLongitude";
		public const string TagGpsDestinationLongitudeRef = "GPSDestLongitudeRef";
		public const string TagGpsDifferential = "GPSDifferential";
		public const string TagGpsDilutionOfPrecision = "GPSDOP";
		public const string TagGpsImgDirection = "GPSImgDirection";
		public const string TagGpsImgDirectionRef = "GPSImgDirectionRef";
		public const string TagGpsLatitude = "GPSLatitude";
		public const string TagGpsLatitudeRef = "GPSLatitudeRef";
		public const string TagGpsLongitude = "GPSLongitude";
		public const string TagGpsLongitudeRef = "GPSLongitudeRef";
		public const string TagGpsMapDatum = "GPSMapDatum";
		public const string TagGpsMeasureMode = "GPSMeasureMode";
		public const string TagGpsProcessingMethod = "GPSProcessingMethod";
		public const string TagGpsSatellites = "GPSSatellites";
		public const string TagGpsSpeed = "GPSSpeed";
		public const string TagGpsSpeedRef = "GPSSpeedRef";
		public const string TagGpsStatus = "GPSStatus";
		public const string TagGpsTimeStamp = "GPSTimeStamp";
		public const string TagGpsTrack = "GPSTrack";
		public const string TagGpsTrackRef = "GPSTrackRef";
		public const string TagGpsVersionId = "GPSVersionID";
		public const string TagImageDescription = "ImageDescription";
		public const string TagImageLength = "ImageLength";
		public const string TagImageUniqueId = "ImageUniqueID";
		public const string TagImageWidth = "ImageWidth";
		public const string TagInteroperabilityIndex = "InteroperabilityIndex";
		public const string TagIsoSpeedRatings = "ISOSpeedRatings";
		public const string TagJpegInterchangeFormat = "JPEGInterchangeFormat";
		public const string TagJpegInterchangeFormatLength = "JPEGInterchangeFormatLength";
		public const string TagLightSource = "LightSource";
		public const string TagMake = "Make";
		public const string TagMakerNote = "MakerNote";
		public const string TagMaxApertureValue = "MaxApertureValue";
		public const string TagMeteringMode = "MeteringMode";
		public const string TagModel = "Model";
		public const string TagNewSubFileType = "NewSubfileType";
		public const string TagOecf = "OECF";
		public const string TagOrfAspectFrame = "AspectFrame";
		public const string TagOrfPreviewImageLength = "PreviewImageLength";
		public const string TagOrfPreviewImageStart = "PreviewImageStart";
		public const string TagOrfThumbnailImage = "ThumbnailImage";
		public const string TagOrientation = "Orientation";
		public const string TagPhotometricInterpretation = "PhotometricInterpretation";
		public const string TagPixelXDimension = "PixelXDimension";
		public const string TagPixelYDimension = "PixelYDimension";
		public const string TagPlanarConfiguration = "PlanarConfiguration";
		public const string TagPrimaryChromaticities = "PrimaryChromaticities";
		public const string TagReferenceBlackWhite = "ReferenceBlackWhite";
		public const string TagRelatedSoundFile = "RelatedSoundFile";
		public const string TagResolutionUnit = "ResolutionUnit";
		public const string TagRowsPerStrip = "RowsPerStrip";
		public const string TagRw2Iso = "ISO";
		public const string TagRw2JpgFromRaw = "JpgFromRaw";
		public const string TagRw2SensorBottomBorder = "SensorBottomBorder";
		public const string TagRw2SensorLeftBorder = "SensorLeftBorder";
		public const string TagRw2SensorRightBorder = "SensorRightBorder";
		public const string TagRw2SensorTopBorder = "SensorTopBorder";
		public const string TagSamplesPerPixel = "SamplesPerPixel";
		public const string TagSaturation = "Saturation";
		public const string TagSceneCaptureType = "SceneCaptureType";
		public const string TagSceneType = "SceneType";
		public const string TagSensingMethod = "SensingMethod";
		public const string TagSharpness = "Sharpness";
		public const string TagShutterSpeedValue = "ShutterSpeedValue";
		public const string TagSoftware = "Software";
		public const string TagSpatialFrequencyResponse = "SpatialFrequencyResponse";
		public const string TagSpectralSensitivity = "SpectralSensitivity";
		public const string TagStripByteCounts = "StripByteCounts";
		public const string TagStripOffsets = "StripOffsets";
		public const string TagSubFileType = "SubfileType";
		public const string TagSubjectArea = "SubjectArea";
		public const string TagSubjectDistance = "SubjectDistance";
		public const string TagSubjectDistanceRange = "SubjectDistanceRange";
		public const string TagSubjectLocation = "SubjectLocation";
		public const string TagSubSecTime = "SubSecTime";
		public const string TagSubSecTimeDigitized = "SubSecTimeDigitized";
		public const string TagSubSecTimeOriginal = "SubSecTimeOriginal";
		public const string TagThumbnailImageLength = "ThumbnailImageLength";
		public const string TagThumbnailImageWidth = "ThumbnailImageWidth";
		public const string TagTransferFunction = "TransferFunction";
		public const string TagUserComment = "UserComment";
		public const string TagWhiteBalance = "WhiteBalance";
		public const string TagWhitePoint = "WhitePoint";
		public const string TagXResolution = "XResolution";
		public const string TagYCbCrCoefficients = "YCbCrCoefficients";
		public const string TagYCbCrPositioning = "YCbCrPositioning";
		public const string TagYCbCrSubSampling = "YCbCrSubSampling";
		public const string TagYResolution = "YResolution";

		public AndroidJavaObject ajo;

		public AGExifInterface(string path)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}
			
			ajo = new AndroidJavaObject(C.AndroidMediaExifInterface, path);
		}

		/// <summary>
		/// The altitude in meters. If the exif tag does not exist, return double.MinValue
		/// </summary>
		[PublicAPI]
		public double Altitude
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return 0;
				}
				
				return ajo.Call<double>("getAltitude", double.MinValue);
			}
		}

		/// <summary>
		/// Returns the JPEG compressed thumbnail inside the image file, or null if there is no JPEG compressed thumbnail.
		/// </summary>
		[PublicAPI]
		public byte[] Thumbnail
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return new byte[]{};
				}
				
				return ajo.Call<byte[]>("getThumbnail");
			}
		}

		/// <summary>
		/// Returns the thumbnail bytes inside the image file, regardless of the compression type of the thumbnail image.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public byte[] ThumbnailBytes
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.O))
				{
					return new byte[]{};
				}

				return ajo.Call<byte[]>("getThumbnailBytes");
			}
		}

		/// <summary>
		/// Returns the offset and length of thumbnail inside the image file.
		/// Two-element array, the offset in the first value, and length in the second, or null if no thumbnail was found.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.N)]
		public long[] ThumbnailRange
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return new long[]{};
				}

				return ajo.Call<long[]>("getThumbnailRange");
			}
		}

		/// <summary>
		/// Returns true if the image file has a thumbnail.
		/// </summary>
		[PublicAPI]
		public bool HasThumbnail
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return false;
				}

				return ajo.CallBool("hasThumbnail");
			}
		}

		/// <summary>
		/// Returns true if thumbnail image is JPEG Compressed, or false if either thumbnail image does not exist or thumbnail image is uncompressed.
		/// </summary>
		[PublicAPI]
		[AndroidApi(AGDeviceInfo.VersionCodes.O)]
		public bool IsThumbnailCompressed
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.O))
				{
					return false;
				}

				return ajo.CallBool("isThumbnailCompressed");
			}
		}

		[PublicAPI]
		public double Aperture
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return double.MinValue;
				}
				
				return GetDoubleAttribute(TagAperture);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagAperture, value.ToString(CultureInfo.InvariantCulture));
			}
		}
		
		/// <summary>
		/// The lens aperture. Should be formatted like f/D, where f is focal length value, D is aperture diameter.
		/// </summary>
		[PublicAPI]
		public string ApertureValue
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagApertureValue);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagApertureValue, value.ToString(CultureInfo.InvariantCulture));
			}
		}
		
		[PublicAPI]
		public string Artist
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagArtist);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagArtist, value);
			}
		}
		
		[PublicAPI]
		public int BitsPerSample
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagBitsPerSample);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagBitsPerSample, value.ToString());
			}
		}
		
		/// <summary>
		/// The value of brightness. The unit is the APEX value. Ordinarily it is given in the range of -99.99 to 99.99.
		/// </summary>
		[PublicAPI]
		public string BrightnessValue
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagBrightnessValue);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagBrightnessValue, value);
			}
		}
		
		[PublicAPI]
		public string CfaPattern
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagCfaPattern);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagCfaPattern, value);
			}
		}
		
		[PublicAPI]
		public ColorSpaces ColorSpace
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return ColorSpaces.Uncalibrated;
				}
				
				return (ColorSpaces) GetIntAttribute(TagColorSpace);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagColorSpace, ((int) value).ToString());
			}
		}
		
		[PublicAPI]
		public string ComponentsConfiguration
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagComponentsConfiguration);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagComponentsConfiguration, value);
			}
		}
		
		[PublicAPI]
		public string CompressedBitsPerPixel
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagCompressedBitsPerPixel);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagCompressedBitsPerPixel, value);
			}
		}
		
		[PublicAPI]
		public CompressionTypes Compression
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return CompressionTypes.Uncompressed;
				}
				
				return (CompressionTypes) GetIntAttribute(TagCompression);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagCompression, ((int)value).ToString());
			}
		}
		
		[PublicAPI]
		public Contrasts Contrast
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return Contrasts.Normal;
				}
				
				return (Contrasts) GetIntAttribute(TagContrast);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagContrast, ((int)value).ToString());
			}
		}
		
		[PublicAPI]
		public string Copyright
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagCopyright);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagCopyright, value);
			}
		}
		
		/// <summary>
		/// 0 - no special processing is used, 1 - special processing is used
		/// </summary>
		[PublicAPI]
		public int CustomRendered
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagCustomRendered);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagCustomRendered, value.ToString());
			}
		}
		
		[PublicAPI]
		public string DateTime
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagDateTime);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagDateTime, value);
			}
		}
		
		[PublicAPI]
		public string DateTimeDigitized
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.M))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagDateTimeDigitized);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.M))
				{
					return;
				}
				
				SetAttribute(TagDateTimeDigitized, value);
			}
		}
		
		[PublicAPI]
		public string DateTimeOriginal
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagDateTimeOriginal);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagDateTimeOriginal, value);
			}
		}
		
		[PublicAPI]
		public int DefaultCropSize
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.O))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagDefaultCropSize);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.O))
				{
					return;
				}
				
				SetAttribute(TagDefaultCropSize, value.ToString());
			}
		}
		
		[PublicAPI]
		public string DeviceSettingDescription
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagDeviceSettingDescription);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagDeviceSettingDescription, value);
			}
		}
		
		[PublicAPI]
		public double DigitalZoomRatio
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return double.MinValue;
				}
				
				return GetDoubleAttribute(TagDigitalZoomRatio);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagDigitalZoomRatio, value.ToString(CultureInfo.InvariantCulture));
			}
		}
		
		[PublicAPI]
		public string ExifVersion
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagExifVersion);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagExifVersion, value);
			}
		}
		
		[PublicAPI]
		public string ExposureBiasValue
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagExposureBiasValue);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagExposureBiasValue, value);
			}
		}
		
		/// <summary>
		/// Indicates the exposure index selected on the camera or input device at the time the image is captured.
		/// </summary>
		[PublicAPI]
		public string ExposureIndex
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagExposureIndex);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagExposureIndex, value);
			}
		}
		
		[PublicAPI]
		public ExposureModes ExposureMode
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return ExposureModes.Auto;
				}
				
				return (ExposureModes) GetIntAttribute(TagExposureMode);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagExposureMode, ((int)value).ToString());
			}
		}
		
		[PublicAPI]
		public ExposureProgrammes ExposureProgram
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return ExposureProgrammes.NotDefined;
				}
				
				return (ExposureProgrammes) GetIntAttribute(TagExposureProgram);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagExposureProgram, ((int)value).ToString());
			}
		}
		
		[PublicAPI]
		public double ExposureTime
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return double.MinValue;
				}
				
				return GetDoubleAttribute(TagExposureTime);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagExposureTime, value.ToString(CultureInfo.InvariantCulture));
			}
		}
		
		[PublicAPI]
		public string FileSource
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagFileSource);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagFileSource, value);
			}
		}
		
		[PublicAPI]
		public int Flash
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagFlash);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagFlash, value.ToString());
			}
		}
		
		[PublicAPI]
		public string FlashPixVersion
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagFlashPixVersion);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagFlashPixVersion, value);
			}
		}
		
		[PublicAPI]
		public string FlashEnergy
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagFlashEnergy);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagFlashEnergy, value);
			}
		}
		
		/// <summary>
		/// The actual focal length of the lens, in mm. Conversion is not made to the focal length of a 35 mm film camera.
		/// </summary>
		[PublicAPI]
		public string FocalLength
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagFocalLength);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagFocalLength, value);
			}
		}
		
		[PublicAPI]
		public int FocalLengthIn35MmFilm
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagFocalLengthIn35MmFilm);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagFocalLengthIn35MmFilm, value.ToString());
			}
		}
		
		[PublicAPI]
		public int FocalPlaneResolutionUnit
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagFocalPlaneResolutionUnit);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagFocalPlaneResolutionUnit, value.ToString());
			}
		}
		
		/// <summary>
		/// Indicates the number of pixels in the image width (X) direction per FocalPlaneResolutionUnit on the camera focal plane.
		/// </summary>
		[PublicAPI]
		public string FocalPlaneXResolution
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagFocalPlaneXResolution);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagFocalPlaneXResolution, value);
			}
		}
		
		/// <summary>
		/// Indicates the number of pixels in the image height (Y) direction per FocalPlaneResolutionUnit on the camera focal plane.
		/// </summary>
		[PublicAPI]
		public string FocalPlaneYResolution
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagFocalPlaneYResolution);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagFocalPlaneYResolution, value);
			}
		}
		
		[PublicAPI]
		public GainControlType GainControl
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return GainControlType.None;
				}
				
				return (GainControlType) GetIntAttribute(TagGainControl);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGainControl, ((int)value).ToString());
			}
		}
		
		/// <summary>
		/// The altitude (in meters) based on the reference in GpsAltitudeRef.
		/// </summary>
		[PublicAPI]
		public string GpsAltitude
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsAltitude);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagGpsAltitude, value);
			}
		}
		
		[PublicAPI]
		public AltitudesRef GpsAltitudeRef
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return AltitudesRef.AboveSeaLevel;
				}
				
				return (AltitudesRef) GetIntAttribute(TagGpsAltitudeRef);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagGpsAltitudeRef, ((int) value).ToString());
			}
		}
		
		[PublicAPI]
		public string GpsAreaInformation
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsAreaInformation);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsAreaInformation, value);
			}
		}
		
		[PublicAPI]
		public string GpsDateStamp
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsDateStamp);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagGpsDateStamp, value);
			}
		}
		
		[PublicAPI]
		public string GpsDestinationBearing
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsDestinationBearing);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsDestinationBearing, value);
			}
		}
		
		[PublicAPI]
		public double GpsDestinationBearingRef
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return double.MinValue;
				}
				
				return GetDoubleAttribute(TagGpsDestinationBearingRef);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsDestinationBearingRef, value.ToString(CultureInfo.InvariantCulture));
			}
		}
		
		[PublicAPI]
		public string GpsDestinationDistance
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsDestinationDistance);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsDestinationDistance, value);
			}
		}
		
		[PublicAPI]
		public string GpsDestinationDistanceRef
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsDestinationDistanceRef);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsDestinationDistanceRef, value);
			}
		}
		
		[PublicAPI]
		public string GpsDestinationLatitude
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsDestinationLatitude);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsDestinationLatitude, value);
			}
		}
		
		[PublicAPI]
		public string GpsDestinationLatitudeRef
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsDestinationLatitudeRef);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsDestinationLatitudeRef, value);
			}
		}
		
		[PublicAPI]
		public string GpsDestinationLongitude
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsDestinationLongitude);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsDestinationLongitude, value);
			}
		}
		
		[PublicAPI]
		public string GpsDestinationLongitudeRef
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsDestinationLongitudeRef);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsDestinationLongitudeRef, value);
			}
		}
		
		/// <summary>
		/// 1 if differential correction is applied, 0 - no differential correction is applied.
		/// </summary>
		[PublicAPI]
		public int GpsDifferential
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagGpsDifferential);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsDifferential, value.ToString());
			}
		}
		
		[PublicAPI]
		public string GpsDilutionOfPrecision
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsDilutionOfPrecision);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsDilutionOfPrecision, value);
			}
		}
		
		[PublicAPI]
		public string GpsImgDirection
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsImgDirection);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsImgDirection, value);
			}
		}
		
		[PublicAPI]
		public string GpsImgDirectionRef
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsImgDirectionRef);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsImgDirectionRef, value);
			}
		}
		
		[PublicAPI]
		public string GpsLatitude
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsLatitude);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagGpsLatitude, value);
			}
		}
		
		[PublicAPI]
		public string GpsLatitudeRef
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsLatitudeRef);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagGpsLatitudeRef, value);
			}
		}
		
		[PublicAPI]
		public string GpsLongitude
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsLongitude);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagGpsLongitude, value);
			}
		}
		
		[PublicAPI]
		public string GpsLongitudeRef
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsLongitudeRef);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagGpsLongitudeRef, value);
			}
		}
		
		[PublicAPI]
		public string GpsMapDatum
		{
			get
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsMapDatum);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsMapDatum, value);
			}
		}
		
		[PublicAPI]
		public string GpsMeasureMode
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsMeasureMode);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsMeasureMode, value);
			}
		}
		
		[PublicAPI]
		public string GpsProcessingMethod
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsProcessingMethod);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsProcessingMethod, value);
			}
		}
		
		[PublicAPI]
		public string GpsSatellites
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsSatellites);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsSatellites, value);
			}
		}
		
		[PublicAPI]
		public string GpsSpeed
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsSpeed);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsSpeed, value);
			}
		}
		
		[PublicAPI]
		public string GpsSpeedRef
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsSpeedRef);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsSpeedRef, value);
			}
		}
		
		[PublicAPI]
		public string GpsStatus
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsStatus);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsStatus, value);
			}
		}
		
		/// <summary>
		/// Format is "hh:mm:ss".
		/// </summary>
		[PublicAPI]
		public string GpsTimeStamp
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsTimeStamp);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagGpsTimeStamp, value);
			}
		}
		
		[PublicAPI]
		public string GpsTrack
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsTrack);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsTrack, value);
			}
		}
		
		[PublicAPI]
		public string GpsTrackRef
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsTrackRef);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsTrackRef, value);
			}
		}
		
		[PublicAPI]
		public string GpsVersionId
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagGpsVersionId);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagGpsVersionId, value);
			}
		}
		
		[PublicAPI]
		public string ImageDescription
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagImageDescription);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagImageDescription, value);
			}
		}
		
		[PublicAPI]
		public int ImageLength
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagImageLength);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagImageLength, value.ToString());
			}
		}
		
		[PublicAPI]
		public string ImageUniqueId
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagImageUniqueId);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagImageUniqueId, value);
			}
		}
		
		[PublicAPI]
		public int ImageWidth
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagImageWidth);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagImageWidth, value.ToString());
			}
		}
		
		[PublicAPI]
		public string InteroperabilityIndex
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagInteroperabilityIndex);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagInteroperabilityIndex, value);
			}
		}
		
		[PublicAPI]
		public int IsoSpeedRatings
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagIsoSpeedRatings);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagIsoSpeedRatings, value.ToString());
			}
		}
		
		[PublicAPI]
		public int JpegInterchangeFormat
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagJpegInterchangeFormat);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagJpegInterchangeFormat, value.ToString());
			}
		}
		
		[PublicAPI]
		public int JpegInterchangeFormatLength
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagJpegInterchangeFormatLength);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagJpegInterchangeFormatLength, value.ToString());
			}
		}
		
		[PublicAPI]
		public LightSourceType LightSource
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return LightSourceType.Unknown;
				}
				
				return (LightSourceType) GetIntAttribute(TagLightSource);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagLightSource, ((int)value).ToString());
			}
		}
		
		[PublicAPI]
		public string Make
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagMake);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagMake, value);
			}
		}
		
		[PublicAPI]
		public string MakerNote
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagMakerNote);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagMakerNote, value);
			}
		}
		
		[PublicAPI]
		public MeteringModes MeteringMode
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return MeteringModes.Unknown;
				}
				
				return (MeteringModes) GetIntAttribute(TagMeteringMode);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagMeteringMode, ((int)value).ToString());
			}
		}
		
		[PublicAPI]
		public string MaxApertureValue
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagMaxApertureValue);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagMaxApertureValue, value);
			}
		}
		
		[PublicAPI]
		public string Model
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagModel);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagModel, value);
			}
		}
		
		[PublicAPI]
		public int NewSubFileType
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagNewSubFileType);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagNewSubFileType, value.ToString());
			}
		}
		
		[PublicAPI]
		public string Oecf
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagOecf);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagOecf, value);
			}
		}
		
		/// <summary>
		/// See Olympus Image Processing tags in http://www.exiv2.org/tags-olympus.html.
		/// </summary>
		[PublicAPI]
		public int OrfAspectFrame
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagOrfAspectFrame);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return;
				}
				
				SetAttribute(TagOrfAspectFrame, value.ToString());
			}
		}
		
		/// <summary>
		/// See Olympus Image Processing tags in http://www.exiv2.org/tags-olympus.html.
		/// </summary>
		[PublicAPI]
		public int OrfPreviewImageLength
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagOrfPreviewImageLength);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return;
				}
				
				SetAttribute(TagOrfPreviewImageLength, value.ToString());
			}
		}
		
		/// <summary>
		/// See Olympus Image Processing tags in http://www.exiv2.org/tags-olympus.html.
		/// </summary>
		[PublicAPI]
		public int OrfPreviewImageStart
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagOrfPreviewImageStart);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return;
				}
				
				SetAttribute(TagOrfPreviewImageStart, value.ToString());
			}
		}
		
		/// <summary>
		/// Type is undefined. See Olympus MakerNote tags in http://www.exiv2.org/tags-olympus.html.
		/// </summary>
		[PublicAPI]
		public string OrfThumbnailImage
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagOrfThumbnailImage);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return;
				}
				
				SetAttribute(TagOrfThumbnailImage, value);
			}
		}

		[PublicAPI]
		public Orientations Orientation
		{
			get
			{
				if (AGUtils.IsNotAndroid())
				{
					return Orientations.Undefined;
				}
				
				return (Orientations) GetIntAttribute(TagOrientation);
			}
			set
			{
				if (AGUtils.IsNotAndroid())
				{
					return;
				}
				
				SetAttribute(TagOrientation, ((int) value).ToString());
			}
		}
		
		[PublicAPI]
		public PhotometricInterpretations PhotometricInterpretation
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return PhotometricInterpretations.Rgb;
				}
				
				return (PhotometricInterpretations)GetIntAttribute(TagPhotometricInterpretation);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagPhotometricInterpretation, ((int)value).ToString());
			}
		}
		
		[PublicAPI]
		public int PixelXDimension
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagPixelXDimension);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagPixelXDimension, value.ToString());
			}
		}
		
		[PublicAPI]
		public int PixelYDimension
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagPixelYDimension);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagPixelYDimension, value.ToString());
			}
		}
		
		/// <summary>
		/// 1 for Chunky format, 2 - for Planar
		/// </summary>
		[PublicAPI]
		public int PlanarConfiguration
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagPlanarConfiguration);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagPlanarConfiguration, value.ToString());
			}
		}
		
		[PublicAPI]
		public string PrimaryChromaticities
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagPrimaryChromaticities);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagPrimaryChromaticities, value);
			}
		}
		
		[PublicAPI]
		public string ReferenceBlackWhite
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagReferenceBlackWhite);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagReferenceBlackWhite, value);
			}
		}
		
		[PublicAPI]
		public string RelatedSoundFile
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagRelatedSoundFile);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagRelatedSoundFile, value);
			}
		}
		
		/// <summary>
		/// 3 - centimeters, 2 - inches
		/// </summary>
		[PublicAPI]
		public int ResolutionUnit
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagResolutionUnit);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagResolutionUnit, value.ToString());
			}
		}
		
		[PublicAPI]
		public int RowsPerStrip
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagRowsPerStrip);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagRowsPerStrip, value.ToString());
			}
		}
		
		/// <summary>
		/// See PanasonicRaw tags in http://www.sno.phy.queensu.ca/~phil/exiftool/TagNames/PanasonicRaw.html
		/// </summary>
		[PublicAPI]
		public int Rw2Iso
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagRw2Iso);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return;
				}
				
				SetAttribute(TagRw2Iso, value.ToString());
			}
		}
		
		/// <summary>
		/// Type is undefined. See PanasonicRaw tags in http://www.sno.phy.queensu.ca/~phil/exiftool/TagNames/PanasonicRaw.html
		/// </summary>
		[PublicAPI]
		public string Rw2JpgFromRaw
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagRw2JpgFromRaw);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return;
				}
				
				SetAttribute(TagRw2JpgFromRaw, value);
			}
		}
		
		/// <summary>
		/// See PanasonicRaw tags in http://www.sno.phy.queensu.ca/~phil/exiftool/TagNames/PanasonicRaw.html
		/// </summary>
		[PublicAPI]
		public int Rw2SensorBottomBorder
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagRw2SensorBottomBorder);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return;
				}
				
				SetAttribute(TagRw2SensorBottomBorder, value.ToString());
			}
		}
		
		/// <summary>
		/// See PanasonicRaw tags in http://www.sno.phy.queensu.ca/~phil/exiftool/TagNames/PanasonicRaw.html
		/// </summary>
		[PublicAPI]
		public int Rw2SensorLeftBorder
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagRw2SensorLeftBorder);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return;
				}
				
				SetAttribute(TagRw2SensorLeftBorder, value.ToString());
			}
		}
		
		/// <summary>
		/// See PanasonicRaw tags in http://www.sno.phy.queensu.ca/~phil/exiftool/TagNames/PanasonicRaw.html
		/// </summary>
		[PublicAPI]
		public int Rw2SensorRightBorder
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagRw2SensorRightBorder);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return;
				}
				
				SetAttribute(TagRw2SensorRightBorder, value.ToString());
			}
		}
		
		/// <summary>
		/// See PanasonicRaw tags in http://www.sno.phy.queensu.ca/~phil/exiftool/TagNames/PanasonicRaw.html
		/// </summary>
		[PublicAPI]
		public int Rw2SensorTopBorder
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagRw2SensorTopBorder);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return;
				}
				
				SetAttribute(TagRw2SensorTopBorder, value.ToString());
			}
		}
		
		[PublicAPI]
		public int SamplesPerPixel
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagSamplesPerPixel);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagSamplesPerPixel, value.ToString());
			}
		}
		
		[PublicAPI]
		public int Saturation
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagSaturation);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagSaturation, value.ToString());
			}
		}
		
		[PublicAPI]
		public SceneCaptureTypes SceneCaptureType
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return SceneCaptureTypes.Standard;
				}
				
				return (SceneCaptureTypes) GetIntAttribute(TagSceneCaptureType);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagSceneCaptureType, ((int)value).ToString());
			}
		}
		
		[PublicAPI]
		public string SceneType
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagSceneType);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagSceneType, value);
			}
		}
		
		[PublicAPI]
		public SensorType SensingMethod
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return SensorType.NotDefined;
				}
				
				return (SensorType) GetIntAttribute(TagSensingMethod);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagSensingMethod, ((int)value).ToString());
			}
		}
		
		/// <summary>
		/// 0 - normal, 1 - soft
		/// </summary>
		[PublicAPI]
		public int Sharpness
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagSharpness);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagSharpness, value.ToString());
			}
		}
		
		[PublicAPI]
		public string ShutterSpeedValue
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagShutterSpeedValue);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagShutterSpeedValue, value);
			}
		}
		
		[PublicAPI]
		public string Software
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagSoftware);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagSoftware, value);
			}
		}
		
		[PublicAPI]
		public string SpatialFrequencyResponse
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagSpatialFrequencyResponse);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagSpatialFrequencyResponse, value);
			}
		}
		
		[PublicAPI]
		public string SpectralSensitivity
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagSpectralSensitivity);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagSpectralSensitivity, value);
			}
		}
		
		[PublicAPI]
		public int StripByteCounts
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagStripByteCounts);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagStripByteCounts, value.ToString());
			}
		}
		
		[PublicAPI]
		public int StripOffsets
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagStripOffsets);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagStripOffsets, value.ToString());
			}
		}
		
		[PublicAPI]
		public int SubFileType
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagSubFileType);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.O))
				{
					return;
				}
				
				SetAttribute(TagSubFileType, value.ToString());
			}
		}
		
		[PublicAPI]
		public int SubjectArea
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagSubjectArea);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagSubjectArea, value.ToString());
			}
		}
		
		[PublicAPI]
		public double SubjectDistance
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return double.MinValue;
				}
				
				return GetDoubleAttribute(TagSubjectDistance);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagSubjectDistance, value.ToString(CultureInfo.InvariantCulture));
			}
		}
		
		[PublicAPI]
		public SubjectDistanceRangeType SubjectDistanceRange
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return SubjectDistanceRangeType.Unknown;
				}
				
				return (SubjectDistanceRangeType) GetIntAttribute(TagSubjectDistanceRange);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagSubjectDistanceRange, ((int)value).ToString());
			}
		}
		
		[PublicAPI]
		public int SubjectLocation
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagSubjectLocation);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagSubjectLocation, value.ToString());
			}
		}
		
		[PublicAPI]
		public string SubSecTime
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagSubSecTime);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
				{
					return;
				}
				
				SetAttribute(TagSubSecTime, value);
			}
		}
		
		[PublicAPI]
		public string SubSecTimeDigitized
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagSubSecTimeDigitized);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
				{
					return;
				}
				
				SetAttribute(TagSubSecTimeDigitized, value);
			}
		}
		
		[PublicAPI]
		public string SubSecTimeOriginal
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagSubSecTimeOriginal);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.M))
				{
					return;
				}
				
				SetAttribute(TagSubSecTimeOriginal, value);
			}
		}
		
		[PublicAPI]
		public int ThumbnailImageLength
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagThumbnailImageLength);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagThumbnailImageLength, value.ToString());
			}
		}
		
		[PublicAPI]
		public int ThumbnailImageWidth
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagThumbnailImageWidth);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagThumbnailImageWidth, value.ToString());
			}
		}
		
		[PublicAPI]
		public int TransferFunction
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagTransferFunction);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagTransferFunction, value.ToString());
			}
		}
		
		[PublicAPI]
		public string UserComment
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagUserComment);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagUserComment, value);
			}
		}
		
		[PublicAPI]
		public WhiteBalances WhiteBalance
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return WhiteBalances.Auto;
				}
				
				return (WhiteBalances) GetIntAttribute(TagWhiteBalance);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagWhiteBalance, ((int) value).ToString());
			}
		}
		
		[PublicAPI]
		public string WhitePoint
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagWhitePoint);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagWhitePoint, value);
			}
		}
		
		[PublicAPI]
		public string XResolution
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagXResolution);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagXResolution, value);
			}
		}
		
		[PublicAPI]
		public string YCbCrCoefficients
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagYCbCrCoefficients);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagYCbCrCoefficients, value);
			}
		}
		
		/// <summary>
		/// 1 - centered, 2 - co-sited
		/// </summary>
		[PublicAPI]
		public int YCbCrPositioning
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagYCbCrPositioning);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagYCbCrPositioning, value.ToString());
			}
		}
		
		[PublicAPI]
		public int YCbCrSubSampling
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return int.MinValue;
				}
				
				return GetIntAttribute(TagYCbCrSubSampling);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagYCbCrSubSampling, value.ToString());
			}
		}
		
		[PublicAPI]
		public string YResolution
		{
			get
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return string.Empty;
				}
				
				return GetStringAttribute(TagYResolution);
			}
			set
			{
				if (AGUtils.IsNotAndroid() || !Check.IsSdkGreaterOrEqual(AGDeviceInfo.VersionCodes.N))
				{
					return;
				}
				
				SetAttribute(TagYResolution, value);
			}
		}

		public override string ToString()
		{
			return string.Format("Altitude: {0}, Thumbnail: {1}, ThumbnailBytes: {2}, ThumbnailRange: {3}, HasThumbnail: {4}, " +
			                     "IsThumbnailCompressed: {5}, Aperture: {6}, ApertureValue: {7}, Artist: {8}, BitsPerSample: {9}," +
			                     " BrightnessValue: {10}, CfaPattern: {11}, ColorSpace: {12}, ComponentsConfiguration: {13}, " +
			                     "CompressedBitsPerPixel: {14}, Compression: {15}, Contrast: {16}, Copyright: {17}, CustomRendered: {18}, " +
			                     "DateTime: {19}, DateTimeDigitized: {20}, DateTimeOriginal: {21}, DefaultCropSize: {22}, " +
			                     "DeviceSettingDescription: {23}, DigitalZoomRatio: {24}, ExifVersion: {25}, ExposureBiasValue: {26}," +
			                     " ExposureIndex: {27}, ExposureMode: {28}, ExposureProgram: {29}, ExposureTime: {30}, FileSource: {31}, " +
			                     "Flash: {32}, FlashPixVersion: {33}, FlashEnergy: {34}, FocalLength: {35}, FocalLengthIn35MmFilm: {36}, " +
			                     "FocalPlaneResolutionUnit: {37}, FocalPlaneXResolution: {38}, FocalPlaneYResolution: {39}, GainControl: {40}, " +
			                     "GpsAltitude: {41}, GpsAltitudeRef: {42}, GpsAreaInformation: {43}, GpsDateStamp: {44}, GpsDestinationBearing: {45}, " +
			                     "GpsDestinationBearingRef: {46}, GpsDestinationDistance: {47}, GpsDestinationDistanceRef: {48}, " +
			                     "GpsDestinationLatitude: {49}, GpsDestinationLatitudeRef: {50}, GpsDestinationLongitude: {51}, " +
			                     "GpsDestinationLongitudeRef: {52}, GpsDifferential: {53}, GpsDilutionOfPrecision: {54}, " +
			                     "GpsImgDirection: {55}, GpsImgDirectionRef: {56}, GpsLatitude: {57}, GpsLatitudeRef: {58}," +
			                     " GpsLongitude: {59}, GpsLongitudeRef: {60}, GpsMapDatum: {61}, GpsMeasureMode: {62}, " +
			                     "GpsProcessingMethod: {63}, GpsSatellites: {64}, GpsSpeed: {65}, GpsSpeedRef: {66}, " +
			                     "GpsStatus: {67}, GpsTimeStamp: {68}, GpsTrack: {69}, GpsTrackRef: {70}, GpsVersionId: {71}, " +
			                     "ImageDescription: {72}, ImageLength: {73}, ImageUniqueId: {74}, ImageWidth: {75}, " +
			                     "InteroperabilityIndex: {76}, IsoSpeedRatings: {77}, JpegInterchangeFormat: {78}, " +
			                     "JpegInterchangeFormatLength: {79}, LightSource: {80}, Make: {81}, MakerNote: {82}, " +
			                     "MeteringMode: {83}, MaxApertureValue: {84}, Model: {85}, NewSubFileType: {86}, Oecf: {87}," +
			                     " OrfAspectFrame: {88}, OrfPreviewImageLength: {89}, OrfPreviewImageStart: {90}, OrfThumbnailImage: {91}, " +
			                     "Orientation: {92}, PhotometricInterpretation: {93}, PixelXDimension: {94}, PixelYDimension: {95}," +
			                     " PlanarConfiguration: {96}, PrimaryChromaticities: {97}, ReferenceBlackWhite: {98}, " +
			                     "RelatedSoundFile: {99}, ResolutionUnit: {100}, RowsPerStrip: {101}, Rw2Iso: {102}, Rw2JpgFromRaw: {103}, " +
			                     "Rw2SensorBottomBorder: {104}, Rw2SensorLeftBorder: {105}, Rw2SensorRightBorder: {106}," +
			                     " Rw2SensorTopBorder: {107}, SamplesPerPixel: {108}, Saturation: {109}, SceneCaptureType: {110}, " +
			                     "SceneType: {111}, SensingMethod: {112}, Sharpness: {113}, ShutterSpeedValue: {114}, Software: {115}, " +
			                     "SpatialFrequencyResponse: {116}, SpectralSensitivity: {117}, StripByteCounts: {118}, " +
			                     "StripOffsets: {119}, SubFileType: {120}, SubjectArea: {121}, SubjectDistance: {122}, " +
			                     "SubjectDistanceRange: {123}, SubjectLocation: {124}, SubSecTime: {125}, SubSecTimeDigitized: {126}," +
			                     " SubSecTimeOriginal: {127}, ThumbnailImageLength: {128}, ThumbnailImageWidth: {129}," +
			                     " TransferFunction: {130}, UserComment: {131}, WhiteBalance: {132}, WhitePoint: {133}, " +
			                     "XResolution: {134}, YCbCrCoefficients: {135}, YCbCrPositioning: {136}, YCbCrSubSampling: {137}, " +
			                     "YResolution: {138}", Altitude, Thumbnail, ThumbnailBytes, ThumbnailRange, HasThumbnail, IsThumbnailCompressed, 
				Aperture, ApertureValue, Artist, BitsPerSample, BrightnessValue, CfaPattern, ColorSpace, ComponentsConfiguration, 
				CompressedBitsPerPixel, Compression, Contrast, Copyright, CustomRendered, DateTime, DateTimeDigitized, DateTimeOriginal, 
				DefaultCropSize, DeviceSettingDescription, DigitalZoomRatio, ExifVersion, ExposureBiasValue, ExposureIndex, ExposureMode, 
				ExposureProgram, ExposureTime, FileSource, Flash, FlashPixVersion, FlashEnergy, FocalLength, FocalLengthIn35MmFilm, 
				FocalPlaneResolutionUnit, FocalPlaneXResolution, FocalPlaneYResolution, GainControl, GpsAltitude, GpsAltitudeRef, 
				GpsAreaInformation, GpsDateStamp, GpsDestinationBearing, GpsDestinationBearingRef, GpsDestinationDistance, 
				GpsDestinationDistanceRef, GpsDestinationLatitude, GpsDestinationLatitudeRef, GpsDestinationLongitude, 
				GpsDestinationLongitudeRef, GpsDifferential, GpsDilutionOfPrecision, GpsImgDirection, GpsImgDirectionRef, 
				GpsLatitude, GpsLatitudeRef, GpsLongitude, GpsLongitudeRef, GpsMapDatum, GpsMeasureMode, GpsProcessingMethod, 
				GpsSatellites, GpsSpeed, GpsSpeedRef, GpsStatus, GpsTimeStamp, GpsTrack, GpsTrackRef, GpsVersionId, ImageDescription, 
				ImageLength, ImageUniqueId, ImageWidth, InteroperabilityIndex, IsoSpeedRatings, JpegInterchangeFormat, 
				JpegInterchangeFormatLength, LightSource, Make, MakerNote, MeteringMode, MaxApertureValue, Model, NewSubFileType,
				Oecf, OrfAspectFrame, OrfPreviewImageLength, OrfPreviewImageStart, OrfThumbnailImage, Orientation, PhotometricInterpretation,
				PixelXDimension, PixelYDimension, PlanarConfiguration, PrimaryChromaticities, ReferenceBlackWhite, RelatedSoundFile, 
				ResolutionUnit, RowsPerStrip, Rw2Iso, Rw2JpgFromRaw, Rw2SensorBottomBorder, Rw2SensorLeftBorder, Rw2SensorRightBorder,
				Rw2SensorTopBorder, SamplesPerPixel, Saturation, SceneCaptureType, SceneType, SensingMethod, Sharpness, ShutterSpeedValue, 
				Software, SpatialFrequencyResponse, SpectralSensitivity, StripByteCounts, StripOffsets, SubFileType, SubjectArea, 
				SubjectDistance, SubjectDistanceRange, SubjectLocation, SubSecTime, SubSecTimeDigitized, SubSecTimeOriginal, 
				ThumbnailImageLength, ThumbnailImageWidth, TransferFunction, UserComment, WhiteBalance, WhitePoint, XResolution, 
				YCbCrCoefficients, YCbCrPositioning, YCbCrSubSampling, YResolution);
		}

		/// <summary>
		/// Save the tag data into the original image file. This is expensive because it involves copying
		/// all the data from one file to another and deleting the old file and renaming the other.
		/// It's best to set all attributes and make a single call rather than multiple calls for each attribute.
		/// 
		/// This method is only supported for JPEG files.
		/// </summary>
		[PublicAPI]
		public void SaveAttributes()
		{
			ajo.Call("saveAttributes");
		}
		
		void SetAttribute(string tagName, string propertyValue)
		{
			ajo.Call("setAttribute", tagName, propertyValue);
		}

		string GetStringAttribute(string tagName)
		{
			return ajo.CallStr("getAttribute", tagName);
		}

		double GetDoubleAttribute(string tagName)
		{
			return ajo.Call<double>("getAttributeDouble", tagName, double.MinValue);
		}
		
		int GetIntAttribute(string tagName)
		{
			return ajo.CallInt("getAttributeInt", tagName, int.MinValue);
		}
	}
}
#endif
