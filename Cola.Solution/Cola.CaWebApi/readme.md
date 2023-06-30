### CaWebApi
#### 1. 配置说明
```json
{
  "ColaWebApi": [
    {
      "ClientName": "Cola",
      "BaseUri": "https://tenapi.cn",
      "TimeOut": 5000,
      "Cert": {
        "CertFilePath": "",
        "CertFilePwd": ""
      },
      /* 默认压缩方式  None,GZip,Deflate,Brotli,All */
      "Decompression": "All"
    }
  ]
}
```

#### 2. 注入说明
```csharp
builder.Services.AddSingletonColaWebApi(config);
var webApi = builder.Services.BuildServiceProvider().GetService<IWebApi>();
var colaClient = webApi!.GetClient("Cola");
var getWebApiResult = colaClient.GetWebApiAsync<Result>(
    "https://tenapi.cn/v2/getip",
    new Dictionary<string, string>
    {
        { "author", "odinsam" }
    }).GetAwaiter().GetResult();
Console.WriteLine(JsonConvert.SerializeObject(getWebApiResult).ToJsonFormatString());
```

#### 3. 接口说明
```csharp
    #region Get method

    /// <summary>
    ///     GetWebApi 方法
    /// </summary>
    /// <param name="requestUri">get uri</param>
    /// <param name="customHeaders">Dictionary customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">result type</typeparam>
    /// <returns>result </returns>
    T? GetWebApi<T>(string requestUri, Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class;
    
    /// <summary>
    /// </summary>
    /// <param name="requestUri"></param>
    /// <param name="customHeaders"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T?> GetWebApiAsync<T>(string requestUri, Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class;

        #endregion

    #region post method

    /// <summary>
    /// PostWebApi
    /// </summary>
    /// <param name="postUri">postUri</param>
    /// <param name="httpContent">httpContent</param>
    /// <param name="customHeaders">customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">post result type</typeparam>
    /// <returns>post result</returns>
    T? PostWebApi<T>(string postUri, HttpContent? httpContent,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// PostWebApiAsync
    /// </summary>
    /// <param name="postUri">postUri</param>
    /// <param name="httpContent">httpContent</param>
    /// <param name="customHeaders">customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">post result type</typeparam>
    /// <returns>post result</returns>
    Task<T?> PostWebApiAsync<T>(string postUri, HttpContent? httpContent,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class;

    #endregion

    #region put

    /// <summary>
    /// PutWebApi
    /// </summary>
    /// <param name="postUri">putUri</param>
    /// <param name="httpContent">httpContent       can use GenerateStringContent method</param>
    /// <param name="customHeaders">customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">post result type</typeparam>
    /// <returns>post result</returns>
    public T? PutWebApi<T>(string postUri, HttpContent? httpContent,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class;
    
    /// <summary>
    /// PutWebApiAsync
    /// </summary>
    /// <param name="postUri">putUri</param>
    /// <param name="httpContent">httpContent       can use GenerateStringContent method</param>
    /// <param name="customHeaders">customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">post result type</typeparam>
    /// <returns>post result</returns>
    Task<T?> PutWebApiAsync<T>(string postUri, HttpContent? httpContent,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class;

    #endregion

    #region DELETE

    /// <summary>
    /// DeleteWebApi
    /// </summary>
    /// <param name="deleteUri">deleteUri</param>
    /// <param name="httpContent">httpContent       can use GenerateStringContent method</param>
    /// <param name="customHeaders">customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">post result type</typeparam>
    /// <returns>Delete result</returns>
    T? DeleteWebApi<T>(string deleteUri, HttpContent? httpContent,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// DeleteWebApiAsync
    /// </summary>
    /// <param name="deleteUri">deleteUri</param>
    /// <param name="httpContent">httpContent       can use GenerateStringContent method</param>
    /// <param name="customHeaders">customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">post result type</typeparam>
    /// <returns>Delete result</returns>
    Task<T?> DeleteWebApiAsync<T>(string deleteUri, HttpContent? httpContent,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class;

    #endregion   
    
    #region OPTIONS

    /// <summary>
    /// optionsWebApi
    /// </summary>
    /// <param name="optionsUri">optionsUri</param>
    /// <param name="customHeaders">customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">post result type</typeparam>
    /// <returns>Delete result</returns>
    T? OptionsWebApi<T>(string optionsUri,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// optionsWebApiAsync
    /// </summary>
    /// <param name="optionsUri">optionsUri</param>
    /// <param name="customHeaders">customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">post result type</typeparam>
    /// <returns>Delete result</returns>
    Task<T?> OptionsWebApiAsync<T>(string optionsUri,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class;

    #endregion

    /// <summary>
    /// 邮件发送
    /// </summary>
    /// <param name="smtpClientModel">smtp客户端</param>
    /// <param name="fromUser">发件人</param>
    /// <param name="toUsers">收件人</param>
    /// <param name="subject">主题</param>
    /// <param name="body">内容</param>
    /// <param name="ccUsers">抄送人</param>
    /// <param name="bccUsers">密送人</param>
    /// <param name="filePaths">附件filePaths</param>
    void SendEmail(SmtpClientModel smtpClientModel, MailUser fromUser, List<MailUser> toUsers, string subject,
        string body, List<MailUser>? ccUsers, List<MailUser>? bccUsers, List<string>? filePaths);
    
    #region GenerateHttpContent

    /// <summary>
    /// GenerateStringContent
    /// </summary>
    /// <param name="data">data</param>
    /// <param name="encoding">default utf8</param>
    /// <returns>StringContent</returns>
    HttpContent GenerateStringContent(string data, Encoding? encoding = null);

    /// <summary>
    /// GenerateFormUrlEncodedContent
    /// </summary>
    /// <param name="data">data</param>
    /// <returns>FormUrlEncodedContent</returns>
    HttpContent GenerateFormUrlEncodedContent(IDictionary<string, string> data);

    /// <summary>
    /// GenerateMultipartFormDataContent
    /// </summary>
    /// <param name="data">data</param>
    /// <returns>MultipartFormDataContent</returns>
    HttpContent GenerateMultipartFormDataContent(IDictionary<string, object> data);

    #endregion
```
