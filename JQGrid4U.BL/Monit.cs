using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace JQGrid4U.BL
{
	public class Monit
	{

		public int autoID { get; set; }
		public string ID { get; set; }
		public string Status { get; set; }
		public string Typ { get; set; }
		public DateTime LastUpdate { get; set; }
		public string Laps { get; set; }
		public int siteId { get; set; }
		public string siteName { get; set; }
        public int conveyorId { get; set; }
        public string deviceID { get; set; }
        public string isAck { get; set; }
    }
    public class LogResolve
    {
        public int LogID { get; set; }
        public DateTime? LogDate { get; set; }
        public int NoteTypeID { get; set; }
        public string Serial { get; set; }
        public string DeviceID { get; set; }
        public string Typ { get; set; }
        public string Note { get; set; }
        public string Action { get; set; }
        public string userID { get; set; }
        public string org_serial { get; set; }
    }
    public class NoteList
    {
        public string Serial { get; set; }
        public DateTime noteDate { get; set; }
        public int NoteTypeID { get; set; }
        public string NoteTypeIdDesc { get; set; }
        public string Note { get; set; }
        public string userID { get; set; }
        public string fullname { get; set; }
    }
    public class tblDeviceHistory
    {
        public string deviceID { get; set; }
        public string deviceSerial { get; set; }
        public DateTime deviceInstallDate { get; set; }
        public DateTime? deviceRetireDate { get; set; }
    }

    public class SerialNo
    {
        public string deviceID { get; set; }
        public string serialno { get; set; }
    }

    public class MonitBusinessLogic
	{
		string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
		public IEnumerable<Monit> Monits
		{
			get
			{
				List<Monit> Monits = new List<Monit>();
				using (SqlConnection conObj = new SqlConnection(conStr))
				{
					string qry;
					qry = "select aa.autoID, aa.id,aa.status,aa.typ ,aa.lastupdate, Days = datediff(dd, 0, DateDif) ," +
						"aa.siteId,aa.siteName, " +
						"Hours = datepart(hour, DateDif)," +
						"Minutes = datepart(minute, DateDif)," +
						"Seconds = datepart(second, DateDif)," +
						"MS = datepart(ms, DateDif) ," +
						"convert(nvarchar(6), datediff(dd, 0, DateDif)) + 'd' + space(1) + " +
						"convert(nvarchar(6), datepart(hour, DateDif)) + 'h' + space(1) + " +
						"convert(nvarchar(6), datepart(minute, DateDif)) + 'm' + space(1) + " +
						"convert(nvarchar(6), datepart(second, DateDif)) + 's' as Laps " +
						"from (select autoID,a.siteid, b.sitename, a.id, status, typ, lastupdate, " +
						"DateDif = getdate() - lastupdate from tblMonitoringSub a left join tblSite b on a.siteid=b.siteid) aa";
					SqlCommand cmdObj = new SqlCommand(qry, conObj);
					conObj.Open();
					SqlDataReader readerObj = cmdObj.ExecuteReader();
					while (readerObj.Read())
					{
						Monit Monit = new Monit();
						Monit.autoID = Convert.ToInt32(readerObj["autoID"]);
						Monit.ID = readerObj["ID"].ToString();
						Monit.Status = readerObj["status"].ToString();
						Monit.Typ = readerObj["typ"].ToString();
						Monit.LastUpdate = Convert.ToDateTime(readerObj["lastupdate"].ToString());
						Monit.Laps = readerObj["laps"].ToString();
						Monit.siteId = Convert.ToInt32(readerObj["siteid"].ToString());
						Monit.siteName = readerObj["siteName"].ToString();
						Monits.Add(Monit);
					}
					conObj.Close();
				}
				return Monits;
			}
		}


        public IEnumerable<Monit> Monitx
        {
            get
            {
                List<Monit> Monits = new List<Monit>();
                using (SqlConnection conObj = new SqlConnection(conStr))
                {
                    string qry;
                    //qry = "select aa.autoID, aa.id,aa.status,aa.typ ,"  +
                    //    "aa.siteId,aa.siteName,'' as LapsX " +
                    //    "from (select autoID,a.siteid, b.sitename, a.ID, status, typ " +
                    //    "from vw_DeviceListX a left join tblSite b on a.siteid=b.siteid) aa";
                    //     qry = "select distinct unitMac as ID,'Roller' as Typ , status, siteID,siteName ,'' as LapsX from vw_tblMonitoringSub";

                    qry = " WITH summary AS(SELECT p.siteID, p.unitMac as ID, p.deviceStamp,p.Temp,"
                                + "p.xval, p.yval, p.zval,ROW_NUMBER() OVER(PARTITION BY p.unitMac "
                     + "ORDER BY p.deviceStamp DESC) AS rk FROM vw_tblMonitoringSub p) "
                    + "SELECT s.*, 'Roller' as Typ,' ' as LapsX,d.siteName, "
                     + "CASE WHEN s.temp < 31 THEN 'Ok' WHEN s.temp >= 31 AND s.temp < 36 "
                     + "THEN 'Warning' WHEN s.temp >= 36 THEN 'Critical' END AS Status "
                   + "FROM summary s left outer join tblSite D on s.siteId = D.siteID WHERE isnull(d.siteID,0)<>0 and s.rk = 1";

                    SqlCommand cmdObj = new SqlCommand(qry, conObj);
                    //cmdObj.CommandType = System.Data.CommandType.StoredProcedure;                    
                    conObj.Open();
                    SqlDataReader readerObj = cmdObj.ExecuteReader();
                    while (readerObj.Read())
                    {
                        Monit Monit = new Monit();
                       //Monit.autoID = Convert.ToInt32(readerObj["autoID"]);
                        Monit.ID = readerObj["ID"].ToString();
                       Monit.Status = readerObj["status"].ToString();
                        Monit.Typ = readerObj["typ"].ToString();
                        Monit.LastUpdate = DateTime.Today; 
                        Monit.Laps = readerObj["lapsX"].ToString();
                        Monit.siteId = Convert.ToInt32(readerObj["siteid"].ToString());
                        Monit.siteName = readerObj["siteName"].ToString();
                        Monits.Add(Monit);
                    }
                    conObj.Close();
                }
                return Monits;
            }
        }

        public IEnumerable<Monit> Monitx1 (int ysiteId)
        {
           
                List<Monit> Monits = new List<Monit>();
                using (SqlConnection conObj = new SqlConnection(conStr))
                {
                    string qry;
                  

                SqlCommand cmdObj = new SqlCommand("sp_MonitX3", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@siteID", ysiteId));
                conObj.Open();
                SqlDataReader readerObj = cmdObj.ExecuteReader();                
                
                    while (readerObj.Read())
                    {
                        Monit Monit = new Monit();
                        //Monit.autoID = Convert.ToInt32(readerObj["autoID"]);
                        Monit.ID = readerObj["location"].ToString();
                        Monit.Status = readerObj["status"].ToString();
                        Monit.Typ = readerObj["typ"].ToString();
                        Monit.LastUpdate = DateTime.Today;
                        Monit.Laps = readerObj["lastupdate"].ToString();
                        Monit.siteId = Convert.ToInt32(readerObj["siteid"].ToString());
                        Monit.conveyorId= Convert.ToInt32(readerObj["conveyorId"].ToString());
                        Monit.siteName = readerObj["siteName"].ToString();
                        Monit.isAck = readerObj["statusChangeAck"].ToString();
                    Monits.Add(Monit);
                    }
                    conObj.Close();
                }
                return Monits;
           
        }


        public IEnumerable<Monit> DeviceList(int yConveyorId)
        {

            List<Monit> Monits = new List<Monit>();
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                string qry;

                SqlCommand cmdObj = new SqlCommand("sp_MonitX3", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@conveyorID", yConveyorId));
                conObj.Open();
                SqlDataReader readerObj = cmdObj.ExecuteReader();

                while (readerObj.Read())
                {
                    Monit Monit = new Monit();
                    //Monit.autoID = Convert.ToInt32(readerObj["autoID"]);
                    Monit.ID = readerObj["ID"].ToString();
                    Monit.Status = readerObj["status"].ToString();
                    Monit.Typ = readerObj["typ"].ToString();
                    Monit.LastUpdate = DateTime.Today;
                    Monit.Laps = readerObj["lapsX"].ToString();
                    Monit.siteId = Convert.ToInt32(readerObj["siteid"].ToString());
                    Monit.deviceID = readerObj["deviceid"].ToString();
                    Monit.conveyorId = Convert.ToInt32(readerObj["conveyorId"].ToString());
                    Monit.siteName = readerObj["siteName"].ToString();

                    Monits.Add(Monit);
                }
                conObj.Close();
            }
            return Monits;

        }

        public IEnumerable<tblDeviceHistory> tblDeviceHistory
        {
            get
            {
                List<tblDeviceHistory> HistoryLists = new List<tblDeviceHistory>();
                using (SqlConnection conObj = new SqlConnection(conStr))
                {
                    string qry;
                    qry = "SELECT * FROM tblDeviceHistory";

                    SqlCommand cmdObj = new SqlCommand(qry, conObj);
                    conObj.Open();
                    SqlDataReader readerObj = cmdObj.ExecuteReader();

                    while (readerObj.Read())
                    {
                        DateTime defaultInstalldate = DateTime.Now;
                        DateTime defaultRetireDate = DateTime.Now;
                        DateTime.TryParse(readerObj["deviceInstallDate"].ToString(), out defaultInstalldate);
                        DateTime.TryParse(readerObj["deviceRetireDate"].ToString(), out defaultRetireDate);

                        tblDeviceHistory HistoryList = new tblDeviceHistory();
                        HistoryList.deviceID = readerObj["deviceID"].ToString();
                        HistoryList.deviceSerial = readerObj["deviceSerial"].ToString();
                        HistoryList.deviceInstallDate = defaultInstalldate;
                        HistoryList.deviceRetireDate = defaultRetireDate;

                        HistoryLists.Add(HistoryList);
                    }
                    conObj.Close();
                }
                return HistoryLists;
            }
        }
        //Save Device History
        public int InsertResolveLogReplace(LogResolve ResolveLog)
        {

            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                SqlCommand cmdObj = new SqlCommand("uspInsertDeviceHistory", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@DeviceID", ResolveLog.DeviceID));
                cmdObj.Parameters.Add(new SqlParameter("@Serial", ResolveLog.Serial));
                cmdObj.Parameters.Add(new SqlParameter("@LogDate", ResolveLog.LogDate));
                cmdObj.Parameters.Add(new SqlParameter("@LogRetireDate", ResolveLog.NoteTypeID == 3 ? ResolveLog.LogDate : null));
                conObj.Open();
                return Convert.ToInt32(cmdObj.ExecuteScalar());
            }
        }
        public int InsertResolveLog(LogResolve ResolveLog)
        {

            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                SqlCommand cmdObj = new SqlCommand("uspInsertDeviceHistory", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@DeviceID", ResolveLog.DeviceID));
                cmdObj.Parameters.Add(new SqlParameter("@Serial", ResolveLog.Serial));
                cmdObj.Parameters.Add(new SqlParameter("@LogDate", ResolveLog.LogDate));
                cmdObj.Parameters.Add(new SqlParameter("@LogRetireDate", DBNull.Value));
                conObj.Open();
                return Convert.ToInt32(cmdObj.ExecuteScalar());
            }
        }

        public int UpdateResolveDevice(LogResolve ResolveDevice)
        {
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                SqlCommand cmdObj = new SqlCommand("uspUpdateFixedDevice", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@DeviceID", ResolveDevice.DeviceID));
                cmdObj.Parameters.Add(new SqlParameter("@Serial", ResolveDevice.Serial));
                cmdObj.Parameters.Add(new SqlParameter("@NoteTypeID", ResolveDevice.NoteTypeID));
                conObj.Open();
                return Convert.ToInt32(cmdObj.ExecuteScalar());
            }
        }
        //Log Note Acknowledgement
        public int LogNoteAck(LogResolve NoteResolveDevice)
        {
            using (SqlConnection conObj = new SqlConnection(conStr))
            {
                SqlCommand cmdObj = new SqlCommand("uspInsertNoteFixedLog", conObj);
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.Parameters.Add(new SqlParameter("@Serial", NoteResolveDevice.Serial));
                cmdObj.Parameters.Add(new SqlParameter("@LogDate", NoteResolveDevice.LogDate));
                cmdObj.Parameters.Add(new SqlParameter("@userID", NoteResolveDevice.userID));
                cmdObj.Parameters.Add(new SqlParameter("@Note", NoteResolveDevice.Note));
                cmdObj.Parameters.Add(new SqlParameter("@DeviceID", NoteResolveDevice.DeviceID));
                conObj.Open();
                return Convert.ToInt32(cmdObj.ExecuteScalar());
            }
        }
        //Tables for Serial Numbers
        public IEnumerable<SerialNo> SerialNo
        {
            get
            {
                List<SerialNo> SerialList = new List<SerialNo>();
                using (SqlConnection conObj = new SqlConnection(conStr))
                {
                    string qry;
                    qry = "select deviceid,serialno from tblSerialNo";

                    SqlCommand cmdObj = new SqlCommand(qry, conObj);
                    conObj.Open();
                    SqlDataReader readerObj = cmdObj.ExecuteReader();

                    while (readerObj.Read())
                    {
                        SerialNo serilaNo = new BL.SerialNo();
                        serilaNo.deviceID = readerObj["deviceid"].ToString();
                        serilaNo.serialno = readerObj["serialno"].ToString();

                        SerialList.Add(serilaNo);
                    }
                    conObj.Close();
                }
                return SerialList;
            }
        }
        public IEnumerable<NoteList> NoteList
        {
            get
            {
                List<NoteList> NoteLists = new List<NoteList>();
                using (SqlConnection conObj = new SqlConnection(conStr))
                {
                    string qry;
                    qry = "select deviceSerial,noteDate,noteTypeID,(select top 1 noteTypeDesc from tblDeviceNoteType where noteTypeID = b.noteTypeID) notetypedesc,note,(select FirstName + ' ' + SurName  from tblUser a where a.userId = b.userId) fullname from tblDeviceNotes	b 	order by noteDate asc";

                    SqlCommand cmdObj = new SqlCommand(qry, conObj);
                    conObj.Open();
                    SqlDataReader readerObj = cmdObj.ExecuteReader();

                    while (readerObj.Read())
                    {
                        NoteList NoteList = new NoteList();
                        NoteList.Serial = readerObj["deviceSerial"].ToString();
                        NoteList.noteDate = Convert.ToDateTime(readerObj["noteDate"].ToString());
                        NoteList.NoteTypeID = Convert.ToInt32(readerObj["noteTypeID"]);
                        NoteList.Note = readerObj["note"].ToString();
                        NoteList.fullname = readerObj["fullname"].ToString();
                        NoteList.NoteTypeIdDesc = readerObj["notetypedesc"].ToString();

                        NoteLists.Add(NoteList);
                    }
                    conObj.Close();
                }
                return NoteLists;
            }
        }
    }

}
