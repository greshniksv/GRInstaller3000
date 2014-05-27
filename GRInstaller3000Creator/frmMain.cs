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
        ScriptEngine _engine = new ScriptEngine();
        public frmMain()
        {
            InitializeComponent();
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            srtbSourceScript.Settings.ManageKeywords.Add("def");
            srtbSourceScript.Settings.ManageKeywords.Add("if");
            srtbSourceScript.Settings.ManageKeywords.Add("then");
            srtbSourceScript.Settings.ManageKeywords.Add("else");
            srtbSourceScript.Settings.ManageKeywords.Add("elseif");
            srtbSourceScript.Settings.ManageKeywords.Add("end");
            srtbSourceScript.Settings.ManageKeywords.Add("var");

            foreach (var name in _engine.Command.GetCommandNameList())
            {
                srtbSourceScript.Settings.Keywords.Add(name);
            }
          
            srtbSourceScript.Settings.KeywordColor = Color.DarkRed;


            // Set the comment identifier. For Lua this is two minus-signs after each other (--). 
            // For C++ we would set this property to "//".
            srtbSourceScript.Settings.Comment = "#";
            
            // Set the colors that will be used.
            srtbSourceScript.Settings.ManageKeywordColor = Color.Blue;
            srtbSourceScript.Settings.CommentColor = Color.Green;
            srtbSourceScript.Settings.StringColor = Color.Gray;
            srtbSourceScript.Settings.IntegerColor = Color.Red;

            // Let's not process strings and integers.
            srtbSourceScript.Settings.EnableStrings = true;
            srtbSourceScript.Settings.EnableIntegers = true;

            // Let's make the settings we just set valid by compiling
            // the keywords to a regular expression.
            srtbSourceScript.CompileKeywords();

            // Load a file and update the syntax highlighting.
            //srtbSourceScript.LoadFile("../script.lua", RichTextBoxStreamType.PlainText);
            srtbSourceScript.ProcessAllLines();


        }
    }
}
