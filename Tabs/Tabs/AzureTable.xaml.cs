using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using System.Threading.Tasks; 

namespace Tabs
{
    public partial class AzureTable : ContentPage
    {
     
        MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;
        public AzureTable()
        {
            InitializeComponent();
        }
        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            List<DataModels.mycognitivetable> notcognitivetable = await AzureManager.AzureManagerInstance.GetCognitiveInformation();
            LandmarkList.ItemsSource = notcognitivetable;
        }
    }
}
