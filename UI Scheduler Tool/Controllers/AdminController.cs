using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using UI_Scheduler_Tool.Maui;
using UI_Scheduler_Tool.Models;
using System;
using System.Diagnostics;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Fonts;
using PdfSharp.Drawing.Layout;

namespace UI_Scheduler_Tool.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult FilterOldClasses()
        {
            using (var db = new DataContext())
            {
                Maui.MauiScripts.filterDatabase(db);
            }
            return RedirectToAction("AdminPage");
        }

        public ActionResult CleanPreqEdges()
        {
            using (var db = new DataContext())
            {
                Maui.MauiScripts.cleanAllPreqEdges(db);
            }
            return RedirectToAction("AdminPage");
        }

        public ActionResult GrabMathCourses()
        {
            Maui.MauiScripts.PopulateCourseFromCollege("MATH");
            return RedirectToAction("AdminPage");
        }

        public ActionResult GrabECECourses()
        {
            Maui.MauiScripts.PopulateCourseFromCollege("ECE");
            return RedirectToAction("AdminPage");
        }

        public ActionResult GrabCSCourses()
        {
            Maui.MauiScripts.PopulateCourseFromCollege("CS");
            return RedirectToAction("AdminPage");
        }

        public ActionResult addPrerequistes()
        {
            using (var db = new DataContext())
            {
                Maui.MauiScripts.addPrerequesiteInformationToAllCourses(db);
            }
            return RedirectToRoute("AdminPage");
        }

        public ActionResult ViewCurrentCourses()
        {
            using (var db = new DataContext())
            {
                List<Course> all = db.Courses.ToList();
                //PreqTable table = new PreqTable(all);
                ViewBag.courses = all;
            }

            return View();
        }

        public void DrawPage(PdfPage page, XGraphics gfx)
        {
            //XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Times New Roman", 12, XFontStyle.Bold);
            //Top Line
            gfx.DrawString("Electrical Engineering Plan of Study", font, XBrushes.Black,
              new XRect(0, 40, page.Width, page.Height),
              XStringFormats.TopCenter);
            gfx.DrawString("This form must be filled out before taking the first Elective Focus Area (EFA) course.", font, XBrushes.Black,
              new XRect(0, 52, page.Width, page.Height),
              XStringFormats.TopCenter);
            gfx.DrawString("Return to ECE Office in 4016 SC.", font, XBrushes.Black,
              new XRect(0, 64, page.Width, page.Height),
              XStringFormats.TopCenter);

            font = new XFont("Times New Roman", 12);

            gfx.DrawString("Name: ", font, XBrushes.Black,
              new XRect(50, 86, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString("ID: ", font, XBrushes.Black,
             new XRect(300, 86, page.Width, page.Height),
             XStringFormats.TopLeft);

            bool computerOrElectrical = true;
            if (computerOrElectrical)
            {
                gfx.DrawString("Track: Computer", font, XBrushes.Black,
                  new XRect(50, 98, page.Width, page.Height),
                  XStringFormats.TopLeft);
            }
            else
            {
                gfx.DrawString("Track: Electrical", font, XBrushes.Black,
                  new XRect(50, 98, page.Width, page.Height),
                  XStringFormats.TopLeft);
            }

            gfx.DrawString("Check if Earning: ", font, XBrushes.Black,
                 new XRect(50, 110, page.Width, page.Height),
                 XStringFormats.TopLeft);

            XPen pen = new XPen(XColors.Black, Math.PI);
            gfx.DrawRectangle(pen, 150, 110, 10, 10);
            gfx.DrawString("Business Minor", font, XBrushes.Black,
                 new XRect(170, 110, page.Width, page.Height),
                 XStringFormats.TopLeft);

            gfx.DrawRectangle(pen, 300, 110, 10, 10);
            gfx.DrawString("Technical Entrepreneurship Certificate", font, XBrushes.Black,
                 new XRect(320, 110, page.Width, page.Height),
                 XStringFormats.TopLeft);
            //gfx.DrawRectangle(XBrushes.Black, )

            gfx.DrawString("Techical Objective (What you want to Learn): ", font, XBrushes.Black,
                 new XRect(50, 130, page.Width, page.Height),
                 XStringFormats.TopLeft);

            gfx.DrawString("Professional Objective (how you plan to use your knowledge): ", font, XBrushes.Black,
                 new XRect(50, 210, page.Width, page.Height),
                 XStringFormats.TopLeft);

            //Header of Table
            gfx.DrawRectangle(pen, 50, 300, 100, 15);
            gfx.DrawRectangle(pen, 150, 300, 250, 15);
            gfx.DrawRectangle(pen, 400, 300, 75, 15);
            gfx.DrawRectangle(pen, 475, 300, 75, 15);
            //gfx.DrawRectangle(pen, 500, 300, 50, 15);

            font = new XFont("Times New Roman", 12, XFontStyle.Bold);

            gfx.DrawString("Elective Courses ", font, XBrushes.Black,
                 new XRect(60, 300, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString("Course Name ", font, XBrushes.Black,
                 new XRect(160, 300, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString("Course #", font, XBrushes.Black,
                new XRect(410, 300, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString("Semester", font, XBrushes.Black,
                new XRect(485, 300, page.Width, page.Height),
                XStringFormats.TopLeft);
            //gfx.DrawString("Business?", font, XBrushes.Black,
            //    new XRect(500, 300, page.Width, page.Height),
            //    XStringFormats.TopLeft);

            //Row 1 Track Depth
            gfx.DrawRectangle(pen, 50, 315, 100, 15);
            gfx.DrawRectangle(pen, 150, 315, 250, 15);
            gfx.DrawRectangle(pen, 400, 315, 75, 15);
            gfx.DrawRectangle(pen, 475, 315, 75, 15);
            //gfx.DrawRectangle(pen, 500, 315, 50, 15);
            font = new XFont("Times New Roman", 12, XFontStyle.Bold);
            gfx.DrawString("Track Depth ", font, XBrushes.Black,
                 new XRect(60, 315, page.Width, page.Height),
                 XStringFormats.TopLeft);
            font = new XFont("Times New Roman", 12);
            gfx.DrawString("Computer Networks", font, XBrushes.Black,
                 new XRect(160, 315, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString("ECE:3540", font, XBrushes.Black,
                new XRect(410, 315, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString("Spring/2015", font, XBrushes.Black,
                new XRect(485, 315, page.Width, page.Height),
                XStringFormats.TopLeft);
           

            //Row 2 Track Breadth
            gfx.DrawRectangle(pen, 50, 330, 100, 15);
            gfx.DrawRectangle(pen, 150, 330, 250, 15);
            gfx.DrawRectangle(pen, 400, 330, 75, 15);
            gfx.DrawRectangle(pen, 475, 330, 75, 15);
            //gfx.DrawRectangle(pen, 500, 330, 50, 15);
            font = new XFont("Times New Roman", 12, XFontStyle.Bold);
            gfx.DrawString("Track Breadth ", font, XBrushes.Black,
                 new XRect(60, 330, page.Width, page.Height),
                 XStringFormats.TopLeft);
            font = new XFont("Times New Roman", 12);
            gfx.DrawString("Fundamentals of Software Engineering ", font, XBrushes.Black,
                 new XRect(160, 330, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString("ECE:5800 ", font, XBrushes.Black,
                new XRect(410, 330, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString("Spring 2015", font, XBrushes.Black,
                new XRect(485, 330, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(" ", font, XBrushes.Black,
                new XRect(500, 330, page.Width, page.Height),
                XStringFormats.TopLeft);

            //ECE 100-Level 1
            gfx.DrawRectangle(pen, 50, 345, 100, 15);
            gfx.DrawRectangle(pen, 150, 345, 250, 15);
            gfx.DrawRectangle(pen, 400, 345, 75, 15);
            gfx.DrawRectangle(pen, 475, 345, 75, 15);
            //gfx.DrawRectangle(pen, 500, 345, 50, 15);
            font = new XFont("Times New Roman", 12, XFontStyle.Bold);
            gfx.DrawString("ECE 100 Level", font, XBrushes.Black,
                 new XRect(60, 345, page.Width, page.Height),
                 XStringFormats.TopLeft);
            font = new XFont("Times New Roman", 12);
            gfx.DrawString("Software Engineering Languages and Tools", font, XBrushes.Black,
                 new XRect(160, 345, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString("ECE:5820 ", font, XBrushes.Black,
                new XRect(410, 345, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString("Fall/2016", font, XBrushes.Black,
                new XRect(485, 345, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(" ", font, XBrushes.Black,
                new XRect(475, 345, page.Width, page.Height),
                XStringFormats.TopLeft);

            //ECE 100-Level 2
            gfx.DrawRectangle(pen, 50, 360, 100, 15);
            gfx.DrawRectangle(pen, 150, 360, 250, 15);
            gfx.DrawRectangle(pen, 400, 360, 75, 15);
            gfx.DrawRectangle(pen, 475, 360, 75, 15);
            //gfx.DrawRectangle(pen, 500, 345, 50, 15);
            font = new XFont("Times New Roman", 12, XFontStyle.Bold);
            gfx.DrawString("ECE 100 Level", font, XBrushes.Black,
                 new XRect(60, 360, page.Width, page.Height),
                 XStringFormats.TopLeft);
            font = new XFont("Times New Roman", 12);
            gfx.DrawString("Software Engineering Project", font, XBrushes.Black,
                 new XRect(160, 360, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString("ECE:5830 ", font, XBrushes.Black,
                new XRect(410, 360, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString("Spring/2016", font, XBrushes.Black,
                new XRect(485, 360, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(" ", font, XBrushes.Black,
                new XRect(475, 360, page.Width, page.Height),
                XStringFormats.TopLeft);


            //ETechnical EFA 1
            gfx.DrawRectangle(pen, 50, 375, 100, 15);
            gfx.DrawRectangle(pen, 150, 375, 250, 15);
            gfx.DrawRectangle(pen, 400, 375, 75, 15);
            gfx.DrawRectangle(pen, 475, 375, 75, 15);
            //gfx.DrawRectangle(pen, 500, 345, 50, 15);
            font = new XFont("Times New Roman", 12, XFontStyle.Bold);
            gfx.DrawString("Technical EFA", font, XBrushes.Black,
                 new XRect(60, 375, page.Width, page.Height),
                 XStringFormats.TopLeft);
            font = new XFont("Times New Roman", 12);
            gfx.DrawString("Computer Science II: Data Structures", font, XBrushes.Black,
                 new XRect(160, 375, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString("CS:2230 ", font, XBrushes.Black,
                new XRect(410, 375, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString("Fall/2014", font, XBrushes.Black,
                new XRect(485, 375, page.Width, page.Height),
                XStringFormats.TopLeft);

            //ETechnical EFA 2
            gfx.DrawRectangle(pen, 50, 390, 100, 15);
            gfx.DrawRectangle(pen, 150, 390, 250, 15);
            gfx.DrawRectangle(pen, 400, 390, 75, 15);
            gfx.DrawRectangle(pen, 475, 390, 75, 15);
            //gfx.DrawRectangle(pen, 500, 345, 50, 15);
            font = new XFont("Times New Roman", 12, XFontStyle.Bold);
            gfx.DrawString("Technical EFA", font, XBrushes.Black,
                 new XRect(60, 390, page.Width, page.Height),
                 XStringFormats.TopLeft);
            font = new XFont("Times New Roman", 12);
            gfx.DrawString("Elementary Numerical Analysis", font, XBrushes.Black,
                 new XRect(160, 390, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString("MATH:3800 ", font, XBrushes.Black,
                new XRect(410, 390, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString("Fall/2014", font, XBrushes.Black,
                new XRect(485, 390, page.Width, page.Height),
                XStringFormats.TopLeft);

            //ETechnical EFA 3
            gfx.DrawRectangle(pen, 50, 405, 100, 15);
            gfx.DrawRectangle(pen, 150, 405, 250, 15);
            gfx.DrawRectangle(pen, 400, 405, 75, 15);
            gfx.DrawRectangle(pen, 475, 405, 75, 15);
            //gfx.DrawRectangle(pen, 500, 345, 50, 15);
            font = new XFont("Times New Roman", 12, XFontStyle.Bold);
            gfx.DrawString("Technical EFA", font, XBrushes.Black,
                 new XRect(60, 405, page.Width, page.Height),
                 XStringFormats.TopLeft);
            font = new XFont("Times New Roman", 12);
            gfx.DrawString("Operating Systems", font, XBrushes.Black,
                 new XRect(160, 405, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString("CS:3620 ", font, XBrushes.Black,
                new XRect(410, 405, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString("Fall/2016", font, XBrushes.Black,
                new XRect(485, 405, page.Width, page.Height),
                XStringFormats.TopLeft);

            //ETechnical EFA 4
            gfx.DrawRectangle(pen, 50, 420, 100, 15);
            gfx.DrawRectangle(pen, 150, 420, 250, 15);
            gfx.DrawRectangle(pen, 400, 420, 75, 15);
            gfx.DrawRectangle(pen, 475, 420, 75, 15);
            //gfx.DrawRectangle(pen, 500, 345, 50, 15);
            font = new XFont("Times New Roman", 12, XFontStyle.Bold);
            gfx.DrawString("Technical EFA", font, XBrushes.Black,
                 new XRect(60, 420, page.Width, page.Height),
                 XStringFormats.TopLeft);
            font = new XFont("Times New Roman", 12);
            gfx.DrawString("Database Systems", font, XBrushes.Black,
                 new XRect(160, 420, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString("CS:4400 ", font, XBrushes.Black,
                new XRect(410, 420, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString("Spring/2016", font, XBrushes.Black,
                new XRect(485, 420, page.Width, page.Height),
                XStringFormats.TopLeft);

            //Extra EFA 1
            gfx.DrawRectangle(pen, 50, 435, 100, 15);
            gfx.DrawRectangle(pen, 150, 435, 250, 15);
            gfx.DrawRectangle(pen, 400, 435, 75, 15);
            gfx.DrawRectangle(pen, 475, 435, 75, 15);
            //gfx.DrawRectangle(pen, 500, 345, 50, 15);
            font = new XFont("Times New Roman", 12, XFontStyle.Bold);
            gfx.DrawString("EFA", font, XBrushes.Black,
                 new XRect(60, 435, page.Width, page.Height),
                 XStringFormats.TopLeft);
            font = new XFont("Times New Roman", 12);
            gfx.DrawString("Topics in Computer Science 1", font, XBrushes.Black,
                 new XRect(160, 435, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString("CS:3980 ", font, XBrushes.Black,
                new XRect(410, 435, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString("Fall/2016", font, XBrushes.Black,
                new XRect(485, 435, page.Width, page.Height),
                XStringFormats.TopLeft);

            //Extra EFA 1
            gfx.DrawRectangle(pen, 50, 450, 100, 15);
            gfx.DrawRectangle(pen, 150, 450, 250, 15);
            gfx.DrawRectangle(pen, 400, 450, 75, 15);
            gfx.DrawRectangle(pen, 475, 450, 75, 15);
            //gfx.DrawRectangle(pen, 500, 345, 50, 15);
            font = new XFont("Times New Roman", 12, XFontStyle.Bold);
            gfx.DrawString("EFA", font, XBrushes.Black,
                 new XRect(60, 450, page.Width, page.Height),
                 XStringFormats.TopLeft);
            font = new XFont("Times New Roman", 12);
            gfx.DrawString("Switching Theory", font, XBrushes.Black,
                 new XRect(160, 450, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString("ECE:5300", font, XBrushes.Black,
                new XRect(410, 450, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString("Spring/2016", font, XBrushes.Black,
                new XRect(485, 450, page.Width, page.Height),
                XStringFormats.TopLeft);

            gfx.DrawString("Undergraduate students must demonstrate they can work on multidisciplinary teams.", font, XBrushes.Black,
                new XRect(50, 465, page.Width, page.Height),
                XStringFormats.TopLeft);

            //Other Stuff
            gfx.DrawString("Course: ", font, XBrushes.Black,
                new XRect(70, 477, page.Width, page.Height),
                XStringFormats.TopLeft);

            pen = new XPen(XColors.Black, Math.PI);
            gfx.DrawRectangle(pen,50, 477, 10, 10);
            gfx.DrawString("Other", font, XBrushes.Black,
                 new XRect(325, 477, page.Width, page.Height),
                 XStringFormats.TopLeft);

            gfx.DrawRectangle(pen, 310, 477, 10, 10);


            gfx.DrawString("Student Signature:", font, XBrushes.Black,
               new XRect(50, 490, page.Width, page.Height),
               XStringFormats.TopLeft);
            gfx.DrawString("Date:", font, XBrushes.Black,
             new XRect(400, 490, page.Width, page.Height),
             XStringFormats.TopLeft);
            gfx.DrawString("Advisor Approval:", font, XBrushes.Black,
              new XRect(50, 502, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString("Date:", font, XBrushes.Black,
             new XRect(400, 502, page.Width, page.Height),
             XStringFormats.TopLeft);
            gfx.DrawString("Dept Chair Approval:", font, XBrushes.Black,
              new XRect(50, 514, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString("Date:", font, XBrushes.Black,
             new XRect(400, 514, page.Width, page.Height),
             XStringFormats.TopLeft);


            //gfx.DrawRectangle(XBrushes.Black, )
        }



        public ActionResult MakePdf()
        {
            PdfDocument document = new PdfDocument();
            //this.time = document.Info.CreationDate;
            document.Info.Title = "Schedule, by UI Scheduler";

            // Create new page
            PdfPage page = document.AddPage();
            page.Width = XUnit.FromMillimeter(200);
            page.Height = XUnit.FromMillimeter(200);

            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

            // Draw the text
            //gfx.DrawString("Hello, World!", font, XBrushes.Black,
            //  new XRect(0, 0, page.Width, page.Height),
            //  XStringFormats.TopLeft);

            DrawPage(page, gfx);

            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.BinaryWrite(stream.ToArray());
            Response.Flush();
            stream.Close();
            Response.End();


            //return File(documentData, "application/pdf");
            //Page_Load();
            return View();
        }

        void Page_Load(object sender, EventArgs e)
        {
            // Create new PDF document
            PdfDocument document = new PdfDocument();
            //this.time = document.Info.CreationDate;
            document.Info.Title = "PDFsharp Clock Demo";
            document.Info.Author = "Stefan Lange";
            document.Info.Subject = "Server time: ";
            // Create new page
            PdfPage page = document.AddPage();
            page.Width = XUnit.FromMillimeter(200);
            page.Height = XUnit.FromMillimeter(200);
            //&nbsp;
            // Create graphics object and draw clock
            XGraphics gfx = XGraphics.FromPdfPage(page);
            //RenderClock(gfx);
            //&nbsp;
            // Send PDF to browser
            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.BinaryWrite(stream.ToArray());
            Response.Flush();
            stream.Close();
            Response.End();
        }



        public ActionResult ViewCurrentPreqEdges()
        {
            using (var db = new DataContext())
            {
                List<PreqEdge> all = db.PreqEdges.ToList();
                //PreqTable table = new PreqTable(all);
                ViewBag.edges = all;

                PreqEdge test = all[0];
                String x = test.Parent.CourseName;
                String y = test.Child.CourseName;
            }

            return View();
        }

        public ActionResult AdminPage()
        {
            return View();
        }
        
    }
}
