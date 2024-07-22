using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterApiSdk.Model.Respense.Upload
{
    public class ImageResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string image_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int w { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int h { get; set; }
    }

    public class MediaUploadResponse : ErrorBase
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
        public string media_key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int expires_after_secs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ImageResponse image { get; set; }
        public Video video { get; set; }
    }

}
