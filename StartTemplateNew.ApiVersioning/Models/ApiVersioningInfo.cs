namespace StartTemplateNew.Shared.ApiVersioning.Models
{
    public class ApiVersioningInfo
    {
        public int CurrentApiVersion { get; }
        public bool AssumeDefaultVersionWhenUnspecified { get; set; }
        public bool ReportApiVersions { get; set; }
    }
}
