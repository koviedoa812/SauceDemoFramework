using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemoFramework.Tests.API.Reqres.Dtos
{
    internal class SingleUserResponse
    {
        public UserDto Data { get; set; } = new();
    }
}
