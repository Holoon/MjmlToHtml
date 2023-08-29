namespace Holoon.MjmlToHtml;
public class Options
{
    public Options()
    {
        TemplateCustomFunctions = new FunctionCollection();
    }
    public string? I18NBasePath { get; set; }
    public FunctionCollection TemplateCustomFunctions { get; }
}
