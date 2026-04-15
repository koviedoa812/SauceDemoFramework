using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemoFramework.Tests.API.Reqres.Dtos
{
    internal sealed class UnknownListResponse
    {
        public List<UnknownDto> Data { get; set; } = new();
    }
}
