﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UvA.DataNose.Connectors.Canvas
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DuplicateAction
    {
        [EnumMember(Value = "overwrite")] Overwrite,
        [EnumMember(Value = "rename")] Rename
    }

    public class File : CanvasObject
    {
        public File(CanvasConnector conn) { Connector = conn; }

        public override string ToString() => $"File {ID}: {Name}";
        internal override string SaveUrl => ID == null ? $"courses/{CourseID}/files/{ID}" : $"files/{ID}";
        internal override bool SendWrapped => false;

        [JsonProperty("course_id")]
        public int CourseID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("parent_folder_id")]
        public int? FolderID { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("filename")]
        public string FileName { get; set; }
        //[JsonProperty("parent_folder_path")]
        //public string FolderPath { get; set; }
        [JsonProperty("content-type")]
        public string ContentType { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("created_at")]
        public DateTime? DateCreated { get; set; }
        [DataMember(Name = "on_duplicate")]
        public DuplicateAction? OnDuplicateAction { get; set; }
    }
}
