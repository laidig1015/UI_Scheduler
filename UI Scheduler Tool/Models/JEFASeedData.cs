using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_Scheduler_Tool.Models
{
    public class JTrack
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
    }

    public class JEFA
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public int trackId { get; set; }
    }

    public class JEFASeedData
    {
        public List<JTrack> tracks;
        public List<List<JEFA>> efas;

        public JEFASeedData()
        {
            tracks = new List<JTrack>();
            efas = new List<List<JEFA>>();
        }

        public bool Load()
        {
            try
            {
                using (var db = new DataContext())
                {
                    foreach (var t in db.Tracks.ToList())
                    {
                        tracks.Add(new JTrack { id = t.ID, name = t.Name, shortName = t.ShortName });
                        efas.Add(db.EFAs.Where(e => e.TrackID == t.ID)
                                   .Select(e => new JEFA { id = e.ID, name = e.Name, shortName = e.ShortName, trackId = t.ID })
                                   .ToList());
                    }
                }
                return true;
            }
            catch (Exception e)// TODO BAD!!!
            {
                Console.WriteLine(e);
            }
            return false;
        }
    }
}