using Microsoft.ApplicationBlocks.Data;
using SVAMITVA.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SVAMITVA
{
    public partial class _Default : Page
    {
        Helper objhelper = new Helper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindDist();
                BindSurveyUnitType();
            }
        }

        protected void ddlPropDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                ddlPropTehsil.Items.Clear();
                ddlPropVillage.Items.Clear();
                ddlPropVillage.Items.Insert(0, new ListItem("Please Select", "Please Select"));
                ddlPropTehsil.Items.Insert(0, new ListItem("Please Select", "Please Select"));
                if (ddlPropDistrict.SelectedValue != "0")
                {
                    string[] dist = ddlPropDistrict.SelectedValue.Split('|');
                    string dcode = Convert.ToString(dist[1].Trim());
                    
                    DataTable dtResponse = objhelper.FetchRegions("Tehsil", dcode);
                    foreach (DataRow row in dtResponse.Rows)
                    {
                        ddlPropTehsil.Items.Add(new ListItem(HttpUtility.UrlDecode(row["location_name_p"].ToString()) + "/" + row["location_name"].ToString(), row["locationID"].ToString()));
                    }
                    dtResponse = null;
                }
            }
            catch (Exception ex)
            {
                //lblerror.Text = ex.Message.ToString();
                //objhelper.insert_AuditLog("PropertyDetails", "ddlPropDistrict_SelectedIndexChanged", loginid, DateTime.Now, myIP, ex.Message.ToString());
            }
        }
        protected void ddlPropTehsil_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                ddlPropVillage.Items.Clear();
                ddlPropVillage.Items.Insert(0, new ListItem("Please Select", "Please Select"));
                GetVillages();
                

            }
            catch (Exception ex)
            {
                //lblerror.Text = ex.Message.ToString();
                //objhelper.insert_AuditLog( "PropertyDetails", "ddlPropTehsil_SelectedIndexChanged", loginid, DateTime.Now, myIP, ex.Message.ToString());
            }

        }
        protected void ddlPropVillage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPropVillage.SelectedValue.ToString();
        }
        public void BindDist()
        {
            try
            {

                DataSet ds;

                ds = SqlHelper.ExecuteDataset(SqlHelper.DBConn, CommandType.Text, "select (cast(DistrictCode as varchar(20)) + ' | ' + cast(LRDID as varchar(10))) as dcode,DistrictNameEng from District where DistrictCode <> 44 order by DistrictNameEng");
                if (((ds.Tables.Count > 0)
                            && (ds.Tables[0].Rows.Count > 0)))
                {
                    ddlPropDistrict.DataSource = ds;
                    ddlPropDistrict.DataTextField = "DistrictNameEng";
                    ddlPropDistrict.DataValueField = "dcode";

                }
                else
                {
                    ddlPropDistrict.DataSource = null;

                }

                ddlPropDistrict.DataBind();
                ddlPropDistrict.Items.Insert(0, new ListItem("--Select--", "0"));

            }
            catch (Exception ex)
            {
                Helper objhelper = new Helper();
                //lblerror.Text = ex.Message.ToString();
                // objhelper.insert_AuditLog("PropertyDetails", "BindDist", loginid, DateTime.Now, myIP, ex.Message.ToString());
            }
        }
        public void BindSurveyUnitType() 
        {
            try
            {

                DataSet ds;

                ds = SqlHelper.ExecuteDataset(SqlHelper.DBConn, CommandType.Text, "select (cast(U.Id as varchar(20)) + ' | ' + cast(U.UnitType as varchar(10))) as code,U.UnitType from UnitType U order by U.UnitType");
                if (((ds.Tables.Count > 0)
                            && (ds.Tables[0].Rows.Count > 0)))
                {
                    ddlSurveyUnittype.DataSource = ds;
                    ddlSurveyUnittype.DataTextField = "UnitType";
                    ddlSurveyUnittype.DataValueField = "code";

                }
                else
                {
                    ddlSurveyUnittype.DataSource = null;

                }

                ddlSurveyUnittype.DataBind();
                ddlSurveyUnittype.Items.Insert(0, new ListItem("--Select--", "0"));

            }
            catch (Exception ex)
            {
                Helper objhelper = new Helper();
                //lblerror.Text = ex.Message.ToString();
                // objhelper.insert_AuditLog("PropertyDetails", "BindDist", loginid, DateTime.Now, myIP, ex.Message.ToString());
            }

        }
        protected void GetVillages()
        {
            try
            {
                
                if (ddlPropTehsil.SelectedValue != "Please Select")
                {
                    DataSet ds;
                    DataTable dtResponse = objhelper.RegionFetch_TehsilVillages(Convert.ToInt32(ddlPropTehsil.SelectedValue));
                    foreach (DataRow row in dtResponse.Rows)
                    {
                        ddlPropVillage.Items.Add(new ListItem(HttpUtility.UrlDecode(row["location_name_p"].ToString()) + "/" + row["location_name"].ToString(), row["locationID"].ToString()));
                    }
                }

            }
            catch (Exception ex)
            {
                //lblerror.Text = ex.Message.ToString();
                //objhelper.insert_AuditLog("PropertyDetails", "ddlPropTehsilSearch_SelectedIndexChanged", loginid, DateTime.Now, myIP, ex.Message.ToString());
            }

        }
    }
}