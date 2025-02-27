﻿
using productpagepr.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace productpagepr.Controllers
{
    public class AboutUsController : Controller
    {
        string constring = @"Data Source=DESKTOP-CIV7264\SQLEXPRESS;Initial Catalog=LifeServeBD_DB;Integrated Security=True";

        // GET: AboutUs
       //returns view to user
        public ActionResult Index()
        {

            ViewBag.Message = "WHO WE ARE";



            SqlConnection con = new SqlConnection(constring);
            string q = "select Id,Name, Position,Department,Workplace,imgid from AboutUs";
            con.Open();
            SqlCommand cmd = new SqlCommand(q, con);
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return View(dt);

        }

        //shows all admin info in admin panel
        public ActionResult AboutInfoTable()
        {
            SqlConnection con = new SqlConnection(constring);
            string q = "select Id,Name, Position,Department,Workplace,imgid,Email from AboutUs";
            SqlCommand cmd = new SqlCommand(q, con);
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return View(dt);

        }

        // create new admin

        public ActionResult Create()
        {
            AboutUsModel ob = new AboutUsModel();
            return View("Create");
        }

        [HttpPost]
        public ActionResult Create(AboutUsModel ob, HttpPostedFileBase file)
        {
            SqlConnection con = new SqlConnection(constring);
            string q = "INSERT into AboutUs(Name, Position,Department,Workplace,imgid,Email,Pass) " +
                "values(@Name,@Position,@Department,@Workplace,@imgid,@Email,@Pass)";

            SqlCommand cmd = new SqlCommand(q, con);
            con.Open();
            cmd.Parameters.AddWithValue("@Name", ob.Name);
            cmd.Parameters.AddWithValue("@Position", ob.Position);
            cmd.Parameters.AddWithValue("@Department", ob.Department);
            cmd.Parameters.AddWithValue("@Workplace", ob.Workplace);

            cmd.Parameters.AddWithValue("@Email", ob.Email);
            cmd.Parameters.AddWithValue("@Pass", "Ad1234");


            if (file != null && file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string imgpath = Path.Combine(Server.MapPath("/aboutimages/"), filename);
                file.SaveAs(imgpath);

            }
            cmd.Parameters.AddWithValue("@imgid", "/aboutimages/" + file.FileName);

            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");


        }
        // delete a record
        public ActionResult Delete(int Id)
        {
            SqlConnection con = new SqlConnection(constring);
            string q = "Delete from AboutUs where Id=@Id";
            con.Open();
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.Parameters.AddWithValue("@Id", Id);

            cmd.ExecuteNonQuery();

            return RedirectToAction("AboutInfoTable");
        }


    }
}