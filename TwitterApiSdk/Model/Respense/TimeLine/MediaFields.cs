using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace TwitterApiSdk.Model.Respense.TimeLine
{
    public class MediaFields
    {
        public HashSet<string> ALL => new HashSet<string>
        {
            DurationMs,
            Height,
            Width,
            Type,
            Url,
            PreviewImageUrl,
            MediaKey,
            NonPublicMetrics,
            OrganicMetrics,
            PromotedMetrics,
            PublicMetrics
        };
        public readonly string DurationMs = "duration_ms";
        public readonly string Height = "height";
        public readonly string Width = "width";
        public readonly string Type = "type";
        public readonly string Url = "url";
        public readonly string PreviewImageUrl = "preview_image_url";
        public readonly string MediaKey = "media_key";
        public readonly string NonPublicMetrics = "non_public_metrics";
        public readonly string OrganicMetrics = "organic_metrics";
        public readonly string PromotedMetrics = "promoted_metrics";
        public readonly string PublicMetrics = "public_metrics";


    }
}
