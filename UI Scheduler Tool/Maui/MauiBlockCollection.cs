using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Maui
{
    public class MauiBlockCollection
    {
        public string Title { get; private set; }
        public string Hours { get; private set; }

        public bool HasData
        {
            get
            {
                return Lectures.Count > 0 ||
                    Labs.Count > 0 ||
                    Discussions.Count > 0;
            }
        }
        public List<MauiBlockSection> Lectures;
        public List<MauiBlockSection> Labs;
        public List<MauiBlockSection> Discussions;

        public MauiBlockCollection(List<MauiBlockSection> sections)
        {
            Lectures = new List<MauiBlockSection>();
            Labs = new List<MauiBlockSection>();
            Discussions = new List<MauiBlockSection>();

            if(sections.Count > 0)
            {
                Title = sections[0].CourseTitle;
                Hours = sections[0].Hours;

                foreach (var s in sections)
                {
                    switch (s.SectionType)
                    {
                        case "LECTURE":
                        case "STANDALONE":
                            Lectures.Add(s); break;
                        case "LAB": Labs.Add(s); break;
                        case "DISCUSSION": Discussions.Add(s); break;
                    }
                }
            }
        }
    }
}