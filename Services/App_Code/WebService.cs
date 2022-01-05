using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    string strCon = "Data Source={USERNAME};Initial Catalog={DBNAME};Integrated Security=True";
    SqlConnection con;
    SqlCommand cmd;
    SqlDataAdapter da;
    DataTable dt;

    public string Date { get; private set; }

    public WebService()
    {
        con = new SqlConnection(strCon);
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public DataTable Users_Meetings_Select(string adminId)
    {
        dt = new DataTable("Meetings");
        da = new SqlDataAdapter("SELECT Meetingtitle, MeetingMessage, MeetingAddress, MeetingDate, MeetingTime, MeetingImportant FROM Meetings WHERE MeetingAdminId='" + adminId + "'", con);
        da.Fill(dt);
        return dt;
    }

    [WebMethod]
    public DataTable Users_Complaints_Select(string userId)
    {
        dt = new DataTable("Complaints");
        da = new SqlDataAdapter("SELECT ComplaintTitle, ComplaintMessage, ComplaintTime, ComplaintDate FROM Complaints WHERE ComplaintUserId='" + userId + "'", con);
        da.Fill(dt);
        return dt;
    }

    [WebMethod]
    public void Users_Complaints_Insert(string userId, string title, string message, string adminId)
    {
        cmd = new SqlCommand("INSERT INTO Complaints(ComplaintTitle, ComplaintMessage, ComplaintTime, ComplaintDate, ComplaintUserId, ComplaintAdminId) VALUES('" + title + "','" + message + "','" + DateTime.Now.ToString("HH:mm") + "','" + DateTime.Now.ToString("dd/MM/yyyy") + "','" + userId + "','" + adminId + "');", con);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
    }

    [WebMethod]
    public DataTable Users_FlatHolders_Select(string adminId)
    {
        dt = new DataTable("Users");
        da = new SqlDataAdapter("SELECT UserName, UserAdminId, UserFlatNumber FROM Users WHERE UserAdminId='" + adminId + "'", con);
        da.Fill(dt);
        return dt;
    }

    [WebMethod]
    public DataTable Users_Services_Select(string adminId)
    {
        dt = new DataTable("Services");
        da = new SqlDataAdapter("SELECT SPName, SPContactNumber, ServiceTypes.ServiceTypeName FROM ServiceProviders INNER JOIN ServiceTypes ON ServiceTypes.ServiceTypeId = ServiceProviders.SPTypeId WHERE SPAdminId='" + adminId + "'", con);
        da.Fill(dt);
        return dt;
    }

    [WebMethod]
    public DataTable Users_Profile_Select(string userId)
    {
        dt = new DataTable("Users");
        da = new SqlDataAdapter("SELECT UserName, UserEmailid, UserMobile, UserGender, UserProfilePicture FROM Users WHERE UserId='" + userId + "'", con);
        da.Fill(dt);
        return dt;
    }

    [WebMethod]
    public void Users_Profile_Update(string userId, string name, string emailId, string mobile, string gender, string profilePicture)
    {

    }

    // Guest Users activity's Services
    [WebMethod]
    public DataTable GuestUser_FlatHolder_Select(string adminId)
    {
        dt = new DataTable("Users");
        da = new SqlDataAdapter("SELECT UserName, UserProfilePicture, UserFlatNumber FROM Users WHERE UserAdminId='" + adminId + "'", con);
        da.Fill(dt);
        return dt;
    }

    [WebMethod]
    public DataTable GuestUser_AdminDetails_Select(string apartmentAdminId)
    {
        dt = new DataTable("Users");
        da = new SqlDataAdapter("SELECT AdminName, Admin FROM Users WHERE UserAdminId='" + apartmentAdminId + "'", con);
        da.Fill(dt);
        return dt;
    }

    /*[WebMethod]
    public DataTable GuseUser_Society_Details(string adminId)
    {
        dt = new DataTable("SocietyDetails");
        da = new SqlDataAdapter("SELECT ");
        return dt;
    }*/

    [WebMethod]
    public void GuestUser_Registration_Insert(string name, string emailid, string password, string mobile, string apartmentId)
    {
        name = HTMLEncoder(name);
        emailid = HTMLEncoder(emailid);
        password = HTMLEncoder(password);
        mobile = HTMLEncoder(mobile);
        apartmentId = HTMLEncoder(apartmentId);
        cmd = new SqlCommand("INSERT INTO GuestUser(GUName, GUEmailid, GUPassword, GUApartmentId, GUMobile) VALUES('" + name + "','" + emailid + "','" + password + "','" + apartmentId + "','" + mobile + "')", con);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
    }

    protected string HTMLEncoder(string str)
    {
        return HttpUtility.HtmlEncode(str);
    }

    protected string HTMLDecoder(string str)
    {
        return HttpUtility.HtmlDecode(str);
    }
}
