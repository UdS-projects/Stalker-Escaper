using System.Linq;
using UnityEngine;

public class Snapshot : MonoBehaviour
{
    public void OnButtonClick()
    {
        //OnRequestPermissions();
        var imageResultSize2 = DeadMosquito.AndroidGoodies.ImageResultSize.Max1024;
        DeadMosquito.AndroidGoodies.AGCamera.TakePhoto(
            selectedImage =>
            {
                // Load received image into Texture2D
                var imageTexture2D = selectedImage.LoadTexture2D();
                string msg = string.Format("{0} was taken from camera with size {1}x{2}",
                selectedImage.DisplayName, imageTexture2D.width, imageTexture2D.height);

                //I don't know why the code part below doesn't work :-( 
                //image.sprite = SpriteFromTex2D(imageTexture2D);

                DeadMosquito.AndroidGoodies.AGGallery.SaveImageToGallery(imageTexture2D, "Escaper", "Goodies", DeadMosquito.AndroidGoodies.ImageFormat.PNG);

                // Clean up
                Resources.UnloadUnusedAssets();
            },
            error => DeadMosquito.AndroidGoodies.AGUIMisc.ShowToast("Cancelled taking photo from camera: " + error), imageResultSize2, false);
    }

    public void OnRequestPermissions()
    {
        // Don't forget to also add the permissions you need to manifest!
        var permissions = new[]
        {
        DeadMosquito.AndroidGoodies.AGPermissions.CAMERA,
        DeadMosquito.AndroidGoodies.AGPermissions.WRITE_EXTERNAL_STORAGE
    };

        // Filter permissions so we don't request already granted permissions,
        // otherwise if the user denies already granted permission the app will be killed
        var nonGrantedPermissions = permissions.ToList().Where(x => !DeadMosquito.AndroidGoodies.AGPermissions.IsPermissionGranted(x)).ToArray();

        if (nonGrantedPermissions.Length == 0)
        {
            Debug.Log("User already granted all these permissions: " + string.Join(",", permissions));
            return;
        }

        // Finally request permissions user has not granted yet and log the results
        DeadMosquito.AndroidGoodies.AGPermissions.RequestPermissions(permissions, results =>
        {
            // Process results of requested permissions
            foreach (var result in results)
            {
                Debug.Log(string.Format("Permission [{0}] is [{1}], should show explanation?: {2}",
                    result.Permission, result.Status, result.ShouldShowRequestPermissionRationale));
                if (result.Status == DeadMosquito.AndroidGoodies.AGPermissions.PermissionStatus.Denied)
                {
                    // User denied permission, now we need to find out if he clicked "Do not show again" checkbox
                    if (result.ShouldShowRequestPermissionRationale)
                    {
                        // User just denied permission, we can show explanation here and request permissions again
                        // or send user to settings to do so
                    }
                    else
                    {
                        // User checked "Do not show again" checkbox or permission can't be granted.
                        // We should continue with this permission denied
                    }
                }
            }
        });
    }
}
