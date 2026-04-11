using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DoctorLicenseManagement.Application.Queries
{
    public abstract class PageQuery
    {
        [JsonPropertyName("search")]
        public string? Search { get; set; }
 
        [JsonPropertyName("pageSize")]
        public int Page { get; set; } = 1;

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 10;
    }
}
