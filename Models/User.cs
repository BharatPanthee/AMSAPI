namespace AMSAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ClientCode { get; set; }
        public string? GoogleSheetId { get; set; }
        public bool? IsValid { get; set; }
        public string? Error { get; set; }
    }
    
}
