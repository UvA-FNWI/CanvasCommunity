using Newtonsoft.Json;
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
    public enum ContentMigrationType
    {
        [EnumMember(Value = "common_cartridge_importer")] CommonCartridgeImport,
        [EnumMember(Value = "course_copy_importer")] CourseCopy
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MigrationState
    {
        [EnumMember(Value = "pre_processing")] PreProcessing,
        [EnumMember(Value = "pre_processed")] PreProcessed,
        [EnumMember(Value = "running")] Running,
        [EnumMember(Value = "waiting_for_select")] WaitingForSelect,
        [EnumMember(Value = "completed")] Completed,
        [EnumMember(Value = "failed")] Failed,
        [EnumMember(Value = "queued")] Queued
    }

    public class ContentMigration : CanvasObject
    {
        public ContentMigration(CanvasConnector conn) { Connector = conn; }

        internal override string SaveUrl => $"courses/{CourseID}/content_migrations";
        internal override string GetUrl => $"{SaveUrl}/{ID}";

        public int CourseID { get; set; }

        [JsonProperty("migration_type")]
        public ContentMigrationType Type { get; set; }
        [JsonProperty("settings[file_url]")]
        public string FileUrl { get; set; }
        [JsonProperty("workflow_state")]
        public MigrationState State { get; set; }
        [JsonProperty("progress_url")]
        public string ProgressUrl { get; set; }
        [JsonProperty("settings[source_course_id]")]
        public string SourceCourseID { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExportState
    {
        [EnumMember(Value = "created")] Created,
        [EnumMember(Value = "exporting")] Exporting,
        [EnumMember(Value = "exported")] Exported,
        [EnumMember(Value = "failed")] Failed
    }

    public class ContentExport : CanvasObject
    {
        public ContentExport(CanvasConnector conn) { Connector = conn; }

        internal override string SaveUrl => $"courses/{CourseID}/content_exports";
        internal override string GetUrl => $"{SaveUrl}/{ID}";

        public int CourseID { get; set; }

        [JsonProperty("export_type")]
        public string Type => "common_cartridge";
        [JsonProperty("skip_notifications")]
        public bool SkipNotifications { get; set; } = true;
        [JsonProperty("workflow_state")]
        public ExportState State { get; set; }
        [JsonProperty("attachment")]
        public Attachment Attachment { get; set; }
        [JsonProperty("progress_url")]
        public string ProgressUrl { get; set; }
    }

    public class Attachment 
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
