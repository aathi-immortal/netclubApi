using Microsoft.IdentityModel.Tokens;
using NetClubApi.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace NetClubApi.Comman
{


    public interface IHelper
    {
        public string generateToken(UserModel user);
        public string DecodeBase64(string strvalue);
        public string EncodeBase64(string strvalue);
    }
    public class Helper:IHelper
    {
        public string DecodeBase64(string strvalue)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(strvalue);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        public string EncodeBase64(string strvalue)
        {
            try
            {
                byte[] encData_byte = new byte[strvalue.Length];
                encData_byte = Encoding.UTF8.GetBytes(strvalue);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        

        public string generateToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("apwmdlliendaddnetknz=3mlkd652341");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("email", user.Email) ,new Claim("id",user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
