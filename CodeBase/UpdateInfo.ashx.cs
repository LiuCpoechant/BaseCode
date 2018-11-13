using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace CodeBase
{
    /// <summary>
    /// UpdateInfo 的摘要说明
    /// </summary>
    public class UpdateInfo : IHttpHandler
    {
        private readonly static string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            //context.Response.Write("Hello World");
             string flag = context.Request.QueryString["flag"];
             if (flag == null)
             {
                 int sno = Int32.Parse(context.Request.Form["sno"]);
                 string sname = context.Request.Form["sname"];
                 string sclass = context.Request.Form["sclass"];
                 string math = context.Request.Form["math"];
                 string chinese = context.Request.Form["chinese"];
                 string english = context.Request.Form["english"];
                 int i = 0;
                 using (SqlConnection conn = new SqlConnection(connStr))
                 {
                     string sql = "update student set sname=@sname,sclass=@sclass,math=@math,chinese=@chinese,english=@english where sno=@sno";
                     using (SqlCommand cmd = new SqlCommand(sql, conn))
                     {
                         SqlParameter[] pars =
                        {
                                    new SqlParameter("@sno",SqlDbType.Int),
                                    new SqlParameter("@sname",SqlDbType.NChar,10),
                                    new SqlParameter("@sclass",SqlDbType.NChar,10),
                                    new SqlParameter("@math",SqlDbType.NChar,10),                                        
                                    new SqlParameter("@chinese",SqlDbType.NChar,10),
                                    new SqlParameter("@english",SqlDbType.NChar,10)
                        };
                         pars[0].Value = sno;
                         pars[1].Value = sname;
                         pars[2].Value = sclass;
                         pars[3].Value = math;
                         pars[4].Value = chinese;
                         pars[5].Value = english;
                         cmd.Parameters.AddRange(pars);
                         conn.Open();
                         i = cmd.ExecuteNonQuery();
                     }
                 }
                 if (i > 0)
                 {
                     context.Response.Redirect("Info.ashx");
                 }
                 else
                 {
                     context.Response.Write("修改失败");
                 }
             }
             else
             {
                 int sno =Int32.Parse(context.Request["sno"]);
                 using (SqlConnection conn = new SqlConnection(connStr))
                 {
                     string sql = "select * from student where sno=@sno";
                     using (SqlDataAdapter apter = new SqlDataAdapter(sql, conn))
                     {
                         SqlParameter[] pars =
                         {
                          new SqlParameter("@sno",SqlDbType.Int)
                         };
                         pars[0].Value = sno;
                         apter.SelectCommand.Parameters.AddRange(pars);                    
                         DataTable da = new DataTable();
                         apter.Fill(da);
                         DataRow row = da.Rows[0];
                         string filePath = context.Request.MapPath("UpdateInfo.html");
                         string fileContext = File.ReadAllText(filePath);
                         fileContext = fileContext.Replace("$sno", row["sno"].ToString()).Replace("$sname", row["sname"].ToString()).Replace("$sclass", row["sclass"].ToString()).Replace("$math", row["math"].ToString()).Replace("$chinese", row["chinese"].ToString()).Replace("$english", row["english"].ToString());
                         context.Response.Write(fileContext);
                     }
                 }
             }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}