using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPTools.Blazor.Services
{
    public class ImageService
    {

        public ImageService()
        {

        }

        public void exportImage(string html)
        {
            var converter = new CoreHtmlToImage.HtmlConverter();

            var bytes = converter.FromHtmlString(html);


        }
    }
}
