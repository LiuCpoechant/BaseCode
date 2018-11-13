using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CodeBase
{
    /// <summary>
    /// AddInfo 的摘要说明
    /// </summary>
    public class AddInfo : IHttpHandler
    {
        private readonly static string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            //context.Response.Write("Hello World");
            int sno =Int32.Parse(context.Request["sno"]);
            string sname = context.Request.Form["sname"];
            string sclass = context.Request.Form["sclass"];
            string math = context.Request.Form["math"];
            string chinese = context.Request.Form["chinese"];
            string english = context.Request.Form["english"];
            int i = 0;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "insert into student values(@sno,@sname,@sclass,@math,@chinese,@english)";
                using (SqlCommand cmd = new SqlCommand(sql,conn))
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
                    pars[0].Value =sno;
                    pars[1].Value = sname;
                    pars[2].Value = sclass;
                    pars[3].Value = math;
                    pars[4].Value = chinese;
                    pars[5].Value = english;
                    cmd.Parameters.AddRange(pars);//将参数数组中的数据添加到数据库中
                    conn.Open();
                    i = cmd.ExecuteNonQuery();
                }
                if (i > 0)
                {
                    context.Response.Redirect("/Info.ashx");
                }
                else
                {
                    context.Response.Write("修改失败");
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