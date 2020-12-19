using razorop.EDM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace razorop.Controllers
{
    public class employeeController : Controller
    {
        razorCrudEntities dc = new razorCrudEntities();
        // GET: employee
        public void getcountry()
        {
            var c = dc.tblcountries.ToList();
            List<SelectListItem> objlist = new List<SelectListItem>();


            foreach (var item in c)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.country_name;
                obj.Value = item.country_id.ToString();
                objlist.Add(obj);
            }

            ViewData["temp"] = objlist;
            //ViewBag.temp = objlist;
        }


        public ActionResult Index()
        {
            getcountry();
            return View();
        }
        [HttpPost]

        public ActionResult Index(tblemp obj, FormCollection fc, HttpPostedFileBase file)
        {
            //var temp = fc["Reading"].ToString();
            //var tm = fc["Writing"].ToString();

            var reading = Convert.ToBoolean(fc["Reading"].Split(',')[0]);
            var writing = Convert.ToBoolean(fc["Writing"].Split(',')[0]);
            var playing = Convert.ToBoolean(fc["Playing"].Split(',')[0]);

            var strhob = "";

            if (reading == true)
            {
                strhob += "Reading" + ",";
            }

            if (writing == true)
            {
                strhob += "Writing" + ",";
            }

            if (playing == true)
            {
                strhob += "Playing" + ",";
            }

            obj.emp_hob = strhob;
            obj.emp_status = "pending";
            //dc.tblemps.Add(obj);
            //dc.SaveChanges();

            var allowedExtension = new[] { ".JPG", ".Jpg", ".png", "jpeg", ".PNG" };

            if (file != null)
            {
                var ext = Path.GetExtension(file.FileName);

                if (allowedExtension.Contains(ext))
                {
                    var filename = file.FileName;
                    var path = Path.Combine(Server.MapPath("~/images"), filename);
                    file.SaveAs(path);
                    obj.emp_profile = filename;
                    dc.tblemps.Add(obj);
                    dc.SaveChanges();

                }
            }
            else
            {
                ViewBag.msg = "your image extension should be in .JPG, .Jpg, .png, jpeg";
            }
            getcountry();

            return View();
        }

        public ActionResult Display()
        {
            var p = (from n in dc.tblemps
                     from c in dc.tblcountries
                     where n.country_id == c.country_id
                     select new { n, c }).ToList();

            List<tblemp> objlist = new List<tblemp>();
            foreach (var item in p)
            {
                tblemp obj = new tblemp();
                obj.emp_name = item.n.emp_name;
                obj.emp_id = item.n.emp_id;
                obj.emp_add = item.n.emp_add;
                obj.emp_mob = item.n.emp_mob;
                obj.emp_dob = item.n.emp_dob;
                obj.emp_hob = item.n.emp_hob;
                obj.emp_gender = item.n.emp_gender;
                obj.emp_profile = item.n.emp_profile;
                obj.emp_status = item.n.emp_status;
                obj.country_name = item.c.country_name;

                objlist.Add(obj);
            }
            

            return View(objlist); ;
        }

        public void getcountry(int id)
        {
            var p = dc.tblcountries.ToList();
            List<SelectListItem> objlist = new List<SelectListItem>();

            foreach (var item in p)
            {
                if (id == item.country_id)
                {
                    SelectListItem obj = new SelectListItem();
                    obj.Text = item.country_name;
                    obj.Value = item.country_id.ToString();
                    obj.Selected = true;

                    objlist.Add(obj);
                }
                else
                {
                    SelectListItem obj = new SelectListItem();
                    obj.Text = item.country_name;
                    obj.Value = item.country_id.ToString();
                    obj.Selected = false;

                    objlist.Add(obj);
                }
            }
            ViewData["temp"] = objlist;
        }

        public List<checkmodel> checklist1()
        {
            List<checkmodel> chklist = new List<checkmodel>()
            { new checkmodel { id = 1, name = "reading", ischecked = false},
              new checkmodel { id = 1, name = "writing", ischecked = false},
              new checkmodel { id = 1,name = "playing",ischecked = false}
            };
            return chklist;
        }

        public List<checkmodel> setchecklist(string strhob)
        {
            var arr = strhob.Split(',');

            var ss = checklist1();

            foreach (var item in ss)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (item.name == arr[i])
                    {
                        item.ischecked = true;
                    }
                }
            }
            return ss;
        }

        public ActionResult edit(int id)
        {
            var pp = dc.tblemps.Find(id);
            getcountry(int.Parse(pp.country_id.ToString()));

            pp.checklist = setchecklist(pp.emp_hob);
            return View(pp);
        }
        [HttpPost]
        public ActionResult edit(tblemp obj, HttpPostedFileBase file)
        {
            var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", ".jpeg", ".JPG", ".PNG", ".Jpg" };
            if (file != null)
            {

                var ext = Path.GetExtension(file.FileName);

                if (allowedExtensions.Contains(ext))
                {

                    var filename = file.FileName;
                    var path = Path.Combine(Server.MapPath("~/images"), filename);

                    obj.emp_profile = filename;

                    file.SaveAs(path);

                    var cheklist = obj.checklist.Where(c => c.ischecked == true);
                    var strhob = "";
                    foreach (var item in cheklist)
                    {
                        if (item.id == 1)
                        {
                            strhob += "reading" + ",";

                        }
                        if (item.id == 2)
                        {
                            strhob += "writing" + ",";

                        }
                        if (item.id == 3)
                        {
                            strhob += "playing" + ",";
                        }
                    }
                    obj.emp_hob = strhob;

                    //obj.emp_status = "pending";

                    dc.Entry(obj).State = EntityState.Modified;

                    //dc.tblemps.Add(obj);
                    dc.SaveChanges();
                }
                else
                {
                    ViewBag.msg = "only image is uploaded";
                    getcountry();
                    return View();

                }
            }
            else
            {

                var cheklist = obj.checklist.Where(c => c.ischecked == true);
                var strhob = "";
                foreach (var item in cheklist)//id is liye li hei taki line wise rkhei (rwp);
                {
                    if (item.id == 1)
                    {
                        strhob += "reading" + ",";
                    }
                    if (item.id == 2)
                    {
                        strhob += "writing" + ",";
                    }
                    if (item.id == 3)
                    {
                        strhob += "playing" + ",";
                    }
                }

                obj.emp_hob = strhob;

                //obj.emp_status = "pending";

                dc.Entry(obj).State = EntityState.Modified; //joh bhi database mei pehlei tha, voh change ho jayega 

                //dc.tblemps.Add(obj);
                dc.SaveChanges();

            }

            getcountry();
            ModelState.Clear();

            return RedirectToAction("Display");

        }
    }
}