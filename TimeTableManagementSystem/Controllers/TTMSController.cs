using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeTableManagementSystem.Controllers.BosinessLayer;
using TimeTableManagementSystem.Models.TTMS;
using System.Data;
using Newtonsoft.Json;

namespace TimeTableManagementSystem.Controllers
{
    public class TTMSController : Controller
    {
        // GET: TTMS
        public ActionResult Index()
        {
            return View();
        }

        // GET: TTMS/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TTMS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TTMS/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TTMS/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TTMS/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TTMS/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TTMS/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult TimeTableCreation()
        {
            return View();
        }

        public ActionResult GenerateTimeTable()
        {

            try
            {
                string html = "";
                int SubjectRowCount = 0;
                SubjectRowCount = Convert.ToInt32(Session["TotalSubjects"].ToString().Trim() == "" ? "0" : Session["TotalSubjects"].ToString());

                html += "<div class=\"row\">";


                html += "<div class=\"col-sm-4\">Enter Subject Name.</div>";

                html += "<div class=\"col-sm-4\">Enter Subject Hours.</div>";

                html += "</div>";

                html += "<br />";
                for (int i = 0; i < SubjectRowCount; i++)
                {

                    html += "<div class=\"row\">";
                    html += "<div class=\"col-sm-2\">";
                    html += "<input id=\"txtSubjectName" + i + "\" type=\"text\" class=\"form-control\" >";
                    html += "</div>";

                    html += "<div class=\"col-sm-2\">";
                    html += "</div>";

                    html += "<div class=\"col-sm-2\">";
                    html += "<input id=\"txtSubjectHour" + i + "\" type=\"text\" class=\"form-control\" onkeypress=\"return isNumber(event)\"  maxlength=\"2\">";
                    html += "</div>";
                    html += "</div>";
                    html += "<br />";
                }
                html += "<div class=\"row\">";
                html += "<div class=\"col-sm-2\"></div>";
                html += "<div class=\"col-sm-2\">";

                html += "<button type=\"button\" id=\"btnGenerate\" class=\"btn btn-success form - control\" style=\"text-align:center;\">Generate</button>";

                html += "</div>";
                html += "</div>";

                ViewBag.TimeTableGeneratorHtml = html;
                return View();
            }
            catch(Exception ex)
            {
                return RedirectToAction("TTMS", "TimeTableCreation");
            }
            
        }

        [HttpPost]
        public JsonResult SubmitTotalHoursForTimeTable(string TotalWorkingDays, string SubjectPerDay, string TotalSubject)
        {
            TTMSModels TTMS = new TTMSModels();
            TTMS.WorkingDayPerWeek = (String)TotalWorkingDays == "" ? 0 : Convert.ToInt32(TotalWorkingDays);
            TTMS.SubjectPerday = (String)SubjectPerDay == "" ? 0 : Convert.ToInt32(SubjectPerDay);
            TTMS.TotalSubjects = (String)TotalSubject == "" ? 0 : Convert.ToInt32(TotalSubject);

            BLTTMS blTimeTable = new BLTTMS();
            DataTable Result = blTimeTable.SubmitTotalHoursForTimeTable(TTMS);
            if(Result.Rows.Count > 0)
            {
                Session["TotalHours"] = Convert.ToInt32(Result.Rows[0]["TotalHours"]);
                Session["TotalSubjects"] = Convert.ToInt32(Result.Rows[0]["TotalSubjects"]);
                Session["WorkingDayPerWeek"] = Convert.ToInt32(Result.Rows[0]["WorkingDayPerWeek"]);
                Session["SubjectPerday"] = Convert.ToInt32(Result.Rows[0]["SubjectPerday"]);
            }

            var json = new { state = JsonConvert.SerializeObject(Result) };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GenerateTimeTable(string AllSubjectName, string AllSubjectHour)
        {

            try
            {



                string[] countAllHour = AllSubjectHour.Split(',');

                int totalSum = countAllHour.Sum(x => int.Parse(x));

                if (totalSum != Convert.ToInt32(Session["TotalHours"]))
                {
                    return Json("2", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TTMSModels TTMS = new TTMSModels();

                    TTMS.AllSubjectsName = (String)AllSubjectName == null ? "" : Convert.ToString(AllSubjectName);
                    TTMS.AllSubjectsHour = (String)AllSubjectHour == null ? "" : Convert.ToString(AllSubjectHour);


                    BLTTMS blTimeTable = new BLTTMS();
                    DataTable Result = blTimeTable.GenerateTimeTable(TTMS);
                    DataTable FinalDT = new DataTable();
                    if (Result.Rows.Count > 0)
                    {
                        int TotalSubjectPerDay = Convert.ToInt32(Session["SubjectPerday"]);
                        int TotalWorkingDays = Convert.ToInt32(Session["WorkingDayPerWeek"]);


                        FinalDT.Columns.Add("Days", typeof(string));

                        for (int i = 0; i < TotalSubjectPerDay; i++)
                        {
                            FinalDT.Columns.Add("Period" + (i + 1).ToString() + "", typeof(string));
                        }

                        string[] weekday = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                        for (int i = 0; i < TotalWorkingDays; i++)
                        {

                            FinalDT.Rows.Add();
                            FinalDT.Rows[i]["Days"] = weekday[i];
                        }

                        for (int i = 0; i < FinalDT.Rows.Count; i++)
                        {
                            for (int j = 1; j < FinalDT.Columns.Count; j++)
                            {
                                Random random = new Random();
                                int randomNumber = random.Next(0, Result.Rows.Count);

                                if (Convert.ToInt32(Result.Rows[randomNumber]["SubjectHour"]) > 0)
                                {
                                    var SubjectText = Convert.ToString(Result.Rows[randomNumber]["SubjectName"]);
                                    var SubjectHour = Convert.ToInt32(Result.Rows[randomNumber]["SubjectHour"]);
                                    FinalDT.Rows[i][j] = SubjectText;
                                    Result.Rows[randomNumber]["SubjectHour"] = SubjectHour - 1;
                                }

                                else if (Convert.ToInt32(Result.Rows[randomNumber]["SubjectHour"]) == 0)
                                {
                                    Result.Rows.RemoveAt(randomNumber);
                                    Result.AcceptChanges();
                                    int randomNumber2 = random.Next(0, Result.Rows.Count);
                                    if (Convert.ToInt32(Result.Rows[randomNumber2]["SubjectHour"]) > 0)
                                    {
                                        var SubjectText = Convert.ToString(Result.Rows[randomNumber2]["SubjectName"]);
                                        var SubjectHour = Convert.ToInt32(Result.Rows[randomNumber2]["SubjectHour"]);
                                        FinalDT.Rows[i][j] = SubjectText;
                                        Result.Rows[randomNumber2]["SubjectHour"] = SubjectHour - 1;
                                    }
                                }
                            }
                        }

                    }

                    var json = new { state = JsonConvert.SerializeObject(FinalDT) };
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
