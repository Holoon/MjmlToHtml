using I18Next.Net;
using I18Next.Net.Backends;
using I18Next.Net.Plugins;

namespace Holoon.MjmlToHtml;
internal class I18Next
{
    private readonly DefaultTranslator _Translator;
    private readonly I18NextNet _I18Next;
    public I18Next(string basePath)
    {
        _Backend = new JsonFileBackend(basePath);
        _Translator = new(_Backend);
        _I18Next = new(_Backend, _Translator)
        {
            FallbackLanguages = new[] { "en" }
        };
    }
    private readonly ITranslationBackend _Backend;
    public string Translate(string? key, string? language, object? args)
    {
        return _I18Next.T(language, key, args);
    }
}
