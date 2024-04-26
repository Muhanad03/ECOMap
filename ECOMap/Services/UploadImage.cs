using System;
using System.IO;
using System.Threading.Tasks;

namespace ECOMap.Services
{
    internal class UploadImage
    {
        public async Task<FileResult> OpenMedia()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Please pick a cool photo!",
                });

                if (result != null && (result.ContentType == "image/png" || result.ContentType == "image/jpg" || result.ContentType == "image/jpeg"
                    || result.ContentType == "image/heic"))
                {
                    return result;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error type image", "Please choose a new image", "Ok");
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ImageSource> GetImageFromFileResult(FileResult fileResult)
        {
            if (fileResult == null)
                return null;

            var stream = await fileResult.OpenReadAsync();
            return ImageSource.FromStream(() => stream);
        }

        public async Task<string> ImageToBase64(FileResult fileResult)
        {
            if (fileResult == null)
                return null;

            var stream = await fileResult.OpenReadAsync();
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        public async Task<Image> Base64ToImage(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;

            byte[] imageBytes = Convert.FromBase64String(base64String);
            Image image = new Image { Source = ImageSource.FromStream(() => new MemoryStream(imageBytes)) };
            return image;
        }
    }
}
