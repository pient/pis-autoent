using System;
using System.Diagnostics;

namespace PIS.AutoEnt.Data
{
    [DebuggerDisplay("Status: {Status}")]
    public class DbOperationStatus : OperationStatus
    {
        public int RecordsAffected { get; set; }
        new public DbErrorCode ErrorCode { get; set; }
    }
}
