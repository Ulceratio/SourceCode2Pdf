using System;
using System.Collections.Generic;
using System.Text;

namespace SourceCode2Pdf.PythonScriptsExecution
{
    public interface IPythonScriptExecutor<T>
    {
        T Execute(string script, string resultVariableName);
    }
}
