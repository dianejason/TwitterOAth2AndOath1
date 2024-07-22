using System.Threading.Tasks;
using TwitterApiSdk.Model;
using TwitterApiSdk.Model.Respense.Upload;

namespace TwitterApiSdk.Api
{
    public interface IUploadApi
    {
        /// <summary>
        /// 上传整个二进制文件
        /// 支持的媒体类型：图片格式：JPG、PNG、GIF、WEBP， 图像大小 <= 5 MB，GIF 动画大小 <= 15 MB
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="byteFile"></param>
        /// <param name="mediaFileType"></param>
        /// <returns></returns>
        Task<MediaUploadResponse> UploadMedia(string accessToken, string accessTokenSecret, byte[] byteFile, string mimeType, UploadMediaFileType mediaFileType);
        Task<string> GetMimeType(string networkFileUrl);
        /// <summary>
        /// 视频格式：文件大小不得超过 512 MB
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="byteSize"></param>
        /// <param name="mimeType"></param>
        /// <param name="mediaFileType"></param>
        /// <returns></returns>
        Task<MediaUploadResponse> InitUpload(string accessToken, string accessTokenSecret, long byteSize, string mimeType, UploadMediaFileType mediaFileType);
        Task<bool> AppendUplaod(string accessToken, string accessTokenSecret, string mediaId, byte[] part, int index);
        Task<UploadFinalizeResponse> FinalizeUplaod(string accessToken, string accessTokenSecret, string mediaId);
        Task<UploadStatusResponse> UploadStatus(string accessToken, string accessTokenSecret, string mediaId);

    }
}
