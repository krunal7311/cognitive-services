using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
namespace Tabs
{
    public partial class AzureTable : ContentPage
    {

        Geocoder geocoder;
        MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;
        public AzureTable()
        {
            InitializeComponent();
            geocoder = new Geocoder();
        }
        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            List<DataModels.mycognitivetable> notcognitivetable = await AzureManager.AzureManagerInstance.GetCognitiveInformation();
            foreach (DataModels.mycognitivetable model in notcognitivetable)
            {
                var position = new Position(model.Latitude, model.Longitude);
                var PossibleAddress = await geocoder.GetAddressesForPositionAsync(position);
                foreach (var address in PossibleAddress)
                {
                    model.City = address;
                }
            }
            LandmarkList.ItemsSource = notcognitivetable;
        }
    }
}
