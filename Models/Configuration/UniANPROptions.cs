using Newtonsoft.Json;

namespace ABDS.Supervisory.GUI.Models.Configuration
{
    /// <summary>
    /// Config file options class for ABDS GUI Supervisory application
    /// </summary>
    public class UniANPROptions
    {
        public const string ConfigSectionName = "ANPRSupervisory";

        public UniANPROptions(IConfiguration appConfiguration)
        {
            try
            {
                appConfiguration.GetSection(UniANPROptions.ConfigSectionName).Bind(this);
            }
            catch (Exception ex)
            {
            }
        }

        [JsonProperty("DatabaseConnectionString")]
        public string DatabaseConnectionString { get; set; }

    }
}
