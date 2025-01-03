namespace PM.API.JWT
{
    public class JWTPayload
    {

        public string sub { get; set; } = default!;
        public DateTime exp { get; set; } = default!;
        public string jti { get; set; }

        public JWTPayload(string _sub, DateTime _exp, string _jti)
        {
            sub = _sub;
            exp = _exp;
            jti = _jti;
        }
    }
}
