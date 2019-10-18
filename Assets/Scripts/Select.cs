using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    public void OnButtonClick()
    {
        var imageResultSize = DeadMosquito.AndroidGoodies.ImageResultSize.Max512;
        DeadMosquito.AndroidGoodies.AGGallery.PickImageFromGallery(
            selectedImage =>
            {
                Texture2D imageTexture2D = selectedImage.LoadTexture2D();
                string msg = string.Format("{0} was loaded from gallery with size {1}x{2}",
                    selectedImage.OriginalPath, imageTexture2D.width, imageTexture2D.height);

                //it doesn't work
                //image.sprite = SpriteFromTex2D(imageTexture2D);

                DeadMosquito.AndroidGoodies.AGGallery.SaveImageToGallery(imageTexture2D, "Escaper", "Goodies", DeadMosquito.AndroidGoodies.ImageFormat.PNG);

                // Clean up
                Resources.UnloadUnusedAssets();
            },
            errorMessage => DeadMosquito.AndroidGoodies.AGUIMisc.ShowToast("Cancelled picking image from gallery: " + errorMessage),
            imageResultSize, false);
    }
}
