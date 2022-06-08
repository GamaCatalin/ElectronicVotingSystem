using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using EVT_FrontendApp.Droid.Services;
using EVT_FrontendApp.Services.Image;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageService))]
namespace EVT_FrontendApp.Droid.Services
{
    public class ImageService : IImageService
    {
        public byte[] ResizeImageToStream(Stream image, int size)
        {
            byte[] imageData;
            using (var ms = new MemoryStream())
            {
                ms.CopyTo(image);
                imageData = ms.ToArray();
            }

            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)size, (int)size, false);
            using (var ms = new MemoryStream())
            {
                resizedImage.CompressAsync(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}