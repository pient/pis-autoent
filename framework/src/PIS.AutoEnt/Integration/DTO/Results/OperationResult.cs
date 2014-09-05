using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    [Serializable]
    public class OperationResult
    {
        public OperationResult(bool success, string message = null, string code = null)
        {
            this.Success = success;
            this.Message = message;
            this.Code = code;
        }

        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public object Tag { get; set; }
    }
}
