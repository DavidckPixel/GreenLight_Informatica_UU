﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;

namespace GreenLight
{
    public static class DrawData
    {
        static PrivateFontCollection Font_collection = new PrivateFontCollection(); //TEMP DIT MOET NOG GLOBAAL
        public static FontFamily Dosis_font_family;

        static DrawData()
        {
            Font_collection.AddFontFile("../../Fonts/Dosis-bold.ttf");
            Dosis_font_family = Font_collection.Families[0];
        }
    }
}
