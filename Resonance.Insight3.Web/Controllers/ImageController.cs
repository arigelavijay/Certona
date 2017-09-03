using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Resonance.Insight3.Web.Controllers
{
    public class ImageController : Controller
    {
        private const string DEFAULT_IMAGE = "~/Images/no-image.png";

        private int imgHeight = 0;
        private int imgWidth = 0;
        private int? imgMaxHeight;
        private int? imgMaxWidth;

        public void GetImage(string img, int? mxH, int? mxW)
        {
            imgMaxHeight = mxH;
            imgMaxWidth = mxW;
            var imgUrl = Server.UrlDecode(img);
            var imgExt = Path.GetExtension(imgUrl);
            Stream imgSource;

            if(imgUrl.Length > 0 && imgUrl.IndexOf("http", 1) > 0)
            {
                imgUrl = imgUrl.Substring(imgUrl.IndexOf("http", 1));
            }

            if (Uri.IsWellFormedUriString(imgUrl, UriKind.Absolute))
            {
                try
                {
                    var imgRequest = (HttpWebRequest) WebRequest.Create(imgUrl);
                    var imgResponse = imgRequest.GetResponse() as HttpWebResponse;
                    if(imgResponse != null && (imgResponse.StatusCode == HttpStatusCode.Accepted || imgResponse.StatusCode == HttpStatusCode.OK || imgResponse.StatusCode == HttpStatusCode.Continue))
                    {
                        imgSource = imgResponse.GetResponseStream();
                    }
                    else
                    {
                        imgSource = GetDefaultImage();
                        imgExt = ".png";
                    }
                }
                catch (Exception ex)
                {
                    imgSource = GetDefaultImage();
                    imgExt = ".png";
                }

                if (imgSource != null)
                {
                    GenerateThumbnailFromStream(imgSource, Response, imgExt);
                }
            }
            else
            {
                imgSource = GetDefaultImage();
                imgExt = ".png";
                GenerateThumbnailFromStream(imgSource, Response, imgExt);
            }
        }

        private void GenerateThumbnailFromStream(Stream imgStream, HttpResponseBase Response, string imgExt)
        {
            if (imgStream == null) return;
            var imgFull = Image.FromStream(imgStream);
            imgStream.Close();
            imgStream.Dispose();
            var curHeight = imgFull.Height;
            var curWidth = imgFull.Width;

            imgHeight = curHeight;
            imgWidth = curWidth;

            if (imgMaxHeight.HasValue || imgMaxWidth.HasValue)
            {
                if (curHeight > imgMaxHeight || curWidth > imgMaxWidth)
                {
                    if (imgMaxHeight.HasValue && imgMaxWidth.HasValue)
                    {
                        if (imgMaxHeight > imgMaxWidth)
                        {
                            imgHeight = (imgMaxWidth.Value * curHeight) / curWidth;
                            imgWidth = imgMaxWidth.Value;
                        }
                        else
                        {
                            imgWidth = (imgMaxHeight.Value * curWidth) / curHeight;
                            imgHeight = imgMaxHeight.Value;
                        }
                    }
                    else if (imgMaxHeight.HasValue)
                    {
                        imgWidth = (imgMaxHeight.Value * curWidth) / curHeight;
                        imgHeight = imgMaxHeight.Value;
                    }
                    else
                    {
                        imgHeight = (imgMaxWidth.Value * curHeight) / curWidth;
                        imgWidth = imgMaxWidth.Value;
                    }
                }
            }

            var img = GenerateThumbnail(imgFull, imgExt);

            Response.Clear();
            Response.ContentType = "image/" + (imgExt.Length >= 4 ? imgExt.Substring(1) : "png");
            Response.BinaryWrite(img);
            Response.End();
        }

        private byte[] GenerateThumbnail(Image imgSource, string ext)
        {
            if (imgSource != null)
            {
                var thumbnail = imgSource.GetThumbnailImage(imgWidth, imgHeight, ThumbnailCallback, IntPtr.Zero);
                var imgStream = new MemoryStream();
                ImageFormat fmt;
                switch (ext.ToLower())
                {
                    case ".bmp":
                        fmt = ImageFormat.Bmp;
                        break;
                    case ".emf":
                        fmt = ImageFormat.Emf;
                        break;
                    case ".exif":
                        fmt = ImageFormat.Exif;
                        break;
                    case ".gif":
                    case ".giff":
                        fmt = ImageFormat.Gif;
                        break;
                    case ".ico":
                        fmt = ImageFormat.Icon;
                        break;
                    case ".jpg":
                    case ".jpeg":
                        fmt = ImageFormat.Jpeg;
                        break;
                    case ".png":
                        fmt = ImageFormat.Png;
                        break;
                    case ".tiff":
                        fmt = ImageFormat.Tiff;
                        break;
                    case ".wmf":
                        fmt = ImageFormat.Wmf;
                        break;
                    default:
                        fmt = ImageFormat.Bmp;
                        break;
                }
                thumbnail.Save(imgStream, fmt);

                var imgContent = new byte[imgStream.Length];
                imgStream.Position = 0;
                imgStream.Read(imgContent, 0, (int)imgStream.Length);
                return imgContent;
            }

            return null;
        }

        private static bool ThumbnailCallback()
        {
            return true;
        }

        private Stream GetDefaultImage()
        {
            try
            {
                return System.IO.File.Open(Server.MapPath(DEFAULT_IMAGE), FileMode.Open, FileAccess.ReadWrite);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
