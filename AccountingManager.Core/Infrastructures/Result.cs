using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingManager.Core.Infrastructures {
    public class Result {
        public bool Status { get; set; }
        public string ErrMsg { get; set; }

        public static Result Success = new Result { Status = true };
    }
}
