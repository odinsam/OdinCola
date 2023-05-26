using Cola.CaUtils.Enums;
using Cola.CaUtils.Extensions;

namespace Cola.CaUtils;

public class ColaException : Exception
{
    public ColaException(EnumException enumException) : base(enumException.GetDescription())
    {
    }

    public ColaException(EnumException enumException, string msg) : base(string.Format(enumException.GetDescription(),
        msg))
    {
    }

    public ColaException(string errorMessage) : base(errorMessage)
    {
    }
}