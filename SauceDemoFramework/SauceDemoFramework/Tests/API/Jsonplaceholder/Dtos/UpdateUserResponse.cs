using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SauceDemoFramework.Tests.API.Reqres.Dtos
{
    internal sealed class UpdateUserResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Job { get; set; } = string.Empty;

        [JsonPropertyName("updatedAt")]
        public string UpdatedAt { get; set; } = string.Empty;
    }
}