using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.DTOs.System
{
    public class PaginationData<T>
    {
        public int TotalEntries { get; set; }
        public int PageEntries { get; set; }
        public int TotalPages { get; set; }      
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
        public List<T> PageData { get; set; }
    }
}
