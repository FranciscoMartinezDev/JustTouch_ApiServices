using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTouch_Shared.Dtos
{
    public class SessionDto
    {
        public string email { get; set; } = string.Empty;
        public string accessToken { get; set; } = string.Empty;
        public string refreshToken { get; set; } = string.Empty;
        public string tokenType { get; set; } = string.Empty;
        public long expiresIn { get; set; }
        public DateTime expiresAt { get; set; }
    }
}
