using System;
using System.Collections;
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
            strPhotos = GetImagesForCarouselFrom(strHTML);

            // 3. count the words
            litTextInfo.Text = GetTextInfo(strHTML);

            pnlResults.Visible = true;
            lkButton.NavigateUrl = tbURL.Text;
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

        private string GetImagesForCarouselFrom(string strHTML)
        {
            // 1. Get the images links from HTML source
            List<Uri> links = FetchLinksFromSource(strHTML);

            // 2. construct the image list for strPhotos--the image source for the Carousel
            int cnt = 0;
            StringBuilder sb = new StringBuilder();
            foreach (Uri link in links)
            {
                if (chkJPGOnly.Checked && !link.AbsoluteUri.ToLower().Contains(".jpg"))
                    continue;
                else if (!link.AbsoluteUri.StartsWith("file"))
                    sb.Append(@"{ url: '" + link.AbsoluteUri + "', title: '"+ (++cnt).ToString() + "' },");                
            }
            litPhotoInfo.Text = cnt.ToString() + (chkJPGOnly.Checked ? " JPEG images found." : " images found.");
            return sb.ToString();          
        }

        // Get all the <img src=' '> from htmlSource
        private List<Uri> FetchLinksFromSource(string htmlSource)
        {
            List<Uri> links = new List<Uri>();
            try
            {
                string regexImgSrc = @"<img.+?src=""(.+?)"".+?/?>";   // ORIG, 3, 33
                MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m in matchesImgSrc)
                {
                    string href = m.Groups[1].Value;
                    if (href.StartsWith(@"/") || href.StartsWith(@"~"))
                        href = tbURL.Text + "/" + href;
                    try
                    {
                        links.Add(new Uri(href));
                    }
                    catch (Exception ex)
                    {
                        // ignore bad image urls for now
                    }
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
            // 1. Get the <body> of HTML
            if (chkBodyOnly.Checked)
                strHTML = GetBodyofHTML(strHTML);

            // 2. let's get rid of all the words from the html tags--pretty sure that's not what we want to count them
            strHTML = Regex.Replace(strHTML, @"<(.|\n)*?>", string.Empty).Replace("&nbsp;", "");

            // 3. next, let's get rid of all the non-alpha numberics, such as [, . { ....etc.
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            strHTML = rgx.Replace(strHTML, ""); //.Replace("  ","");

            // 4. Split them
            int iWordCnt = 0;
            Hashtable hWords = new Hashtable();
            string[] arr = strHTML.Split(' ');
            //StringBuilder sb = new StringBuilder();
            foreach (string w in arr)
            {
                var word = w.Trim();
                // 5. Also get rid of really short and really long words--just taking the liberty that we don't want them...Ming
                if (word.Length > 3 && word.Length < 14 && !word.Contains("-") && !word.ToCharArray().Any(char.IsNumber))
                {
                    //sbAppendLI(ref sb, word);
                    iWordCnt++;
                    // 6. Store into hashtable if not already in, if already in, increment it.
                    if (!hWords.Contains(word))
                        hWords.Add(word, 1);
                    else  // otherwise, we increment the count
                        hWords[word] = (int)hWords[word] + 1;
                }
            }

            return "Found " + iWordCnt.ToString() + " words.<br/>" +
                   "The top 10 most frequent words are :" + GetTop10Words(hWords); // + 
                   //"And here are all the words that I found:" +
                   //"<ol>" + sb.ToString() + "</ol>";
        }

        // Get to just the text between <body> and </body>
        private string GetBodyofHTML(string strHTML)
        {
            int iStartBody = 0;
            int iEndBody = -1;
            strHTML = strHTML.ToLower();
            if (strHTML.IndexOf("<body>") > 0)
                iStartBody = strHTML.IndexOf("<body>");
            else if (strHTML.IndexOf("<body ") > 0)
                iStartBody = strHTML.IndexOf("<body ");

            if (strHTML.IndexOf("</body>") > 0)
                iEndBody = strHTML.IndexOf("</body>");

            return (iEndBody == -1) ? strHTML.Substring(iStartBody) : strHTML.Substring(iStartBody, iEndBody - iStartBody + 7);
        }

        // This is done by inserting the Hash table into List, Sort it by Count, and then reverse it to get the top 10
        private string GetTop10Words(Hashtable hWords)
        {
            List<WordAndCnt> list = new List<WordAndCnt>();
            //StringBuilder sb2 = new StringBuilder();
            //sb2.Append("<ol>");
            for (IDictionaryEnumerator e = hWords.GetEnumerator(); e.MoveNext(); )
            {
                list.Add(new WordAndCnt(e.Key.ToString(), Convert.ToInt32(e.Value.ToString())));
                //sbAppendLI(ref sb2, e.Key + " " + e.Value);
            }
            //sb2.Append("</ol>");

            list.Sort(delegate(WordAndCnt p1, WordAndCnt p2) { return p1.Count.CompareTo(p2.Count); });
            list.Reverse();

            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='1' cellspacing='0' cellpadding='4'>");
            sb.Append("<tr><td><b>#</b></td><td><b>Word</b></td><td><b>Occured</b></td></tr>");
            int iCnt = 0;
            foreach (WordAndCnt w in list)
            {
                if (w.Count > 1)
                {
                    sb.Append("<tr><td><b>" + (++iCnt).ToString() + "</b></td><td><b>" + w.Word + "</b></td>" + "<td><b>" + w.Count + "</b></td></tr>");
                    //sbAppendLI(ref sb, "<b>" + w.Word + "</b> occured " + w.Count + " times");
                    if (iCnt >= 10)
                        break;
                }
            }
            sb.Append("</table>");

            return sb.ToString(); // +sb2.ToString();
        }

        private void sbAppendLI(ref StringBuilder sb, string str)
        {
            sb.Append("<li>" + str + "</li>");
        }

        public class WordAndCnt
        {
            public string Word;
            public int Count;

            public WordAndCnt(string w, int c)
            {
                this.Word = w;
                this.Count = c;
            }
        }

        #endregion  // Text
    }
}