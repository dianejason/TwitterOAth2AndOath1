using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterApiSdk.Model.Respense.Upload
{
    public class UploadFinalizeResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public int media_id { get; set; }
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
        public int size { get; set; }
        /// <summary>
        /// Example of async FINALIZE response which requires further STATUS command call(s)
        /// </summary>
        public ProcessingInfo processing_info { get; set; }
        /// <summary>
        /// Example of sync FINALIZE response
        /// </summary>
        public Video video { get; set; }
    }

}
