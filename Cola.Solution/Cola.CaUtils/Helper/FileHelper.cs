using System.Text;

namespace Cola.CaUtils.Helper;

public class FileHelper
{
    /// <summary>
    ///     系统分隔符  win '\'  other '/'
    /// </summary>
    /// <returns></returns>
    public static string DirectorySeparatorChar = Path.DirectorySeparatorChar.ToString();

    public static string GetFileBase64(string filePath)
    {
        using var filestream = new FileStream(filePath, FileMode.Open);
        var arr = new byte[filestream.Length];
        filestream.Read(arr, 0, (int)filestream.Length);
        var baser64 = Convert.ToBase64String(arr);
        filestream.Close();
        return baser64;
    }

    /// <summary>
    ///     CreateOrReWriteFile
    /// </summary>
    /// <param name="filePath">file full path and name</param>
    /// <param name="fileContent">file content</param>
    /// <returns>if the file is successfully createOrReWrite; otherwise</returns>
    public static bool CreateOrReWriteFile(string filePath, string fileContent)
    {
        using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
        {
            using (var stw = new StreamWriter(fs, Encoding.UTF8))
            {
                stw.Write(fileContent);
                stw.Flush();
                fs.Flush();
                return true;
            }
        }
    }

    /// <summary>
    ///     AppendWriteFile
    /// </summary>
    /// <param name="filePath">file full path and name</param>
    /// <param name="fileContent">append file content</param>
    /// <returns>if the file is successfully append; otherwise</returns>
    public static bool AppendWriteFile(string filePath, string fileContent)
    {
        using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.ReadWrite))
        {
            using (var stw = new StreamWriter(fs, Encoding.UTF8))
            {
                stw.Write(fileContent);
                stw.Flush();
                fs.Flush();
                return true;
            }
        }
    }

    /// <summary>
    ///     read file content
    /// </summary>
    /// <param name="filePath">file full path and name</param>
    /// <returns>file content</returns>
    public static string? ReadFileContent(string filePath)
    {
        if (File.Exists(filePath))
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (var str = new StreamReader(fs, Encoding.UTF8))
                {
                    return str.ReadToEnd();
                }
            }

        return null;
    }
}