using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MediaTypeHeaderValue = Microsoft.Net.Http.Headers.MediaTypeHeaderValue;

namespace WebApplication1Test.Controllers;

public class VideoController : Controller
{
    public IActionResult StreamVideo()
    {
        var videoPath = "your_video_file_path"; // 视频文件路径
        var stream = new FileStream(videoPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        // 设置响应头信息
        HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        HttpContext.Response.Headers.Add("Cache-Control", "no-cache");
        HttpContext.Response.Headers.Add("Content-Disposition",
            new ContentDispositionHeaderValue("attachment").ToString());

        // 设置响应内容类型
        HttpContext.Response.ContentType = "application/vnd.apple.mpegurl";
        var segmentDuration = 5; // ts分片时长
        var segmentCount = (int)(stream.Length / (segmentDuration * 1000 * 10)); // 计算分片数
        var m3u8Content = "#EXTM3U\n#EXT-X-VERSION:3\n";
        for (var i = 0; i < segmentCount; i++)
        {
            var segmentName = $"segment{i}.ts"; // 分片文件名称
            var segmentUrl = $"{Request.Scheme}://{Request.Host}/Video/{segmentName}"; // 分片访问链接
            m3u8Content += $"#EXTINF:{segmentDuration},\n{segmentUrl}\n"; // 添加分片信息
        }

        var m3u8Stream = new MemoryStream(Encoding.UTF8.GetBytes(m3u8Content)); // 生成m3u8内容流
        return new FileStreamResult(m3u8Stream, new MediaTypeHeaderValue("application/vnd.apple.mpegurl"));
    }
}