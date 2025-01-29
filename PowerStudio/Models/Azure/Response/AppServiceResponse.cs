namespace PowerStudio.Models.Azure
{
    public class AppServiceResponse
    {
        public AppServiceData[] Value { get; set; }
    }
    public class AppServiceData 
    {
        public string Name { get; set; }
        public AppServiceProperties Properties { get; set; }
    }
    public class AppServiceProperties
    {
        public string ResourceGroup { get; set; }
    }
}
