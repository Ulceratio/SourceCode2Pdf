using System;
using System.Collections.Generic;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace SourceCode2Pdf.PythonScriptsExecution
{
    public class PythonScriptsExecution<T> : PythonScriptExecutor<T>
    {
        public T Execute(string scriptFile)
        {
            ScriptEngine pythonScript = Python.CreateEngine();
            ScriptScope scope = pythonScript.CreateScope();
            pythonScript.ExecuteFile(scriptFile);
            return scope.GetVariable("result");// изменить имя переменной в зависимости от ее имени в скрипте
        }

    }
}
