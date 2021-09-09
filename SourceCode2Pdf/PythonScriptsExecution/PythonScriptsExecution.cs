using System;
using System.Collections.Generic;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace SourceCode2Pdf.PythonScriptsExecution
{
    public class PythonScriptsExecution <T>
    {
        public T Execute(string file)
        {
            ScriptEngine pythonScript = Python.CreateEngine();
            ScriptScope scope = pythonScript.CreateScope();
            pythonScript.ExecuteFile(file);
            return scope.GetVariable("result");// изменить имя переменной в зависимости от ее имени в скрипте
        }

    }
}
