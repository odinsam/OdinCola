using System.Security.Cryptography;
using Cola.CaUtils.Enums;

namespace Cola.CaUtils.Helper;

/// <summary>
///     RandomHelper
/// </summary>
public class RandomHelper
{
    /// <summary>
    ///     ~ 随机种子值
    /// </summary>
    /// <returns></returns>
    public static int GetRandomSeed()
    {
        var bytes = new byte[4];
        var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }

    public static int GetRandomByWeight(List<int> lstWeight)
    {
        if (lstWeight == null)
            throw new ColaException(EnumException.ParamNotNull);
        if (lstWeight.Count == 0)
            return 0;
        var length = lstWeight.Count;
        //将权重数组分段
        for (var i = 1; i < length; i++) lstWeight[i] = lstWeight[i] + lstWeight[i - 1];
        var total = lstWeight[length - 1];
        int random;
        //初始化一个数组来统计出现的次数
        var count = new int[lstWeight.Count];
        //生成小于等于总权重的随机数
        random = new Random().Next(1, total);
        for (var j = 0; j < length; j++)
            if (random <= lstWeight[j])
                //修改出现的次数
                return j;
        return -1;
    }

    /// <summary>
    ///     ~  按权重返回对应需要个数的数组
    /// </summary>
    /// <param name="list">受权重影响的数组</param>
    /// <param name="count">需要返回的个数</param>
    /// <typeparam name="T">权重对象的类型，唯一标识</typeparam>
    /// <returns></returns>
    public static IEnumerable<KeyValuePair<T, int>> GetRandomListByWeight<T>(List<KeyValuePair<T, int>> list, int count)
    {
        if (list == null)
            throw new ColaException(EnumException.ParamNotNull);
        if (count <= 0)
            throw new ColaException(EnumException.ParamNotLteZero);
        if (list.Count <= count) return list;
        //计算权重总和
        var totalWeights = list.Sum(ls => ls.Value);
        //随机赋值权重
        var ran = new Random(GetRandomSeed()); //GetRandomSeed()随机种子，防止快速频繁调用导致随机一样的问题 
        var weightlst = new List<KeyValuePair<T, int>>();
        for (var i = 0; i < list.Count; i++)
        {
            var w = list[i].Value + 1 + ran.Next(0, totalWeights); // （权重+1） + 从0到（总权重-1）的随机数
            weightlst.Add(new KeyValuePair<T, int>(list[i].Key, w));
        }

        //排序
        weightlst.Sort(
            delegate(KeyValuePair<T, int> kvp1, KeyValuePair<T, int> kvp2) { return kvp2.Value - kvp1.Value; });
        return weightlst.Take(count);
    }
}