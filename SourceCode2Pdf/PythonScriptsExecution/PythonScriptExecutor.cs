using System;
using System.Collections.Generic;
using System.Text;

namespace SourceCode2Pdf.PythonScriptsExecution
{
    interface PythonScriptExecutor<T>
    {
        T Execute(string script);
    }
}
