using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterApiSdk.Model.Respense
{
    public class TwitterResponse<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public T data { get; set; }
    }
}
