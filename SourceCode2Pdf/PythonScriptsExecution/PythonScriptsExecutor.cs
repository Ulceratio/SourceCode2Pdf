using System;
using System.Collections.Generic;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace SourceCode2Pdf.PythonScriptsExecution
{
    public class PythonScriptExecutor<T> : IPythonScriptExecutor<T>
    {
        public T Execute(string script, string resultVariableName)
        {
            T result = default(T);

            ScriptEngine pythonScript = Python.CreateEngine();
            ScriptScope scope = pythonScript.CreateScope();
            pythonScript.ExecuteFile(script, scope);
            result = scope.GetVariable(resultVariableName);
            return result;

        }
    }
}
