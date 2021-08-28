using System.Threading.Tasks;

namespace SourceCode2Pdf.SourceCodeToHtml
{
    public interface ISourceCodeToHtmlConverter
    {
        Task<string> Convert(string path);
    }
}