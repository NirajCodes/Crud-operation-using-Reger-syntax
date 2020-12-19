using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace razorop.EDM
{
    public partial class tblemp
    {
        public string country_name { get; set; }
        public List<checkmodel> checklist { get; set; }
    }

    public class checkmodel
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool ischecked { get; set; }
    }
}
