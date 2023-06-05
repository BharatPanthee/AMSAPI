using AMSAPI.Models;
using Google.Apis.Forms.v1.Data;
using Google.Apis.Sheets.v4;
using Microsoft.AspNetCore.Http;

namespace AMSAPI.HelperClasses
{
    public class Clients
    {
        SpreadsheetsResource.ValuesResource _googleSheetValues;
        private const string spreadsheetId = "17HMruiBj1T-q_TLdt9J_Ii5M19DuH1VhWsuO8umA5sk";
        private const string sheetName = "Clients";
        public Clients(GoogleSheetsHelper? googleSheetsHelper)
        {
            _googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
        }
        

        public Client? GetClient(string? clientCode)
        {
            Client? client = null;
            var request = _googleSheetValues.Get(spreadsheetId, sheetName);

            var response = request.Execute();
            var values = response.Values;
           
            List<Student> students = new List<Student>();
            if (values != null && values.Count > 0)
            {
                values.RemoveAt(0);
                List<Client> clients = ClientsMapper.MapFromRangeData(values);

                client = clients.Where(clt=>clt.Code == clientCode).FirstOrDefault() ;
            }

            return client;
        }
        public string? GetGoogleSheetId(string? clientCode)
        {
            Client? client = null;
            var request = _googleSheetValues.Get(spreadsheetId, sheetName);

            var response = request.Execute();
            var values = response.Values;

            List<Student> students = new List<Student>();
            if (values != null && values.Count > 0)
            {
                values.RemoveAt(0);
                List<Client> clients = ClientsMapper.MapFromRangeData(values);

                client = clients.Where(clt => clt.Code == clientCode).FirstOrDefault();
            }
            if(client != null)
            {
                return client.GoogleSheetId;
            }
            else
            {
                return null;
            }
        }
    }
}
