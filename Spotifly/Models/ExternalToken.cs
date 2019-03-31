using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotifly.Models
{
    public class ExternalToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
    }
}
