using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace JQGrid4U.BL
{
    public class Conveyor
    {
        public int siteID { get; set; }
        public string siteName { get; set; }
        public int conveyorID { get; set; }
        public string conveyorName { get; set; }
        public int internalTemp { get; set; }
        public string deviceID { get; set; }
        public int xval { get; set; }
        public int yval { get; set; }
        public int zval { get; set; }

    }


    public class ConveyorBusinessLogic
    {
        string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        public IEnumerable<Conveyor> Conveyors
        {
            get
            {
                List<Conveyor> Conveyors = new List<Conveyor>();
                using (SqlConnection conObj = new SqlConnection(conStr))
                {
                    string qry;
                    //                    qry = "WITH summary AS (" 
                    //                      + "SELECT p.unitMac, p.timestamp as deviceStamp,xval,yval,zval, "
                    //        + "floor(p.temp) as deviceInternalTemp, ROW_NUMBER() OVER(PARTITION BY p.unitMac ORDER BY p.timeStamp DESC) AS rk "
                    //      + "FROM periodicDataTest p ) " 
                    //    + "SELECT s.*,c.siteID,C.Site as SiteName,c.ConveyorId,c.Conveyor as conveyorName,c.Location FROM summary s "
                    //  + "left outer join vwDustMotesTest C on s.unitMac = C.unitMac "            
                    //+ "WHERE s.rk = 1 and isnull(c.siteID,0)<> 0";

                    qry = "SELECT c.temp,c.siteID,C.Site as SiteName,c.ConveyorId,c.Conveyor as conveyorName,c.Location FROM vwDustMotesTest c "                              
                                + "WHERE isnull(c.siteID,0)<> 0";


                    SqlCommand cmdObj = new SqlCommand(qry, conObj);
                    conObj.Open();
                    SqlDataReader readerObj = cmdObj.ExecuteReader();

                    while (readerObj.Read())
                    {

                        string vDeviceId = "";
                        string vSiteName = "";
                        string vConveyorName = "";
                        int vSiteID = 0;
                        int vConveyorId = 0;
                        double vTemp = 0;
                        int vInternalTemp = 0;
                        double vxval = 0;
                        double vyval = 0;
                        double vzval = 0;
                        //int xVal = 0;
                        //int yVal = 0;
                        int zVal = 0;
                        vSiteID = Convert.ToInt32(readerObj["siteID"].ToString());
                        vSiteName = readerObj["siteName"].ToString();
                        vConveyorId= Convert.ToInt32(readerObj["conveyorID"].ToString());
                        vConveyorName = readerObj["conveyorName"].ToString();
                        if (!DBNull.Value.Equals(readerObj["location"]))
                        {
                            vDeviceId = readerObj["location"].ToString();
                        }
                        //vTemp = Math.Round(Convert.ToDouble(readerObj["temp"].ToString()),0);
                        vInternalTemp = Convert.ToInt32(vTemp);

                        //vxval = Math.Round(Convert.ToDouble(readerObj["temp"].ToString()), 0);
                        //vyval= Math.Round(Convert.ToDouble(readerObj["yval"].ToString()), 0);
                        vzval= Math.Round(Convert.ToDouble(readerObj["temp"].ToString()), 0);
                        //xVal = Convert.ToInt32(vxval);
                        //yVal = Convert.ToInt32(vyval);
                        zVal = Convert.ToInt32(vzval);


                        Conveyor Conveyor = new Conveyor();
                        Conveyor.deviceID = vDeviceId;
                        //Conveyor.internalTemp = vInternalTemp;
                        //Conveyor.xval = xVal;
                        //Conveyor.yval = yVal;
                        Conveyor.zval = zVal;
                        Conveyor.siteID = vSiteID;
                        Conveyor.siteName = vSiteName;
                        Conveyor.conveyorID = vConveyorId;
                        Conveyor.conveyorName = vConveyorName;

                        //     Notes.emailAdd = readerObj["emailadd"].ToString();
                        Conveyors.Add(Conveyor);
                    }
                    conObj.Close();
                }
                return Conveyors;
            }
        }





    }





}
