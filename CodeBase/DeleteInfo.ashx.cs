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
    /// DeleteInfo 的摘要说明
    /// </summary>
    public class DeleteInfo : IHttpHandler
    {
        private readonly static string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            //context.Response.Write("Hello World");
            int sno=Int32.Parse(context.Request.QueryString["sno"]);
            int i = 0;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "delete student where sno=@sno";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlParameter[] pars =
                {
                new SqlParameter("@sno",SqlDbType.Int)
                };
                    pars[0].Value = sno;
                    cmd.Parameters.AddRange(pars);
                    conn.Open();
                    i = cmd.ExecuteNonQuery();           
                }
                if (i > 0)
                {
                    context.Response.Redirect("Info.ashx");
                }
                else
                {
                    context.Response.Write("删除失败");
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