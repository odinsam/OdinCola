using System.Collections;
using Newtonsoft.Json.Linq;

namespace Cola.CaUtils.Helper;

public class JObjectList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
{
    private readonly JArray? jary;

    public JObjectList()
    {
        jary = new JArray();
    }

    public JObjectList(JArray? jary)
    {
        this.jary = jary;
    }

    public T this[int index]
    {
        get => (T)Convert.ChangeType(jary![index], typeof(T));
        set => jary![index] = JObject.FromObject(value!);
    }

    public int Count => jary!.Count;

    public bool IsReadOnly => throw new NotImplementedException();

    public void Add(T item)
    {
        jary!.Add(JObject.FromObject(item!));
    }

    public void Clear()
    {
        jary!.Clear();
    }

    public bool Contains(T item)
    {
        return jary!.Contains(JObject.FromObject(item!));
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator()
    {
        var jaryEnum = (IEnumerable<T>)jary!.ToObject(typeof(IEnumerable<T>))!;
        return jaryEnum.GetEnumerator();
    }

    public bool Remove(T item)
    {
        return item != null && jary!.Remove(JObject.FromObject(item));
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public void AddRangs(IEnumerable<T> items)
    {
        foreach (var item in items) jary!.Add(JObject.FromObject(item!));
    }

    public int IndexOf(T item)
    {
        return jary!.IndexOf(JObject.FromObject(item ?? throw new ArgumentNullException(nameof(item))));
    }

    public void Insert(int index, T item)
    {
        if (item != null) jary!.Insert(index, JObject.FromObject(item));
    }

    public void RemoveAt(int index)
    {
        jary!.RemoveAt(index);
    }

    public JArray? ToJArray()
    {
        return (JArray)jary!.ToObject(typeof(JArray))!;
    }
}