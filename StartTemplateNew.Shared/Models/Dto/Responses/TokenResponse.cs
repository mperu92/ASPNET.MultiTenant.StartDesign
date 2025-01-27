using Newtonsoft.Json;

namespace StartTemplateNew.Shared.Models.Dto.Responses
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public required string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public required string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }
        public required string Scope { get; set; }
    }
}
