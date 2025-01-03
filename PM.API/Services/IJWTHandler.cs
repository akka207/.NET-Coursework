using StaffManagerModels;

namespace PM.API.Services
{
    public interface IJWTHandler
    {
        /// <summary>
        /// Writes newly generated JWT and refresh tokens.
        /// </summary>
        /// <param name="staff"></param>
        /// <returns>JSON of JWT and refresh token "{"jwt":"...","refreshToken":"..."}"</returns>
        Task<string> RegisterJWTAsync(Person person);

        /// <summary>
        /// Validate token by signature and expiration date.
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns>"ERROR: Token expired" if token expired, 
        /// "ERROR: Invalide token" if signature not matches, 
        /// Staff id if everything OK
        /// </returns>
        string ValidateJWT(string jwt, bool ignoreTime);

        /// <summary>
        /// Update JWT generating new id to it. Updates JWT expiration.
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="refreshToken"></param>
        /// <returns>
        /// "ERROR: Refresh token expired".
        /// "ERROR: Invalide access token" if decoded jwt from parameters do not match.
        /// "ERROR: Unauthorized" if can`t find record of refreshToken in database.
        /// New JWT if everything success.
        /// </returns>
        Task<string> RefreshAsync(string jwt, string refreshToken);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="jwt"></param>
        ///// <returns>"sub" element of JWT payload</returns>
        //string GetSub(string jwt);
    }
}
