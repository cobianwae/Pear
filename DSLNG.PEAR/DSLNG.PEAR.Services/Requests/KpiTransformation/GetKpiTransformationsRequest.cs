﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSLNG.PEAR.Services.Requests.KpiTransformation
{
    public class GetKpiTransformationsRequest : GridBaseRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public int UserId { get; set; }
    }
}
