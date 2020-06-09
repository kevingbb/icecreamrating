using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace icecream_api
{
    public partial class InputRating
    {
        [JsonProperty("id")]
        public Guid ID { get; set; }
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("productId")]
        public Guid ProductId { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; }

        [JsonProperty("rating")]
        public long Rating { get; set; }

        [JsonProperty("userNotes")]
        public string UserNotes { get; set; }
    }

    public partial class CreatedRating
    {
        [JsonConstructor]
        public CreatedRating()
        {
            ID = Guid.NewGuid();
            Timestamp = System.DateTime.UtcNow;
        }
        [JsonProperty("id")]
        public Guid ID { get; set; }
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("productId")]
        public Guid ProductId { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; }

        [JsonProperty("rating")]
        public long Rating { get; set; }

        [JsonProperty("userNotes")]
        public string UserNotes { get; set; }
    }

    public partial class RatingTable
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Text { get; set; }
    }
}
