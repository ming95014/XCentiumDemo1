<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="XCentiumDemo1._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">    
    <html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en-us">
    <head>
        <title>jCarousel Examples</title>
        <link href="../Styles/style.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="../lib/jquery-1.9.1.min.js"></script>
        <script type="text/javascript" src="../lib/jquery.jcarousel.min.js"></script>
        <link rel="stylesheet" type="text/css" href="../Styles/skin.css" />
        <script type="text/javascript">

            var mycarousel_itemList = [<%= strPhotos %>];

            function mycarousel_itemLoadCallback(carousel, state) {
                for (var i = carousel.first; i <= carousel.last; i++) {
                    if (carousel.has(i)) {
                        continue;
                    }

                    if (i > mycarousel_itemList.length) {
                        break;
                    }

                    carousel.add(i, mycarousel_getItemHTML(mycarousel_itemList[i - 1]));
                }
            };

            /**
            * Item html creation helper.
            */
            function mycarousel_getItemHTML(item) {
                return '<img src="' + item.url + '" width="75" height="75" title="' + item.title + '" alt="' + item.url + '" />';
            };

            jQuery(document).ready(function () {
                jQuery('#mycarousel').jcarousel({
                    size: mycarousel_itemList.length,
                    scroll: 1,
                    itemFallbackDimension: mycarousel_itemList.length,
                    itemLoadCallback: { onBeforeAnimation: mycarousel_itemLoadCallback }
                });
            });

        </script>
    </head>
    <body>
        <div id="wrap">
            <p>Enter a valid URL, and I will show you all the images from that URL in a Carousel, and also show you some info about the page.</p>
            <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
  
            Enter your URL here : <asp:TextBox ID="tbURL" Width="700px" runat="server" Text="http://www.flickr.com/explore" /><br />
            <asp:CheckBox ID="chkJPGOnly" runat="server" Text="JPEG images only" Checked="true" /><img src="/images/question_mark.jpg" width="15px" title="Why?  Since many gif/png are too small to see." /> 
            <asp:CheckBox ID="chkBodyOnly" runat="server" Text="Only Search for Words within BODY of html" Checked="true" /><img src="/images/question_mark.jpg" width="15px" title="Only look for words within body tags" /> 
            <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="OnClick_Submit" ValidationGroup="vg1" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbURL" ValidationGroup="vg1" CssClass="field-validation-error" 
                                            ErrorMessage="Please enter a URL" Display="Dynamic" />
            <asp:RegularExpressionValidator runat="Server" ControlToValidate="tbURL" ValidationGroup="vg1" CssClass="field-validation-error"
                                            ValidationExpression="(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?" 
                                            ErrorMessage="Please enter a valid URL" Display="Dynamic"/>
            <hr />
            <asp:Panel ID="pnlResults" runat="server" Visible="false">
                <h3>Here are the images that I found on this page</h3>
                <asp:Literal ID="litPhotoInfo" runat="server" />
                <ul id="mycarousel" class="jcarousel-skin-ie7">
                    <!-- The content will be dynamically loaded in here -->
                </ul>
                <asp:HyperLink ID="lkButton" runat="server" Target="_blank" Text="Open this page in a new browser window" />
                <br />
                <h3>Some interesting stats about the words in this page</h3>
                <asp:Literal ID="litTextInfo" runat="server" />           
            </asp:Panel>
            
        </div>
    </body>
    </html>
</asp:Content>