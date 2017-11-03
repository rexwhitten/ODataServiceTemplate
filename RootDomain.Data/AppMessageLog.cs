namespace RootDomain.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AppMessageLog")]
    public partial class AppMessageLog
    {
        public long ID { get; set; }

        [Required]
        public string AppMessageEntryId { get; set; }

        [Required]
        [StringLength(500)]
        public string ApplicationName { get; set; }

        [Required]
        [StringLength(500)]
        public string User { get; set; }

        public string Machine { get; set; }

        public string RequestIpAddress { get; set; }

        public string RequestContentType { get; set; }

        public string RequestContentBody { get; set; }

        public string RequestUri { get; set; }

        public string RequestMethod { get; set; }

        public string RequestRouteTemplate { get; set; }

        public string RequestRouteData { get; set; }

        public string RequestHeaders { get; set; }

        public DateTime? RequestTimestamp { get; set; }

        public string ResponseContentType { get; set; }

        public string ResponseContentBody { get; set; }

        public int? ResponseStatusCode { get; set; }

        public string ResponseHeaders { get; set; }

        public DateTime? ResponseTimestamp { get; set; }
    }
}
