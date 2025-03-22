using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Dtos.Response
{
    public class ProfileResponse
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string? Location { get; set; }
        public string? Title { get; set; }
        public string? AboutMe { get; set; }
        public string? ProfileImage { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TotalVote { get; set; }
        public string AliasName { get; set; }
    }
}
