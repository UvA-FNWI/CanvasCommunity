using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UvA.DataNose.Connectors.Canvas
{
    public enum CourseEvent
    {
        [EnumMember(Value = "offer")] Publish,
        [EnumMember(Value = "claim")] Unpublish,
        [EnumMember(Value = "conclude")] Conclude,
        [EnumMember(Value = "delete")] Delete,
        [EnumMember(Value = "undelete")] Undelete,
    }

    public enum CourseState
    {
        [EnumMember(Value = "unpublished")] Unpublished,
        [EnumMember(Value = "available")] Available,
        [EnumMember(Value = "completed")] Concluded,
        [EnumMember(Value = "deleted")] Deleted
    }

    public class Course : CanvasObject
    {
        public Course(CanvasConnector conn) { Connector = conn; }

        public override string ToString() => $"Course {ID}: {Name} ({CourseCode})";
        internal override string CanvasObjectID => "course";
        internal override string SaveUrl => ID != null ? $"courses/{ID}" : $"accounts/{AccountID}/courses";

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("account_id")]
        public int AccountID { get; set; }
        [JsonProperty("term_id")]
        public string TermID { get; set; }
        [JsonProperty("uuid")]
        public string UUID { get; set; }
        [JsonProperty("start_at")]
        public DateTime? StartDate { get; set; }
        [JsonProperty("end_at")]
        public DateTime? EndDate{ get; set; }
        [JsonProperty("course_code")]
        public string CourseCode { get; set; }
        [JsonProperty("sis_course_id")]
        public string SISCourseID { get; set; }
        [JsonProperty("event")]
        public CourseEvent? Event { get; set; }
        [JsonProperty("restrict_enrollments_to_course_dates")]
        public bool UseOverrideDates { get; set; }
        [JsonProperty("workflow_state")]
        public CourseState State { get; set; }

        [JsonProperty("grading_standard_enabled")]
        public bool? EnableGradingScheme { get; set; }
        [JsonProperty("grading_standard_id")]
        public int? GradingSchemeID { get; set; }
        [JsonProperty("hide_final_grades")]
        public bool? HideGradeTotals { get; set; }

        [DataMember(Name = "offer")]
        public bool? Publish { get; set; }
        [DataMember(Name = "enroll_me")]
        public bool? EnrollCurrentUser { get; set; }

        /// <summary>
        /// Sets the TermID based on SIS term ID
        /// </summary>
        public string SISTermID { set => TermID = "sis_term_id:" + value; }

        private List<Section> _Sections;
        [JsonIgnore]
        public List<Section> Sections { get => _Sections ?? (_Sections = Connector.RetrieveCollection<Section>(this)); }

        private List<Assignment> _Assignments;
        [JsonIgnore]
        public List<Assignment> Assignments => _Assignments ?? (_Assignments = Connector.RetrieveCollection<Assignment>(this));

        private List<CustomGradeColumn> _CustomGradeColumns;
        [JsonIgnore]
        public List<CustomGradeColumn> CustomGradeColumns => _CustomGradeColumns ?? (_CustomGradeColumns = Connector.RetrieveCollection<CustomGradeColumn>(this, param: ("include_hidden", "true"), path: "custom_gradebook_column"));

        public void LoadAssignments() => _Assignments = Connector.RetrieveCollection<Assignment>(this);

        private List<Enrollment> _Enrollments;
        [JsonIgnore]
        public List<Enrollment> Enrollments => _Enrollments ?? (_Enrollments = Connector.RetrieveCollection<Enrollment>(this));

        public List<Enrollment> GetEnrollmentsByType(EnrollmentType type) => Connector.RetrieveCollection<Enrollment>(this, null, ("type", ToCanvasString(type)));
        public List<User> GetUsersByType(EnrollmentType type) => Connector.RetrieveCollection<User>(this, null, ("enrollment_type", type.ToString().ToLower()));

        private List<Folder> _Folders;
        [JsonIgnore]
        public List<Folder> Folders => _Folders ?? (_Folders = Connector.RetrieveCollection<Folder>(this));

        private List<File> _Files;
        [JsonIgnore]
        public List<File> Files => _Files ?? (_Files = Connector.RetrieveCollection<File>(this));

        private List<GroupCategory> _GroupCategories;
        [JsonIgnore]
        public List<GroupCategory> GroupCategories => _GroupCategories ?? (_GroupCategories = Connector.RetrieveCollection<GroupCategory>(this, path: "group_categorie"));

        [JsonIgnore]
        public IEnumerable<Tab> Tabs => Connector.RetrieveArray<Tab>($"{SaveUrl}/tabs");
        public void UpdateTab(Tab tab) => Connector.Update($"{SaveUrl}/tabs/{tab.ID}",
            ("hidden", tab.IsHidden));

        public IEnumerable<Submission> GetGradedSubmissions(string sisuserID = null) 
            => Connector.RetrieveCollection<Submission>(this, path: "students/submission", extraParams: new Dictionary<string, string>
            {
                ["student_ids[]"] = sisuserID != null ? "sis_user_id:" + sisuserID : "all",
                ["workflow_state"] = "graded"
            }, include: sisuserID != null ? null : "user");
    }
}
