namespace Cola.CaSnowFlake;

public interface IColaSnowFlake
{
    /// <summary>
    ///     创建雪花Id
    /// </summary>
    /// <returns></returns>
    long CreateSnowFlakeId();

    /// <summary>
    ///     解析雪花ID
    /// </summary>
    /// <param name="snowFlakeId">雪花ID</param>
    /// <returns>
    ///     解析后的字符串格式:
    ///     datetime_datacenterId_workerId_sequence
    /// </returns>
    string AnalyzeId(long snowFlakeId);
}