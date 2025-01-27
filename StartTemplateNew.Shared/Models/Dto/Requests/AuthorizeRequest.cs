using Newtonsoft.Json;

namespace StartTemplateNew.Shared.Models.Dto.Requests
{
    public class AuthorizeRequest
    {
        [JsonProperty("response_type")]
        public string ResponseType { get; set; } = default!;
        [JsonProperty("client_id")]
        public string ClientId { get; set; } = default!;
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; } = default!;
        public string? Scope { get; set; }
        public string State { get; set; } = default!;
    }
}