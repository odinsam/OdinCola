namespace Cola.CaUtils.Helper;

/// <summary>
///     GenerationCodeHelper
/// </summary>
public class GenerationCodeHelper
{
    private static int _randomIndex;

    private static readonly List<string> LstNumber = new() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

    private static readonly List<string> LstValidateCodeNumber = new() { "2", "3", "4", "5", "6", "7", "8", "9" };

    private static readonly List<string> LstUpperLetter = new()
    {
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V",
        "W", "X", "Y", "Z"
    };

    private static readonly List<string> LstValidateCodeUpperLetter = new()
    {
        "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X",
        "Y", "Z"
    };

    private static readonly List<string> LstLowerLetter = new()
    {
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v",
        "w", "x", "y", "z"
    };

    private static readonly List<string> LstValidateCodeLowerLetter = new()
    {
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x",
        "y", "z"
    };

    private static readonly List<string> LstSpecialCode = new()
    {
        "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+", "-", "=", "[", "]", "{", "}", "\\", "|", ";",
        ":", "'", "\"", ",", "<", ".", ">", "/", "?"
    };

    /// <summary>
    ///     按长度生成对应的字符串
    /// </summary>
    /// <param name="length">长度</param>
    /// <param name="containNum">是否包含数字</param>
    /// <param name="containLetter">是否包含字母</param>
    /// <param name="containSpecial">是否包含特殊字符</param>
    /// <returns>生成的字符串</returns>
    public static string GenerationCode(int length, bool containNum = true, bool containLetter = true,
        bool containSpecial = true)
    {
        var code = new List<string>();
        if (containNum)
            code = code.Union(LstNumber).ToList();
        if (containLetter)
            code = code.Union(LstUpperLetter).Union(LstLowerLetter).ToList();
        if (containSpecial)
            code = code.Union(LstSpecialCode).ToList();
        var result = new List<string>();
        for (var i = 0; i < length; i++)
        {
            _randomIndex = new Random((int)DateTime.Now.Ticks).Next(0, code.Count());
            result.Add(code[_randomIndex]);
            Thread.Sleep(_randomIndex);
        }

        return string.Join("", result);
    }

    /// <summary>
    ///     不包含 0 o 1 I 等容易混淆的之母和数字
    /// </summary>
    /// <param name="length">长度</param>
    /// <param name="containNum">是否包含数据</param>
    /// <param name="containLetter">是否包含字母</param>
    /// <param name="containSpecial">是否包含特殊字符</param>
    /// <returns>生成的字符串</returns>
    public static string GenerationlimpidCode(int length, bool containNum = true, bool containLetter = true,
        bool containSpecial = true)
    {
        var code = new List<string>();
        if (containNum)
            code = code.Union(LstValidateCodeNumber).ToList();
        if (containLetter)
            code = code.Union(LstValidateCodeUpperLetter).Union(LstValidateCodeLowerLetter).ToList();
        if (containSpecial)
            code = code.Union(LstSpecialCode).ToList();
        var result = new List<string>();
        for (var i = 0; i < length; i++)
        {
            _randomIndex = new Random((int)DateTime.Now.Ticks).Next(0, code.Count);
            result.Add(code[_randomIndex]);
            Thread.Sleep(_randomIndex);
        }

        return string.Join("", result);
    }
}