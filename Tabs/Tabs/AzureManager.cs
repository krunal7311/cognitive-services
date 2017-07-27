using System;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace Tabs
{
    public class AzureManager
    {
        private static AzureManager instance;
		private MobileServiceClient client;
        private IMobileServiceTable<DataModels.mycognitivetable> notcognitivetable; 
		private AzureManager()
		{
			this.client = new MobileServiceClient("http://mycognitiveapp.azurewebsites.net/");
            this.notcognitivetable = this.client.GetTable<DataModels.mycognitivetable>();
		}

		public MobileServiceClient AzureClient
		{
			get { return client; }
		}

		public static AzureManager AzureManagerInstance
		{
			get
			{
				if (instance == null)
				{
					instance = new AzureManager();
				}

				return instance;
			}
		}
        public async Task<List<DataModels.mycognitivetable>> GetCognitiveInformation()
        {
            return await this.notcognitivetable.ToListAsync(); 
        }
    }
}
