using System.ComponentModel.DataAnnotations;

namespace PM.API.JWT
{
    public class JWTStoredRecord
    {
        public int Id { get; set; }
        public string JWT { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
