namespace AMSAPI.Models
{
    public class Client
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Details { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? Status { get; set; }
        public string? UserId { get; set; }
        public string? Password { get; set; }
        public string? GoogleSheetId { get; set; }
    }

    public class ClientUser
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
        public DateTime? Validity { get; set; }
    }

    public static class ClientsMapper
    {
        public static List<Client> MapFromRangeData(IList<IList<object>> values)
        {
            var clients = new List<Client>();
            foreach (var value in values)
            {
                Client client = new()
                {
                    Id = value[0].ToString(),
                    Name = value[1].ToString(),
                    Code = value[2].ToString(),
                    Details = value[3].ToString(),
                    StartDate = string.IsNullOrEmpty(value[4].ToString()) ? DateTime.MinValue : Convert.ToDateTime(value[4].ToString()),
                    EndDate = string.IsNullOrEmpty(value[5].ToString()) ? DateTime.MinValue : Convert.ToDateTime(value[5].ToString()),
                    Status = Convert.ToInt16((value[6] ?? 0).ToString()),
                    UserId = value[7].ToString(),
                    Password = value[8].ToString(),
                    GoogleSheetId = value[9].ToString(),
                };
                clients.Add(client);
            }
            return clients;
        }
    }
    public static class ClientUserMapper
    {
        public static List<ClientUser> MapFromRangeData(IList<IList<object>> values)
        {
            var clientUsers = new List<ClientUser>();
            foreach (var value in values)
            {
                ClientUser clientUser = new()
                {
                    Id =  Int32.Parse(value[0].ToString()??"0"),
                    Username = value[1].ToString(),
                    Password = value[2].ToString(),
                    Token = value[3].ToString(),
                    Validity = DateTime.Parse(value[4].ToString()??"2023-01-01"),
                };
                clientUsers.Add(clientUser);
            }
            return clientUsers;
        }

    }
}
