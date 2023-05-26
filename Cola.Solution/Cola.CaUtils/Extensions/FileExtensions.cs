using Cola.CaUtils.Helper;

namespace Cola.CaUtils.Extensions;

/// <summary>
///     FileExtensions
/// </summary>
public static class FileExtensions
{
    /// <summary>
    ///     GetFileName
    /// </summary>
    /// <param name="fileFullName">fill full path and name</param>
    /// <returns></returns>
    public static string GetFileName(this string fileFullName)
    {
        var fileName = fileFullName.Split(
            FileHelper.DirectorySeparatorChar)[fileFullName.Split(FileHelper.DirectorySeparatorChar).Length - 1];
        return fileName.Substring(0, fileName.LastIndexOf('.'));
    }
}