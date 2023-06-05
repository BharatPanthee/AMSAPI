using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace AMSAPI.HelperClasses
{
    public class GoogleSheetsHelper
    {
        public SheetsService? Service { get; set; }
        const string APPLICATION_NAME = "Attendance_Maintenance_System";
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private string? apiSecurityFile;
        Microsoft.Extensions.Hosting.IHostEnvironment? env;
        public GoogleSheetsHelper(IHostEnvironment? hostEnvironment, string? folderName="default")
        {
           env = hostEnvironment;
           var _contentPath = env.ContentRootPath;
            _contentPath = Path.Combine(_contentPath, "GoogleSheetSecurity", folderName??"default");
            foreach(var file in Directory.GetFiles(_contentPath))
            {
                apiSecurityFile = file;
            }

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
            //get file from Google Sheet Security folder
            using (var stream = new FileStream(apiSecurityFile??"", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }
            return credential;
        }

    }
}
