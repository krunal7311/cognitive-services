using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Diagnostics.Contracts;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using Tabs.Model;
using System.Linq;

namespace Tabs
{
	public partial class CustomVision : ContentPage
	{
		public CustomVision()
		{
			InitializeComponent();
		}

        private async void loadCamera(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });

            await MakePredictionRequest(file);
        }
        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }
        async Task MakePredictionRequest(MediaFile file)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Prediction-Key", "5cf82e0055814a0c85cc7267bc990f90");
            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/585c3df4-01eb-4be3-ad93-7b1bb6e489a9/image?iterationId=6e75c1cd-fc72-4aa6-b773-b1b319ebaa38";
            HttpResponseMessage response;
            byte[] byteData = GetImageAsByteArray(file);
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

					JObject rss = JObject.Parse(responseString);

					//Querying with LINQ
					//Get all Prediction Values
					var Probability = from p in rss["Predictions"] select (string)p["Probability"];
					var Tag = from p in rss["Predictions"] select (string)p["Tag"];

					//Truncate values to labels in XAML
					foreach (var item in Tag)
					{
						TagLabel.Text += item + ": \n";
					}

					foreach (var item in Probability)
					{
						PredictionLabel.Text += item + "\n";
					}

                }
            }
			file.Dispose();

		}
		}
	
}
