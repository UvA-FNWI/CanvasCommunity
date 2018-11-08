using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.DataNose.Connectors.Canvas
{
    public class Account : CanvasObject
    {
        public Account(CanvasConnector conn) { Connector = conn; }

        public override string ToString() => $"Account {ID}: {Name}";
        internal override string CanvasObjectID => "account";
        internal override string SaveUrl => $"accounts/{ID}";

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("parent_account_id")]
        public int ParentAccountID { get; set; }

        List<Course> _Courses;
        public List<Course> Courses => _Courses ?? (_Courses = Connector.RetrieveCollection<Course>(this));

        List<Admin> _Admins;
        public List<Admin> Admins
        {
            get
            {
                if (_Admins == null)
                {
                    _Admins = Connector.RetrieveCollection<Admin>(this);
                    _Admins.ForEach(a => a.AccountID = ID.Value);
                }
                return _Admins;
            }
        }

        public IEnumerable<Course> GetCoursesByTerm(int term) => Connector.RetrieveCollection<Course>(this, param: ("enrollment_term_id", term.ToString()));
        public IEnumerable<Account> GetSubAccounts(bool recursive) => Connector.RetrieveCollection<Account>(this, param: ("recursive", recursive.ToString()), path: "sub_account");
    }
}
