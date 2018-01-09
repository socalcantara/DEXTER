using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace JQGrid4U.BL
{
    public class Site
    {
        public int siteid { get; set; }
        public string sitename { get; set; }
        public int conveyorid { get; set; }
        public string conveyorname { get; set; }

    }


    public class SiteBusinessLogic
    {
        string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;


        int counter = 1;
        public IEnumerable<Site> Sites
        {
            get
            {
                List<Site> Sites = new List<Site>();
                using (SqlConnection conObj = new SqlConnection(conStr))
                {

                    string qry;
                    qry = "select a.conveyorID,a.siteid,a.description as conveyorname,b.description as sitename from "
                        + "conveyor a left join site b on a.siteid=b.siteid order by a.conveyorid";
                    SqlCommand cmdObj = new SqlCommand(qry, conObj);
                    conObj.Open();
                    SqlDataReader readerObj = cmdObj.ExecuteReader();

                    while (readerObj.Read())
                    {
                        Site Site = new Site();
                        Site.siteid = Convert.ToInt32(readerObj["siteid"].ToString());
                        Site.sitename = readerObj["sitename"].ToString();
                        Site.conveyorid= Convert.ToInt32(readerObj["conveyorId"].ToString());
                        Site.conveyorname= readerObj["conveyorname"].ToString();
                        Sites.Add(Site);
                        counter = counter + 1;
                    }
                }
                return Sites;
            }
        }

    }


}
