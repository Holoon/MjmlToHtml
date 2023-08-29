namespace Holoon.MjmlToHtml;
public class LanguageContext
{
    public LanguageContext(object? templateArgument, string language)
    {
        TemplateArgument = templateArgument;
        Language = language;
    }
    public virtual object? TemplateArgument { get; set; }
    public string Language { get; set; }
}