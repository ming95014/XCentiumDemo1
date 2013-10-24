<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="XCentiumDemo1.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        About
    </h2>
    <p>Here is the challenge from : Martin C. Knudsen  |  XCentium  |  Partner  |  Office (949) 864-6070 | Cell (949) 874 0068</p>

     <p>   Write a small user friendly program that allows the user to type a URL for a given website page and the program will then do the following:</p>
     <ul>
        <li>List all images and show them to the user appropriately in a carousel or gallery control of some kind (borrow from the internet or write your own)</li>
        <li>Count all the words (display the total) and display the top 10 occurring words and their count (eithers as a table or chart of some kind, again you choose or write your own)</li>
     </ul>

    <p>Please send the program to me (as a zip) by Friday 10/25 at 4pm. </p>
    
    <h2>Author</h2>
    <p>Ming Chien (mingch@yahoo.com, (408-252-1933) Cupertino, CA.</p>

    <h2>Tools/Resources/Credits</h2>
    <ul>
        <li>Build on Visual Studio 2010</li>
        <li>ASP.NET Framework 4.0</li>
        <li>C#</li>
        <li>jQuery Carousel : http://sorgalla.com/projects/jcarousel/examples/dynamic_javascript.html </li>
        <li>Repository : Github : https://github.com/ming95014/XCentiumDemo1 </li>
        <li>Firebug</li>
    </ul>

    <h2>Coding Technique demonstrated</h2>
    <ul>
        <li>HttpWebRequest, HttpWebResponse</li>
        <li>Hashtable</li>
        <li>List&lt;T&gt;</li>
        <li>Regex</li>
        <li>jQuery</li>
        <li>delegate</li>
    </ul>

    <h2>Release notes</h2>
    <ul>
        <li>By default only showing JPEG since there're many small filler png/gif.</li>
        <li>When JPEG-only is not checked, there're many tiny png/gif that may not show well on carousel.</li>
        <li>HTML tags are not part of 'words' counted.</li>
        <li>Words length < 4 or > 14 are skipped to avoid common words such as I, and, the...etc.</li>
        <li>When BODY-only is not checked, many more 'words' could be found.</li>
        <li>images and text from iframe or Sprints are not pulled.</li>
    </ul>

    <h2>Release History</h2>
    <ul>
        <li>V0.1  Ming  10/23/2013</li>
        <li>V0.2  Ming  10/24/2013</li>
    </ul>
</asp:Content>