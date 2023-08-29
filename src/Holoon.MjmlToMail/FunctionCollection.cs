using Newtonsoft.Json.Linq;

namespace Holoon.MjmlToHtml;
public class FunctionCollection
{
    private readonly List<(string FunctionName, Type ArgumentType, Func<object?, string>? Function)> _Functions;
    public FunctionCollection()
    {
        _Functions = new();
    }
    internal IEnumerable<(string FunctionName, Type ArgumentType, Func<object?, string>? Function)> GetAll() => _Functions.ToList();
    public void Add<T>(string functionName, Func<T?, string> function)
        where T : class => _Functions.Add((functionName, typeof(T), obj => function?.Invoke(obj as T) ?? String.Empty));

    public int Count => _Functions.Count;
    public bool ContainsKey(string key) => _Functions.Any(f => f.FunctionName == key);
    public bool Remove(string key) => _Functions.RemoveAll(f => f.FunctionName == key) != 0;
    public void Clear() => _Functions.Clear();
}
