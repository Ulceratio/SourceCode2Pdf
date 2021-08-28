using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SourceCode2Pdf.SourceCodeToHtml
{
    public class SourceCodeToHtmlConverter : ISourceCodeToHtmlConverter
    {
        //private const string HILITE_ME_URL = "http://hilite.me/";
        private const string HILITE_ME_URL = "http://192.168.50.195:5000/";
        private readonly IRestClient _restClient = new RestClient(HILITE_ME_URL);

        private const string CODE_PARAMETER = "code";
        private const string LEXER_PARAMETER = "lexer";
        private const string API = "api";

        private Dictionary<string, string> _extensionToLexerMappings = new Dictionary<string, string>
        {
            { "cs", "csharp" },
            { "xaml", "xml" },
            { "mrt", "xml" },
        };

        private async Task<IEnumerable<string>> GetFiles(string path) => await Task.Run(() =>
        {
            string[] extensionCriterias = _extensionToLexerMappings.Select(x => @$"^.*\.({x.Key})$").ToArray();

            return Directory
                   .EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                   .Where(file => extensionCriterias.Any(criteria => Regex.IsMatch(file, criteria)));
        });

        private string CreateHtmlPage(string body)
        {
            return $@"<!DOCTYPE html>
                          <html>
                          <body>
                          
                          { body }
                          
                          </body>
                          </html>";
        }

        private string AddTitleToPage(string title, string page) => $@"<h1 style=""color: #5e9ca0;"">{title}</h1>{page}";

        private string WrapTextInLines(string source)
        {
            var lines = source.Split(Environment.NewLine);

            IEnumerable<string> SplitText(string text, int length)
            {
                for (int i = 0; i < text.Length; i += length)
                {
                    yield return text.Substring(i, Math.Min(length, text.Length - i));
                }
            }

            return String.Join(Environment.NewLine, lines.SelectMany(x => 
            {
                if (x.Length >= 100)
                {
                    var splitedText = SplitText(x, 100);

                    return splitedText;
                }
                else
                {
                    return new string[] { x };
                }
            }));
        }

        public async Task<string> Convert(string path)
        {
            return await Task.Run(async () =>
            {
                var files = (await GetFiles(path))/*.Take(10)*/.ToList();

                #region Fastest but hilite.me is cutting off too many requests
                //var pages = (await Task.WhenAll(files
                //            .AsParallel()
                //            .Select(async file =>
                //            {
                //                var restRequest = new RestRequest(API, Method.POST) { AlwaysMultipartFormData = true };

                //                restRequest.AddParameter(CODE_PARAMETER, File.ReadAllText(file));

                //                foreach (var extensionToLexerMapping in _extensionToLexerMappings)
                //                {
                //                    if (Regex.IsMatch(file, @$"^.*\.({extensionToLexerMapping.Key})$"))
                //                    {
                //                        restRequest.AddParameter(LEXER_PARAMETER, extensionToLexerMapping.Value);
                //                    }
                //                }

                //                var html = await _restClient.ExecuteAsync<string>(restRequest);

                //                return (file, html);

                //            })))
                //            .Where(x => x.html.StatusCode == HttpStatusCode.OK)
                //            .Select(x => AddTitleToPage(Path.GetFileName(x.file), x.html.Content))
                //            .ToList();
                #endregion

                var pages = files
                            .Select(file =>
                            {
                                var restRequest = new RestRequest(API, Method.POST) { AlwaysMultipartFormData = true };

                                var code = File.ReadAllText(file);

                                var wrappedText = WrapTextInLines(code);

                                restRequest.AddParameter(CODE_PARAMETER, wrappedText);

                                foreach (var extensionToLexerMapping in _extensionToLexerMappings)
                                {
                                    if (Regex.IsMatch(file, @$"^.*\.({extensionToLexerMapping.Key})$"))
                                    {
                                        restRequest.AddParameter(LEXER_PARAMETER, extensionToLexerMapping.Value);
                                    }
                                }

                                var html = _restClient.Execute<string>(restRequest);

                                return (file, html);
                            })
                            .Where(x => x.html.StatusCode == HttpStatusCode.OK)
                            .Select(x => AddTitleToPage(Path.GetFileName(x.file), x.html.Content))
                            .ToList();

                return CreateHtmlPage(String.Join("\n", pages));
            });
        }
    }
}