using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace final_project_be.Helpers
{
    public class JWTHelper
    {
        public static string KEY = "7ed4169fc5ef61270f840f11c2daf7c6bc5a282e9ffbf87e8f6d70b93aa873309770e4152bff4a6d531313e6206b38dab67d88f6aeba677280c52e35c5e716ab";
        public static string Generate(int id_user, string role)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Sid, id_user.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            //set jwt config
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY)), SecurityAlgorithms.HmacSha256)
            );
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}