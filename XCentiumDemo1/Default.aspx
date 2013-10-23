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

            var mycarousel_itemList = [
                                        { url: "http://static.flickr.com/66/199481236_dc98b5abb3_s.jpg", title: "Flower1" },
                                        { url: "http://static.flickr.com/75/199481072_b4a0d09597_s.jpg", title: "Flower2" },
                                        { url: "http://static.flickr.com/57/199481087_33ae73a8de_s.jpg", title: "Flower3" },
                                        { url: "http://static.flickr.com/77/199481108_4359e6b971_s.jpg", title: "Flower4" },
                                        { url: "http://static.flickr.com/58/199481143_3c148d9dd3_s.jpg", title: "Flower5" },
                                        { url: "http://static.flickr.com/72/199481203_ad4cdcf109_s.jpg", title: "Flower6" },
                                        { url: "http://static.flickr.com/58/199481218_264ce20da0_s.jpg", title: "Flower7" },
                                        { url: "http://static.flickr.com/69/199481255_fdfe885f87_s.jpg", title: "Flower8" },
                                        { url: "http://static.flickr.com/60/199480111_87d4cb3e38_s.jpg", title: "Flower9" },
                                        { url: "http://static.flickr.com/70/229228324_08223b70fa_s.jpg", title: "Flower10" },
                                        { url: "http://farm8.staticflickr.com/7425/10421900155_7a2de128f3_z.jpg", title: "People" }
                                    ];

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
                return '<img src="' + item.url + '" width="75" height="75" alt="' + item.url + '" />';
            };

            jQuery(document).ready(function () {
                jQuery('#mycarousel').jcarousel({
                    size: mycarousel_itemList.length,
                    scroll: 1,
                    wrap: "circular",
                    width: 50,
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
  
            Enter your URL here : <asp:TextBox ID="tbURL" Width="400px" runat="server" />
            <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="OnClick_Submit" />
            <hr />
            <asp:Panel ID="pnlResults" runat="server" Visible="false">
                <asp:Literal ID="litPhotoInfo" runat="server" />
                <ul id="mycarousel" class="jcarousel-skin-ie7">
                    <!-- The content will be dynamically loaded in here -->
                </ul>
                <br />
                <br />
                <h2>Some interesting stats about this page</h2>
                <asp:Literal ID="litTextInfo" runat="server" />           
            </asp:Panel>
            
        </div>
    </body>
    </html>
</asp:Content>
