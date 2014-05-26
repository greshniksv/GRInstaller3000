using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GRInstaller3000Classes;

namespace GRInstaller3000Creator
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            srtbSourceScript.Settings.Keywords.Add("function");
            srtbSourceScript.Settings.Keywords.Add("if");
            srtbSourceScript.Settings.Keywords.Add("then");
            srtbSourceScript.Settings.Keywords.Add("else");
            srtbSourceScript.Settings.Keywords.Add("elseif");
            srtbSourceScript.Settings.Keywords.Add("end");

            Commands.GetList();

            // Set the comment identifier. For Lua this is two minus-signs after each other (--). 
            // For C++ we would set this property to "//".
            srtbSourceScript.Settings.Comment = "#";
            
            // Set the colors that will be used.
            srtbSourceScript.Settings.KeywordColor = Color.Blue;
            srtbSourceScript.Settings.CommentColor = Color.Green;
            srtbSourceScript.Settings.StringColor = Color.Gray;
            srtbSourceScript.Settings.IntegerColor = Color.Red;

            // Let's not process strings and integers.
            srtbSourceScript.Settings.EnableStrings = false;
            srtbSourceScript.Settings.EnableIntegers = false;

            // Let's make the settings we just set valid by compiling
            // the keywords to a regular expression.
            srtbSourceScript.CompileKeywords();

            // Load a file and update the syntax highlighting.
            //srtbSourceScript.LoadFile("../script.lua", RichTextBoxStreamType.PlainText);
            srtbSourceScript.ProcessAllLines();


        }
    }
}
