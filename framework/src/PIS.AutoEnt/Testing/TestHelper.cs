using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace PIS.AutoEnt
{
    public sealed class TestHelper
    {
        /// <summary>
        /// 获取方法堆栈调用信息
        /// </summary>
        /// <returns></returns>
        public static string GetStackTraceInfo()
        {
            string rtn = String.Empty;

            StackTrace st = new StackTrace(true);
            StackFrame[] sfs = st.GetFrames();

            foreach (StackFrame tsf in sfs)
            {
                rtn += tsf.GetMethod().ToString();
            }

            return rtn;
        }
    }
}
