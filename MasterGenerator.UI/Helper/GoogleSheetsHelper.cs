using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace MasterGenerator.UI.Helper
{
    public class GoogleSheetsHelper
    {
        public SheetsService Service { get; set; }
        private const string APPLICATION_NAME = "Customer Portal";
        private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private const string GoogleCredentialsFileName = "google-credentials.json";
        public GoogleSheetsHelper()
        {
            InitializeService();
        }
        private void InitializeService()
        {
            var credential = GetCredentialsFromFile();
            Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME
            });
        }
        private GoogleCredential GetCredentialsFromFile()
        {
            GoogleCredential credential;
            using (var stream = new FileStream(GoogleCredentialsFileName, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }
            return credential;
        }
    }
}
