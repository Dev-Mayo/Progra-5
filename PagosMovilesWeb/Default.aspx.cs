using System;

namespace PagosMovilesWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Portal/Login.aspx", false);
        }
    }
}