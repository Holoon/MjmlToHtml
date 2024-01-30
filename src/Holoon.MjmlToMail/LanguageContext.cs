namespace Holoon.MjmlToHtml;
public class LanguageContext
{
    public LanguageContext(object? templateArgument, string language, object? args)
    {
        TemplateArgument = templateArgument;
        Language = language;
        Args = args;
    }
    public virtual object? TemplateArgument { get; set; }
    public string Language { get; set; }
    public object? Args { get; set; }
}