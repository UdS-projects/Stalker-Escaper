#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using UnityEngine;
	using JetBrains.Annotations;
	using Internal;

	/// <summary>
	/// Rect holds four integer coordinates for a rectangle.
	/// The rectangle is represented by the coordinates of its 4 edges (left, top, right bottom).
	/// These fields can be accessed directly.
	/// </summary>
	[PublicAPI]
	public class AndroidRect
	{
		public AndroidJavaObject ajo;

		public AndroidRect(int left, int top, int right, int bottom)
		{
			ajo = new AndroidJavaObject(C.AndroidGraphicsRect, left, top, right, bottom);
			this.bottom = ajo.Get<int>("bottom");
			this.left = ajo.Get<int>("left");
			this.right = ajo.Get<int>("right");
			this.top = ajo.Get<int>("top");
		}

		[PublicAPI]
		public int bottom, left, right, top;
	}
}
#endif
