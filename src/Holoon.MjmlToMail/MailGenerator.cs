using Mjml.Net;
using Scriban;
using Scriban.Runtime;

namespace Holoon.MjmlToHtml;
public class MailGenerator
{
    private readonly FunctionCollection _TemplateDefaultFunctions;
    private readonly MjmlRenderer _MjmlRenderer;
    public MailGenerator(Options options)
    {
        Options = options;
        _TemplateDefaultFunctions = new FunctionCollection();

        var i18N = new I18Next(Options.I18NBasePath ?? String.Empty);
        _TemplateDefaultFunctions.Add<LanguageContext>(
            "t",
            context => i18N.Translate((context?.TemplateArgument as string) ?? String.Empty, context?.Language, context?.Args));

        _MjmlRenderer = new MjmlRenderer();
    }
    public Options Options { get; set; }

    public string? GetMail<T>(string templateFilename, T data, string language, Format format = Format.Mjml)
    {
        var templateText = File.ReadAllText(templateFilename);

        var template = GetTemplate(templateFilename, templateText);
        var context = GetContext(data, language);

        var result = template.Render(context);

        result = RenderMjml(format, result);
        return result;
    }
    public async Task<string?> GetMailAsync<T>(string templateFilename, T data, string language, Format format, CancellationToken cancellationToken)
    {
        var templateText = await File.ReadAllTextAsync(templateFilename, cancellationToken);

        var template = GetTemplate(templateFilename, templateText);
        var context = GetContext(data, language);

        var result = await template.RenderAsync(context);

        result = RenderMjml(format, result);
        return result;
    }

    private string RenderMjml(Format format, string result)
    {
        if (format == Format.Mjml)
        {
            (result, var errors) = _MjmlRenderer.Render(result);

            if (errors != null && errors.Count > 0)
                Options.OnMjmlErrors?.Invoke(errors);
        }
        return result;
    }
    private Template GetTemplate(string templateFilename, string templateText)
    {
        var template = Template.Parse(templateText,
            sourceFilePath: templateFilename); // NOTE: This parameter is useful only for debug messages.

        if (template.HasErrors)
        {
            Options.OnScribanErrors?.Invoke(template.Messages
                .Where(m => m.Type == Scriban.Parsing.ParserMessageType.Error));
        }

        return template;
    }
    private TemplateContext GetContext<T>(T data, string language)
    {
        var context = new TemplateContext();
        var scriptObject1 = new ScriptObject();

        foreach (var func in _TemplateDefaultFunctions.GetAll().Union(Options.TemplateCustomFunctions.GetAll()))
        {
            if (func.ArgumentType.IsAssignableTo(typeof(LanguageContext)))
            {
                string? translate(string key, object? args = null) => func.Function?.Invoke(new LanguageContext(key, language, args));
                scriptObject1.Import(func.FunctionName, translate);
            }
            else
            {
                scriptObject1.Import(func.FunctionName, func.Function);
            }
        }

        scriptObject1.Import(data);
        context.PushGlobal(scriptObject1);

        if (Options.TemplateLoader != null)
            context.TemplateLoader = Options.TemplateLoader;

        return context;
    }
}
