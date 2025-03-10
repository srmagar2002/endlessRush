using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace App25
{
    public class PixelFont
    {
        public SKTypeface LoadCustomfont()
        {
            SKTypeface typeface = null;
            var assembly = typeof(PixelFont).Assembly;
            var resourceName = "App25.assets.font.PixelifySans.ttf";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    typeface = SKTypeface.FromStream(stream);
                }
            }
            return typeface ?? SKTypeface.Default;
        }

    }
}
