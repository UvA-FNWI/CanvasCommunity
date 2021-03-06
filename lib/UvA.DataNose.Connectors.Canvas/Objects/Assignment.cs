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
    public enum GradingType
    {
        [EnumMember(Value = "pass_fail")] PassFail,
        [EnumMember(Value = "percent")] Percentage,
        [EnumMember(Value = "letter_grade")] Letters,
        [EnumMember(Value = "gpa_scale")] GPA,
        [EnumMember(Value = "points")] Points,
        [EnumMember(Value = "not_graded")] NotGraded
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SubmissionType
    {
        [EnumMember(Value = "online_quiz")] Quiz,
        [EnumMember(Value = "none")] None,
        [EnumMember(Value = "on_paper")] OnPaper,
        [EnumMember(Value = "discussion_topic")] Discussion,
        [EnumMember(Value = "media_recording")] MediaRecording,
        [EnumMember(Value = "external_tool")] ExternalTool,
        [EnumMember(Value = "online_upload")] Upload,
        [EnumMember(Value = "online_text_entry")] TextEntry,
        [EnumMember(Value = "online_url")] Url,
        [EnumMember(Value = "not_graded")] NotGraded
    }

    public class Assignment : CanvasObject
    {
        public Assignment(CanvasConnector conn) { Connector = conn; }

        public override string ToString() => $"Assignment {ID}: {Name}";
        internal override string CanvasObjectID => "assignment";
        internal override string SaveUrl => $"courses/{CourseID}/assignments/{ID}";
        internal override string GetUrl => $"courses/{CourseID}/assignments/{ID}";

        [JsonProperty("course_id")]
        public int CourseID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("grading_type")]
        public GradingType GradingType { get; set; }
        [JsonProperty("published")]
        public bool IsPublished { get; set; }
        [JsonProperty("points_possible")]
        public double? PointsPossible { get; set; }
        [JsonProperty("due_at")]
        public DateTime? DueDate { get; set; }
        [JsonProperty("submission_types")]
        public SubmissionType[] SubmissionTypes { get; set; }
        [JsonProperty("muted")]
        public bool IsMuted { get; set; }
        [JsonProperty("external_tool_tag_attributes")]
        public ExternalToolAttributes ExternalToolAttributes { get; set; }
        [JsonProperty("has_submitted_submissions")]
        public bool HasSubmittedSubmissions { get; set; }

        private List<Submission> _Submissions;
        [JsonIgnore]
        public List<Submission> Submissions => _Submissions ?? (_Submissions = Connector.RetrieveCollection<Submission>(this, "user"));

        public void LoadSubmissions() => _Submissions = Connector.RetrieveCollection<Submission>(this, "user");
    }

    public class ExternalToolAttributes
    {
        [JsonProperty("content_id")]
        public string ContentID { get; set; }

        /// <summary>
        /// This is probably an enum but it's not documented.
        /// Only value observed so far: context_external_tool
        /// </summary>
        [JsonProperty("content_type")]
        public string ContentType { get; set; } 
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("new_tab")]
        public bool? NewTab { get; set; }
    }
}
