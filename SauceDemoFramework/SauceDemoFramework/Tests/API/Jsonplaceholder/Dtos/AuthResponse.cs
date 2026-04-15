using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemoFramework.Tests.API.Reqres.Dtos
{
    internal sealed class AuthResponse
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
    }

}
