using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App25.Models
{
    public class SkinItem
    {
        public string Name { get; set; }
        public ImageSource Image { get; set; }
        public string Code { get; set; }

        public SkinItem(string name, ImageSource image, string code)
        {
            Name = name;
            Image = image;
            Code = code;
        }
    }
}
