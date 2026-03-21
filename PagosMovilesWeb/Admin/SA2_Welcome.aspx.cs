using System;
using System.Web.UI;
using PagosMovilesWeb.Services;

namespace PagosMovilesWeb.Admin
{
    public partial class SA2_Welcome : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblNombreCompleto.Text = SessionHelper.NombreCompleto;
            }
        }
    }
}