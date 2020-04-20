using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviIDApiCore.V1.DTOs.System
{
    public class PaginatedResponse<T> : BaseResponse
    {
        public PaginationData<T> Data { get; set; }
    }
}
