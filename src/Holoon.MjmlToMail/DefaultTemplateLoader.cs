using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;

namespace Holoon.MjmlToHtml;
public class DefaultTemplateLoader : ITemplateLoader
{
    public DefaultTemplateLoader(string? templateBasePath = null)
    {
        TemplateBasePath = templateBasePath ?? String.Empty;
    }
    public string TemplateBasePath { get; set; }
    public async ValueTask<string> LoadAsync(TemplateContext context, SourceSpan callerSpan, string templatePath) => await File.ReadAllTextAsync(templatePath);
    public string GetPath(TemplateContext context, SourceSpan callerSpan, string templateName) => Path.Combine(TemplateBasePath, templateName);
    public string Load(TemplateContext context, SourceSpan callerSpan, string templatePath) => File.ReadAllText(templatePath);
}