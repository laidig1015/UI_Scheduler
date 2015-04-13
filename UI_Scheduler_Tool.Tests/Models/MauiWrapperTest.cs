using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UI_Scheduler_Tool.Maui;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using UI_Scheduler_Tool.Models;

namespace UI_Scheduler_Tool.Tests.WrapperTests
{
    [TestClass]
    public class MauiWrapperTest
    {
        [TestMethod]
        public void GetCourse()
        {
            string result = MauiWrapper.GetCourse("055:032");
            MauiCourse course = new JavaScriptSerializer().Deserialize<MauiCourse>(result);
            Assert.IsTrue(course.title.Contains("Digital Design"),
                          "Unable to locate 'Digital Design' in resulting GetCourse JSON");
        }

        [TestMethod]
        public void GetAllECE()
        {
            string result = MauiWrapper.GetCourse("ECE");
            var courses = new JavaScriptSerializer().Deserialize<List<MauiCourse>>(result);
            Assert.IsTrue(courses.Count > 1,
                          "Unable to find all ECE courses in result Course List");
        }

        [TestMethod]
        public void GetMinors()
        {
            string result = MauiWrapper.GetMinors();
            Assert.IsTrue(result.Contains("Aerospace Studies"),
                          "Unable to locate 'Aerospace Studies' in resulting GetMinors JSON");
        }

        [TestMethod]
        public void GetProgramsofStudyByNatKey()
        {
            string result = MauiWrapper.GetProgramsOfStudyByNatKey("R");
            Assert.IsTrue(result.Contains("Aerospace Studies"),
                          "Unable to locate 'Aerospace Studies' in resulting GetProgramsofStudyByNatKey JSON");
        }

        [TestMethod]
        public void GetProgramofStudyByID()
        {
            string result = MauiWrapper.GetProgramOfStudyByID("305");
            Assert.IsTrue(result.Contains("Communication Studies"),
                          "Unable to locate 'Communication Studies' in resulting GetProgramofStudyByID JSON");
        }

        [TestMethod]
        public void GetCurrentSession()
        {
            string result = MauiWrapper.GetCurrentSession();
            Assert.IsTrue(result.Contains("startDate"),
                          "Unable to locate 'startDate' in resulting GetCurrentSession JSON");
        }

        [TestMethod]
        public void GetAllSessions()
        {
            string result = MauiWrapper.GetAllSessions();
            Assert.IsTrue(result.Contains("startDate"),
                          "Unable to locatel 'startDate' in resulting GetAllSessions JSON");
        }

        [TestMethod]
        public void GetSection()
        {
            // TODO: maybe deserialize
            string result = MauiWrapper.GetSections(59, "CS", "3330");
            Assert.IsTrue(result.Contains("Algorithms"),
                          "Unable to locate 'Algorithms' in resulting GetSection JSON");
            MauiCourse dummy = new MauiCourse()
            {
                title = "Algorithms",
                catalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                lastTaught = "Spring 2015",
                lastTaughtId= 59,
                lastTaughtCode= "20148",
                courseNumber= "CS:3330",
                legacyCourseNumber= "22C:031",
                creditHours= "3"
            };
            var sections = MauiSection.Get(dummy);
        }

        [TestMethod]
        public void createPrerequesites()
        {
            Course dummy = new Course()
            {
                CourseName = "Algorithms",
                CatalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "CS:3330",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };
            //MauiSection.createPrerequesties(dummy);

            Course dummy2 = new Course()
            {
                CourseName = "Embedded Systems",
                CatalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "ECE:3360",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };


            //MauiSection.createPrerequesties(dummy2);
            //https://api.maui.uiowa.edu/maui/api/pub/registrar/sections?json={sessionId: 59, courseSubject: 'CS', courseNumber: '3330'}&pageStart=0&pageSize=2147483647&
            //string college = 'ECE';
            
            //MauiScripts.PopulateCourseFromCollege("ECE");

        }

        [TestMethod]
        public void IsClassInFallSemester()
        {
            //First Course Test
            Course dummy = new Course()
            {
                CourseName = "Algorithms",
                CatalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "CS:3330",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };

            Course dummy2 = new Course()
            {
                CourseName = "Introduction to Digital Design",
                CatalogDescription = "Modern design and analysis of digital switching circuits; combinational logic; sequential circuits and system controllers; interfacing and busing techniques; design methodologies using medium- and large-scale integrated circuits; lab arranged. ",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "ECE:3320",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };

            Course dummy3 = new Course()
            {
                CourseName = "Linear Systems I",
                CatalogDescription = "Introduction to continuous and discrete time signals and systems with emphasis on Fourier analysis; examples of signals and systems; notion of state and finite state machines; causality; linearity and time invariance; periodicity; Fourier transforms; frequency response; convolution; IIR and FIR filters, continuous and discrete Fourier transforms; sampling and reconstruction; stability. ",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "ECE:2400",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };

            bool firstTest = MauiSection.IsClassInFallSemester(dummy);
            bool secondTest = MauiSection.IsClassInFallSemester(dummy2);
            bool thirdTest = MauiSection.IsClassInFallSemester(dummy3);
            Assert.IsTrue(firstTest);
            Assert.IsTrue(secondTest);
            Assert.IsFalse(thirdTest);

        }

        [TestMethod]
        public void IsClassInSpringSemester()
        {
            //First Course Test
            Course dummy = new Course()
            {
                CourseName = "Algorithms",
                CatalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "CS:3330",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };

            Course dummy2 = new Course()
            {
                CourseName = "Introduction to Digital Design",
                CatalogDescription = "Modern design and analysis of digital switching circuits; combinational logic; sequential circuits and system controllers; interfacing and busing techniques; design methodologies using medium- and large-scale integrated circuits; lab arranged. ",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "ECE:3320",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };

            Course dummy3 = new Course()
            {
                CourseName = "Linear Systems I",
                CatalogDescription = "Introduction to continuous and discrete time signals and systems with emphasis on Fourier analysis; examples of signals and systems; notion of state and finite state machines; causality; linearity and time invariance; periodicity; Fourier transforms; frequency response; convolution; IIR and FIR filters, continuous and discrete Fourier transforms; sampling and reconstruction; stability. ",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "ECE:2400",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };

            bool firstTest = MauiSection.IsClassInSpringSemester(dummy);
            bool secondTest = MauiSection.IsClassInSpringSemester(dummy2);
            bool thirdTest = MauiSection.IsClassInSpringSemester(dummy3);
            Assert.IsFalse(secondTest);
            Assert.IsTrue(firstTest);
            Assert.IsTrue(thirdTest);
        }

        [TestMethod]
        public void GetUpdatedSection()
        {
            string result = MauiWrapper.GetSections(59, "CS", "3330");
            Assert.IsTrue(result.Contains("Algorithms"),
                          "Unable to locate 'Algorithms' in resulting GetSection JSON");
            MauiCourse dummy = new MauiCourse()
            {
                title = "Algorithms",
                catalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                lastTaught = "Spring 2015",
                lastTaughtId = 59,
                lastTaughtCode = "20148",
                courseNumber = "CS:3330",
                legacyCourseNumber = "22C:031",
                creditHours = "3"
            };
            var sections = MauiSection.Get(dummy);
        }
    
        [TestMethod]
        public void CheckEFAType()
        {
            Course dummy = new Course()
            {
                CourseName = "Algorithms",
                CatalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "CS:3330",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void CheckTotalHoursType()
        {
            Course dummy = new Course()
            {
                CourseName = "Algorithms",
                CatalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "CS:3330",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };
            bool result;
            if (dummy.CreditHours.Equals("3"))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckCourseName()
        {
            Course dummy = new Course()
            {
                CourseName = "Algorithms",
                CatalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "CS:3330",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };
            bool result;
            if (dummy.CourseName.Equals("Algorithms"))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckCourseNumber()
        {
            Course dummy = new Course()
            {
                CourseName = "Algorithms",
                CatalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "CS:3330",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };
            bool result;
            if (dummy.CourseNumber.Equals("CS:3330"))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckBackwardsPrerequesites()
        {

             Course dummy = new Course()
            {
                CourseName = "Algorithms",
                CatalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "CS:3330",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };

            Course dummy2 = new Course()
            {
                CourseName = "Introduction to Digital Design",
                CatalogDescription = "Modern design and analysis of digital switching circuits; combinational logic; sequential circuits and system controllers; interfacing and busing techniques; design methodologies using medium- and large-scale integrated circuits; lab arranged. ",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "ECE:3320",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };


            PreqEdge dummyEdge = new PreqEdge()
            {
               Parent = dummy,
               Child = dummy2,
               IsRequired = true
            };
            bool result;
            if (dummyEdge.Child == dummy2)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckForwardsPrerequesites()
        {

            Course dummy = new Course()
            {
                CourseName = "Algorithms",
                CatalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "CS:3330",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };

            Course dummy2 = new Course()
            {
                CourseName = "Introduction to Digital Design",
                CatalogDescription = "Modern design and analysis of digital switching circuits; combinational logic; sequential circuits and system controllers; interfacing and busing techniques; design methodologies using medium- and large-scale integrated circuits; lab arranged. ",
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "ECE:3320",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };


            PreqEdge dummyEdge = new PreqEdge()
            {
                Parent = dummy,
                Child = dummy2,
                IsRequired = true
            };
            bool result;
            if (dummyEdge.Parent == dummy)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckOptionalPrerequesites()
        {

            Course dummy = new Course()
            {
                CourseName = "Algorithms",
                CatalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                Occurence = 2,
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "CS:3330",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };

            Course dummy2 = new Course()
            {
                CourseName = "Introduction to Digital Design",
                CatalogDescription = "Modern design and analysis of digital switching circuits; combinational logic; sequential circuits and system controllers; interfacing and busing techniques; design methodologies using medium- and large-scale integrated circuits; lab arranged. ",
                Occurence = 2,
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "ECE:3320",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };


            PreqEdge dummyEdge = new PreqEdge()
            {
                Parent = dummy,
                Child = dummy2,
                IsRequired = false
            };
            bool result;
            if (dummyEdge.IsRequired)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckRequiredPrerequesites()
        {

            Course dummy = new Course()
            {
                CourseName = "Algorithms",
                CatalogDescription = "Algorithm design techniques (e.g., greedy algorithms, divide-and-conquer, dynamic programming, randomization); fundamental algorithms (e.g., basic graph algorithms); techniques for efficiency analysis; computational intractability and NP-completeness.",
                Occurence = 2,
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "CS:3330",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };

            Course dummy2 = new Course()
            {
                CourseName = "Introduction to Digital Design",
                CatalogDescription = "Modern design and analysis of digital switching circuits; combinational logic; sequential circuits and system controllers; interfacing and busing techniques; design methodologies using medium- and large-scale integrated circuits; lab arranged. ",
                Occurence = 2,
                LastTaughtID = 59,
                //lastTaughtCode = "20148",
                CourseNumber = "ECE:3320",
                //legacyCourseNumber = "22C:031",
                CreditHours = "3"
            };


            PreqEdge dummyEdge = new PreqEdge()
            {
                Parent = dummy,
                Child = dummy2,
                IsRequired = true
            };
            bool result;
            if (dummyEdge.IsRequired)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            Assert.IsTrue(result);
        }
    }
}
