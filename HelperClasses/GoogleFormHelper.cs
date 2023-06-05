using Google.Apis.Auth.OAuth2;
using Google.Apis.Forms.v1;
using Google.Apis.Forms.v1.Data;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Intrinsics;

namespace AMSAPI.HelperClasses
{
    public class GoogleFormHelper
    {
        public FormsService? Service { get; set; }
        const string APPLICATION_NAME = "Attendance_Maintenance_System";
        static readonly string[] Scopes = { FormsService.Scope.FormsBody };
        private string? apiSecurityFile;
        Microsoft.Extensions.Hosting.IHostEnvironment? env;
        public GoogleFormHelper(IHostEnvironment? hostEnvironment, string? folderName = "default")
        {
            env = hostEnvironment;
            var _contentPath = env.ContentRootPath;
            _contentPath = Path.Combine(_contentPath, "GoogleSheetSecurity", folderName);
            foreach (var file in Directory.GetFiles(_contentPath))
            {
                if(file.Contains("googleformapi.json"))
                {
                    apiSecurityFile = file;
                }
                    
            }

            InitializeService();
        }

        private void InitializeService()
        {
            var credential = GetCredentialsFromFile();
            Service = new FormsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME
            });
        }
        private GoogleCredential GetCredentialsFromFile()
        {
            GoogleCredential credential;
            //get file from Google Sheet Security folder
            using (var stream = new FileStream(apiSecurityFile, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            return credential;
        }
        public string CreateForm()
        {


            // Create a form
            FormsService service = new FormsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredentialsFromFile()
            });

            Form form = new Form
            {
                Info = new Info
                {
                    Title = "Google Form"
                },
            };
            // Create the Google Form

            var createdForm1 = service.Forms.Create(form).ExecuteAsync().Result;
            var request = new BatchUpdateFormRequest();
            request.Requests = new List<Google.Apis.Forms.v1.Data.Request>();
            // Add 10 multiple-choice questions to the form
            for (int i = 1; i <= 10; i++)
            {
                Item question = new Item
                {
                    Title = $"Question {i}",
                    QuestionItem = new QuestionItem
                    {
                        
                        Question = new Question
                        {
                            Required = true,
                            ChoiceQuestion = new ChoiceQuestion
                            {  
                                Type = "RADIO",
                                Options = new List<Option>
                                {
                                    new Option { Value = "Option 1" },
                                    new Option { Value = "Option 2" },
                                    new Option { Value = "Option 3" },
                                },
                           
                            }
                           
                        },
                      
                        
                    },
                    
                };

                request.Requests.Add(new Google.Apis.Forms.v1.Data.Request
                {
                    CreateItem = new CreateItemRequest
                    {
                        Location = new Location
                        {
                            Index = i-1,
                        },
                        Item = question
                    }
                });
            }
  
            // Set other form settings
            request.Requests.Add(new Google.Apis.Forms.v1.Data.Request
            {
                UpdateSettings = new UpdateSettingsRequest
                {
                    UpdateMask = "*",

                    Settings = new FormSettings
                    {
                        QuizSettings = new QuizSettings
                        {
                            IsQuiz = true
                        }
                    }

                }
            }); ;

            var response = service.Forms.BatchUpdate(request, createdForm1.FormId).Execute();

            // Create the form
            return createdForm1.ResponderUri.ToString();
            //Console.WriteLine("Form created successfully!");
            //Console.WriteLine($"Form ID: {createdForm.FormId}");
            //Console.WriteLine($"Form URL: {createdForm.ResponderUri}");

        }
    }
}
