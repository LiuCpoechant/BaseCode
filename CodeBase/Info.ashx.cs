using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace CodeBase
{
    /// <summary>
    /// Info 的摘要说明
    /// </summary>
    public class Info : IHttpHandler
    {
        private readonly static string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            //context.Response.Write("Hello World");
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "select * from student";
                using (SqlDataAdapter apter = new SqlDataAdapter(sql,conn))
                {
                    DataTable da = new DataTable();
                    apter.Fill(da);
                    int i=da.Rows.Count;
                    if(i>0)
                    {
                        StringBuilder builder = new StringBuilder();
                        foreach (DataRow row in da.Rows)
                        {
                            builder.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td><a href='javascript:mydelete({0})'>删除</a></td><td><a href='UpdateInfo.ashx?sno={0}&flag=1'>修改</a></td></tr>",
                                row["sno"], row["sname"], row["sclass"], row["math"], row["chinese"], row["english"]);
                        }
                        string filePath = context.Request.MapPath("Info.html");
                        string fileContext = File.ReadAllText(filePath);
                        fileContext = fileContext.Replace("$tbody", builder.ToString());
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