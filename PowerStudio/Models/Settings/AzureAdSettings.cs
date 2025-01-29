namespace PowerStudio.Models.Settings
{
    public class AzureAdSettings
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string RedirectUrl { get; set; }
        public string Scopes { get; set; }
    }
}
