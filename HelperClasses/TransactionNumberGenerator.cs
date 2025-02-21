using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using AIIcsoftAPI.Enums;
using AIIcsoftAPI.HelperClasses;
using System.Data;

namespace AIIcsoftAPI.HelperClasses
{
    public class TransactionNumberGenerator
    {
        private readonly IConfiguration _configuration;
        // global variables
        bool gblnProfitCentrePrefixReqd = false;
        string gstrDbType = string.Empty;
        string gstrModuleName = string.Empty;
        int glngLocId = 0;


        public TransactionNumberGenerator(string loginlocation)
        {
            glngLocId = Convert.ToInt32(loginlocation);
        }


        private DataTable GetPrefixGlobalSettingDetail(long LngFormId, ref string StrPrefix, ref string StrPrefixStartNo, ref string StrTranPrefixTable, ref string StrTransPrefixColumn, ref string StrOrderByCol, ref string strTransDB, DateOnly TransDate, ref string StrPrefixSeperator, SqlTransaction SqlBiz = null)
        {
            SqlConnection con;
            string conStr = string.Empty;
            DBConfig db = new DBConfig();
            conStr = db.GetMainConnectionString();
            DataTable Dt = new DataTable();
            using (con = new SqlConnection(conStr))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "prefixglobalsettingsub_getprefixglobalsettingdetail";    // kiicsoftmain
                    cmd.Parameters.AddWithValue("pglngLocId", glngLocId);
                    cmd.Parameters.AddWithValue("pTransDate", TransDate);
                    cmd.Parameters.AddWithValue("pLngFormId", LngFormId);
                    //cmd.Transaction = SqlBiz;

                    SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                    Adp.SelectCommand.Transaction = SqlBiz;
                    Adp.Fill(Dt);


                    if (Dt.Rows.Count > 0)
                    {
                        StrPrefix = Dt.Rows[0]["Prefix"].ToString();
                        StrPrefixStartNo = Dt.Rows[0]["PrefixStartNo"].ToString();
                        StrTranPrefixTable = Dt.Rows[0]["TranPrefixTable"].ToString();
                        StrTransPrefixColumn = Dt.Rows[0]["TranPrefixColumn"].ToString();
                        StrOrderByCol = Dt.Rows[0]["OrderByCol"].ToString();
                        strTransDB = Dt.Rows[0]["TransDB"].ToString();
                        StrPrefixSeperator = Dt.Rows[0]["PrefixSeperator"].ToString().Trim();
                    }
                    else
                    {

                        SqlCommand cmd2 = con.CreateCommand();
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.CommandText = "prefixglobalsettingsub_getprefixglobalsettingdetail2";  //kiicsoftmain
                        cmd2.Parameters.AddWithValue("pTransDate", TransDate);
                        cmd2.Parameters.AddWithValue("pLngFormId", LngFormId);


                        SqlDataAdapter Adp2 = new SqlDataAdapter(cmd2);
                        //DataTable Dt2;
                        //Adp2.SelectCommand.Transaction = SqlBiz;
                        Dt = new DataTable();
                        Adp2.Fill(Dt);

                        if (Dt.Rows.Count > 0)
                        {
                            StrPrefix = Dt.Rows[0]["Prefix"].ToString();
                            StrPrefixStartNo = Dt.Rows[0]["PrefixStartNo"].ToString();
                            StrTranPrefixTable = Dt.Rows[0]["TranPrefixTable"].ToString();
                            StrTransPrefixColumn = Dt.Rows[0]["TranPrefixColumn"].ToString();
                            StrOrderByCol = Dt.Rows[0]["OrderByCol"].ToString();
                            strTransDB = Dt.Rows[0]["TransDB"].ToString();
                            StrPrefixSeperator = Dt.Rows[0]["PrefixSeperator"].ToString().Trim();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    con.Close();
                }

                return Dt;
            }

        }


        private string GetProfitCentrePrefix(long LngPCId, SqlTransaction SqlBiz = null)
        {
            string conStr = string.Empty;
            DBConfig db = new DBConfig();
            conStr = db.GetMainConnectionString();
            string costcentre = string.Empty;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.Transaction = SqlBiz;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "acc_costcentre_getprofitcentreprefix";
                    cmd.Parameters.AddWithValue("pLngPCId", LngPCId);
                    costcentre = cmd.ExecuteScalarAsync().Result.ToString();
                }
                catch (Exception ex)
                {

                }
            }

            return costcentre;
        }


        private void GetTransPrefixAll(DataTable dtDet, string StrPrefix, string StrMonthDisplay, string StrYearDisplay, ref string StrPrefixAll, string StrPrefixSeperator, string StrProfitCentrePrefix, DateOnly TransDate, SqlTransaction SqlTrans = null, SqlTransaction SqlBiz = null)
        {
            string StrPrefixCombination = string.Empty;
            string[] StrPrefixCombSplit;

            GetPrefixCombination(dtDet, ref StrPrefixCombination, ref StrMonthDisplay, ref StrYearDisplay, TransDate, SqlBiz, SqlTrans);

            //StrPrefixCombSplit = Split(StrPrefixCombination, ",")
            StrPrefixCombSplit = StrPrefixCombination.Split(",");
            for (int IntI = 0; IntI < StrPrefixCombSplit.Length; IntI++)
            {
                if (StrPrefixCombSplit[IntI] == "Location")
                {
                    if (StrPrefixAll != "")
                        StrPrefixAll = StrPrefixAll + StrPrefixSeperator + GetLocationPrefix(SqlTrans);
                    else
                        StrPrefixAll = GetLocationPrefix(SqlTrans);
                }
                else if (StrPrefixCombSplit[IntI] == "Month")
                {
                    if (StrPrefixAll != "")
                        StrPrefixAll = StrPrefixAll + StrPrefixSeperator + StrMonthDisplay;
                    else
                        StrPrefixAll = StrMonthDisplay;
                }
                else if (StrPrefixCombSplit[IntI] == "Year")
                {
                    if (StrPrefixAll != "")
                        StrPrefixAll = StrPrefixAll + StrPrefixSeperator + StrYearDisplay;
                    else
                        StrPrefixAll = StrYearDisplay;
                }
                else if (StrPrefixCombSplit[IntI] == "Prefix")
                {
                    if (StrPrefixAll != "")
                        StrPrefixAll = StrPrefixAll + StrPrefixSeperator + StrPrefix;
                    else
                        StrPrefixAll = StrPrefix; //'Value is comming from the function
                }

                else if (StrPrefixCombSplit[IntI] == "Number")
                {
                    if (StrPrefixAll != "")
                        StrPrefixAll = StrPrefixAll + "::NUMBER::";
                    else
                        StrPrefixAll = "::NUMBER::";
                }

                else if (StrPrefixCombSplit[IntI] == "ProfitCentre")
                {
                    if (StrProfitCentrePrefix != "")
                    {
                        if (StrPrefixAll != "")
                            StrPrefixAll = StrPrefixAll + StrPrefixSeperator + StrProfitCentrePrefix;
                        else
                            StrPrefixAll = StrProfitCentrePrefix;
                    }
                }
            }
        }

        private void GetPrefixCombination(DataTable Dt, ref string StrPrefixCombination, ref string StrMonthDisplay, ref string StrYearDisplay, DateOnly TransDate, SqlTransaction SqlBiz = null, SqlTransaction SqlTrans = null)
        {
            if (Dt.Rows.Count > 0)
            {
                string gstrModuleName = "DealerICSoft";
                if (gstrModuleName == "DealerICSoft")
                {
                    string conStr = string.Empty;
                    DBConfig db = new DBConfig();
                    conStr = db.GetMainConnectionString();
                    string yearcode = string.Empty;

                    //string vdate = DateTime.ParseExact(TransDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    string vdate = TransDate.ToString("yyyy-MM-dd");

                    SqlConnection con = new SqlConnection(conStr);
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.Transaction = SqlBiz;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbdetail_getyearcode";
                    cmd.Parameters.AddWithValue("vdate", vdate);
                    yearcode = cmd.ExecuteScalarAsync().Result.ToString();
                    con.Close();
                    //if (yearcode == "" || yearcode == null || yearcode == string.Empty)
                    //{
                    //    //MsgBox("Finanacial Year not created for the selected date. Please create and continue.", MsgBoxStyle.Exclamation)
                    //    return "Finanacial Year not created for the selected date. Please create and continue.";
                    //    //Exit Sub
                    //}
                    StrYearDisplay = yearcode;
                }
                else
                {
                    //StrMonthDisplay = Dt.Rows[0]["MonthDisplay"].ToString();
                    //StrYearDisplay = Dt.Rows[0]["YearDisplay"].ToString();

                    DateTime dt = DateTime.Now;
                    StrMonthDisplay = dt.Month.ToString();
                    if (StrMonthDisplay.Length == 1)
                    {
                        StrMonthDisplay = "0" + StrMonthDisplay;
                    }
                    StrYearDisplay = dt.Year.ToString();
                }
                if (StrPrefixCombination != "")
                    StrPrefixCombination = StrPrefixCombination + "," + Dt.Rows[0]["LocationDispPriority"];
                else
                    StrPrefixCombination = Dt.Rows[0]["LocationDispPriority"].ToString();

                if (StrPrefixCombination != "")
                    StrPrefixCombination = StrPrefixCombination + "," + Dt.Rows[0]["MonthDispPriority"];
                else StrPrefixCombination = Dt.Rows[0]["MonthDispPriority"].ToString();

                if (StrPrefixCombination != "")
                    StrPrefixCombination = StrPrefixCombination + "," + Dt.Rows[0]["YearDispPriority"];
                else StrPrefixCombination = Dt.Rows[0]["YearDispPriority"].ToString();

                if (StrPrefixCombination != "")
                    StrPrefixCombination = StrPrefixCombination + "," + Dt.Rows[0]["TransPrefixDispPriority"];
                else StrPrefixCombination = Dt.Rows[0]["TransPrefixDispPriority"].ToString();

                if (StrPrefixCombination != "")
                    StrPrefixCombination = StrPrefixCombination + "," + Dt.Rows[0]["NumberPriority"];
                else StrPrefixCombination = Dt.Rows[0]["NumberPriority"].ToString();

                if (StrPrefixCombination != "")
                    StrPrefixCombination = StrPrefixCombination + "," + Dt.Rows[0]["ProfitCentreDispPriority"];
                else StrPrefixCombination = Dt.Rows[0]["ProfitCentreDispPriority"].ToString();

            }

            string[] StrSplit;
            string StrT = "";

            //SqlCommand CmdCreate = new SqlCommand();
            DataTable dtTempDisplayOrder = new DataTable();
            DataRow drTempDisplayOrder;
            DataView dvTempDisplayOrder;

            dtTempDisplayOrder.Columns.Add("DisplayName", typeof(String));
            dtTempDisplayOrder.Columns.Add("DisplayOrder", typeof(Int32));

            string TempString = "";
            if (StrPrefixCombination != "")
            {
                StrSplit = StrPrefixCombination.Split(",");
                for (int IntI = 0; IntI < StrSplit.Length; IntI++)
                {
                    if (IntI == 0)
                        TempString = "Location";
                    else if (IntI == 1)
                        TempString = "Month";
                    else if (IntI == 2)
                        TempString = "Year";
                    else if (IntI == 3)
                        TempString = "Prefix";
                    else if (IntI == 4)
                        TempString = "Number";
                    else if (IntI == 5)
                        TempString = "ProfitCentre";

                    drTempDisplayOrder = dtTempDisplayOrder.NewRow();
                    drTempDisplayOrder["DisplayName"] = TempString;
                    drTempDisplayOrder["DisplayOrder"] = 1;//StrSplit[IntI];
                    dtTempDisplayOrder.Rows.Add(drTempDisplayOrder);
                }
            }

            dvTempDisplayOrder = dtTempDisplayOrder.DefaultView;
            //dvTempDisplayOrder.RowFilter = "DisplayOrder != 0";
            dvTempDisplayOrder.RowFilter = "DisplayOrder > 0";
            dvTempDisplayOrder.Sort = "DisplayOrder";

            StrPrefixCombination = "";

            foreach (DataRowView Dr in dvTempDisplayOrder)
            {
                if (StrPrefixCombination != "")
                    StrPrefixCombination = StrPrefixCombination + "," + Dr["DisplayName"];
                else StrPrefixCombination = Dr["DisplayName"].ToString();
            }

            dtTempDisplayOrder.Dispose();
            dvTempDisplayOrder.Dispose();
        }



        private string GetLatestSlNo(string StrPrefix, string StrPrefixTable, string StrPrefixColumn, string StrPrefixStartNo, string StrOrderByCol, string strTransDB, string StrPrefixSeperator, SqlTransaction SqlTrans = null, SqlTransaction SqlTransMain = null)
        {
            string result = string.Empty;
            int IntValLen;

            string strSQL1 = string.Empty;
            string strSQL2 = string.Empty;
            long strSQLVal = 0;
            string StrSqlValue = string.Empty;

            SqlCommand CmdSql = null;
            SqlCommand CmdSql2 = null;


            string gstrDbType = "";
            int strLen = StrPrefix.Length;
            if (StrPrefix.Substring(0, 1) != StrPrefixSeperator)
            {
                //if (gstrDbType == "mssql")   ////////////////////// gstrDbType not defined
                //{
                //    strSQL1 = "select max(len(" + StrPrefixColumn + "))as MaxLen from " + StrPrefixTable + " with (NOLOCK) Where Left(LTrim(RTrim(" + StrPrefixColumn + "))," + strLen + ")='" + StrPrefix + "' and Isnumeric(Right(" + StrPrefixColumn + ",len(" + StrPrefixColumn + ")-" + strLen + "))=1 ";
                //}
                //else if (gstrDbType == "db2")
                //{
                //    strSQL1 = "select max(len(" + StrPrefixColumn + "))as MaxLen from " + StrPrefixTable + " with (NOLOCK) Where Left(LTrim(RTrim(" + StrPrefixColumn + "))," +
                //strLen + ")='" + StrPrefix + "' and Isnumeric(Right(" + StrPrefixColumn + ",len(" + StrPrefixColumn + ")-" + strLen + "))=1 ";
                //}


                //mssql query taken
                strSQL1 = "select max(len(" + StrPrefixColumn + "))as MaxLen from " + StrPrefixTable + " Where Left(LTrim(RTrim(" + StrPrefixColumn + "))," + strLen + ")='" + StrPrefix + "' and Isnumeric(Right(" + StrPrefixColumn + ",len(" + StrPrefixColumn + ")-" + strLen + "))=1 ";

                //strSQL1 = "select max(len(" + StrPrefixColumn + "))as MaxLen from " + StrPrefixTable + " Where Left(LTrim(RTrim(" + StrPrefixColumn + "))," + strLen + ")='" + StrPrefix + "' and ((Right(" + StrPrefixColumn + ",len(" + StrPrefixColumn + ")-" + strLen + "))  REGEXP '^[0-9]+\\.?[0-9]*$')  = 1 ";


                //////////debugged query for above
                //////////select max(len(TransactionNumber))as MaxLen
                //////////from dbo.Transactions
                //////////Where Left(LTrim(RTrim(TransactionNumber)), 18) = 'KI-CHA-JE-02-2023-'
                //////////AND((Right(TransactionNumber, len(TransactionNumber) - 18)) REGEXP '^[0-9]+\\.?[0-9]*$') = 1
            }
            else
            {
                //if (gstrDbType == "mssql")
                //{
                //    strSQL1 = "Select max(len(" + StrPrefixColumn + "))as MaxLen from " + StrPrefixTable + " with (NOLOCK) Where Right(LTrim(RTrim(" + StrPrefixColumn + "))," + strLen + ")='" + StrPrefix.Trim() + "'";
                //}
                //else
                //{
                //    strSQL1 = "Select max(len(" + StrPrefixColumn + "))as MaxLen from " + StrPrefixTable + " with (NOLOCK) Where Right(LTrim(RTrim(" + StrPrefixColumn + "))," + strLen + ")='" + StrPrefix.Trim() + "'";
                //}

                ////mssql query taken
                strSQL1 = "Select max(len(" + StrPrefixColumn + "))as MaxLen from " + StrPrefixTable + " Where Right(LTrim(RTrim(" + StrPrefixColumn + "))," + strLen + ")='" + StrPrefix.Trim() + "'";
            }

            //if (strTransDB.ToLower() == "Main".ToLower())
            //    CmdSql = new SqlCommand(strSQL1, ConMain, SqlTransMain);
            //else if (strTransDB.ToLower() == "Ledger".ToLower())
            //    CmdSql = new SqlCommand(strSQL1, conAcc, SqlTrans);

            string conStr = string.Empty;
            DBConfig db = new DBConfig();
          if (strTransDB == "Ledger") 
{
    conStr = db.GetConnectionString();
}
else
{
    conStr = db.GetMainConnectionString();
}
            
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            CmdSql = new SqlCommand(strSQL1, con, SqlTransMain);

            CmdSql.CommandTimeout = 300;

            if (Convert.IsDBNull(CmdSql.ExecuteScalar()) == false)
            {
                StrSqlValue = CmdSql.ExecuteScalar().ToString();
            }
            con.Close();


            if (StrSqlValue != "")
            {
                IntValLen = Convert.ToInt32(StrSqlValue);
                if (StrPrefix.Substring(0, 1) != StrPrefixSeperator)
                {
                    //if (gstrDbType == "mssql")
                    //{
                    //    strSQL2 = "Select max(right(" + StrPrefixColumn + "," + (IntValLen - strLen).ToString() + ")) As MaxRefNo from " + StrPrefixTable + " with (NOLOCK) Where len(" + StrPrefixColumn + ")= " + IntValLen + " and Left(LTrim(RTrim(" + StrPrefixColumn + "))," + strLen + ")='" + StrPrefix + "' and Isnumeric(Right(" + StrPrefixColumn + ",len(" + StrPrefixColumn + ")-" + strLen + "))=1";
                    //}
                    //else if (gstrDbType == "db2")
                    //{
                    //    strSQL2 = "Select max(right(" + StrPrefixColumn + "," + (IntValLen - strLen).ToString() + ")) As MaxRefNo from " + StrPrefixTable + " with (NOLOCK) Where len(" + StrPrefixColumn + ")= " + IntValLen + " and Left(LTrim(RTrim(" + StrPrefixColumn + "))," + strLen + ")='" + StrPrefix + "' and Isnumeric(Right(" + StrPrefixColumn + ",len(" + StrPrefixColumn + ")-" + strLen + "))=1 ";
                    //}

                    //mssql
                    strSQL2 = "Select max(right(" + StrPrefixColumn + "," + (IntValLen - strLen).ToString() + ")) As MaxRefNo from " + StrPrefixTable + " Where len(" + StrPrefixColumn + ")= " + IntValLen + " and Left(LTrim(RTrim(" + StrPrefixColumn + "))," + strLen + ")='" + StrPrefix + "' and Isnumeric(Right(" + StrPrefixColumn + ",len(" + StrPrefixColumn + ")-" + strLen + "))=1";

                    // strSQL2 = "Select max(right(" + StrPrefixColumn + "," + (IntValLen - strLen).ToString() + ")) As MaxRefNo from " + StrPrefixTable + " Where len(" + StrPrefixColumn + ")= " + IntValLen + " and Left(LTrim(RTrim(" + StrPrefixColumn + "))," + strLen + ")='" + StrPrefix + "' and ((Right(" + StrPrefixColumn + ",len(" + StrPrefixColumn + ")-" + strLen + ")) REGEXP '^[0-9]+\\.?[0-9]*$') = 1";


                    ////////Select max(right(TransactionNumber,-18)) As MaxRefNo
                    ////////from Transactions Where len(TransactionNumber) = 0
                    ////////and Left(LTrim(RTrim(TransactionNumber)),18)= 'KI-CHA-JE-07-2023-'
                    ////////AND((Right(TransactionNumber, len(TransactionNumber) - 18)) REGEXP '^[0-9]+\\.?[0-9]*$') = 1
                }
                else
                {
                    //if (gstrDbType == "mssql")
                    //{
                    //    strSQL2 = "Select Max(Left(LTrim(RTrim(" + StrPrefixColumn + "))," + IntValLen + ")) as mx from " + StrPrefixTable + " with (NOLOCK) Where Right(LTrim(RTrim(" + StrPrefixColumn + " ))," + strLen + ")='" + StrPrefix.Trim() + "'" + " AND LEN(LTrim(RTrim(" + StrPrefixColumn + " )))>=" + (strLen + 1) + " Group by " + StrOrderByCol + " Order By " + StrOrderByCol + "  DESC";
                    //}

                    //else if (gstrDbType == "db2")
                    //{
                    //    strSQL2 = "Select Max(Left(LTrim(RTrim(" + StrPrefixColumn + "))," + IntValLen + ")) as mx from " + StrPrefixTable + " with (NOLOCK) Where Right(LTrim(RTrim(" + StrPrefixColumn + " ))," + strLen + ")='" + StrPrefix.Trim() + "'" + " AND len(LTrim(RTrim(" + StrPrefixColumn + " )))>=" + (strLen + 1) + " Group by " + StrOrderByCol + " Order By " + StrOrderByCol + "  DESC";
                    //}

                    //mssql
                    strSQL2 = "Select Max(Left(LTrim(RTrim(" + StrPrefixColumn + "))," + IntValLen + ")) as mx from " + StrPrefixTable + " Where Right(LTrim(RTrim(" + StrPrefixColumn + " ))," + strLen + ")='" + StrPrefix.Trim() + "'" + " AND len(LTrim(RTrim(" + StrPrefixColumn + " )))>=" + (strLen + 1) + " Group by " + StrOrderByCol + " Order By " + StrOrderByCol + "  DESC";

                }

                //if (strTransDB.ToLower() == "Main".ToLower())
                //    CmdSql2 = new SqlCommand(strSQL2, ConMain, SqlTransMain);
                //else if (strTransDB.ToLower() == "Ledger".ToLower())
                //    CmdSql2 = new SqlCommand(strSQL2, conAcc, SqlTrans);


               if (strTransDB == "Ledger") 
{
    conStr = db.GetConnectionString();
}
else
{
    conStr = db.GetMainConnectionString();
}
                con = new SqlConnection(conStr);
                con.Open();
                CmdSql = new SqlCommand(strSQL2, con, SqlTransMain);
                CmdSql.CommandTimeout = 300;

                //strSQLVal = Val(CmdSql2.ExecuteScalar);
                strSQLVal = Convert.ToInt32(CmdSql.ExecuteScalar());
                con.Close();

                if (Convert.IsDBNull(strSQLVal) == true)
                {
                    //if (gstrDbType == "mssql")
                    //{
                    //    strSQL2 = "Select Max(Right(LTrim(RTrim(" + StrPrefixColumn + "))," + IntValLen + ")) as mx from " + StrPrefixTable + " with (NOLOCK) Where Left(LTrim(RTrim(" + StrPrefixColumn + " ))," + strLen + ")='" + StrPrefix.Trim() + "'" + " AND LEN(LTrim(RTrim(" + StrPrefixColumn + " )))>" + strLen + " Group by " + StrOrderByCol + " Order By " + StrOrderByCol + "  DESC";
                    //}

                    //else if (gstrDbType == "db2")
                    //{
                    //    strSQL2 = "Select Max(Right(LTrim(RTrim(" + StrPrefixColumn + "))," + IntValLen + ")) as mx from " + StrPrefixTable + " with (NOLOCK) Where Left(LTrim(RTrim(" + StrPrefixColumn + " ))," + strLen + ")='" + StrPrefix.Trim() + "'" + " AND len(LTrim(RTrim(" + StrPrefixColumn + " )))>" + strLen + " Group by " + StrOrderByCol + " Order By " + StrOrderByCol + "  DESC";
                    //}

                    //mssql
                    strSQL2 = "Select Max(Right(LTrim(RTrim(" + StrPrefixColumn + "))," + IntValLen + ")) as mx from " + StrPrefixTable + " Where Left(LTrim(RTrim(" + StrPrefixColumn + " ))," + strLen + ")='" + StrPrefix.Trim() + "'" + " AND len(LTrim(RTrim(" + StrPrefixColumn + " )))>" + strLen + " Group by " + StrOrderByCol + " Order By " + StrOrderByCol + "  DESC";


                    strSQLVal = 0;
                    //if (strTransDB.ToLower() == "Main".ToLower())
                    //    CmdSql2 = new SqlCommand(strSQL2, ConMain, SqlTransMain);
                    //else if (strTransDB.ToLower() == "Ledger".ToLower())
                    //    CmdSql2 = new SqlCommand(strSQL2, conAcc, SqlTrans);

                    conStr = db.GetConnectionString();
                    con = new SqlConnection(conStr);
                    con.Open();
                    CmdSql2 = new SqlCommand(strSQL2, con, SqlTransMain);

                    CmdSql2.CommandTimeout = 300;

                    strSQLVal = Convert.ToInt32(CmdSql2.ExecuteScalar());
                    con.Close();
                    return strSQLVal.ToString();
                }
                else
                {
                    long vGetLatestSlNo = strSQLVal + 1;

                    //''Added by saranya for kiswok(Prefix issue)
                    //if (Convert.ToDouble(vGetLatestSlNo) < Convert.ToDouble(StrPrefixStartNo))  //' Added By Rishabh For SCIT
                    //{
                    //    vGetLatestSlNo = Convert.ToInt64(StrPrefixStartNo);
                    //}
                    return vGetLatestSlNo.ToString();
                }
            }
            else
            {
                return StrPrefixStartNo;
            }
        }

        private string? GetDataBaseNames(DataBaseType dataBaseType)
        {
            var databaseNames = _configuration.GetSection("DatabaseNames");

            string dbname = dataBaseType switch
            {
                DataBaseType.ICSOFT => databaseNames["IcSoft"],
                DataBaseType.BIZSOFT => databaseNames["BizSoft"],
                DataBaseType.REPORT => databaseNames["Report"],
                DataBaseType.LEDZER => databaseNames["Ledger"],
                _ => string.Empty
            };
            return dbname;
        }

        private string GetLocationPrefix(SqlTransaction SqlTrans = null)
        {
            // to get the location id form the login screen
            // profitcenter prefix to fetch while entering data, JV and month
            //Dim Adp As New SqlDataAdapter, Dt As New DataTable
            //GetLocationPrefix = ""
            //Adp = New SqlDataAdapter("Select CompShort from Company with (NOLOCK) Where CompanyId = " & glngLocId, ConMain)
            //Adp.SelectCommand.Transaction = SqlTrans
            //Adp.Fill(Dt)
            //If Dt.Rows.Count > 0 Then
            //    If Trim(Dt.Rows(0)("CompShort")) <> "" Then GetLocationPrefix = Dt.Rows(0)("CompShort")
            //End If

            //company_GetCompShortByCompaniId
            string conStr = string.Empty;
            DBConfig db = new DBConfig();
            conStr = db.GetMainConnectionString();
            string compshort = string.Empty;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.Transaction = SqlTrans;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "company_getcompshortbycompaniId";
                    cmd.Parameters.AddWithValue("glngLocId", glngLocId);
                    compshort = cmd.ExecuteScalarAsync().Result.ToString();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally { con.Close(); }
            }

            return compshort;
        }

        private string GetDisplayOrder(string StrTableName)
        {
            //master_GetDisplayFields
            string[] StrSplit;
            string vGetDisplayOrder = string.Empty;

            string conStr = string.Empty;
            DBConfig db = new DBConfig();
            conStr = db.GetBizsoftConnectionString();
            string displayFields = string.Empty;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "master_getdisplayfields";
                    cmd.Parameters.AddWithValue("pStrTableName", StrTableName);
                    displayFields = cmd.ExecuteScalarAsync().Result.ToString();

                    if (displayFields != "" && displayFields != null)
                    {
                        //StrSplit = Split(Dt.Rows(0)("DisplayFields"), ",")
                        StrSplit = displayFields.Split(",");

                        for (int IntI = 0; IntI < StrSplit.Length; IntI++)
                        {
                            if (vGetDisplayOrder != "")
                                vGetDisplayOrder = vGetDisplayOrder + " | " + StrSplit[IntI];
                            else
                                vGetDisplayOrder = StrSplit[IntI];
                        }
                    }


                    if (vGetDisplayOrder.Trim() != "" && vGetDisplayOrder != null)
                        vGetDisplayOrder = vGetDisplayOrder + " As DispName";

                    return vGetDisplayOrder;

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

        }


        public string GetMaxIds(string modulename, int companyid)
        {
            SqlConnection con;
            string conStr = string.Empty;
            DBConfig db = new DBConfig();
            string qry;

            conStr = db.GetMainConnectionString();
            con = new SqlConnection(conStr);
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (companyid == 0)
            {
                qry = "prefixglobalsettingsub_getmaxids";
                cmd.CommandText = qry;
                cmd.Parameters.AddWithValue("pmodulename", modulename);

            }
            else
            {
                qry = "prefixglobalsettingsub_getmaxids2";
                cmd.CommandText = qry;
                cmd.Parameters.AddWithValue("pmodulename", modulename);
                cmd.Parameters.AddWithValue("pcompanyid", companyid);
            }

            SqlDataAdapter Adp = new SqlDataAdapter(cmd);
            DataTable Dt = new DataTable();
            Adp.Fill(Dt);


            string vGetMaxIds = "0";

            foreach (DataRow Dr in Dt.Rows)
            {
                if (Dr["Id"] != null && Dr["Id"].ToString().Length > 0)
                {
                    vGetMaxIds = vGetMaxIds + ", " + Dr["Id"].ToString().Trim();
                }
            }

            con.Close();
            return vGetMaxIds;
        }



        //Public Function fnGetFormId(ByVal strFormName As String, ByVal strAddnlParameter As String, Optional ByVal strModuleName As String = "") As Long
        public int fnGetFormIdmaria(string strFormName, string strAddnlParameter, string strModuleName = "")
        {
            string qry;

            if (strModuleName.Trim() == "" || strModuleName == null)
            {
                strModuleName = gstrModuleName;
            }
            qry = "Select masterid from dbo.master where modulename='" + strModuleName + "' and mastername='" + strFormName + "' and addnlparameter='" + strAddnlParameter + "' order by masterid LIMIT 0, 1";


            DBConfig db = new DBConfig();
            string constr = db.GetBizsoftConnectionString();
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = qry;

            string masterid = cmd.ExecuteScalar().ToString();
            con.Close();

            if (masterid == "" || masterid == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(masterid);
            }
        }

        public int fnGetFormId(string strFormName, string strAddnlParameter = "", string strModuleName = "")
        {
            string qry;

            if (strModuleName.Trim() == "" || strModuleName == null)
            {
                strModuleName = gstrModuleName;
            }

            qry = "Select top 1  masterid from dbo.master where mastername='" + strFormName + "' and modulename='" + strModuleName + "' and addnlparameter='" + strAddnlParameter + "' order by masterid";
            //qry = "Select Masterid from dbo.transactiontype where transtypeno=" + trtypeno + " Order by Masterid fetch first 1 row only";

            DBConfig db = new DBConfig();
            string constr = db.GetBizsoftConnectionString();
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = qry;

            if (cmd.ExecuteScalar() == null)
            {
                con.Close();
                return 0;
            }
            string masterid = cmd.ExecuteScalar().ToString();
            con.Close();
            return Convert.ToInt32(masterid);
        }


public int fnGetMasterId(int strFormName)
        {
            string qry;

            qry = "Select top 1  masterid from dbo.TransactionType where TransTypeNo='" + strFormName + "' ";
            //qry = "Select Masterid from dbo.transactiontype where transtypeno=" + trtypeno + " Order by Masterid fetch first 1 row only";

            DBConfig db = new DBConfig();
            string constr = db.GetConnectionString();
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = qry;

            if (cmd.ExecuteScalar() == null)
            {
                con.Close();
                return 0;
            }
            string masterid = cmd.ExecuteScalar().ToString();
            con.Close();
            return Convert.ToInt32(masterid);
        }
        public int fnGetFormIdFinance(int trtypeno, string strAddnlParameter = "", string strModuleName = "")
        {
            string qry;

            if (strModuleName.Trim() == "" || strModuleName == null)
            {
                strModuleName = gstrModuleName;
            }

            qry = "Select top 1  masterid from dbo.transactiontype where transtypeno=" + trtypeno + " order by masterid";
            //qry = "Select Masterid from dbo.transactiontype where transtypeno=" + trtypeno + " Order by Masterid fetch first 1 row only";

            DBConfig db = new DBConfig();
            string constr = db.GetConnectionString();
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = qry;

            if (cmd.ExecuteScalar() == null)
            {
                con.Close();
                return 0;
            }
            string masterid = cmd.ExecuteScalar().ToString();
            con.Close();
            return Convert.ToInt32(masterid);
        }



        //1st call GetPrefixAndSlNo() for transaction number generation after calling fnGetFormId()
        //LngFromID = masterId (get masterid based on trtypeno from transactiontype table)
        public string GetPrefixAndSlNo(long LngFormId, DateOnly DtTransDate, string StrSubPrefix = "", SqlTransaction SqlTrans = null, SqlTransaction SqlTransMain = null, SqlTransaction SqlBiz = null, long LngProfitCentreId = 0)
        {
            //' Trans Prefix return Value
            string StrTransPrefixAll = "";

            //     LngFormId = 558;

            //'For Each Transaction Type
            string StrTransPrefix = "";
            string StrStartNo, StrTranPrefixTable, StrTransPrefixColumn, StrOrderByCol, strTransDB;
            string StrPrefixTempString = "";
            string StrMonthDisplay = "";
            string StrYearDisplay = "";
            string StrPrefixSeperator = "|";
            string StrProfitCentrePrefix = "";
            string[] StrSplit;
            DataTable dtDet;

            if (gblnProfitCentrePrefixReqd == false)
            {
                LngProfitCentreId = 0;
            }

            //StrStartNo = "" : StrTranPrefixTable = "" : StrTransPrefixColumn = "" : StrOrderByCol = "" : strTransDB = ""

            StrStartNo = "";
            StrTranPrefixTable = "";
            StrTransPrefixColumn = "";
            StrOrderByCol = "";
            strTransDB = "";

            dtDet = GetPrefixGlobalSettingDetail(LngFormId, ref StrTransPrefix, ref StrStartNo, ref StrTranPrefixTable, ref StrTransPrefixColumn, ref StrOrderByCol, ref strTransDB, DtTransDate, ref StrPrefixSeperator, SqlBiz);

            if (StrTranPrefixTable == "")
            {
                return "Prefix is not defined in PrefixGlobalSetting table. Please set it and then continue...!";
            }

            if (LngProfitCentreId != 0)
            {
                StrProfitCentrePrefix = GetProfitCentrePrefix(LngProfitCentreId, SqlTrans);
            }

            if (StrSubPrefix != "")
            {
                if (StrTransPrefix.Trim().Length > 0)
                {
                    StrTransPrefix = StrTransPrefix + "" + StrPrefixSeperator + "" + StrSubPrefix;
                }
                else
                {
                    StrTransPrefix = StrSubPrefix;
                }
            }

            GetTransPrefixAll(dtDet, StrTransPrefix, StrMonthDisplay, StrYearDisplay, ref StrTransPrefixAll, StrPrefixSeperator, StrProfitCentrePrefix, DtTransDate, SqlTransMain, SqlBiz);

            StrSplit = StrTransPrefixAll.Split("::");

            for (int IntI = 0; IntI < StrSplit.Length; IntI++)
            {
                if (IntI == 0)
                {
                    if (StrSplit[IntI] == "NUMBER")
                    {
                        StrPrefixTempString = GetLatestSlNo(StrPrefixSeperator, StrTranPrefixTable, StrTransPrefixColumn, StrStartNo, StrOrderByCol, strTransDB, StrPrefixSeperator, SqlTrans, SqlTransMain);
                        if (StrSplit[1] != "")
                        {
                            if (StrSplit[2] != "")
                            {
                                StrPrefixTempString = StrPrefixTempString + StrPrefixSeperator + StrSplit[1] + StrPrefixSeperator + StrSplit[2];
                            }
                            else
                            {
                                StrPrefixTempString = StrPrefixTempString + StrPrefixSeperator + StrSplit[1];
                            }
                        }
                        else if (StrSplit[2] != "")
                        {
                            StrPrefixTempString = StrPrefixTempString + StrPrefixSeperator + StrSplit[2];
                        }
                        else
                        {
                            StrPrefixTempString = StrPrefixTempString;
                        }
                        break;
                    }
                }
                else if (IntI == 1)
                {
                    if (StrSplit[IntI] == "NUMBER")
                    {
                        if (StrSplit[0] != "")
                        {
                            StrPrefixTempString = GetLatestSlNo(StrSplit[0] + StrPrefixSeperator, StrTranPrefixTable, StrTransPrefixColumn, StrStartNo, StrOrderByCol, strTransDB, StrPrefixSeperator, SqlTrans, SqlTransMain);
                        }
                        else
                        {
                            StrPrefixTempString = GetLatestSlNo(StrSplit[2], StrTranPrefixTable, StrTransPrefixColumn, StrStartNo, StrOrderByCol, strTransDB, StrPrefixSeperator, SqlTrans, SqlTransMain);
                        }
                        if (StrSplit[0] != "")
                        {
                            if (StrSplit[2] != "")
                            {
                                StrPrefixTempString = StrSplit[0] + StrPrefixSeperator + StrPrefixTempString + StrPrefixSeperator + StrSplit[2];
                            }
                            else
                            {
                                StrPrefixTempString = StrSplit[0] + StrPrefixSeperator + StrPrefixTempString;
                            }
                        }
                        else if (StrSplit[2] != "")
                        {
                            StrPrefixTempString = StrPrefixTempString + StrPrefixSeperator + StrSplit[2].Replace(StrPrefixSeperator, "");
                            //Replace(StrSplit[2], StrPrefixSeperator, "", 1, 1);
                        }
                        else
                        {
                            StrPrefixTempString = StrPrefixTempString;
                        }
                        break;
                    }
                }
                else
                {
                    if (StrSplit[IntI] == "NUMBER")
                    {
                        StrPrefixTempString = GetLatestSlNo(StrSplit[0] + StrSplit[1] + StrPrefixSeperator, StrTranPrefixTable, StrTransPrefixColumn, StrStartNo, StrOrderByCol, strTransDB, StrPrefixSeperator, SqlTrans, SqlTransMain);

                        if (StrSplit[0] != "")
                        {
                            if (StrSplit[1] != "")
                            {
                                StrPrefixTempString = StrSplit[0] + StrPrefixSeperator + StrSplit[1] + StrPrefixSeperator + StrPrefixTempString;
                            }
                            else
                            {
                                StrPrefixTempString = StrSplit[0] + StrPrefixSeperator + StrPrefixTempString;
                            }
                        }
                        else
                        {
                            if (StrSplit[1] != "")
                            {
                                StrPrefixTempString = StrSplit[1] + StrPrefixSeperator + StrPrefixTempString;
                            }
                            else
                            {
                                StrPrefixTempString = StrPrefixTempString;
                            }
                        }

                        break;
                    }
                }
            }
            return StrPrefixTempString;
        }


    }
}
