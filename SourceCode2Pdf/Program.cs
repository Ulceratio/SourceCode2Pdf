using SourceCode2Pdf.HtmlToPdf;
using SourceCode2Pdf.SourceCodeToHtml;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;


namespace SourceCode2Pdf
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var a = new SourceCodeToHtmlConverter();
            var result = await a.Convert(@"C:\Users\aleki\source\repos\proleit.backend.cip");

            var b = new HtmlToPdfConverter();

            var pdf = await b.Convert(result);

            //File.WriteAllText(@"C:\Users\aleki\source\repos\mkn_cip_new\result.html", result);

            await File.WriteAllBytesAsync(@"C:\Users\aleki\source\repos\mkn_cip_new\result.pdf", pdf);

            Console.ReadLine();
        }
    }
}
