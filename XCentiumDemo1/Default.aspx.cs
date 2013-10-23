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
            string strHTML = GetHTMLSourceFromURL(tbURL.Text);

            // 2. Load Images Into Carousel
            strPhotos = LoadImagesIntoCarousel(strHTML);

            // 3. count the words
            litTextInfo.Text = GetTextInfo(strHTML);
            pnlResults.Visible = true;
        }

        private string GetHTMLSourceFromURL(string strURL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            return string.Empty;
        }

        #region Images
        private string LoadImagesIntoCarousel(string strHTML)
        {
            // 1. Get the images links from HTML source
            List<Uri> links = FetchLinksFromSource(strHTML);

            // 2. Load the strPhotos--the image source for the Carousel
            int cnt = 0;
            StringBuilder sb = new StringBuilder();
            foreach (Uri link in links)
            {
                if (link.AbsoluteUri.ToLower().Contains(".jpg"))
                {
                    sb.Append(@"{ url: '" + link.AbsoluteUri + "', title: 'img' },");
                    cnt++;
                }
            }
            litPhotoInfo.Text = cnt.ToString() + " JPEG images found.";
            return sb.ToString();          
        }

        private List<Uri> FetchLinksFromSource(string htmlSource)
        {
            List<Uri> links = new List<Uri>();
            try
            {
                
                string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
                MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m in matchesImgSrc)
                {
                    string href = m.Groups[1].Value;
                    links.Add(new Uri(href));
                }
            }
            catch (Exception ex)
            {
                // ignore bad image urls for now
            }
            return links;
        }
        #endregion // Images

        #region Text

        // Get the Text information from the scrapped html
        private string GetTextInfo(string strHTML)
        {
            int iWordCnt = 0;
            // 1. let's get rid of all the words from the html tags--pretty sure that's not what we want to count them
            strHTML = Regex.Replace(strHTML, @"<(.|\n)*?>", string.Empty);
            // 2. next, let's get rid of all the non-alpha numberics, such as [, . { ....etc.
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            strHTML = rgx.Replace(strHTML, ""); //.Replace("  ","");
            // 3. Split them
            string[] arr = strHTML.Split(' ');
            StringBuilder sb = new StringBuilder();
            foreach (string word in arr)
            {
                var w = word.Trim();
                // Also get rid of really short and really long words--just taking the liberty that we don't want them...Ming
                if (w.Length > 3 && w.Length < 14 && !w.Contains("-") && !w.ToCharArray().Any(char.IsNumber))
                {
                    sb.Append("<li>" + w + "</li>");
                    iWordCnt++;
                }
            }

            return "Found " + iWordCnt.ToString() + " words. <ol>" + sb.ToString() + "</ol>";
        }
        #endregion  // Text
    }
}