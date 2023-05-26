using Cola.CaUtils.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Cola.CaWebApi;

public class OdinBadRequest : BadRequestObjectResult
{
    public OdinBadRequest(string errorCode, string message) : base(new ErrorModel(errorCode, message))
    {
        StatusCode = errorCode.ToInt();
    }
}