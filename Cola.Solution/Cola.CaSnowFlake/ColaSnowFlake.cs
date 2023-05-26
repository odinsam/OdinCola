using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cola.CaUtils;
using Cola.CaUtils.Enums;
using Cola.Models.Core.Models.SnowFlake;

namespace Cola.CaSnowFlake;

public class ColaSnowFlake : IColaSnowFlake
{
    // 机器id所占的位数
    private const int WorkerIdBits = 5;

    // 数据标识id所占的位数
    private const int DatacenterIdBits = 5;

    // 支持的最大机器id，结果是31 (这个移位算法可以很快的计算出几位二进制数所能表示的最大十进制数)
    private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);

    // 支持的最大数据标识id，结果是31
    private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);

    // 序列在id中占的位数
    private const int SequenceBits = 12;

    // 数据标识id向左移17位(12+5)
    private const int DatacenterIdShift = SequenceBits + WorkerIdBits;

    // 机器ID向左移12位
    private const int WorkerIdShift = SequenceBits;

    // 时间截向左移22位(5+5+12)
    private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;

    // 生成序列的掩码，这里为4095 (0b111111111111=0xfff=4095)
    private const long SequenceMask = -1L ^ (-1L << SequenceBits);
    private static Dictionary<long, long> _dicContainer;

    private static readonly DateTime Jan1St1970 = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    // 开始时间截((new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)-Jan1st1970).TotalMilliseconds)
    private readonly long _twepoch;

    /// <summary>
    ///     雪花ID
    /// </summary>
    /// <param name="model">雪花模型</param>
    public ColaSnowFlake(SnowFlakeModel model)
    {
        _twepoch = (long)(new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc) - Jan1St1970).TotalMilliseconds;
        if (model.DatacenterId > MaxDatacenterId || model.DatacenterId < 0)
            throw new ColaException(EnumException.SnowFlakeDatacenterIdLengthGtOrLt, MaxDatacenterId.ToString());
        if (model.WorkerId > MaxWorkerId || model.WorkerId < 0)
            throw new ColaException(EnumException.SnowFlakeWorkerIdLengthGtOrLt, MaxWorkerId.ToString());
        LogWorkerId = model.WorkerId;
        LogDatacenterId = model.DatacenterId;
        LogSequence = 0L;
        LogLastTimestamp = -1L;
        if (_dicContainer == null)
            _dicContainer = new Dictionary<long, long>();
    }

    /// <summary>
    ///     雪花ID
    /// </summary>
    /// <param name="datacenterId">数据中心ID</param>
    /// <param name="workerId">工作机器ID</param>
    public ColaSnowFlake(long datacenterId, long workerId)
    {
        _twepoch = (long)(new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc) - Jan1St1970).TotalMilliseconds;
        if (datacenterId > MaxDatacenterId || datacenterId < 0)
            throw new ColaException(EnumException.SnowFlakeDatacenterIdLengthGtOrLt, MaxDatacenterId.ToString());
        if (workerId > MaxWorkerId || workerId < 0)
            throw new ColaException(EnumException.SnowFlakeWorkerIdLengthGtOrLt, MaxWorkerId.ToString());

        LogWorkerId = workerId;
        LogDatacenterId = datacenterId;
        LogSequence = 0L;
        LogLastTimestamp = -1L;
        if (_dicContainer == null)
            _dicContainer = new Dictionary<long, long>();
    }

    // 数据中心ID(0~31)
    private long LogDatacenterId { get; }

    // 工作机器ID(0~31)
    private long LogWorkerId { get; }

    // 毫秒内序列(0~4095)
    private long LogSequence { get; set; }

    // 上次生成ID的时间截
    private long LogLastTimestamp { get; set; }

    /// <summary>
    ///     生成snowflake Id
    /// </summary>
    /// <returns>snowflake Id</returns>
    public long CreateSnowFlakeId()
    {
        return NextId();
    }

    /// <summary>
    ///     解析雪花ID
    /// </summary>
    /// <param name="snowFlakeId">雪花ID</param>
    /// <returns>
    ///     解析后的字符串格式:
    ///     datetime_datacenterId_workerId_sequence
    /// </returns>
    public string AnalyzeId(long snowFlakeId)
    {
        var sb = new StringBuilder();
        var timestamp = snowFlakeId >> TimestampLeftShift;
        var time = Jan1St1970.AddMilliseconds(timestamp + _twepoch);
        sb.Append(time.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss:fff"));
        var dataCenterId = (snowFlakeId ^ (timestamp << TimestampLeftShift)) >> DatacenterIdShift;
        sb.Append("_" + LogDatacenterId);
        var workerId = (snowFlakeId ^ ((timestamp << TimestampLeftShift) | (LogDatacenterId << DatacenterIdShift))) >>
                       WorkerIdShift;
        sb.Append("_" + workerId);
        var sequence = snowFlakeId & SequenceMask;
        sb.Append("_" + sequence);
        return sb.ToString();
    }

    private void InitDic()
    {
        if (_dicContainer == null)
            _dicContainer = new Dictionary<long, long>();
    }

    private void ClearDic()
    {
        if (_dicContainer != null)
            _dicContainer.Clear();
    }

    private long NextId()
    {
        lock (this)
        {
            var timestamp = GetCurrentTimestamp();
            if (timestamp > LogLastTimestamp) //时间戳改变，毫秒内序列重置
            {
                LogSequence = 0L;
            }
            else if (timestamp == LogLastTimestamp) //如果是同一时间生成的，则进行毫秒内序列
            {
                LogSequence = (LogSequence + 1) & SequenceMask;
                if (LogSequence == 0) //毫秒内序列溢出
                    timestamp = GetNextTimestamp(LogLastTimestamp); //阻塞到下一个毫秒,获得新的时间戳
            }
            else //当前时间小于上一次ID生成的时间戳，证明系统时钟被回拨，此时需要做回拨处理
            {
                LogSequence = (LogSequence + 1) & SequenceMask;
                if (LogSequence > 0)
                    timestamp = LogLastTimestamp; //停留在最后一次时间戳上，等待系统时间追上后即完全度过了时钟回拨问题。
                else //毫秒内序列溢出
                    timestamp = LogLastTimestamp + 1; //直接进位到下一个毫秒
                //throw new Exception(string.Format("Clock moved backwards.  Refusing to generate id for {0} milliseconds", lastTimestamp - timestamp));
            }

            LogLastTimestamp = timestamp; //上次生成ID的时间截

            //移位并通过或运算拼到一起组成64位的ID
            var id = ((timestamp - _twepoch) << TimestampLeftShift) |
                     (LogDatacenterId << DatacenterIdShift) |
                     (LogWorkerId << WorkerIdShift) |
                     LogSequence;
            if (_dicContainer != null && !_dicContainer.ContainsKey(id))
            {
                _dicContainer.Add(id, id);
                return id;
            }

            Thread.Sleep(1);
            return NextId();
        }
    }

    /// <summary>
    ///     阻塞到下一个毫秒，直到获得新的时间戳
    /// </summary>
    /// <param name="lastTimestamp">上次生成ID的时间截</param>
    /// <returns>当前时间戳</returns>
    private static long GetNextTimestamp(long lastTimestamp)
    {
        var timestamp = GetCurrentTimestamp();
        while (timestamp <= lastTimestamp) timestamp = GetCurrentTimestamp();

        return timestamp;
    }

    /// <summary>
    ///     获取当前时间戳
    /// </summary>
    /// <returns></returns>
    private static long GetCurrentTimestamp()
    {
        return (long)(DateTime.UtcNow - Jan1St1970).TotalMilliseconds;
    }
}