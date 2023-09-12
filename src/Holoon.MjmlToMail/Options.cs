using Mjml.Net;
using Scriban.Parsing;
using Scriban.Runtime;

namespace Holoon.MjmlToHtml;
public class Options
{
    public Options()
    {
        TemplateCustomFunctions = new FunctionCollection();
    }
    public string? I18NBasePath { get; set; }
    public FunctionCollection TemplateCustomFunctions { get; }
    public Action<IEnumerable<LogMessage>>? OnScribanErrors { get; set; }
    public Action<IEnumerable<ValidationError>>? OnMjmlErrors { get; set; }
    public ITemplateLoader? TemplateLoader { get; set; }
}
