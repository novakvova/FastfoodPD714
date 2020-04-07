using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid19Back.Helpers
{
    public static class ImageHelper
    {
        public static Bitmap FromBase64StringToImage(this string base64String)
        {
            //Encoding utf32 = Encoding.UTF32;
            //byte[] byteBuffer = utf32.GetBytes(base64String);
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            try
            {
                //return BitmapFactory.DecodeByteArray(byteBuffer, 0, byteBuffer.Length);
                using (MemoryStream memoryStream = new MemoryStream(byteBuffer))
                {
                    memoryStream.Position = 0;
                    using (Image imgReturn = Image.FromStream(memoryStream))
                    {
                        memoryStream.Close();
                        byteBuffer = null;
                        return new Bitmap(imgReturn);
                    }
                }
            }
            catch(Exception ex) { 
                string message = ex.Message; 
                return null; }

        }

    }
}
