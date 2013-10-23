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

        protected void OnClick_Submit(object sender, EventArgs e)
        {
            // 1. read the source of the URL
            LoadImagesIntoCarousel(tbURL.Text);

            // 2. count the words

            pnlResults.Visible = true;
        }

        private void LoadImagesIntoCarousel(string strURL)
        {
            // 1. get each image links from the URL
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            List<Uri> links = null;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using( StreamReader sr = new StreamReader( response.GetResponseStream()))
                {
                    links = FetchLinksFromSource( sr.ReadToEnd() );
                }
            }
            // 2. Load the strPhotos
            StringBuilder sb = new StringBuilder();
            foreach (Uri link in links)
            {
                if (link.AbsoluteUri.ToLower().Contains(".jpg"))
                sb.Append(@"{ url: '" + link.AbsoluteUri + "', title: 'img' },");
            }
            strPhotos = sb.ToString();
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