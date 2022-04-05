using Syncfusion.EJ2.Base;

namespace MasterGenerator.UI.Extensions
{
    public class DataManagerRequestExtension : DataManagerRequest
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string PODate { get; set; }
        public string ProjectStatus { get; set; }
    }
}
