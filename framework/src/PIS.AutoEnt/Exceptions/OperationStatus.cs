using System;
using System.Diagnostics;

namespace PIS.AutoEnt
{
    [DebuggerDisplay("Status: {Status}")]
    public class OperationStatus
    {
        public bool Status { get; set; }
        public ErrorCode ErrorCode { get; set; }
        public string Message { get; set; }
        public Object OperationID { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string ExceptionInnerMessage { get; set; }
        public string ExceptionInnerStackTrace { get; set; }

        public static OperationStatus CreateFromException(Exception ex)
        {
            return CreateFromException(ex.Message, ex);
        }

        public static OperationStatus CreateFromException(string message, Exception ex)
        {
            OperationStatus opStatus = new OperationStatus
            {
                Status = false,
                Message = message,
                OperationID = null
            };

            if (ex != null)
            {
                opStatus.ExceptionMessage = ex.Message;
                opStatus.ExceptionStackTrace = ex.StackTrace;
                opStatus.ExceptionInnerMessage = (ex.InnerException == null) ? null : ex.InnerException.Message;
                opStatus.ExceptionInnerStackTrace = (ex.InnerException == null) ? null : ex.InnerException.StackTrace;
            }
            return opStatus;
        }

        public static OperationStatus CreateFromException(PISException pisex)
        {
            return CreateFromException(pisex.Message, pisex);
        }

        public static OperationStatus CreateFromException(string message, PISException pisex)
        {
            OperationStatus opStatus = CreateFromException(message, pisex);
            opStatus.ErrorCode = pisex.ErrorCode;

            return opStatus;
        }
    }
}
