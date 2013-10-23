using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XCentiumDemo1
{
    public partial class _Default : System.Web.UI.Page
    {
        protected string strPhotos = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnClick_Submit(object sender, EventArgs e)
        {
            // 1. read the source of the URL

            // 2. find the images

            // 3. count the words

            StringBuilder sb = new StringBuilder();
            sb.Append(@"{ url: ""http://static.flickr.com/66/199481236_dc98b5abb3_s.jpg"", title: ""Flower1"" },");
            sb.Append(@"{ url: ""http://static.flickr.com/75/199481072_b4a0d09597_s.jpg"", title: ""Flower2"" },");
            sb.Append(@"{ url: ""http://static.flickr.com/57/199481087_33ae73a8de_s.jpg"", title: ""Flower3"" },");
            sb.Append(@"{ url: ""http://farm8.staticflickr.com/7425/10421900155_7a2de128f3_z.jpg"", title: ""People"" }");
            strPhotos = sb.ToString();
            pnlResults.Visible = true;
        }

        private void GetLinks()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.example.com");
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using( StreamReader sr = new StreamReader( response.GetResponseStream()))
                {
                    List<Uri> links = FetchLinksFromSource( sr.ReadToEnd() );
                }
            }
        }

        private List<Uri> FetchLinksFromSource(string htmlSource)
        {
            List<Uri> links = new List<Uri>();
            string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
            MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match m in matchesImgSrc)
            {
                string href = m.Groups[1].Value;
                links.Add(new Uri(href));
            }
            return links;
        }
    }
}
