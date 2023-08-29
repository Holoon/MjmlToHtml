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
            context => i18N.Translate((context?.TemplateArgument as string) ?? String.Empty, context?.Language));

        _MjmlRenderer = new MjmlRenderer();
    }
    public Options Options { get; set; }

    public object GetMail<T>(string templateFilename, T data, string language, Format format = Format.Mjml)
    {
        var templateText = File.ReadAllText(templateFilename); 
        var template = Template.Parse(templateText, 
            sourceFilePath: templateFilename); // NOTE: This parameter is useful only for debug messages.

        var context = new TemplateContext();
        var scriptObject1 = new ScriptObject();

        foreach (var func in _TemplateDefaultFunctions.GetAll().Union(Options.TemplateCustomFunctions.GetAll()))
        {
            if (func.ArgumentType.IsAssignableTo(typeof(LanguageContext)))
            {
                Func<object?, string?>? function = data => func.Function?.Invoke(new LanguageContext(data, language));
                scriptObject1.Import(func.FunctionName, function);
            }
            else
            {
                scriptObject1.Import(func.FunctionName, func.Function);
            }
        }

        scriptObject1.Import(data);
        context.PushGlobal(scriptObject1);

        var result = template.Render(context);

        if (format == Format.Mjml)
        {
            object? error = null;
            (result, error) = _MjmlRenderer.Render(result);
        }
        // TODO : Log

        return result;
    }
}
