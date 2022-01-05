using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

public partial class Registration : System.Web.UI.Page
{
    string strCon = "Data Source={USERNAME};Initial Catalog={DBNAME};Integrated Security=True";
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    private bool checkAlreadyRegistered()
    {
        DataTable dt = new DataTable();
        new SqlDataAdapter("SELECT * FROM Admins WHERE AdminEmailid='" + txtEmail.Text.ToLower() + "'", new SqlConnection(strCon)).Fill(dt);
        if (dt.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void register()
    {
        string name = txtName.Text.ToLower();
        string email = txtEmail.Text.ToLower();
        string password = txtPassword.Text.ToLower();
        string mobile = txtMobileNumber.Text.ToLower();
        string gender = "";
        mobile = mobile.Replace(" ", string.Empty).Replace("_", string.Empty).Replace("-", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty);
        string flatNumber = txtFlatNumber.Text.ToLower();
        string wingName = txtWingName.Text.ToLower();
        string apartmentName = txtApartmentName.Text.ToLower();
        string pincode = txtPincode.Text.ToLower();
        string address = txtAddress.Value.ToLower();
        string city = txtCity.Text.ToLower();
        if (rdbMale.Checked == true)
        {
            gender = "male";
        }
        else if (rdbFemale.Checked == true)
        {
            gender = "female";
        }
        SqlConnection con = new SqlConnection(strCon);
        DataTable dt = new DataTable();
        con.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO Apartments(ApartmentName, ApartmentAddress, ApartmentWingName, ApartmentPincode, ApartmentCity) VALUES('" + apartmentName + "', '" + address + "', '" + wingName + "', '" + pincode + "', '" + city + "'); SELECT SCOPE_IDENTITY() AS ID", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        da.Fill(dt);
        string apartmentId = dt.Rows[0]["ID"].ToString();
        cmd = new SqlCommand("INSERT INTO Admins(AdminName, AdminEmailid, AdminPassword, AdminMobileNumber, AdminGender, AdminFlatNumber, AdminApartmentId) VALUES('" + name + "', '" + email + "', '" + password + "', '" + mobile + "', '" + gender + "', '" + flatNumber + "', '" + apartmentId + "'); SELECT SCOPE_IDENTITY() AS AID", con);
        cmd.ExecuteNonQuery();
        //SqlDataAdapter adminAD = new SqlDataAdapter(cmd);
        //adminAD.Fill(dt);
        //string adminId = dt.Rows[0]["AID"].ToString();
        //cmd = new SqlCommand("INSERT INTO Users(UserName, UserEmailid, UserPassword, UserMobile, UserGender, UserFlatNumber, UserAdminId, UserApartmentId) VALUES('" + name + "', '" + email + "', '" + password + "', '" + mobile + "', '" + gender + "', '" + flatNumber + "', '" + adminId + "', '" + apartmentId + "')", con);
        //cmd.ExecuteNonQuery();
    }

    private string GenerateOTP()
    {
        string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string numbers = "1234567890";

        string characters = numbers;

        characters += alphabets + numbers;
        string otp = string.Empty;
        for (int i = 0; i < 12; i++)
        {
            string character = string.Empty;
            do
            {
                int index = new Random().Next(0, characters.Length);
                character = characters.ToCharArray()[index].ToString();
            } while (otp.IndexOf(character) != -1);
            otp += character;
        }
        return otp;
    }

    protected void btnSignUp_Click(object sender, EventArgs e)
    {
        if (!checkAlreadyRegistered())
        {
            register();
            Response.Redirect("SignIn.aspx");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error", "alert('Already registred')", true);
        }
    }
}
