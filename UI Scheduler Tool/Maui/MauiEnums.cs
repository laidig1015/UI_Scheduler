using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Maui
{
    public enum SectionRelationships
    {
        RelatedMandatorySections,
        RelatedPreferredSections,
        Mandatory,
        Preferred
    }

    public static class SectionRelationshipExt
    {
        public static SectionRelationships AliasRelatedToUnrelated(this SectionRelationships r)
        {
            switch (r)
            {
                case SectionRelationships.RelatedMandatorySections: return SectionRelationships.Mandatory;
                case SectionRelationships.RelatedPreferredSections: return SectionRelationships.Preferred;
                default: return r;
            }
        }

        public static SectionRelationships AliasUnrelatedToRelated(this SectionRelationships r)
        {
            switch (r)
            {
                case SectionRelationships.Mandatory: return SectionRelationships.RelatedMandatorySections;
                case SectionRelationships.Preferred: return SectionRelationships.RelatedPreferredSections;
                default: return r;
            }
        }

        public static string ToURLString(this SectionRelationships r)
        {
            switch (r)
            {
                case SectionRelationships.RelatedMandatorySections: return "related-mandatory-sections";
                case SectionRelationships.RelatedPreferredSections: return "related-preferred-sections";
                case SectionRelationships.Preferred: return "preferred";
                case SectionRelationships.Mandatory: return "mandatory";
                default: return String.Empty;// should cause a 404 (throw exception instead?)
            }
        }
    }

    public enum SectionTypes
    {
        Standalone,
        Lecture,
        Lab,
        Discussion,
        LabDiscussion,
        Screening,
        AdditionalTime,
        IndependantStudy,
        MidtermExam,
        FinalExam
    }

    public static class SectionTypeExt
    {
        public static string ToURLString(this SectionTypes t)
        {
            switch (t)
            {
                case SectionTypes.Standalone: return "STANDALONE";
                case SectionTypes.Lecture: return "LECTURE";
                case SectionTypes.Lab: return "LAB";
                case SectionTypes.Discussion: return "DISCUSSION";
                case SectionTypes.LabDiscussion: return "LAB-DISCUSSION";
                case SectionTypes.Screening: return "SCREENING";
                case SectionTypes.AdditionalTime: return "ADDITIONAL_TIME";
                case SectionTypes.IndependantStudy: return "INDEPENDENTSTUDY";
                case SectionTypes.MidtermExam: return "MIDTERMEXAM";
                case SectionTypes.FinalExam: return "FINALEXAM";
                default: return String.Empty;// should cause a 404 (throw exception instead?)
            }
        }
    }
}