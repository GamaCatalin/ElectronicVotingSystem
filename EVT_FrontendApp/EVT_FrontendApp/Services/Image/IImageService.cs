using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EVT_FrontendApp.Services.Image
{
    public interface IImageService
    {
        byte[] ResizeImageToStream(Stream image, int size);
    }
}