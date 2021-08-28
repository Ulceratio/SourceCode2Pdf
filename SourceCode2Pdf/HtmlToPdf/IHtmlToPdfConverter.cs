using System.Threading.Tasks;

namespace SourceCode2Pdf.HtmlToPdf
{
    public interface IHtmlToPdfConverter
    {
        Task<byte[]> Convert(string html);
    }
}