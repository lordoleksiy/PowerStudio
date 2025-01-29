using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.AppService.Models;
using Azure.ResourceManager.Resources;
using PowerStudio.Models.Azure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerStudio.Services
{
    public class AzureService
    {
        private readonly ArmClient _armClient;

        public AzureService(ArmClient armClient)
        {
            _armClient = armClient;
        }

        public async Task<ICollection<Subscription>> GetSubscriptionsAsync()
        {
            var subscriptions = new List<Subscription>();

            SubscriptionCollection collection = _armClient.GetSubscriptions();

            await foreach (SubscriptionResource item in collection.GetAllAsync())
            {
                subscriptions.Add(new Subscription
                {
                    Name = item.Data.DisplayName,
                    SubscriptionId = item.Data.SubscriptionId
                });
            }
            return subscriptions;
        }

        public async Task<ICollection<AppService>> GetAppServicesBySubscriptionAsync(string subscriptionId)
        {
            var appServices = new List<AppService>();
            ResourceIdentifier subscriptionResourceId = SubscriptionResource.CreateResourceIdentifier(subscriptionId);
            SubscriptionResource subscriptionResource = _armClient.GetSubscriptionResource(subscriptionResourceId);
            await foreach (WebSiteResource item in subscriptionResource.GetWebSitesAsync())
            {
                appServices.Add(new AppService
                {
                    SubscriptionId = subscriptionId,
                    ResourceGroupName = item.Data.ResourceGroup,
                    Name = item.Data.Name
                });
            }

            return appServices;
        }

        public async Task<ICollection<AppSettingsModel>> GetAppServiceSettingsAsync(AppService appService)
        {
            var settings = new Dictionary<string, string>();

            var subscription = _armClient.GetSubscriptionResource(new Azure.Core.ResourceIdentifier($"/subscriptions/{appService.SubscriptionId}"));
            ResourceIdentifier webSiteResourceId = WebSiteResource.CreateResourceIdentifier(appService.SubscriptionId, appService.ResourceGroupName, appService.Name);
            WebSiteResource webSite = _armClient.GetWebSiteResource(webSiteResourceId);
            AppServiceConfigurationDictionary result = await webSite.GetApplicationSettingsAsync();
            return result.Properties.Select(x => new AppSettingsModel { Key = x.Key, Value = x.Value, Type = AppSettingsType.NoChange }).ToArray();
        }
    }
}
