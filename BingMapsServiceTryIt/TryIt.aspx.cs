using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TryIt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit1_Click(object sender, EventArgs e)
    {
        BingMapsService.Service1Client fromService = new BingMapsService.Service1Client();

        lblCoord1.Text = fromService.getGeocode(txtAdd1.Text);
    }

    protected void btnSubmit2_Click(object sender, EventArgs e)
    {
        BingMapsService.Service1Client fromService = new BingMapsService.Service1Client();

        Double.TryParse(txtLat.Text, out double lat);
        Double.TryParse(txtLong.Text, out double lon);

        lblAdd.Text = fromService.getAddress(lat, lon);
    }

    protected void btnSubmit3_Click(object sender, EventArgs e)
    {
        BingMapsService.Service1Client fromService = new BingMapsService.Service1Client();

        lblInstructions.Text = fromService.findRoute(txtStartAdd1.Text, txtDestAdd1.Text);
    }

    protected void btnSubmit4_Click(object sender, EventArgs e)
    {
        BingMapsService.Service1Client fromService = new BingMapsService.Service1Client();

        IMGMap.ImageUrl = fromService.getMap(txtStartAdd2.Text, txtDestAdd2.Text);
    }
}