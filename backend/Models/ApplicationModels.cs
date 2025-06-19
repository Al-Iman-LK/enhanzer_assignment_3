using System.Text.Json.Serialization;

namespace backend.Models
{    public class LocationDetail
    {
        public int Id { get; set; }
        public string Location_Code { get; set; } = string.Empty;
        public string Location_Name { get; set; } = string.Empty;
        public int Stock_Handle { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Status { get; set; }
    }public class LoginRequest
    {
        [JsonPropertyName("API_Action")]
        public string API_Action { get; set; } = "GetLoginData";
        [JsonPropertyName("Device_Id")]
        public string Device_Id { get; set; } = "D001";
        [JsonPropertyName("Sync_Time")]
        public string Sync_Time { get; set; } = "";
        [JsonPropertyName("Company_Code")]
        public string Company_Code { get; set; } = "info@enhanzer.com";
        [JsonPropertyName("API_Body")]
        public ApiBody API_Body { get; set; } = new ApiBody();
    }

    public class ApiBody
    {
        [JsonPropertyName("Username")]
        public string Username { get; set; } = string.Empty;
        [JsonPropertyName("Pw")]
        public string Pw { get; set; } = string.Empty;
    }    public class LoginResponse
    {
        [JsonPropertyName("Success")]
        public bool Success { get; set; }
        [JsonPropertyName("Message")]
        public string Message { get; set; } = string.Empty;
        [JsonPropertyName("User_Locations")]
        public List<UserLocation> User_Locations { get; set; } = new List<UserLocation>();
    }public class UserLocation
    {
        [JsonPropertyName("Location_Code")]
        public string Location_Code { get; set; } = string.Empty;
        [JsonPropertyName("Location_Name")]
        public string Location_Name { get; set; } = string.Empty;
        [JsonPropertyName("Stock_Handle")]
        public int Stock_Handle { get; set; }
        [JsonPropertyName("Address")]
        public string Address { get; set; } = string.Empty;
        [JsonPropertyName("Phone")]
        public string Phone { get; set; } = string.Empty;
        [JsonPropertyName("Status")]
        public int Status { get; set; }
    }

    public class PurchaseBillItem
    {
        public int Id { get; set; }
        public string Item { get; set; } = string.Empty;
        public string Batch { get; set; } = string.Empty;
        public decimal StandardCost { get; set; }
        public decimal StandardPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalSelling { get; set; }
    }
}
