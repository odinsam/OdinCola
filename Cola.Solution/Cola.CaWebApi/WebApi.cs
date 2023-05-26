using System.Net;
using System.Net.Mail;
using System.Text;
using Cola.CaException;
using Cola.CaUtils.Constants;
using Cola.CaUtils.Extensions;
using Cola.CaWebApi.MailModels;
using Cola.Models.Core.Models.ColaWebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Cola.CaWebApi;

public class WebApi : IWebApi
{
    #region 构造函数与私有变量

    private IHttpClientFactory HttpClientFactory { get; }
    private IConfiguration Config { get; }
    private HttpClient? Client { get; set; }
    private IColaException ExceptionHelper { get; }

    public WebApi(IHttpClientFactory httpClientFactory, IConfiguration config, IColaException colaException)
    {
        HttpClientFactory = httpClientFactory;
        Config = config;
        ExceptionHelper = colaException;
    }
    
    #endregion

    public WebApi GetClient(string clientName)
    {
        var ex = ExceptionHelper.ThrowStringIsNullOrEmpty(clientName, "HttpClient 配置 ClientName");
        if (ex == null)
        {
            if (Config.GetSection(SystemConstant.CONSTANT_COLAWEBAPI_SECTION).Get<List<ClientConfig>>()
                .Any(c => c.ClientName.StringCompareIgnoreCase(clientName)))
            {
                Client = HttpClientFactory.CreateClient(clientName);
                return this;
            }

            throw ExceptionHelper.ThrowException("clientName 与 自定义配置文件不相符");
        }

        throw ex;
    }

    #region Get

    /// <summary>
    ///     GetWebApi 方法
    /// </summary>
    /// <param name="requestUri">get uri</param>
    /// <param name="customHeaders">Dictionary customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">result type</typeparam>
    /// <returns>result </returns>
    public T? GetWebApi<T>(string requestUri, Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class
    {
        AddCustomHeaders(Client, customHeaders);
        var getResult = Client!.GetAsync(requestUri, cancellationToken).GetAwaiter().GetResult();
        return GetWebApiResultAsync<T>(getResult).GetAwaiter().GetResult();
    }
    
    
    /// <summary>
    ///     GetWebApiResultAsync 异步方法
    /// </summary>
    /// <param name="requestUri">get uri</param>
    /// <param name="customHeaders">Dictionary customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">result type</typeparam>
    /// <returns>result </returns>
    public async Task<T?> GetWebApiAsync<T>(string requestUri, Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class
    {
        AddCustomHeaders(Client, customHeaders);
        return await GetWebApiResultAsync<T>(await Client!.GetAsync(requestUri, cancellationToken));
    }

    #endregion

    #region Post
    
    /// <summary>
    /// PostWebApi
    /// </summary>
    /// <param name="postUri">postUri</param>
    /// <param name="httpContent">httpContent       can use GenerateStringContent method</param>
    /// <param name="customHeaders">customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">post result type</typeparam>
    /// <returns>post result</returns>
    public T? PostWebApi<T>(string postUri, HttpContent? httpContent,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class
    {
        AddCustomHeaders(Client, customHeaders);
        var postResult = Client!.PostAsync(postUri, httpContent, cancellationToken).GetAwaiter().GetResult();
        return GetWebApiResultAsync<T>(postResult).GetAwaiter().GetResult();
    }

    /// <summary>
    /// PostWebApiAsync
    /// </summary>
    /// <param name="postUri">postUri</param>
    /// <param name="httpContent">httpContent       can use GenerateStringContent method</param>
    /// <param name="customHeaders">customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">post result type</typeparam>
    /// <returns>post result</returns>
    public async Task<T?> PostWebApiAsync<T>(string postUri, HttpContent? httpContent,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class
    {
        AddCustomHeaders(Client, customHeaders);
        return await GetWebApiResultAsync<T>(await Client!.PostAsync(postUri, httpContent, cancellationToken));
    }

    #endregion

    #region Put

    /// <summary>
    /// PutWebApi
    /// </summary>
    /// <param name="putUri">putUri</param>
    /// <param name="httpContent">httpContent       can use GenerateStringContent method</param>
    /// <param name="customHeaders">customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">post result type</typeparam>
    /// <returns>Put result</returns>
    public T? PutWebApi<T>(string putUri, HttpContent? httpContent,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class
    {
        AddCustomHeaders(Client, customHeaders);
        var putResult = Client!.PutAsync(putUri, httpContent, cancellationToken).GetAwaiter().GetResult();
        return GetWebApiResultAsync<T>(putResult).GetAwaiter().GetResult();
    }

    /// <summary>
    /// PutWebApiAsync
    /// </summary>
    /// <param name="putUri">putUri</param>
    /// <param name="httpContent">httpContent       can use GenerateStringContent method</param>
    /// <param name="customHeaders">customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">post result type</typeparam>
    /// <returns>Put result</returns>
    public async Task<T?> PutWebApiAsync<T>(string putUri, HttpContent? httpContent,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class
    {
        AddCustomHeaders(Client, customHeaders);
        return await GetWebApiResultAsync<T>(await Client!.PutAsync(putUri, httpContent, cancellationToken));
    }

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
    public T? DeleteWebApi<T>(string deleteUri, HttpContent? httpContent,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class
    {
        AddCustomHeaders(Client, customHeaders);
        var deleteResult = Client!.DeleteAsync(deleteUri, cancellationToken).GetAwaiter().GetResult();
        return GetWebApiResultAsync<T>(deleteResult).GetAwaiter().GetResult();
    }

    /// <summary>
    /// DeleteWebApiAsync
    /// </summary>
    /// <param name="deleteUri">deleteUri</param>
    /// <param name="httpContent">httpContent       can use GenerateStringContent method</param>
    /// <param name="customHeaders">customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">post result type</typeparam>
    /// <returns>Delete result</returns>
    public async Task<T?> DeleteWebApiAsync<T>(string deleteUri, HttpContent? httpContent,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class
    {
        AddCustomHeaders(Client, customHeaders);
        return await GetWebApiResultAsync<T>(await Client!.DeleteAsync(deleteUri, cancellationToken));
    }

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
    public T? OptionsWebApi<T>(string optionsUri,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class
    {
        AddCustomHeaders(Client, customHeaders);
        var optionsResult = Client!.SendAsync(new HttpRequestMessage(HttpMethod.Options, optionsUri), cancellationToken).GetAwaiter().GetResult();
        return GetWebApiResultAsync<T>(optionsResult).GetAwaiter().GetResult();
    }

    /// <summary>
    /// optionsWebApiAsync
    /// </summary>
    /// <param name="optionsUri">optionsUri</param>
    /// <param name="customHeaders">customHeaders</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="T">post result type</typeparam>
    /// <returns>Delete result</returns>
    public async Task<T?> OptionsWebApiAsync<T>(string optionsUri,
        Dictionary<string, string>? customHeaders = null,
        CancellationToken cancellationToken = default) where T : class
    {
        AddCustomHeaders(Client, customHeaders);
        return await GetWebApiResultAsync<T>(await Client!.SendAsync(new HttpRequestMessage(HttpMethod.Options, optionsUri), cancellationToken));
    }

    #endregion

    #region 邮件发送

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
    public void SendEmail(SmtpClientModel smtpClientModel, MailUser fromUser,List<MailUser> toUsers,string subject, string body, List<MailUser>? ccUsers,List<MailUser>? bccUsers,List<string>? filePaths)
    {
        var fromAddress = new MailAddress(fromUser.MailAddress, fromUser.MailUserName);
        
        var message = new MailMessage()
        {
            From = fromAddress,
            Subject = subject,
            Body = body
        };

        #region 增加 收件人、抄送人、密送人
        
        var toAddressList = new List<MailAddress>();
        var ccAddressList = new List<MailAddress>();
        var bccAddressList = new List<MailAddress>();
        
        foreach (var toUser in toUsers)
        {
            toAddressList.Add(new MailAddress(toUser.MailAddress, toUser.MailUserName));
        }

        if (ccUsers!=null)
        {
            foreach (var ccUser in ccUsers)
            {
                ccAddressList.Add(new MailAddress(ccUser.MailAddress, ccUser.MailUserName));
            }
        }

        if (bccUsers != null)
        {
            foreach (var bccUser in bccUsers)
            {
                bccAddressList.Add(new MailAddress(bccUser.MailAddress, bccUser.MailUserName));
            }
        }

        // send all toUsers
        foreach (var toAddress in toAddressList)
        {
            message.To.Add(toAddress);
        }
        // cc all ccUsers
        foreach (var ccAddress in ccAddressList)
        {
            message.CC.Add(ccAddress);
        }
        // bcc all ccUsers
        foreach (var bccAddress in bccAddressList)
        {
            message.Bcc.Add(bccAddress);
        }

        #endregion

        #region 添加附件

        // 添加附件
        if (filePaths != null)
        {
            foreach (var filePath in filePaths)
            {
                var attachment = new Attachment(filePath);
                message.Attachments.Add(attachment);
            }
        }

        #endregion
        
        var smtpClient = new SmtpClient
        {
            Host = smtpClientModel.Host,
            Port = smtpClientModel.Port,
            Credentials = new NetworkCredential(smtpClientModel.CredentialsUserName, smtpClientModel.CredentialsUserPwd),
            EnableSsl = smtpClientModel.EnableSsl
        };

        smtpClient.Send(message);
    }

    #endregion

    #region GenreateHttpContent

    /// <summary>
    /// GenerateStringContent
    /// </summary>
    /// <param name="data">data</param>
    /// <param name="encoding">default utf8</param>
    /// <returns>StringContent</returns>
    public HttpContent GenerateStringContent(string data,Encoding? encoding=null)
    {
        encoding ??= Encoding.UTF8;
        return new StringContent(data, encoding);
    }
    
    /// <summary>
    /// GenerateFormUrlEncodedContent
    /// </summary>
    /// <param name="data">data</param>
    /// <returns>FormUrlEncodedContent</returns>
    public HttpContent GenerateFormUrlEncodedContent(IDictionary<string, string> data)
    {
        return new FormUrlEncodedContent(data);
    }
    
    /// <summary>
    /// GenerateMultipartFormDataContent
    /// </summary>
    /// <param name="data">data</param>
    /// <returns>MultipartFormDataContent</returns>
    public HttpContent GenerateMultipartFormDataContent(IDictionary<string, object> data)
    {
        var content = new MultipartFormDataContent();
        foreach (var item in data)
        {
            if (item.Value is Stream stream)
            {
                var streamContent = new StreamContent(stream);
                content.Add(streamContent, item.Key, Guid.NewGuid().ToString());
            }
            else if (item.Value is string str)
            {
                var stringContent = new StringContent(str, Encoding.UTF8);
                content.Add(stringContent, item.Key);
            }
        }
        return content;
    }

    #endregion

    #region 添加自定义头

    private void AddCustomHeaders(HttpClient? client, Dictionary<string, string>? customHeaders = null)
    {
        if (client != null && customHeaders != null && customHeaders.Count > 0)
            foreach (var headerKey in customHeaders.Keys)
                client.DefaultRequestHeaders.Add(headerKey, customHeaders[headerKey]);
    }

    #endregion

    #region private get dynamic type async

    private async Task<T?> GetWebApiResultAsync<T>(HttpResponseMessage? responseMessage) where T : class
    {
        if (typeof(T) == typeof(string)) return await GetStringAsync(responseMessage)! as T;
        if (typeof(T) == typeof(Stream)) return await GetSteamAsync(responseMessage)! as T;
        if (typeof(T) == typeof(byte[])) return await GetByteArrayAsync(responseMessage)! as T;
        if (typeof(T) == typeof(List<byte>)) return await GetByteListAsync(responseMessage)! as T;
        return await GetModelAsync<T>(responseMessage);
    }

    private async Task<string>? GetStringAsync(HttpResponseMessage? response,
        CancellationToken cancellationToken = default)
    {
        if (response!.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return responseContent;
        }
        var errorModel = GetErrorModel(response).GetAwaiter().GetResult();
        throw ExceptionHelper.ThrowException($"Web Request Error. ErrorCode: {errorModel!.ErrorCode}, ErrorMessage:{ errorModel!.ErrorMessage }");
    }

    private async Task<Stream?>? GetSteamAsync(HttpResponseMessage? response,
        CancellationToken cancellationToken = default)
    {
        if (response!.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken);
            if (responseContent == null) return null;
            return responseContent;
        }
        var errorModel = GetErrorModel(response).GetAwaiter().GetResult();
        throw ExceptionHelper.ThrowException($"Web Request Error. ErrorCode: {errorModel!.ErrorCode}, ErrorMessage:{ errorModel!.ErrorMessage }");
    }


    private async Task<byte[]?>? GetByteArrayAsync(HttpResponseMessage? response,
        CancellationToken cancellationToken = default)
    {
        if (response!.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            if (responseContent == null) return null;
            return responseContent;
        }
        var errorModel = GetErrorModel(response).GetAwaiter().GetResult();
        throw ExceptionHelper.ThrowException($"Web Request Error. ErrorCode: {errorModel!.ErrorCode}, ErrorMessage:{ errorModel!.ErrorMessage }");
    }

    private async Task<List<byte>?>? GetByteListAsync(HttpResponseMessage? response,
        CancellationToken cancellationToken = default)
    {
        if (response!.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            if (responseContent == null) return null;
            return responseContent.ToList();
        }
        var errorModel = GetErrorModel(response).GetAwaiter().GetResult();
        throw ExceptionHelper.ThrowException($"Web Request Error. ErrorCode: {errorModel!.ErrorCode}, ErrorMessage:{ errorModel!.ErrorMessage }");
    }

    private async Task<T?> GetModelAsync<T>(HttpResponseMessage? response,
        CancellationToken cancellationToken = default)
    {
        if (response!.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            if (responseContent.IsNullOrEmpty()) return default;
            return JsonConvert.DeserializeObject<T>(responseContent)!;
        }
        var errorModel = GetErrorModel(response).GetAwaiter().GetResult();
        throw ExceptionHelper.ThrowException($"Web Request Error. ErrorCode: {errorModel!.ErrorCode}, ErrorMessage:{ errorModel!.ErrorMessage }");
    }


    private async Task<ErrorModel> GetErrorModel(HttpResponseMessage response)
    {
        var errorContent = await response.Content.ReadAsStringAsync();
        return  JsonConvert.DeserializeObject<ErrorModel>(errorContent)!;
    }
    #endregion
}