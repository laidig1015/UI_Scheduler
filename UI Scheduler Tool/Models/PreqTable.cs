using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public class PreqTable
    {
        public Dictionary<string, PreqNode> Table { get; set; }

        public PreqTable(List<Course> courses)
        {
            // initialize all buckets in our table so we have valid references
            // we have to use two seperate loops so when we link our references
            // we don't link to a null table
            foreach(var c in courses)
            {
                Table.Add(c.CourseNumber, new PreqNode(c));
            }
            
            // go through all coures
            foreach (var c in courses)
            {
                // find node we are updating
                PreqNode n = Table[c.CourseNumber];
                // add all parents
                foreach (var parent in c.Parents)
                {
                    n.Parents.Add(Table[parent.Parent.CourseNumber]);
                }
                // add all children
                foreach (var child in c.Children)
                {
                    n.Children.Add(Table[child.Child.CourseNumber]);
                }
            }
        }
    }
}