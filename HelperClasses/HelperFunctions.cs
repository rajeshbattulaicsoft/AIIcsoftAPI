//using AIIcsoftAPI.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.ComponentModel.DataAnnotations;
using AIIcsoftAPI.Dto;
using Microsoft.Data.SqlClient;

namespace AIIcsoftAPI.HelperClasses
{
    public class HelperFunctions
    {
       
        public HelperFunctions()
        {
            
        }

        public static Boolean IsNumeric(String s)
        {
            Boolean value = true;
            if (s == String.Empty || s == null)
            {
                value = false;
            }
            else
            {
                foreach (Char c in s.ToCharArray())
                {
                    value = value && (Char.IsDigit(c) || (c == '-'));
                }
            }
            return value;
        }

        // 06BZAHM6385P6Z2 L12345AB7684CDE123456
        public static bool IsValidGSTNo(string gststr, string panstr, int stateid)
        {
            string strRegex = string.Empty;
            string gstcode = string.Empty;
            string pancode = string.Empty;
            bool isvalid = false;

            strRegex = @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$";

            Regex rgx = new Regex(strRegex);

            if (rgx.IsMatch(gststr))
            {

                gstcode = gststr.Substring(0, 2);// it will return 0,1 positions char
                pancode = gststr.Substring(2, 10); //

                if (IsStateCodeInGST(stateid, gstcode) && String.Equals(panstr, pancode))
                {
                    isvalid = true;
                }

                return isvalid;
            }
            else
            {
                return isvalid;
            }

        }
        public static List<TreeNode> FillRecursive(List<AccountHeadsDto> flatObjects, int parentId)
        {
            List<TreeNode> tn = flatObjects.Where(x => x.ParentAccountId.Equals(parentId)).Select(item => new TreeNode
            {
                AccountName = item.AccountName,
                AccountId = item.AccountId,
                ParentAccountId = item.ParentAccountId,
                Treelevel = item.TreeLevel,
                OlevelId = item.OLevelId,
                Children = FillRecursive(flatObjects, item.AccountId)
            }).ToList();
            return tn;

        }

        public static string RemoveSpecialCharacters(string s)
        {
            return s.Replace(" ", "").Replace("&", "").Replace(",", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("/", "").Replace(" - ", "").Replace("*", "");
        }
        public static bool IsStateCodeInGST(int sid, string scode)
        {
            SqlConnection con;
            string conStr = string.Empty;
            DBConfig db = new DBConfig();
            conStr = db.GetMainConnectionString();

            con = new SqlConnection(conStr);
            con.Open();

            bool isexist = false;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_isstatecodeexist";
                //cid CompanyId
                cmd.Parameters.AddWithValue("sid", sid);
                cmd.Parameters.AddWithValue("scode", scode);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    isexist = true;
                }

                // isexist = Convert.ToBoolean(reader["total"]);

                return isexist;
            }
            catch (Exception ex)
            {
                return isexist;
            }
            finally { con.Close(); }
        }




        //BZAHM6385P
        public static bool IsValidPANNo(string panstr)
        {
            string strRegex = string.Empty;

            strRegex = @"^[A-Z]{5}[0-9]{4}[A-Z]{1}$";

            Regex rgx = new Regex(strRegex);

            if (rgx.IsMatch(panstr))
            {
                return true;
            }
            return false;

        }



        //L12345AB7684CDE123456
        public static bool IsValidCINNo(string cinstr)
        {
            string strRegex = string.Empty;

            strRegex = @"^([LUu]{1})([0-9]{5})([A-Za-z]{2})([0-9]{4})([A-Za-z]{3})([0-9]{6})$";

            Regex rgx = new Regex(strRegex);

            if (rgx.IsMatch(cinstr))
            {
                return true;
            }
            return false;

        }



        public static string SHA512(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString().ToLower();
            }
        }

        public static string Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            else
            {
                byte[] x = Encoding.UTF8.GetBytes(input);   //ASCIIEncoding.ASCII.GetBytes(input);
                string encryptedx = Convert.ToBase64String(x);
                return encryptedx;
            }
        }

        public static string Decrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            else
            {
                byte[] x = Convert.FromBase64String(input);
                string decryptedx = Encoding.UTF8.GetString(x);   //ASCIIEncoding.ASCII.GetBytes(input);
                return decryptedx;
            }
        }


        public static DateTime GetCurrentServerDateTime()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            var dt = new DateTime(year, month, day);
            return dt;
        }


        public static DateTime GetDateFromYYYYMMDD(string strdate)
        {
            int year = int.Parse(strdate.Substring(0, 4));
            int month = int.Parse(strdate.Substring(5, 2));
            int day = int.Parse(strdate.Substring(8, 2));
            var dt = new DateTime(year, month, day);
            return dt;
        }

        public static DateTime GetDateFromDDMMYYYY(string strdate)
        {
            int day = int.Parse(strdate.Substring(0, 2));
            int month = int.Parse(strdate.Substring(3, 2));
            int year = int.Parse(strdate.Substring(6, 4));
            var dt = new DateTime(year, month, day);
            return dt;
        }

        public static DateOnly GetDateOnlyFromYYYYMMDD(string strdate)
        {
            int year = int.Parse(strdate.Substring(0, 4));
            int month = int.Parse(strdate.Substring(5, 2));
            int day = int.Parse(strdate.Substring(8, 2));
            var dt = new DateOnly(year, month, day);
            return dt;
        }

       

        //public static string GetLoggedInUserName()
        //{
        //    var indentity = HttpContext.User.Identity as ClaimsIdentity;
        //    var empid = string.Empty;
        //    if (indentity != null)
        //    {
        //        var userClaims = indentity.Claims;
        //        empid = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //        HttpContext.Session.SetInt32("empid", empid);
        //    }
        //}

        public static bool IsVaildEmail(string emailId)
        {
            bool isValidEmail = Regex.IsMatch(emailId, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

            if (!isValidEmail)
            {
                //errorcount++;
                //errors += " Enter valid email id";
                return false;
            }

            if (!(new EmailAddressAttribute().IsValid(Convert.ToString(emailId))))
            {
                //errorcount++;
                //errors += " Enter valid email id";
                return false;
            }
            return true;
        }
    }
}
