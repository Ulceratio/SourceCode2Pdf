from pygments import highlight
from pygments.lexers import get_lexer_by_name
from pygments.formatters import HtmlFormatter

code = 'int x = 11; int y = 13; int z = x + y; Console.WriteLine("Hello World!");'
lexer = 'c#'
style = 'vs'
defstyles = 'overflow:auto;width:auto;'

formatter = HtmlFormatter(style=style,
                              linenos=False,
                              noclasses=True,
                              cssclass='',
                              cssstyles=defstyles,
                              prestyles='margin: 0')

html = highlight(code, get_lexer_by_name(lexer), formatter)

print (html)
