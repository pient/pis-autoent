using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    [Serializable]
    public class ExecutionResult : OperationResult
    {
        public ExecutionResult(bool success, string message = null, string code = null)
            : base(success, message, code)
        {
        }
    }
}
