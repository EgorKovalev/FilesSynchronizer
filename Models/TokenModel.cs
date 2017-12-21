namespace Models
{
    public class TokenModel
    {
        public string access_token  { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
    }

    public class DropboxTokenModel
    {
        public string access_token { get; set; }
        public string account_id { get; set; }
        public int uid { get; set; }
        public string token_type { get; set; }
    }
}
