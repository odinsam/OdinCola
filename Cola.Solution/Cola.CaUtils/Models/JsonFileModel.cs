namespace Cola.CaUtils.Models;

/// <summary>
///     JsonFileModel
/// </summary>
public class JsonFileModel
{
    /// <summary>
    ///     JsonFilePath
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    ///     文件是否为可选文件  当false且文件不存在，则抛出异常.默认值为 true
    /// </summary>
    public bool Optional { get; set; } = true;

    /// <summary>
    ///     修改是否重新加载
    /// </summary>
    public bool ReloadOnChange { get; set; } = true;
}