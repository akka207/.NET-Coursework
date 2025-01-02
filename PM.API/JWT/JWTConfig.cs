using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;

namespace PM.API.JWT
{
    public class JWTConfig
    {
        public string Header { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;

        public JWTConfig(string header, string payload)
        {
            Header = header;
            Payload = payload;
        }

        public JWTConfig() { }

        public string Encrypt(string secret)
        {
            if (Header == string.Empty || Payload == string.Empty)
                return string.Empty;

            string encHeader = Base64UrlTextEncoder.Encode(Encoding.UTF8.GetBytes(Header));
            string encPayload = Base64UrlTextEncoder.Encode(Encoding.UTF8.GetBytes(Payload));

            byte[] key = Encoding.UTF8.GetBytes(secret);

            using (var hmac = new HMACSHA256(key))
            {
                byte[] signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes($"{encHeader}.{encPayload}"));
                string signature = Base64UrlTextEncoder.Encode(signatureBytes);

                return $"{encHeader}.{encPayload}.{signature}";
            }
        }

        public bool TryDecode(string jwt, string secret)
        {
            if (string.IsNullOrEmpty(jwt))
                return false;

            string[] parts = jwt.Split('.');
            if (parts.Length != 3)
                return false;

            string encHeader = parts[0];
            string encPayload = parts[1];
            string receivedSignature = parts[2];

            Header = Encoding.UTF8.GetString(Base64UrlTextEncoder.Decode(encHeader));
            Payload = Encoding.UTF8.GetString(Base64UrlTextEncoder.Decode(encPayload));

            string signingInput = $"{encHeader}.{encPayload}";
            byte[] key = Encoding.UTF8.GetBytes(secret);

            using (var hmac = new HMACSHA256(key))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signingInput));
                string computedSignature = Base64UrlTextEncoder.Encode(computedHash);

                if (string.Equals(computedSignature, receivedSignature, StringComparison.Ordinal))
                {
                    return true;
                }
                else
                {
                    Header = string.Empty;
                    Payload = string.Empty;
                    return false;
                }
            }
        }

        public JWTPayload? DecodePayload()
        {
            if (Payload == string.Empty)
                return null;

            return JsonConvert.DeserializeObject<JWTPayload>(Payload);
        }
    }
}
