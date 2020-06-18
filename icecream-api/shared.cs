using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;

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

        [JsonProperty("sentimentScore")]
        public string SentimentScore { get; set; }

    }

    public partial class RatingTable : TableEntity
    {
        public string Text { get; set; }
    }

    public partial class SentimentPackage
    {
        [JsonProperty("documents")]
        public Document[] Documents { get; set; }
    }

    public partial class Document
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
