using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Net;
using System;
using System.Windows.Controls;
using System.Net.Mail;
using System.Linq;
using System.Configuration;

namespace SMSParameters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class JobSearchPage : Page
    {
        //enum _JobBoardName
        //{
        //    LinkedIn,
        //    Indeed,
        //    Glassdoor,
        //    Dice,
        //    //    Monster
        //}
        JobBoardManager jobBoardManager = new JobBoardManager();
        KeywordManager keywordManager = new KeywordManager();
        LocationManager LocationManager = new LocationManager();

        public JobSearchPage()
        {
            bool boolRunningFromHome = false;

            IdealAutomate.Core.Methods myActions = new Methods();
            myActions.ScriptStartedUpdateStats();

            InitializeComponent();
            

            //_JobBoardName jobBoard = _JobBoardName.Dice;
            string strWindowTitle = myActions.PutWindowTitleInEntity();
            if (strWindowTitle.StartsWith("JobSearch"))
            {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);
            SqlConnection con;

            if (!String.IsNullOrEmpty(ConnectionString1.SqlConnString))
            {
                con = new SqlConnection(ConnectionString1.SqlConnString);
            } else
            {
                goto myExit;
            }
            SqlCommand cmd = new SqlCommand();
            bool developermode = false;


            cmd.Connection = con;

            //            string[] locations =
            //            {
            //                "Northbrook|101594833|SRCH_IL.0,10_IC1128934_KO11,12|42.1275267|-87.82895479999999",
            //                "Skokie|104075955|SRCH_IL.0,6_IC1128973_KO7,8|42.03240249999999|-87.7416246",
            //                "Wilmette|105652554|SRCH_IL.0,8_IC1129007_KO9,10|42.0722513|-87.7228384",
            //"Evanston|103984459|SRCH_IL.0,8_IC1128835_KO9,10|42.04507219999999|-87.68769689999999",
            //"Lake Forest|101957782|SRCH_IL.0,11_IC1128891_KO12,13|42.25863419999999|-87.840625",
            //"Morton Grove|107073968|SRCH_IL.0,12_IC1128925_KO13,14|42.0405852|-87.78256209999999",
            //"Deerfield|103159347|SRCH_IL.0,9_IC1128821_KO10,11|42.1711365|-87.8445119",
            //"Niles|104326590|SRCH_IL.0,5_IC1128931_KO6,7|42.0189191|-87.80284019999999",
            //"Lincolnwood|100872165|SRCH_IL.0,11_IC1128899_KO12,13|42.0044757|-87.73005940000002",
            //// "Northfield|",
            //"Winnetka|101817485|SRCH_IL.0,8_IC1129010_KO9,10|42.10808340000001|-87.735895",
            //"Glenview|103959268|SRCH_IL.0,8_IC1128852_KO9,10|42.07780649999999|-87.8223368",
            //"Glencoe|100896055|SRCH_IL.0,7_IC1128850_KO8,9|42.13502679999999|-87.75811879999999",
            //"Wheeling|101337567|SRCH_IL.0,8_IC1129004_KO9,10|42.1391927|-87.9289591",
            //"Buffalo Grove|104624642|SRCH_IL.0,13_IC1128800_KO14,15|42.1662831|-87.9631308",
            ////"Rosemont|",
            //"Park Ridge|103579343|SRCH_IL.0,10_IC1128947_KO11,12|42.0111412|-87.84061919999999",
            ////"Remote|92000001|SRCH_IL.0,6_IS11047_KO7,8_IP2"

            //            };
            //            string[] keywords =
            //{
            //                "c%23",
            //"WPF",
            //"Machine Learning",
            //"OutSystems"

            //            };
           List<JobApplicationJobBoards> jobBoards = jobBoardManager.GetJobBoardsEnabled().ToList();
           List<JobApplicationKeywords> keywords = keywordManager.GetKeywordsEnabled().ToList();
            List<JobApplicationLocations> locations =  LocationManager.GetLocationsEnabled().ToList();
            foreach (var jobBoardx in jobBoards)
            {

                ////jobBoard = jobBoardx;
                //if (jobBoardx == _JobBoardName.LinkedIn)
                //{
                //    continue;
                //}
                //if (jobBoardx == _JobBoardName.Indeed)
                //{
                //    continue;
                //}
                //if (jobBoardx == _JobBoardName.Dice)
                //{
                //    continue;
                //}
                foreach (var keyword in keywords)
                {
                    foreach (var location in locations)
                    {
                        bool processNextPage = true;
                        int start = 0;
                        while (processNextPage == true)
                        {
                            PerformSearch(con, cmd, jobBoardx, myActions, keyword, location, ref developermode, ref processNextPage, ref start);
                            start += 25;
                        }

                    }
                }
            }
            string userid = myActions.GetValueByKey("UserId");
            string password = myActions.GetValueByKey("Password");
            string clientHost = myActions.GetValueByKey("ClientHost");
            string carrierSuffix = myActions.GetValueByKey("cbxcarriersmsemailsuffix");
           string phoneComplete = myActions.GetValueByKey("PhoneComplete");
           string subjectComplete = myActions.GetValueByKey("SubjectComplete");
            string messageBodyComplete = myActions.GetValueByKey("MessageBodyComplete");
           string phoneForError = myActions.GetValueByKey("PhoneError");
           string subjectError = myActions.GetValueByKey("SubjectError");
           string messageBodyError = myActions.GetValueByKey("MessageBodyError");
            var message = new MailMessage();
            message.From = new MailAddress("harvey007y@gmail.com");

            message.To.Add(new MailAddress(phoneComplete + carrierSuffix));//See carrier destinations below
                                                                                  //message.To.Add(new MailAddress("5551234568@txt.att.net"));

            //message.CC.Add(new MailAddress("carboncopy@foo.bar.com"));
            message.Subject = subjectComplete;
            message.Body = messageBodyComplete;

            var client = new SmtpClient();
            client.Host =
                clientHost;
            message.Priority = MailPriority.High;


            client.Credentials = new System.Net.NetworkCredential(userid, password);
            client.Port = 587;

            
            client.EnableSsl = true;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            myActions.MessageBoxShow("Done");
           
            goto myExit;

        myExit:
            string a = "";
        }
        /// <summary>
        /// Writes jobs found to database jobapplications
        /// </summary>
        /// <param name="con"></param>
        /// <param name="cmd"></param>
        /// <param name="jobBoard"></param>
        /// <param name="myActions"></param>
        /// <param name="keyword"></param>
        /// <param name="location"></param>
        /// <param name="developermode">the first pass switch</param>
        /// <param name="processNextPage"></param>
        /// <param name="start"></param>
        void PerformSearch(SqlConnection con,
            SqlCommand cmd,
            JobApplicationJobBoards jobBoard,
            Methods myActions,
            JobApplicationKeywords keyword,
            JobApplicationLocations location,
            ref bool developermode,
            ref bool processNextPage,
            ref int start)
        {
            //
            // S T A R T   N E W   P R O C E S S:  chrome
            //
            List<string> myWindowTitles4 = myActions.GetWindowTitlesByProcessName("chrome");
            myWindowTitles4.RemoveAll(item => item == "");
            //string[] locgeo = location.Split('|');
            string myUrlTarget = "";
            if (keyword.Keyword == "C#" || keyword.Keyword == "c#")
            {
                keyword.Keyword = "c%23";
            }
            switch (jobBoard.Name)
            {
                case "LinkedIn":
                    myUrlTarget = "https://www.linkedin.com/jobs/search/?distance=1&geoId=" + location.LinkedInGeoId + "&keywords=" + keyword.Keyword + "&location=" + location.LocationName + "%2C%20Illinois%2C%20United%20States&msgConversationId=6647936427130699776&msgOverlay=true&start=" + start;
                    break;
                case "Indeed":
                    myUrlTarget = "https://www.indeed.com/jobs?q=" + keyword.Keyword + "&l=" + location.LocationName + "%2C+IL&radius=0";
                    if (myUrlTarget.Contains("Remote"))
                    {
                        myUrlTarget = myUrlTarget.Replace("Remote%2C+IL", "Remote");
                    }
                    break;
                case "Glassdoor":
                    if (keyword.Keyword == "c%23")
                    {
                        if (location.LocationName.ToLower() == "remote")
                        {
                            myUrlTarget = "https://www.glassdoor.com/Job/jobs.htm?suggestCount=0&suggestChosen=false&clickSource=searchBtn&typedKeyword=&locT=S&locId=11047&jobType=&context=Jobs&sc.keyword=c%23&dropdown=0";
                        }
                        else
                        {
                            myUrlTarget = "https://www.glassdoor.com/Job/" + location.LocationName + "-" + "c" + "-" + location.GlassdoorLocation + ".htm?radius=0";
                        }
                    }
                    else
                    {
                        if (location.LocationName.ToLower() == "remote") {
                            myUrlTarget = "https://www.glassdoor.com/Job/jobs.htm?suggestCount=0&suggestChosen=false&clickSource=searchBtn&typedKeyword=&locT=S&locId=11047&jobType=&context=Jobs&sc.keyword=" + keyword.Keyword + "&dropdown=0";
                        }
                        else
                        {
                            myUrlTarget = "https://www.glassdoor.com/Job/" + location.LocationName + "-" + keyword.Keyword + "-" + location.GlassdoorLocation + ".htm?radius=0";
                        }
                        }
                    break;
                case "Dice":
                    myUrlTarget = "https://www.dice.com/jobs?q=" + keyword.Keyword + "&location=" + location.LocationName + ",%20IL,%20USA&latitude=" + location.DiceLatitude + "&longitude=" + location.DiceLongitude + "&countryCode=US&locationPrecision=City&radius=1&radiusUnit=mi&page=1&pageSize=100&filters.employerType=Direct%20Hire&language=en&includeRemote=false";
                    if (myUrlTarget.Contains("Remote%2C+IL") || myUrlTarget.Contains("Remote,%20IL,%20USA"))
                    {
                        myUrlTarget = "https://www.dice.com/jobs?q=" + keyword.Keyword + "&countryCode=US&radius=30&radiusUnit=mi&page=1&pageSize=100&filters.employerType=Direct%20Hire&filters.isRemote=true&language=en";
                    }

                    break;
                case "CareerBuilder":
                    myUrlTarget = "https://www.careerbuilder.com/jobs?utf8=%E2%9C%93&posted=30&pay=&radius=1&emp=&cb_apply=false&keywords=" + keyword.Keyword + "&location=" + location.LocationName + "%2C+IL";
                  

                    break;
                //case _JobBoardName.Monster:
                //    break;
                default:
                    break;
            }

            if (!myWindowTitles4[0].Contains(jobBoard.Name))
            {
                // You may need to manually add content parameter as second parameter for run???
                myActions.Run(@"C:\Program Files\Google\Chrome\Application\chrome.exe", myUrlTarget);
                myActions.Sleep(1000);
            }
            if (myWindowTitles4.Count > 0)
            {
                myActions.ActivateWindowByTitle(myWindowTitles4[0], (int)WindowShowEnum.SW_SHOWMAXIMIZED);
            }
            if (start > 0)
            {
                myActions.Sleep(2000);
            }

            myActions.TypeText("%(d)", 1500); // select address bar
            myActions.TypeText("^(a)", 1500);
            myActions.TypeText("^(c)", 1500);

            string myUrl = myActions.PutClipboardInEntity();
            if ((myUrl != myUrlTarget && jobBoard.Name != "Glassdoor") ||
                (myUrl == myUrlTarget && jobBoard.Name == "Dice") ||
                 jobBoard.Name == "CareerBuilder" ||
                (myUrl != myUrlTarget && jobBoard.Name == "Glassdoor" && location.LocationName.ToLower() == "remote"))
            {
                myActions.SelectAllPasteFromEntity(myUrlTarget, 500);
                myActions.TypeText("{ENTER}", 500);
                //  Console.WriteLine("Enter pressed");
            }
            int count = 0;
            ImageEntity myImage = new ImageEntity();
            if (jobBoard.Name != "Glassdoor")
            {
                // wait for location in title except for dice because dice does not display location in title
                if (jobBoard.Name != "Dice")
                {
                    while (!myWindowTitles4[0].Contains(location.LocationName) && count < 10)
                    {
                        myActions.Sleep(1000);
                        myWindowTitles4 = myActions.GetWindowTitlesByProcessName("chrome");
                        myWindowTitles4.RemoveAll(item => item == "");
                        count++;
                    }
                    if (count == 10)
                    {
                        Console.WriteLine(string.Format("could not find {0} in title", location.LocationName));
                    }
                }
            checkLoaded:
                myImage = new ImageEntity();
                switch (jobBoard.Name)
                {
                    case "LinkedIn":
                        myImage.ImageFile = "Images\\Jobs.PNG";
                        break;
                    case "Indeed":
                        myImage.ImageFile = "Images\\IndeedLoaded.PNG";
                        break;
                    case "Glassdoor":
                        myImage.ImageFile = "Images\\GlassDoorLoaded.PNG";
                        break;
                    case "Dice":
                        myImage.ImageFile = "Images\\DiceLoaded.PNG";
                        myImage.Tolerance = 99;
                        break;
                    case "CareerBuilder":
                        myImage.ImageFile = "Images\\CareerBuilderLoaded.PNG";
                        myImage.Tolerance = 99;
                        break;
                    //case _JobBoardName.Monster:
                    //    break;
                    default:
                        break;
                }



                myImage.Sleep = 2000;
                myImage.Attempts = 20;
                myImage.RelativeX = 10;
                myImage.RelativeY = 10;




                int[,] myArray2 = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
                if (myArray2.Length == 0)
                {
                    Console.WriteLine("page not loaded");
                    goto checkLoaded;
                }
            }

            myImage = new ImageEntity();
            myImage.Sleep = 500;
            myImage.Attempts = 20;
            myImage.RelativeX = 10;
            myImage.RelativeY = 10;
            myImage.Tolerance = 97;

            switch (jobBoard.Name)
            {
                case "LinkedIn":

                    break;
                case "Indeed":

                    break;
                case "Glassdoor":
                    if (location.LocationName.ToLower() != "remote")
                    {
                        myImage.RelativeX = -100;
                        myImage.RelativeY = 20;
                        myImage.ImageFile = "Images\\SearchJobs.PNG";
                        myActions.ClickImageIfExists(myImage);
                        myActions.TypeText("^(a)", 500);
                        myActions.TypeText(location.LocationName + ", IL", 500);
                        myActions.TypeText("+({TAB})", 500);
                        myActions.TypeText("+({TAB})", 500);
                        myActions.TypeText("^(a)", 500);
                        myActions.TypeText(keyword.Keyword.Replace("c%23", "c#"), 500);
                        myImage.ImageFile = "Images\\SearchJobs.PNG";
                        myImage.RelativeX = 10;
                        myImage.RelativeY = 10;
                        myActions.ClickImageIfExists(myImage);
                        myActions.Sleep(1000);
                        myImage.Tolerance = 99;
                        myImage.Attempts = 4;
                        myImage.ImageFile = "Images\\ExactLocation.PNG";
                        myActions.ClickImageIfExists(myImage);
                        myImage.ImageFile = "Images\\ExactLocationChecked.PNG";
                        myActions.ClickImageIfExists(myImage);
                        myActions.Sleep(5000);
                    }

                    break;
                case "Dice":
                    break;
                //case _JobBoardName.Monster:
                //    break;
                case "CareerBuilder":
                    break;
                default:
                    break;
            }





            myImage = new ImageEntity();

            myImage.Sleep = 200;
            myImage.Attempts = 1;
            myImage.RelativeX = 10;
            myImage.RelativeY = 10;
            myImage.Tolerance = 99;
            switch (jobBoard.Name)
            {
                case "LinkedIn":
                    myImage.ImageFile = "Images\\NoMatchingJobsLinkedIn.PNG";
                    break;
                case "Indeed":
                    myImage.ImageFile = "Images\\NoJobsFoundWithinIndeed.PNG";
                    break;
                case"Glassdoor":
                    myImage.ImageFile = "Images\\NoJobsGlassDoor.PNG";
                    break;
                case "Dice":
                    myImage.ImageFile = "Images\\NoJobsDice.PNG";

                    break;
                //case _JobBoardName.Monster:
                //    break;
                case "CareerBuilder":
                    myImage.ImageFile = "Images\\NoJobsCareerBuilder.PNG";

                    break;
                default:
                    break;
            }






            int[,] myArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
            if (myArray.Length == 0)
            {
                Console.WriteLine(location.LocationName + "jobs found");
                // linkedin steps
                // use ctrl-shift-j to open console
                // ctrl-[ to go to elements
                // ctrl-c
                // read each line looking for href="/jobs/view
                // when line contains href="/jobs/view, need to find closing double quote and add the 
                //  text found to url list
                // begin-delim = href="/jobs/view
                // end-delim = double quote

                // first time do ctrl-shift j only once; other times, do it twice to reset everything
                myImage = new ImageEntity();

                myImage.Sleep = 200;
                myImage.Attempts = 1;
                myImage.RelativeX = 10;
                myImage.RelativeY = 10;
                myImage.Tolerance = 99;
                myImage.ImageFile = "Images\\DevToolsModeOn.PNG";           






                int[,] myArray2 = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
                // if already in dev mode, we want to get out and get back in to refresh
                if (myArray2.Length != 0)
                {
                    // linkedin steps
                    myActions.TypeText("^+(j)", 100);
                    myActions.TypeText("^+(j)", 3500);
                }
                else
                {
                    myActions.TypeText("^+(j)", 100);
                }
                myImage = new ImageEntity();



                myImage.ImageFile = "Images\\DevToolsElements.PNG";



                myImage.Sleep = 500;
                myImage.Attempts = 5;
                myImage.RelativeX = 10;
                myImage.RelativeY = 10;
                myImage.Tolerance = 99;



                myArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
                if (myArray.Length == 0)
                {
                    Console.WriteLine("could not find elements tab in developer tools");
                }
                myActions.TypeText("^([)", 3500);
                developermode = true;


                myActions.Sleep(2000);
                myActions.TypeText("^(a)", 500);
                myActions.TypeText("^(c)", 500);
                string myPage = myActions.PutClipboardInEntity();
             //   string myPage = myActions.SelectAllCopyIntoEntity(2500);
                int indexOfPagination = myPage.IndexOf("pagination");
                if (indexOfPagination == -1 || jobBoard.Name == "Indeed" || jobBoard.Name == "Dice"
                     || jobBoard.Name == "Glassdoor")
                {
                    processNextPage = false;
                }
                switch (jobBoard.Name)
                {
                    case "LinkedIn":
                        myPage = FindJobsUpdateDB_LinkedIn(con, cmd, myActions, keyword.Keyword, location.LocationName, myPage);

                        break;
                    case "Indeed":
                        myPage = FindJobsUpdateDB_Indeed(con, cmd, myActions, keyword.Keyword, location.LocationName, myPage);

                        break;
                    case "Glassdoor":
                        myPage = FindJobsUpdateDB_Glassdoor(con, cmd, myActions, keyword.Keyword, location.LocationName, myPage);
                        break;
                    case "Dice":
                        myPage = FindJobsUpdateDB_Dice(con, cmd, myActions, keyword.Keyword, location.LocationName, myPage);
                        break;
                    //case _JobBoardName.Monster:
                    //    break;
                    case "CareerBuilder":
                        myPage = FindJobsUpdateDB_CareerBuilder(con, cmd, myActions, keyword.Keyword, location.LocationName, myPage);
                        break;
                    default:
                        break;
                }

                // Console.WriteLine("Jobs found");                
            }
            else
            {
                Console.WriteLine(location.LocationName + "no jobs found");
                processNextPage = false;
                return;
            }
        }

        private static string FindJobsUpdateDB_LinkedIn(SqlConnection con, SqlCommand cmd, Methods myActions, string keyword, string locgeo, string myPage)
        {
            bool eof = false;
            int indexBeginDelim = -1;
            int indexEndDelim = -1;
            int indexSpanEndDelim = -1;
            string myUrlFound = "";
            string myJobTitleFound = "";
            string myCompanyTitleFound = "";
            while (!eof)
            {
                indexBeginDelim = myPage.IndexOf("/jobs/view");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim);
                indexEndDelim = myPage.IndexOf("\"");
                if (indexEndDelim == -1)
                {
                    Console.WriteLine("closing quote not found for url" + myPage.Substring(0, 250));
                    eof = true;
                    continue;
                }
                myUrlFound = "https://www.linkedin.com" + myPage.Substring(0, indexEndDelim);

                // get job title
                indexEndDelim = myPage.IndexOf("</a>");
                indexSpanEndDelim = myPage.IndexOf("<span");
                if (indexSpanEndDelim != -1 && indexSpanEndDelim < indexEndDelim)
                {
                    indexEndDelim = indexSpanEndDelim;
                }
                if (indexEndDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myJobTitleFound = myPage.Substring(0, indexEndDelim - 4);

                indexBeginDelim = myJobTitleFound.LastIndexOf("\">") + 2;
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myJobTitleFound = myJobTitleFound.Substring(indexBeginDelim).Replace("\r\n", "").Replace("<!---->", "").Trim();

                if (myJobTitleFound == "")
                {
                    myPage = myPage.Substring(indexBeginDelim);
                    continue;
                }

                // get company name
                indexBeginDelim = myPage.IndexOf("company_link");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim);

                indexBeginDelim = myPage.IndexOf(">") + 1;
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim);

                indexEndDelim = myPage.IndexOf("<");
                if (indexEndDelim == -1)
                {
                    Console.WriteLine("closing angle bracket not found for company title" + myPage.Substring(0, 250));
                    eof = true;
                    continue;
                }
                myCompanyTitleFound = myPage.Substring(0, indexEndDelim).Trim();

                try
                {
                    con.Open();
                    EscapeSingleQuote(ref myUrlFound);
                    EscapeSingleQuote(ref keyword);
                    EscapeSingleQuote(ref myJobTitleFound);
                   EscapeSingleQuote(ref myCompanyTitleFound);
                    ExecuteSQLToInsertUpdateKeyValuePair(cmd, myUrlFound, keyword, locgeo, myJobTitleFound, myCompanyTitleFound);


                }
                finally
                {
                    con.Close();

                }
                //   Console.WriteLine(myUrlFound);
                myPage = myPage.Substring(indexEndDelim);
            }

            return myPage;
        }

        static void EscapeSingleQuote(ref string stringToClean)
        {
            stringToClean = stringToClean.Replace("'", "''");
        }
        private static string FindJobsUpdateDB_Indeed(SqlConnection con, SqlCommand cmd, Methods myActions, string keyword, string locgeo, string myPage)
        {
            bool eof = false;
            int indexBeginDelim = -1;
            int indexEndDelim = -1;
            int indexSpanEndDelim = -1;
            string myUrlFound = "";
            string myJobTitleFound = "";
            string myCompanyTitleFound = "";
            while (!eof)
            {
                // find job card
                indexBeginDelim = myPage.IndexOf("<h2 class=\"title\">");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                // find job link
                myPage = myPage.Substring(indexBeginDelim);
                indexBeginDelim = myPage.IndexOf("href=\"");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim + 6);
                indexEndDelim = myPage.IndexOf("\"");
                if (indexEndDelim == -1)
                {
                    Console.WriteLine("closing quote not found for url" + myPage.Substring(0, 250));
                    eof = true;
                    continue;
                }

                if (myPage.StartsWith("http"))
                {
                    myUrlFound = myPage.Substring(0, indexEndDelim);
                }
                else
                {
                    myUrlFound = "https://www.indeed.com" + myPage.Substring(0, indexEndDelim);
                }

                // get job title
                indexBeginDelim = myPage.IndexOf("title=\"");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim + 7);
                indexEndDelim = myPage.IndexOf("\"");
                myJobTitleFound = myPage.Substring(0, indexEndDelim - 4);

                // get company name
                indexBeginDelim = myPage.IndexOf("company");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim);

                indexBeginDelim = myPage.IndexOf("companyName");
                if (indexBeginDelim != -1)
                {
                    myPage = myPage.Substring(indexBeginDelim);
                }
                

                indexBeginDelim = myPage.IndexOf(">") + 1;
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim);

                indexEndDelim = myPage.IndexOf("<");
                if (indexEndDelim == -1)
                {
                    Console.WriteLine("closing angle bracket not found for company title" + myPage.Substring(0, 250));
                    eof = true;
                    continue;
                }
                myCompanyTitleFound = myPage.Substring(0, indexEndDelim).Trim();

                try
                {
                    con.Open();
                    EscapeSingleQuote(ref myUrlFound);
                    EscapeSingleQuote(ref keyword);
                    EscapeSingleQuote(ref myJobTitleFound);
                    EscapeSingleQuote(ref myCompanyTitleFound);
                    ExecuteSQLToInsertUpdateJob_Indeed(cmd, myUrlFound, keyword, locgeo, myJobTitleFound, myCompanyTitleFound);


                }
                finally
                {
                    con.Close();

                }
                //   Console.WriteLine(myUrlFound);
                myPage = myPage.Substring(indexEndDelim);
            }

            return myPage;
        }

        private static string FindJobsUpdateDB_Glassdoor(SqlConnection con, SqlCommand cmd, Methods myActions, string keyword, string locgeo, string myPage)
        {
            bool eof = false;
            int indexBeginDelim = -1;
            int indexEndDelim = -1;
            int indexSpanEndDelim = -1;
            string myUrlFound = "";
            string myJobTitleFound = "";
            string myCompanyTitleFound = "";
            while (!eof)
            {
                // find job card
                indexBeginDelim = myPage.IndexOf("jobHeader");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                // find job link
                myPage = myPage.Substring(indexBeginDelim);
                indexBeginDelim = myPage.IndexOf("href=\"");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim + 6);
                indexEndDelim = myPage.IndexOf("\"");
                if (indexEndDelim == -1)
                {
                    Console.WriteLine("closing quote not found for url" + myPage.Substring(0, 250));
                    eof = true;
                    continue;
                }


                myUrlFound = myPage.Substring(0, indexEndDelim);


                // get company title
                indexBeginDelim = myPage.IndexOf("<span>");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim + 5);
                indexEndDelim = myPage.IndexOf("</span>");

                myCompanyTitleFound = myPage.Substring(0, indexEndDelim);

                indexBeginDelim = myCompanyTitleFound.LastIndexOf("\">") + 2;
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myCompanyTitleFound = myCompanyTitleFound.Substring(indexBeginDelim).Trim();

                if (myCompanyTitleFound == "")
                {
                    myPage = myPage.Substring(indexBeginDelim);
                    continue;
                }

                // get Job name
                indexBeginDelim = myPage.IndexOf("jobTitle");
                myPage = myPage.Substring(indexBeginDelim + 8);
                indexEndDelim = myPage.IndexOf("</a>");

                myJobTitleFound = myPage.Substring(0, indexEndDelim);

                myJobTitleFound = myJobTitleFound.Replace("<span>","").Replace("</span>", "");

               indexBeginDelim = myJobTitleFound.LastIndexOf("\">") + 2;
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myJobTitleFound = myJobTitleFound.Substring(indexBeginDelim).Trim();

                if (myJobTitleFound == "")
                {
                    myPage = myPage.Substring(indexBeginDelim);
                    continue;
                }

                try
                {
                    con.Open();
                    EscapeSingleQuote(ref myUrlFound);
                    EscapeSingleQuote(ref keyword);
                    EscapeSingleQuote(ref myJobTitleFound);
                    EscapeSingleQuote(ref myCompanyTitleFound);
                    ExecuteSQLToInsertUpdateJob_Glassdoor(cmd, myUrlFound, keyword, locgeo, myJobTitleFound, myCompanyTitleFound);


                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();

                }
                //   Console.WriteLine(myUrlFound);
                myPage = myPage.Substring(indexEndDelim);
            }

            return myPage;
        }

        private static string FindJobsUpdateDB_Dice(SqlConnection con, SqlCommand cmd, Methods myActions, string keyword, string locgeo, string myPage)
        {
            bool eof = false;
            int indexBeginDelim = -1;
            int indexEndDelim = -1;
            int indexSpanEndDelim = -1;
            string myUrlFound = "";
            string myJobTitleFound = "";
            string myCompanyTitleFound = "";
            while (!eof)
            {
                // find job card
                indexBeginDelim = myPage.IndexOf("card-title-link");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                // find job link
                myPage = myPage.Substring(indexBeginDelim);
                indexBeginDelim = myPage.IndexOf("href=\"");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim + 6);
                indexEndDelim = myPage.IndexOf("\"");
                if (indexEndDelim == -1)
                {
                    Console.WriteLine("closing quote not found for url" + myPage.Substring(0, 250));
                    eof = true;
                    continue;
                }


                myUrlFound = myPage.Substring(0, indexEndDelim);


                // get job title
                indexBeginDelim = myPage.IndexOf(">");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim + 1);
                indexEndDelim = myPage.IndexOf("<");
                myJobTitleFound = myPage.Substring(0, indexEndDelim - 1);

                // get company name
                indexBeginDelim = myPage.IndexOf("search-result-company-name");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim);

                indexBeginDelim = myPage.IndexOf(">") + 1;
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim);

                indexEndDelim = myPage.IndexOf("<");
                if (indexEndDelim == -1)
                {
                    Console.WriteLine("closing angle bracket not found for company title" + myPage.Substring(0, 250));
                    eof = true;
                    continue;
                }
                myCompanyTitleFound = myPage.Substring(0, indexEndDelim).Trim();

                try
                {
                    con.Open();
                    EscapeSingleQuote(ref myUrlFound);
                    EscapeSingleQuote(ref keyword);
                    EscapeSingleQuote(ref myJobTitleFound);
                    EscapeSingleQuote(ref myCompanyTitleFound);
                    ExecuteSQLToInsertUpdateJob_Dice(cmd, myUrlFound, keyword, locgeo, myJobTitleFound, myCompanyTitleFound);


                }
                finally
                {
                    con.Close();

                }
                //   Console.WriteLine(myUrlFound);
                myPage = myPage.Substring(indexEndDelim);
            }

            return myPage;
        }

        private static string FindJobsUpdateDB_CareerBuilder(SqlConnection con, SqlCommand cmd, Methods myActions, string keyword, string locgeo, string myPage)
        {
            bool eof = false;
            int indexBeginDelim = -1;
            int indexEndDelim = -1;
            int indexSpanEndDelim = -1;
            string myUrlFound = "";
            string myJobTitleFound = "";
            string myCompanyTitleFound = "";
            while (!eof)
            {
                // find job card
                indexBeginDelim = myPage.IndexOf("data-results-content block job-listing-item active");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                // find job link
                myPage = myPage.Substring(indexBeginDelim);
                indexBeginDelim = myPage.IndexOf("href=\"");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim + 6);
                indexEndDelim = myPage.IndexOf("\"");
                if (indexEndDelim == -1)
                {
                    Console.WriteLine("closing quote not found for url" + myPage.Substring(0, 250));
                    eof = true;
                    continue;
                }


                myUrlFound = "https://www.careerbuilder.com" + myPage.Substring(0, indexEndDelim);


                // get job title
                indexBeginDelim = myPage.IndexOf("data-results-title");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim + 1);

                indexBeginDelim = myPage.IndexOf(">");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim + 1);
                indexEndDelim = myPage.IndexOf("<");
                myJobTitleFound = myPage.Substring(0, indexEndDelim - 1);

                // get company name
                indexBeginDelim = myPage.IndexOf("data-details");
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim);

                indexBeginDelim = myPage.IndexOf("<span>") + 6;
                if (indexBeginDelim == -1)
                {
                    eof = true;
                    continue;
                }
                myPage = myPage.Substring(indexBeginDelim);

                indexEndDelim = myPage.IndexOf("<");
                if (indexEndDelim == -1)
                {
                    Console.WriteLine("closing angle bracket not found for company title" + myPage.Substring(0, 250));
                    eof = true;
                    continue;
                }
                myCompanyTitleFound = myPage.Substring(0, indexEndDelim).Trim();

                try
                {
                    con.Open();
                    EscapeSingleQuote(ref myUrlFound);
                    EscapeSingleQuote(ref keyword);
                    EscapeSingleQuote(ref myJobTitleFound);
                    EscapeSingleQuote(ref myCompanyTitleFound);
                    ExecuteSQLToInsertUpdateJob_CareerBuilder(cmd, myUrlFound, keyword, locgeo, myJobTitleFound, myCompanyTitleFound);


                }
                finally
                {
                    con.Close();

                }
                //   Console.WriteLine(myUrlFound);
                myPage = myPage.Substring(indexEndDelim);
            }

            return myPage;
        }

        private static void ExecuteSQLToInsertUpdateKeyValuePair(SqlCommand cmd, string myUrlFound, string keyword, string location, string jobTitle, string companyTitle)
        {
            int indexBeginDelim = -1;
            int indexEndDelim = -1;
            string urlNumber = "";

            indexBeginDelim = myUrlFound.IndexOf("/jobs/view") + 11;

            urlNumber = myUrlFound.Substring(indexBeginDelim);
            indexEndDelim = urlNumber.IndexOf("/");
            if (indexEndDelim == -1)
            {
                indexEndDelim = urlNumber.Length;
            }


            urlNumber = "https://www.linkedin.com/jobs/view/" + urlNumber.Substring(0, indexEndDelim);
            cmd.CommandText = " IF EXISTS ( " +
  "		SELECT * " +
  "		FROM JobApplications " +
  "		WHERE JobUrl = '" + myUrlFound + "' " +
  "		) " +
  " BEGIN " +
  "	UPDATE [dbo].[JobApplications] " +
  /*
   *    SET [JobUrl] = <JobUrl, nvarchar(450),>
  ,[JobBoard] = <JobBoard, nvarchar(500),>
  ,[DateAdded] = <DateAdded, datetime,>
  ,[DateLastModified] = <DateLastModified, datetime,>
  ,[DateApplied] = <DateApplied, datetime,>
  ,[ApplicationStatus] = <ApplicationStatus, nvarchar(50),>
  */
  "	SET [DateLastModified] = '" + System.DateTime.Now + "', " +
"	 [JobBoard] = '" + "LinkedIn" + "', " +
"	 [Keyword] = '" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "', " +
 "	 [Location] = '" + location + "', " +
  "	 [JobTitle] = '" + jobTitle + "', " +
  "	 [CompanyTitle] = '" + companyTitle + "' " +
  "		WHERE JobUrl = '" + myUrlFound + "' " +
  " END; " +
  " ELSE " +
  " BEGIN " +
                  " IF NOT EXISTS ( " +
  "		SELECT * " +
  "		FROM JobApplications " +
  "		WHERE JobUrl like '" + urlNumber + "%' and keyword = '" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "'" +
  "		) " +
  " BEGIN " +
  "	INSERT INTO [dbo].[JobApplications] ( " +
  "		[JobUrl] " +
   "		,[JobBoard] " +
    "		,[Keyword] " +
     "		,[Location] " +
         "		,[JobTitle] " +
             "		,[CompanyTitle] " +
  "		,[DateAdded] " +
        "		,[DateLastModified] " +
  "		) " +
  "	VALUES ( " +
  "		'" + myUrlFound + "' " +
   "		,'" + "LinkedIn" + "' " +
      "		,'" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "' " +
         "		,'" + location + "' " +
          "		,'" + jobTitle + "' " +
           "		,'" + companyTitle + "' " +
  "		,'" + System.DateTime.Now + "' " +
     "		,'" + System.DateTime.Now + "' " +
  "		) " +
  "END; " +
        "END; ";
            cmd.ExecuteNonQuery();
        }

        private static void ExecuteSQLToInsertUpdateJob_Indeed(SqlCommand cmd, string myUrlFound, string keyword, string location, string jobTitle, string companyTitle)
        {

            cmd.CommandText = " IF EXISTS ( " +
  "		SELECT * " +
  "		FROM JobApplications " +
  "		WHERE JobUrl = '" + myUrlFound + "' " +
  "		) " +
  " BEGIN " +
  "	UPDATE [dbo].[JobApplications] " +
  /*
   *    SET [JobUrl] = <JobUrl, nvarchar(450),>
  ,[JobBoard] = <JobBoard, nvarchar(500),>
  ,[DateAdded] = <DateAdded, datetime,>
  ,[DateLastModified] = <DateLastModified, datetime,>
  ,[DateApplied] = <DateApplied, datetime,>
  ,[ApplicationStatus] = <ApplicationStatus, nvarchar(50),>
  */
  "	SET [DateLastModified] = '" + System.DateTime.Now + "', " +
"	 [JobBoard] = '" + "Indeed" + "', " +
"	 [Keyword] = '" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "', " +
 "	 [Location] = '" + location + "', " +
  "	 [JobTitle] = '" + jobTitle + "', " +
  "	 [CompanyTitle] = '" + companyTitle + "' " +
  "		WHERE JobUrl = '" + myUrlFound + "' " +
  " END; " +
  " ELSE " +
  " BEGIN " +
                  " IF NOT EXISTS ( " +
  "		SELECT * " +
  "		FROM JobApplications " +
  "		WHERE JobUrl = '" + myUrlFound + "' and keyword = '" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "'" +
  "		) " +
  " BEGIN " +
  "	INSERT INTO [dbo].[JobApplications] ( " +
  "		[JobUrl] " +
   "		,[JobBoard] " +
    "		,[Keyword] " +
     "		,[Location] " +
         "		,[JobTitle] " +
             "		,[CompanyTitle] " +
  "		,[DateAdded] " +
        "		,[DateLastModified] " +
  "		) " +
  "	VALUES ( " +
  "		'" + myUrlFound + "' " +
   "		,'" + "Indeed" + "' " +
      "		,'" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "' " +
         "		,'" + location + "' " +
          "		,'" + jobTitle + "' " +
           "		,'" + companyTitle + "' " +
  "		,'" + System.DateTime.Now + "' " +
     "		,'" + System.DateTime.Now + "' " +
  "		) " +
  "END; " +
        "END; ";
            cmd.ExecuteNonQuery();
        }

        private static void ExecuteSQLToInsertUpdateJob_Dice(SqlCommand cmd, string myUrlFound, string keyword, string location, string jobTitle, string companyTitle)
        {
            // there is a searchId with a guid that we need to remove to prevent dups
            int indexBeginDelim = myUrlFound.IndexOf("&amp;searchId");

            if (indexBeginDelim > 0)
            {
                myUrlFound = myUrlFound.Substring(0,indexBeginDelim);
            }

            cmd.CommandText = " IF EXISTS ( " +
  "		SELECT * " +
  "		FROM JobApplications " +
  "		WHERE JobUrl = '" + myUrlFound + "' " +
  "		) " +
  " BEGIN " +
  "	UPDATE [dbo].[JobApplications] " +
  /*
   *    SET [JobUrl] = <JobUrl, nvarchar(450),>
  ,[JobBoard] = <JobBoard, nvarchar(500),>
  ,[DateAdded] = <DateAdded, datetime,>
  ,[DateLastModified] = <DateLastModified, datetime,>
  ,[DateApplied] = <DateApplied, datetime,>
  ,[ApplicationStatus] = <ApplicationStatus, nvarchar(50),>
  */
  "	SET [DateLastModified] = '" + System.DateTime.Now + "', " +
"	 [JobBoard] = '" + "Dice" + "', " +
"	 [Keyword] = '" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "', " +
 "	 [Location] = '" + location + "', " +
  "	 [JobTitle] = '" + jobTitle + "', " +
  "	 [CompanyTitle] = '" + companyTitle + "' " +
  "		WHERE JobUrl = '" + myUrlFound + "' " +
  " END; " +
  " ELSE " +
  " BEGIN " +
                  " IF NOT EXISTS ( " +
  "		SELECT * " +
  "		FROM JobApplications " +
  "		WHERE JobUrl = '" + myUrlFound + "' and keyword = '" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "'" +
  "		) " +
  " BEGIN " +
  "	INSERT INTO [dbo].[JobApplications] ( " +
  "		[JobUrl] " +
   "		,[JobBoard] " +
    "		,[Keyword] " +
     "		,[Location] " +
         "		,[JobTitle] " +
             "		,[CompanyTitle] " +
  "		,[DateAdded] " +
        "		,[DateLastModified] " +
  "		) " +
  "	VALUES ( " +
  "		'" + myUrlFound + "' " +
   "		,'" + "Dice" + "' " +
      "		,'" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "' " +
         "		,'" + location + "' " +
          "		,'" + jobTitle + "' " +
           "		,'" + companyTitle + "' " +
  "		,'" + System.DateTime.Now + "' " +
     "		,'" + System.DateTime.Now + "' " +
  "		) " +
  "END; " +
        "END; ";
            cmd.ExecuteNonQuery();
        }

        private static void ExecuteSQLToInsertUpdateJob_CareerBuilder(SqlCommand cmd, string myUrlFound, string keyword, string location, string jobTitle, string companyTitle)
        {
            // there is a searchId with a guid that we need to remove to prevent dups
            int indexBeginDelim = myUrlFound.IndexOf("&amp;searchId");

            if (indexBeginDelim > 0)
            {
                myUrlFound = myUrlFound.Substring(0, indexBeginDelim);
            }

            cmd.CommandText = " IF EXISTS ( " +
  "		SELECT * " +
  "		FROM JobApplications " +
  "		WHERE JobUrl = '" + myUrlFound + "' " +
  "		) " +
  " BEGIN " +
  "	UPDATE [dbo].[JobApplications] " +
  /*
   *    SET [JobUrl] = <JobUrl, nvarchar(450),>
  ,[JobBoard] = <JobBoard, nvarchar(500),>
  ,[DateAdded] = <DateAdded, datetime,>
  ,[DateLastModified] = <DateLastModified, datetime,>
  ,[DateApplied] = <DateApplied, datetime,>
  ,[ApplicationStatus] = <ApplicationStatus, nvarchar(50),>
  */
  "	SET [DateLastModified] = '" + System.DateTime.Now + "', " +
"	 [JobBoard] = '" + "CareerBuilder" + "', " +
"	 [Keyword] = '" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "', " +
 "	 [Location] = '" + location + "', " +
  "	 [JobTitle] = '" + jobTitle + "', " +
  "	 [CompanyTitle] = '" + companyTitle + "' " +
  "		WHERE JobUrl = '" + myUrlFound + "' " +
  " END; " +
  " ELSE " +
  " BEGIN " +
                  " IF NOT EXISTS ( " +
  "		SELECT * " +
  "		FROM JobApplications " +
  "		WHERE JobUrl = '" + myUrlFound + "' and keyword = '" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "'" +
  "		) " +
  " BEGIN " +
  "	INSERT INTO [dbo].[JobApplications] ( " +
  "		[JobUrl] " +
   "		,[JobBoard] " +
    "		,[Keyword] " +
     "		,[Location] " +
         "		,[JobTitle] " +
             "		,[CompanyTitle] " +
  "		,[DateAdded] " +
        "		,[DateLastModified] " +
  "		) " +
  "	VALUES ( " +
  "		'" + myUrlFound + "' " +
   "		,'" + "CareerBuilder" + "' " +
      "		,'" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "' " +
         "		,'" + location + "' " +
          "		,'" + jobTitle + "' " +
           "		,'" + companyTitle + "' " +
  "		,'" + System.DateTime.Now + "' " +
     "		,'" + System.DateTime.Now + "' " +
  "		) " +
  "END; " +
        "END; ";
            cmd.ExecuteNonQuery();
        }
        private static void ExecuteSQLToInsertUpdateJob_Glassdoor(SqlCommand cmd, string myUrlFound, string keyword, string location, string jobTitle, string companyTitle)
        {
            myUrlFound = "https://www.glassdoor.com/partner/jobListing.htm?jobListingId=" + myUrlFound.Substring(myUrlFound.IndexOf("jobListingId=") + 13);
            cmd.CommandText = " IF EXISTS ( " +
  "		SELECT * " +
  "		FROM JobApplications " +
  "		WHERE JobUrl = '" + myUrlFound + "' " +
  "		) " +
  " BEGIN " +
  "	UPDATE [dbo].[JobApplications] " +
  /*
   *    SET [JobUrl] = <JobUrl, nvarchar(450),>
  ,[JobBoard] = <JobBoard, nvarchar(500),>
  ,[DateAdded] = <DateAdded, datetime,>
  ,[DateLastModified] = <DateLastModified, datetime,>
  ,[DateApplied] = <DateApplied, datetime,>
  ,[ApplicationStatus] = <ApplicationStatus, nvarchar(50),>
  */
  "	SET [DateLastModified] = '" + System.DateTime.Now + "', " +
"	 [JobBoard] = '" + "Glassdoor" + "', " +
"	 [Keyword] = '" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "', " +
 "	 [Location] = '" + location + "', " +
  "	 [JobTitle] = '" + jobTitle + "', " +
  "	 [CompanyTitle] = '" + companyTitle + "' " +
  "		WHERE JobUrl = '" + myUrlFound + "' " +
  " END; " +
  " ELSE " +
  " BEGIN " +
                  " IF NOT EXISTS ( " +
  "		SELECT * " +
  "		FROM JobApplications " +
  "		WHERE JobUrl = '" + myUrlFound + "' and keyword = '" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "'" +
  "		) " +
  " BEGIN " +
  "	INSERT INTO [dbo].[JobApplications] ( " +
  "		[JobUrl] " +
   "		,[JobBoard] " +
    "		,[Keyword] " +
     "		,[Location] " +
         "		,[JobTitle] " +
             "		,[CompanyTitle] " +
  "		,[DateAdded] " +
        "		,[DateLastModified] " +
  "		) " +
  "	VALUES ( " +
  "		'" + myUrlFound + "' " +
   "		,'" + "Glassdoor" + "' " +
      "		,'" + System.Net.WebUtility.HtmlDecode(keyword.Replace("%23", "#")) + "' " +
         "		,'" + location + "' " +
          "		,'" + jobTitle + "' " +
           "		,'" + companyTitle + "' " +
  "		,'" + System.DateTime.Now + "' " +
     "		,'" + System.DateTime.Now + "' " +
  "		) " +
  "END; " +
        "END; ";
            cmd.ExecuteNonQuery();
        }
    }
}

