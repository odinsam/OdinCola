using System.ComponentModel;

namespace Cola.CaWebApi;

public enum WebApiMediaType
{
    [Description("text/html")] MEDIATYPE_TEXT_HTML,
    [Description("text/plain")] MEDIATYPE_TEXT_PLAIN,
    [Description("text/xml")] MEDIATYPE_TEXT_XML,
    [Description("text/x-markdown")] MEDIATYPE_TEXT_MARKDOWN,
    [Description("image/gif")] MEDIATYPE_IMAGE_GIF,
    [Description("image/jpeg")] MEDIATYPE_IMAGE_JPEG,
    [Description("image/png")] MEDIATYPE_IMAGE_PNG,
    [Description("application/xhtml+xml")] MEDIATYPE_APPLICATION_XHTML_XML,
    [Description("application/xml")] MEDIATYPE_APPLICATION_XML,
    [Description("application/json")] MEDIATYPE_APPLICATION_JSON,
    [Description("application/pdf")] MEDIATYPE_APPLICATION_PDF,
    [Description("application/msword")] MEDIATYPE_APPLICATION_MSWORD,

    [Description("application/octet-stream")]
    MEDIATYPE_APPLICATION_OCTET_STREAM,

    [Description("application/x-www-form-urlencoded")]
    MEDIATYPE_APPLICATION_X_WWW_FORM_URLENCODED,
    [Description("multipart/form-data")] MEDIATYPE_MULTIPART_FORM_DATA
}