using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public enum EFAType : int
    {
        // NOTE: HARDCODE THE CODES BECAUSE THEY COME OUT DIRECTLY FROM DB AS
        // RAW AND WE NEED TO MAKE SURE WE ARE EXPLICIT
        BREDTH = 0,
        UPPER = 1,
        DEPTH = 2,
        TECHNICAL = 3,
        OTHER = 4
    }
}