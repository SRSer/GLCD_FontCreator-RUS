/*
Part of GLCD_FontCreator - Copyright 2015 Martin Burri

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using GLCD_FontCreator.CustomFontDialog;
using GLCD_FontCreator.FontCreators;

namespace GLCD_FontCreator
{




    public partial class FontCreator : Form
  {
    // stores the FontCreator supported
    private Dictionary<String, Func<FontOptimizer,FCBase>> m_FCchooser = new  Dictionary<string, Func<FontOptimizer, FCBase>>();

    public FontCreator( )
    {
      InitializeComponent( );

      this.Text = String.Format( "{0} V {1}", Application.ProductName, Application.ProductVersion );

            // the default test char line
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            Encoding win1251 = Encoding.GetEncoding(1251);
            int first = win1251.GetBytes("я")[0];
            byte[] bytes = win1251.GetBytes("я");
            char[] chars = win1251.GetChars(bytes);
            //   char lastChar = (char)first;
       //     byte[] win1251Bytes = Encoding.Convert(win1251, utf8, bytes);
        //    char lastChar = chars[0];
            char lastChar = win1251.GetChars(bytes)[0];

            byte[] arrBytes = Enumerable.Range(192, 64).Select(x => (byte)x).ToArray();
            Encoding w1251 = Encoding.GetEncoding(1251);
            char[] arrChars = w1251.GetChars(arrBytes);
        //    Console.WriteLine(string.Join(" ", arrChars));

       
        
            c_testString = "";
          //      for (Char c = ' '; c < lastChar; c++)   {
          //      c_testString += c;
          //  }

            var encoding = Encoding.GetEncoding(1251);
            var chars3 = new char[1];
            var bytes3 = new byte[1];

            for (int i = 32; i < 256; i++)
            {
                bytes3[0] = (byte)i;
                encoding.GetChars(bytes3, 0, 1, chars3, 0);
                c_testString += chars3[0];
             //   Console.WriteLine("{0} {1}", i, chars3[0]);
            }

            m_font = ( Font )fDlg.Font.Clone( );
      ValidateChars( );


      // ************ ADD NEW FONT CREATOR FORMATS HERE *********************
      // add known file formats
      comFileFormat.Items.Add( GLCD_FC2_Compatible.FontCreatorName );
      m_FCchooser.Add( GLCD_FC2_Compatible.FontCreatorName, GLCD_FC2_Compatible.GetInstance );
      // next one
      // comFileFormat.Items.Add( FC_Template.FontCreatorName );
      // m_FCchooser.Add( FC_Template.FontCreatorName, FC_Template.GetInstance );


      // finally set the default selected Save File Format
      comFileFormat.SelectedIndex = 0; // just select the first one as default

      // more init
      hScrTargetHeight.Value = 16; // start value Height target
      OptimizeFont( ); // optimize with default values
    }



    private String c_testString = "";
    private Font m_font = null;
    private FCBase FC = null;
    private FontOptimizer FO = null;
    private PrivateFontCollection MYFONTS = null; // contains loadable fonts
    private CFontDialog cfDlg = new CFontDialog(null);
    private AppSettings appSettings = new AppSettings();


        static string UTF8ToWin1251(string sourceStr)
        {
            Encoding utf8 = Encoding.UTF8;
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");
            byte[] utf8Bytes = utf8.GetBytes(sourceStr);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
            return win1251.GetString(win1251Bytes);
        }

        static private string Win1251ToUTF8(string source)
        {
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");
            byte[] utf8Bytes = win1251.GetBytes(source);
            byte[] win1251Bytes = Encoding.Convert(win1251, utf8, utf8Bytes);
            source = win1251.GetString(win1251Bytes);
            return source;
        }

        private void InitText( )
    {

            //  Encoding win1251 = Encoding.GetEncoding("Windows-1251");
            //    string text = txLastChar.Text;
            //   byte[] bytes4 = win1251.GetBytes(text);
            // string txMyText = win1251.GetString(txMyText);

       //     Encoding win1251 = Encoding.GetEncoding("Windows-1251");
       //     byte[] bytes6 = win1251.GetBytes(txTxFont.Text);
      //      txTxFont.Text = Encoding.UTF8.GetString(bytes6);
      //      byte[] bytes7 = win1251.GetBytes(txMyText.Text);
      //      txMyText.Text = Encoding.UTF8.GetString(bytes7);
      //      byte[] bytes8 = win1251.GetBytes(c_testString);
      //      c_testString = Encoding.UTF8.GetString(bytes8);

            txTxFont.Text = UTF8ToWin1251(txTxFont.Text);
            txMyText.Text = UTF8ToWin1251(txMyText.Text);
            c_testString = UTF8ToWin1251(c_testString);

            if ( String.IsNullOrEmpty( txMyText.Text ) ) {
        txTxFont.Text = c_testString;
             //   Console.WriteLine(c_testString);
            }
      else {
        txTxFont.Text = txMyText.Text;
          //      Console.WriteLine(txMyText.Text);
            }
        //    Console.WriteLine(txTxFont.Text);
        }

    private void ShowFontProps( )
    {
      InitText( );
      txTxFont.Font = m_font;

      // get font props shown
      lbFontProps.Items.Clear( );
      String tx = "";
      tx = String.Format( "Bold                  {0}", m_font.Bold ); lbFontProps.Items.Add( tx );
      tx = String.Format( "Italic                {0}", m_font.Italic ); lbFontProps.Items.Add( tx );
      tx = String.Format( "Underline             {0}", m_font.Underline ); lbFontProps.Items.Add( tx );
      tx = String.Format( "Strikeout             {0}", m_font.Strikeout ); lbFontProps.Items.Add( tx );
      tx = String.Format( "Family                {0}", m_font.FontFamily.ToString( ) ); lbFontProps.Items.Add( tx );
      tx = String.Format( "Name                  {0}", m_font.Name ); lbFontProps.Items.Add( tx );
      tx = String.Format( "Charset               {0}", m_font.GdiCharSet ); lbFontProps.Items.Add( tx );
      tx = String.Format( "Height (line spacing) {0}", m_font.Height ); lbFontProps.Items.Add( tx );
      tx = String.Format( "Size [pts]            {0}", m_font.SizeInPoints ); lbFontProps.Items.Add( tx );
    }


    private void OptimizeFont( )
    {
      if ( m_TotalChars <= 0 ) return; // TODO complain..

      // use optimizer to get the height
      FO = new FontOptimizer( m_font, MYFONTS );
            Encoding win1251 = Encoding.GetEncoding(1251);
            int first = win1251.GetBytes(txFirstChar.Text)[0];
            FO.FirstChar = first;

         //   FO.FirstChar = txFirstChar.Text[0];
      FO.CharCount = m_TotalChars;
      int tSize;
      if ( !int.TryParse( txTargetSize.Text, out tSize ) ) {
        tSize = 12; txTargetSize.Text = tSize.ToString( ); // fix if not a number..
      }
      FO.TargetHeight = tSize;
      FO.RemoveTop = cbxRemoveTop.Checked;
      FO.RemoveBottom = cbxRemoveBottom.Checked;

      FO.Optimize( ); // this should now reveal the font to use
      // carry into globals and GUI
      m_font = FO.FontToUse;
      ShowFontProps( );
      txFinalSize.Text = String.Format( "{0} x {1}", FO.MinimumRect.Width, FO.MinimumRect.Height );
      Clean( );
    }

    private void LoadFont( )
    {
      float tSize;
      if ( !float.TryParse( txTargetSize.Text, out tSize ) ) {
        tSize = 12; txTargetSize.Text = tSize.ToString( ); // fix if not a number..
      }
      m_font = new Font( cfDlg.Font.FontFamily, tSize, cfDlg.Font.Style );
      cfDlg.Font = m_font;
      // using a 'tuned font dialog that deals with the exception issue of the original one
      cfDlg.AddPrivateFonts( MYFONTS );
      if (  cfDlg.ShowDialog( this ) != DialogResult.Cancel ) {
        m_font = ( Font )cfDlg.Font.Clone();

        OptimizeFont( );
      }
    }

    private void LoadFontFile( )
    {
      String exlist = "";

      ofDlg.InitialDirectory = appSettings.FontDirPath;
      if ( ofDlg.ShowDialog( this ) != DialogResult.Cancel ) {
        if ( MYFONTS == null )
          MYFONTS = new PrivateFontCollection( );

        foreach ( String fn in ofDlg.FileNames ) {
          try {
            MYFONTS.AddFontFile( fn );
          }
          catch ( Exception ) {
            exlist += String.Format( "    {0}\n", Path.GetFileNameWithoutExtension( fn ) );
          }
        }
        if ( ! String.IsNullOrEmpty(exlist)) {
          MessageBox.Show( String.Format( "The font types of: \n{0}\n   are unfortunately not supported!", exlist ), "Add font file", MessageBoxButtons.OK );
        }
      }
    }


    private void LoadFontDirectory( )
    {
      String exlist = "";

      fbDlg.SelectedPath = appSettings.FontDirPath;
      if ( fbDlg.ShowDialog( this ) != DialogResult.Cancel ) {
        appSettings.FontDirPath = fbDlg.SelectedPath; appSettings.Save( );
        if ( MYFONTS == null )
          MYFONTS = new PrivateFontCollection( );

        foreach ( String fn in Directory.EnumerateFiles( fbDlg.SelectedPath, "*.ttf", SearchOption.AllDirectories  ) ) {
          try {
            MYFONTS.AddFontFile( fn );
          }
          catch ( Exception ) {
            exlist += String.Format( "    {0}\n", Path.GetFileNameWithoutExtension( fn ) );
          }
        }
        if ( !String.IsNullOrEmpty( exlist ) ) {
          MessageBox.Show( String.Format( "The font types of: \n{0}\n   are unfortunately not supported!", exlist ), "Add font file", MessageBoxButtons.OK );
        }
      }
    }


    private void SaveFontAs( )
    {
      if ( FO == null ) return;
      if ( m_TotalChars <= 0 ) return;

      if ( comFileFormat.SelectedItem == null ) return;
      if ( !m_FCchooser.ContainsKey( ( String )comFileFormat.SelectedItem ) ) return; // ERROR exit


      FC = m_FCchooser[( String )comFileFormat.SelectedItem]( FO ); // call the selected format instance factory
 //           Console.WriteLine(FC);
            String ret = "";
      FontOptimizer.WidthTarget widthTarget = FontOptimizer.WidthTarget.WT_None;
      if ( rbTrimMono.Checked )
        widthTarget = FontOptimizer.WidthTarget.WT_Mono;
      else if ( rbTrimMinimum.Checked )
        widthTarget = FontOptimizer.WidthTarget.WT_Minimum;

            Encoding win1251 = Encoding.GetEncoding(1251);
            int first = win1251.GetBytes(txFirstChar.Text)[0];
            var bytes = new byte[1];
            var chars = new char[1];
            bytes[0] = (byte)first;
            win1251.GetChars(bytes, 0, 1, chars, 0);
           // char character = (char)first;

            ret = FC.FontFile(first, m_TotalChars, widthTarget );
         //   ret = FC.FontFile(txFirstChar.Text[0], m_TotalChars, widthTarget);
           // Console.WriteLine(txFirstChar.Text[0]);
            //         Console.WriteLine(ret);

            String fName = "";
      fName = FC.FontNameCreated + ".h";

      sfDlg.FileName = fName;
      sfDlg.InitialDirectory = appSettings.SaveDirPath;
      if ( sfDlg.ShowDialog( this ) != DialogResult.Cancel ) {
        using ( TextWriter tw = new StreamWriter( sfDlg.FileName, false ) ) {
          tw.Write( ret );
          FO.MakeThumbnail( sfDlg.FileName );
          txFontName.Text = String.Format( "File: {0} created, code size is {1} bytes", Path.GetFileName( sfDlg.FileName ), FC.CodeSize );
          appSettings.SaveDirPath = Path.GetDirectoryName( sfDlg.FileName ); appSettings.Save( );
        }
      }
    }



    private void Dirty( )
    {
      lblDirty.Visible = true;
    }

    private void Clean( )
    {
      lblDirty.Visible = false;
    }


    private void btToTest_Click( object sender, EventArgs e )
    {

           

            if ( m_TotalChars <= 0 ) return; // TODO complain..
      // create string
   //   String testString = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            String testString = "";

            Encoding win1251 = Encoding.GetEncoding("Windows-1251");
            byte[] bytes4 = win1251.GetBytes("я");
            //int lastChar = bytes4[0];
      //      int lastChar =  254;
            //    int lastChar = Convert.ToChar( Convert.ToByte(txFirstChar.Text[0]) + m_TotalChars -  1);
            //     for ( Char c = txFirstChar.Text[0]; c <= lastChar; c++ ) {
            //        testString += c;
            //      }
            string display = string.Empty;

            var enc = Encoding.GetEncoding(1251);
          int first =  enc.GetBytes(txFirstChar.Text)[0];
          //  Console.Write(first);
            int lastnum = first + m_TotalChars-1;
          //  Console.WriteLine("total");
        //    Console.WriteLine("total: "+m_TotalChars);
           // Console.WriteLine("last");
        //    Console.WriteLine("last: "+lastnum);
            for (int c = first; c <= lastnum; c++)
            {                
                int intValue = c;

                byte[] bytes = new byte[4];

             
                bytes[3] = (byte)intValue;

                display += win1251.GetString(new byte[] { bytes[3] });
                 

          //      Console.Write(c.ToString().PadRight(10));
          //      Console.Write(display.PadRight(10));
           //     Console.Write(c.ToString("X2"));
          //      Console.WriteLine();

         //       var enc = Encoding.GetEncoding(1251);
          //      Console.WriteLine(enc.GetBytes("Ы")[0]); //will print 219
          //      Console.WriteLine(enc.GetString(new byte[] { 219 })); //will pring Ы

                
            }

            testString = display;
        //    Console.WriteLine("new");
         //   Console.WriteLine(testString);

            /*
            Encoding cp437 = Encoding.GetEncoding("Windows-1251");
            byte[] firstbyte = win1251.GetBytes(" ");
            byte[] lastbyte = win1251.GetBytes("я");

            Encoding win12513 = Encoding.GetEncoding("Windows-1251");
            string text = testString;
            byte[] bytes5 = win12513.GetBytes(testString);
            for (int i = 0; i < bytes5.Length; i++)
            {
                Console.WriteLine("text");
                Console.WriteLine($"{text[i]} = {bytes5[i]}");
            }

            */

            /*
            string b = testString;
            Console.WriteLine(b);
            Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding win12511 = Encoding.GetEncoding("Windows-1251");
            byte[] uniByte = utf8.GetBytes(b);
            Console.WriteLine(ToReadableByteArray(uniByte));
            byte[] win1251Bytes = Encoding.Convert(utf8, win12511, uniByte);
            Console.WriteLine(ToReadableByteArray(win1251Bytes));
            testString = win12511.GetString(win1251Bytes);


            Console.WriteLine(win12511.GetString(win1251Bytes));

            Console.WriteLine("yes");
            Console.WriteLine(testString);
            */
            txMyText.Text = testString;


       //     txMyText.Text = UTF8ToWin1251(txMyText.Text);
            //  Console.WriteLine(testString);
      //      Console.WriteLine("in");
       //     Console.WriteLine(txMyText.Text);
          //  txMyText.Text = UTF8ToWin1251(txMyText.Text);
        }

    private void btClearTest_Click( object sender, EventArgs e )
    {
      txMyText.Text = "";
    }

    private void txMyText_TextChanged( object sender, EventArgs e )
    {
      InitText( );
    }

    private void txFirstChar_TextChanged( object sender, EventArgs e )
    {
      ValidateChars( );
      Dirty( );
    }

    private void txLastChar_TextChanged( object sender, EventArgs e )
    {
      ValidateChars( );
      Dirty( );
    }

      

        static public string ToReadableByteArray(byte[] bytes)
        {
            return string.Join(", ", bytes);
        }

        private static string GetCodePoint(char ch)
        {
            string retVal = "u+";
            byte[] bytes = Encoding.Unicode.GetBytes(ch.ToString());
            for (int ctr = bytes.Length - 1; ctr >= 0; ctr--)
                retVal += bytes[ctr].ToString("X2");

            return retVal;
        }


        public static string ToCodePointNotation(char c)
        {

            return "U+" + ((int)c).ToString("X4");
        }

        /// <summary>
        /// 
        /// </summary>
        private void ValidateChars( )
    {
      if ( txFirstChar.TextLength > 0 ) {
                Encoding win1251 = Encoding.GetEncoding("Windows-1251");
                txFirstCharASC.Text = String.Format("{0,2:D2}", win1251.GetBytes(txFirstChar.Text)[0]);
              //  txFirstCharASC.Text = String.Format( "{0,2:D2}", Convert.ToByte( txFirstChar.Text[0] ) );

        if ( txLastChar.TextLength > 0 ){
 

            //          Encoding win1251 = Encoding.GetEncoding("Windows-1251");
                    string text = txLastChar.Text;
                    byte[] bytes4 = win1251.GetBytes(text);
                    for (int i = 0; i < bytes4.Length; i++)
                    {
             //           Console.WriteLine($"{text[i]} = {bytes4[i]}");
                    }

                    txLastCharASC.Text = String.Format("{0,2:D2}", win1251.GetBytes(txLastChar.Text)[0]) ;
                //    Console.WriteLine(txLastCharASC.Text);
                    //   txLastCharASC.Text = String.Format("{0,2:D2}", Convert.ToByte( txLastChar.Text[0] ) );
                    //  Console.WriteLine("text origin ", txLastCharASC.Text);
                    //   Console.WriteLine(txLastChar.Text[0]);

                    if ( win1251.GetBytes(txLastChar.Text)[0] >= win1251.GetBytes(txFirstChar.Text)[0] )
                    {

                        txCharCount.Text = MakeTotalChars().ToString();

                 
                    }
/*
                    if ( Convert.ToByte( txLastChar.Text[0] ) >= Convert.ToByte( txFirstChar.Text[0] ) ) {
            txCharCount.Text = MakeTotalChars( ).ToString( );
          }
                    */
          else {
            txCharCount.Text = "-";
          }
        }
        else {
          txLastCharASC.Text = "-";
        }
      }
      else {
        txFirstCharASC.Text = "-";
      }


    }

    private int m_TotalChars = 0;

    private int MakeTotalChars( )
    {
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");
            string text = txLastChar.Text;
            byte[] bytes4 = win1251.GetBytes(text);
            if ( ( txFirstChar.TextLength > 0 ) && ( txLastChar.TextLength > 0 ) ) {
                //     m_TotalChars = Convert.ToByte( txLastChar.Text[0] );
                //      m_TotalChars = bytes4[0];
                m_TotalChars = win1251.GetBytes(txLastChar.Text)[0];
                m_TotalChars -= win1251.GetBytes(txFirstChar.Text)[0];

            //    m_TotalChars -= Convert.ToByte( txFirstChar.Text[0] );
                m_TotalChars++;
           //     Console.WriteLine(m_TotalChars);
            }
      else {
        m_TotalChars = 0;
      }
      return m_TotalChars;
    }

    private void hScrTargetHeight_ValueChanged( object sender, EventArgs e )
    {
      txTargetSize.Text = hScrTargetHeight.Value.ToString( );
      Dirty( );
    }


    #region Menu & Buttons

    private void btNewFont_Click( object sender, EventArgs e )
    {
      LoadFont( );
    }

    private void btSaveFontAs_Click( object sender, EventArgs e )
    {
      SaveFontAs( );
    }

    private void btOptimizeFont_Click( object sender, EventArgs e )
    {
      OptimizeFont( );
    }

    private void loadFontToolStripMenuItem_Click( object sender, EventArgs e )
    {
      LoadFont( );
    }

    private void addFontfilesToolStripMenuItem_Click( object sender, EventArgs e )
    {
      LoadFontFile( );
    }

    private void AddFontDirectorytoolStripMenuItem_Click( object sender, EventArgs e )
    {
      LoadFontDirectory( );
    }

    private void saveAsToolStripMenuItem_Click( object sender, EventArgs e )
    {
      SaveFontAs( );
    }

    private void exitToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Application.Exit( );
    }



        #endregion

        private void FontCreator_Load(object sender, EventArgs e)
        {

        }
    }
}
