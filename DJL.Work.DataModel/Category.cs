using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DJL.Work.DataModel
{
    public class Category
    {
        public int CategoryID { get; set; }
        public int CategoryNo { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> ParentID { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
    }
}
