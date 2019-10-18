#if UNITY_ANDROID
using DeadMosquito.AndroidGoodies;
#endif
using UnityEngine;

public class PrintTest : MonoBehaviour
{
	[SerializeField]
	Texture2D image;

	const string PrintJobName = "GoodiesPrintJob";

#if UNITY_ANDROID
	public void OnPrintImage()
	{
		if (AGPrintHelper.SystemSupportsPrint)
		{
			AGPrintHelper.PrintImage(image, PrintJobName, () => { AGUIMisc.ShowToast("Printing success"); },
				AGPrintHelper.ColorModes.Monochrome, AGPrintHelper.Orientations.Landscape, AGPrintHelper.ScaleModes.Fill);
		}
		else
		{
			AGUIMisc.ShowToast("Printing is not supported");
		}
	}

	public void OnPrintHtmlText()
	{
		if (AGPrintHelper.SystemSupportsPrint)
		{
			const string htmlDocument = "<html><body><h1>Test Content</h1><p>Testing, testing, testing...</p></body></html>";
			AGPrintHelper.PrintHtmlPage(htmlDocument, PrintJobName);
		}
		else
		{
			AGUIMisc.ShowToast("Printing is not supported");
		}
	}

	public void OnPrintHtmlFromUrl()
	{
		if (AGPrintHelper.SystemSupportsPrint)
		{
			AGPrintHelper.PrintHtmlPageFromUrl("https://ninevastudios.com/", PrintJobName);
		}
		else
		{
			AGUIMisc.ShowToast("Printing is not supported");
		}
	}
#endif
}