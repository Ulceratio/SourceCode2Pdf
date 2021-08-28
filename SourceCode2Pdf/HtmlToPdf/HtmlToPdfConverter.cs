using OpenHtmlToPdf;
using System.Threading.Tasks;

namespace SourceCode2Pdf.HtmlToPdf
{
    public class HtmlToPdfConverter : IHtmlToPdfConverter
    {
        public async Task<byte[]> Convert(string html)
        {
            return await Task.Run(() =>
            {
                return Pdf
                       .From(html)
                       .OfSize(PaperSize.A4)
                       //.WithOutline()
                       //.WithTitle("Title")
                       //.WithoutOutline()
                       //.WithMargins(1.25.Centimeters())
                       //.Portrait()
                       //.Comressed()
                       .Content();
            });
        }
    }
}
