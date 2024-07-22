using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterApiSdk.Model.Respense.Upload
{
    public class ProcessingInfo
    {
        /// <summary>
        /// state transition flow is pending -> in_progress -> [failed|succeeded]
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// check for the update after 10 seconds
        /// </summary>
        public int check_after_secs { get; set; }
        /// <summary>
        /// Optional [0-100] int value. Please don't use it as a replacement of "state" field.
        /// </summary>
        public int progress_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Error error { get; set; }
    }

    public class UploadStatusResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public long media_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string media_id_string { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int expires_after_secs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Video video { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ProcessingInfo processing_info { get; set; }
    }


    public class Error
    {
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
    }

    public class Video
    {
        /// <summary>
        /// 
        /// </summary>
        public string video_type { get; set; }
    }

}
