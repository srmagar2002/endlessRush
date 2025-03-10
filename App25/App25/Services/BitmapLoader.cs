using App25.Views;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace App25.Services
{
    public class BitmapLoader
    {
        public SKBitmap LoadBitmapFromResource(string resourcePath, Type type)
        {
            var assembly = type.Assembly; // Get the current assembly
            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream != null)
                {
                    return SKBitmap.Decode(stream); // Decode the stream to SKBitmap
                }
                else
                {
                    Console.WriteLine($"Resource not found: {resourcePath}");
                    return null;
                }
            }
        }
    }
}
