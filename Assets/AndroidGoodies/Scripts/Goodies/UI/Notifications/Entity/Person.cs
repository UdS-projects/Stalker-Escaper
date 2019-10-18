#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	///     Provides an immutable reference to an entity that appears repeatedly on different surfaces of the platform.
	///     For example, this could represent the sender of a message.
	/// </summary>
	[PublicAPI]
	public class Person
	{
		public AndroidJavaObject AJO;

		public Person(AndroidJavaObject ajo)
		{
			AJO = ajo;
		}

		/// <summary>
		///     The icon provided for this person or null if no icon was provided.
		/// </summary>
		[PublicAPI]
		public Texture2D Icon
		{
			get
			{
				var iconAJO = AJO.CallAJO("getIcon");
				var uri = iconAJO.CallAJO("getUri").JavaToString();
				return AGFileUtils.ImageUriToTexture2D(uri);
			}
		}

		/// <summary>
		///     The key provided for this person or null if no key was provided.
		/// </summary>
		[PublicAPI]
		public string Key
		{
			get { return AJO.CallStr("getKey"); }
		}

		/// <summary>
		///     The name provided for this person or null if no name was provided.
		/// </summary>
		[PublicAPI]
		public string Name
		{
			get { return AJO.CallStr("getName"); }
		}

		/// <summary>
		///     The uri provided for this person or null if no Uri was provided.
		/// </summary>
		[PublicAPI]
		public string Uri
		{
			get { return AJO.CallStr("getUri"); }
		}

		/// <summary>
		///     Whether this Person is a machine.
		/// </summary>
		[PublicAPI]
		public bool IsBot
		{
			get { return AJO.CallBool("isBot"); }
		}

		/// <summary>
		///     Whether this Person is important.
		/// </summary>
		[PublicAPI]
		public bool IsImportant
		{
			get { return AJO.CallBool("isImportant"); }
		}

		public static bool IsNotSupported
		{
			get { return AGUtils.IsNotAndroid() && Check.IsSdkSmallerThan(AGDeviceInfo.VersionCodes.P); }
		}

		/// <summary>
		///     Creates and returns a new Person.Builder initialized with this Person's data.
		/// </summary>
		[PublicAPI]
		public Builder ToBuilder()
		{
			return IsNotSupported ? null : new Builder(AJO.CallAJO("toBuilder"));
		}

		public class Builder
		{
			readonly AndroidJavaObject _ajo;

			public Builder()
			{
				if (IsNotSupported)
				{
					return;
				}

				_ajo = new AndroidJavaObject(C.AndroidAppPersonBuilder);
			}

			public Builder(AndroidJavaObject ajo)
			{
				_ajo = ajo;
			}

			/// <summary>
			///     Sets whether this person is a machine rather than a human.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetBot(bool isBot)
			{
				_ajo.CallAJO("setBot", isBot);
				return this;
			}

			/// <summary>
			///     Add an icon for this person. The system will prefer this icon over any images that are resolved from the URI.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetIcon(Texture2D tex)
			{
				var bitmap = AGUtils.Texture2DToAndroidBitmap(tex);
				Debug.Log("Person builder: bitmap was created.");
				var iconAjo = C.AndroidGraphicsDrawableIcon.AJCCallStaticOnceAJO("createWithBitmap", bitmap);
				Debug.Log("Person builder: icon was created.");
				_ajo.CallAJO("setIcon", iconAjo);
				Debug.Log("Person builder: method was performed.");
				return this;
			}

			/// <summary>
			///     Sets whether this is an important person. Use this method to denote users who frequently interact with the user of
			///     this device.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetImportant(bool isImportant)
			{
				_ajo.CallAJO("setImportant", isImportant);
				return this;
			}

			/// <summary>
			///     Add a key to this person in order to uniquely identify it.
			///     This is especially useful if the name doesn't uniquely identify this person or if the display name is a short
			///     handle of the actual name.
			///     If no key is provided, the name serves as the key for the purpose of identification.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetKey(string key)
			{
				_ajo.CallAJO("setKey", key);
				return this;
			}

			/// <summary>
			///     Give this person a name.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetName(string name)
			{
				_ajo.CallAJO("setName", name);
				return this;
			}

			/// <summary>
			///     Set a URI associated with this person.
			///     The person should be specified by the String representation of a ContactsContract.Contacts.CONTENT_LOOKUP_URI.
			///     The system will also attempt to resolve mailto: and tel: schema URIs.
			///     The path part of these URIs must exist in the contacts database, in the appropriate column,
			///     or the reference will be discarded as invalid. Telephone schema URIs will be resolved by
			///     ContactsContract.PhoneLookup.
			/// </summary>
			/// <returns> The same Builder instance </returns>
			[PublicAPI]
			public Builder SetUri(string uri)
			{
				_ajo.CallAJO("setUri", uri);
				return this;
			}

			/// <summary>
			///     Creates and returns the Person this builder represents.
			/// </summary>
			[PublicAPI]
			public Person Build()
			{
				return new Person(_ajo.CallAJO("build"));
			}
		}
	}
}
#endif