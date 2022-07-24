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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Globalization;

namespace GLCD_FontCreator
{

  static class Program
  {

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main( )
    {

            /*
            // Creates and initializes the CultureInfo which uses the international sort.
            CultureInfo myCIintl = new CultureInfo("ru", false);

            // Creates and initializes the CultureInfo which uses the traditional sort.
            CultureInfo myCItrad = new CultureInfo(0x040A, false);

            // Displays the properties of each culture.
            Console.WriteLine("{0,-31}{1,-47}{2,-25}", "PROPERTY", "INTERNATIONAL", "TRADITIONAL");
            Console.WriteLine("{0,-31}{1,-47}{2,-25}", "CompareInfo", myCIintl.CompareInfo, myCItrad.CompareInfo);
            Console.WriteLine("{0,-31}{1,-47}{2,-25}", "DisplayName", myCIintl.DisplayName, myCItrad.DisplayName);
            Console.WriteLine("{0,-31}{1,-47}{2,-25}", "EnglishName", myCIintl.EnglishName, myCItrad.EnglishName);
            Console.WriteLine("{0,-31}{1,-47}{2,-25}", "IsNeutralCulture", myCIintl.IsNeutralCulture, myCItrad.IsNeutralCulture);
            Console.WriteLine("{0,-31}{1,-47}{2,-25}", "IsReadOnly", myCIintl.IsReadOnly, myCItrad.IsReadOnly);
            Console.WriteLine("{0,-31}{1,-47}{2,-25}", "LCID", myCIintl.LCID, myCItrad.LCID);
            Console.WriteLine("{0,-31}{1,-47}{2,-25}", "Name", myCIintl.Name, myCItrad.Name);
            Console.WriteLine("{0,-31}{1,-47}{2,-25}", "NativeName", myCIintl.NativeName, myCItrad.NativeName);
            Console.WriteLine("{0,-31}{1,-47}{2,-25}", "Parent", myCIintl.Parent, myCItrad.Parent);
            Console.WriteLine("{0,-31}{1,-47}{2,-25}", "TextInfo", myCIintl.TextInfo, myCItrad.TextInfo);
            Console.WriteLine("{0,-31}{1,-47}{2,-25}", "ThreeLetterISOLanguageName", myCIintl.ThreeLetterISOLanguageName, myCItrad.ThreeLetterISOLanguageName);
            Console.WriteLine("{0,-31}{1,-47}{2,-25}", "ThreeLetterWindowsLanguageName", myCIintl.ThreeLetterWindowsLanguageName, myCItrad.ThreeLetterWindowsLanguageName);
            Console.WriteLine("{0,-31}{1,-47}{2,-25}", "TwoLetterISOLanguageName", myCIintl.TwoLetterISOLanguageName, myCItrad.TwoLetterISOLanguageName);
            Console.WriteLine();

            // Compare two strings using myCIintl.
            Console.WriteLine("Comparing \"llegar\" and \"lugar\"");
            Console.WriteLine("   With myCIintl.CompareInfo.Compare: {0}", myCIintl.CompareInfo.Compare("llegar", "lugar"));
            Console.WriteLine("   With myCItrad.CompareInfo.Compare: {0}", myCItrad.CompareInfo.Compare("llegar", "lugar"));
            */
            Application.EnableVisualStyles( );
      Application.SetCompatibleTextRenderingDefault( false );
      Application.Run( new FontCreator( ) );
    }
  }
}
