using System.Text;

namespace StartTemplateNew.Shared.Models
{
    public class IssuerCredentials
    {
        public required string Issuer { get; set; }
        public required string Audiences { get; set; }
        public required string SigningKey { get; set; }

        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateAudience { get; set; } = true;
        public bool ValidateIssuerSigningKey { get; set; } = true;
        public bool ValidateLifetime { get; set; } = true;

        public ISet<string> AudienceList
            => GetAudienceList();

        public byte[] SigningKeyBytes
            => Encoding.UTF8.GetBytes(SigningKey);

        private HashSet<string> GetAudienceList()
            => Audiences.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()).ToHashSet();
    }
}
