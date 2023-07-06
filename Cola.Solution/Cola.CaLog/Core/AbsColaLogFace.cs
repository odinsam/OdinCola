using System.Text;
using Cola.CaUtils.Extensions;
using Cola.CaUtils.Helper;
using Cola.Models.Core.Enums.Logs;
using Cola.Models.Core.Models.ColaLog;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SqlSugar;

namespace Cola.CaLog.Core;

public abstract class AbsColaLogFace : IColaLogFace
{
    private static readonly object FileLock = new();

    protected AbsColaLogFace(EnumLogLevel logLevel, LogConfig? config, IServiceCollection? serviceProvider)
    {
        LogLevel = logLevel;
        Config = config;
        Service = serviceProvider;
    }

    public IServiceCollection? Service { get; set; }
    public EnumLogLevel LogLevel { get; set; }
    public LogConfig? Config { get; set; }

    public abstract LogResponse? WriteLog(LogInfo log);

    protected LogResponse? WriteLogContent(LogInfo log)
    {
        var logInfo = OdinLogHelper.GetInstance(Config, Service).GenerateLog(LogLevel, log);
        logInfo.LogOriginalContent = log.LogContent;
        Config!.SaveType = string.IsNullOrEmpty(Config!.SaveType) ? "Print" : Config!.SaveType;
        logInfo.LogSaveType = Config!.SaveType?.Split(',').ConvertStringEnumerableToEnums<EnumLogSaveType>().ToArray();
        logInfo.LogException = log is ExceptionLog ? (log as ExceptionLog)?.LogException : null;
        logInfo.LogMark = log.LogMark;
        return WriteLogInfo(logInfo);
    }

    private LogResponse? WriteLogInfo(LogModel logInfo)
    {
        if (logInfo.LogSaveType != null && (logInfo.LogSaveType.Contains(EnumLogSaveType.File) ||
                                            logInfo.LogSaveType.Contains(EnumLogSaveType.All))) 
            LogWriteFile(logInfo);

        if (logInfo.LogSaveType != null && (logInfo.LogSaveType.Contains(EnumLogSaveType.MySql) ||
                                            logInfo.LogSaveType.Contains(EnumLogSaveType.SqlServer) ||
                                            logInfo.LogSaveType.Contains(EnumLogSaveType.All)))
            LogWriteDataBase(logInfo);
        Console.WriteLine(logInfo.LogContent);
        return new LogResponse { LogId = logInfo.LogId, LogLevel = LogLevel };
    }

    private void LogWriteFile(LogModel logInfo)
    {
        var logPath = CreateCommonDirectory();
        if (logInfo.LogContent != null) WriteLog(logPath, logInfo.LogContent);
    }

    private ConnectionConfig GenerateConnectionConfig(LogModel logInfo)
    {
        var connectionConfig = new ConnectionConfig
        {
            ConnectionString = Config?.DbConnectionString,
            IsAutoCloseConnection = true
        };
        if (logInfo.LogSaveType != null && logInfo.LogSaveType.Contains(EnumLogSaveType.MySql))
            connectionConfig.DbType = DbType.MySql;

        if (logInfo.LogSaveType != null && logInfo.LogSaveType.Contains(EnumLogSaveType.SqlServer))
            connectionConfig.DbType = DbType.SqlServer;

        return connectionConfig;
    }

    private void LogWriteDataBase(LogModel logInfo)
    {
        using (var db = new SqlSugarClient(GenerateConnectionConfig(logInfo)))
        {
            var sqlStr = new StringBuilder();
            sqlStr.Append("insert into tb_OdinLog");
            sqlStr.Append(
                "(Id,LogLevel,LogContent,ExceptionInfo,Remark,CreateUser,CreateTime,UpdateUser,UpdateTime,IsDelete)");
            sqlStr.Append(" values ");
            sqlStr.Append(
                "(@Id,@LogLevel,@LogContent,@ExceptionInfo,@Remark,@CreateUser,@CreateTime,@UpdateUser,@UpdateTime,0)");
            db.Ado.ExecuteCommand(
                sqlStr.ToString(),
                new List<SugarParameter>
                {
                    new("@Id", logInfo.LogId),
                    new("@LogLevel", logInfo.LogLevel.ToString()),
                    new("@LogContent", logInfo.LogOriginalContent),
                    new("@ExceptionInfo",
                        JsonConvert.SerializeObject(logInfo.LogException)),
                    new("@Remark", logInfo.LogMark),
                    new("@CreateUser", "OdinLog"),
                    new("@CreateTime",
                        DateTime.Now.ToString(Config?.LogTimeFormat)),
                    new("@UpdateUser", "OdinLog"),
                    new("@UpdateTime",
                        DateTime.Now.ToString(Config?.LogTimeFormat)),
                    new("@IsDelete", 0)
                });
        }
    }

    private string CreateCommonDirectory()
    {
        var commonDirPath = Path.Combine(AppContext.BaseDirectory, Config!.DirectoryName);
        if (!Directory.Exists(commonDirPath)) Directory.CreateDirectory(commonDirPath);
        var levelDirPath = Path.Combine(commonDirPath, LogLevel.ToString());
        if (!Directory.Exists(levelDirPath)) Directory.CreateDirectory(levelDirPath);
        DeleteExceedLog(levelDirPath);
        var logTimePath = Path.Combine(levelDirPath, DateTime.Now.ToString("yyyy-MM-dd"));
        if (!Directory.Exists(logTimePath)) Directory.CreateDirectory(logTimePath);
        var logFiles = Directory.GetFiles(logTimePath);
        string logFileName;
        string logFilePath;
        lock (FileLock)
        {
            if (logFiles.Length == 0)
            {
                logFileName = "0.txt";
            }
            else
            {
                var file = logFiles.Last();
                logFileName = new FileInfo(file).Length > 5 * 1024 * 1024 ? $"{logFiles.Length}.txt" : file;
            }

            logFilePath = Path.Combine(logTimePath, logFileName);
            if (!File.Exists(logFilePath))
                using (File.Create(logFilePath))
                {
                }
        }

        return logFilePath;
    }

    private void DeleteExceedLog(string levelDirPath)
    {
        foreach (var directory in Directory.GetDirectories(levelDirPath))
            if (!directory.IsNullOrEmpty())
            {
                var dirName = directory.Split(FileHelper.DirectorySeparatorChar).Last();
                var dtNow = DateTime.Now;
                var dtDir = new DateTime(dirName.Split('-')[0].ToInt(), dirName.Split('-')[1].ToInt(),
                    dirName.Split('-')[2].ToInt());
                if ((dtNow - dtDir).TotalDays > Config!.KeepTime)
                {
                    foreach (var exceedFile in Directory.GetFiles(directory)) File.Delete(exceedFile);
                    Directory.Delete(directory);
                }
            }
    }

    private void WriteLog(string filePath, string fileContent)
    {
        using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
        {
            using (var stw = new StreamWriter(fs, Encoding.UTF8))
            {
                stw.Write(fileContent);
                stw.Flush();
                fs.Flush();
            }
        }
    }
}