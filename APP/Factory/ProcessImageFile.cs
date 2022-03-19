using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace APP.Factory
{
    public class ProcessImageFile : IProcessFile
    {
        private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
        {
            { "gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
            { "png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
            { "jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            { "jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                }
            },

        };

        private string RemoveNonLettersOrDigits(string input)
        {
            return new string((from c in input
                               where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c)
                               select c)
                               .ToArray());
        }


        public string SetPsyhicalPath(HttpPostedFileBase file, string fileName)
        {
            var psyhicalPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Images"), fileName);
            return psyhicalPath;
        }

        public void SaveFile(HttpPostedFileBase file, string psyhicalPath)
        {
            file.SaveAs(psyhicalPath);
        }     

        public (string Name, string Extension) GetSafeFileName(HttpPostedFileBase file)
        {
            if(file == null)
            {
                throw new ArgumentException("File is empty");
            }

            var separator = '.';
            var segments = file.FileName.Split(separator);
            string name, extension;

            if (segments?.Length > 1)
            {
                var subtractedLength = segments.Length - 1;
                string[] segmentsExceptLast = new string[subtractedLength];
                Array.Copy(segments, segmentsExceptLast, subtractedLength);
                name = segmentsExceptLast.Length == 1 ? segmentsExceptLast[0] : string.Join(separator.ToString(), segmentsExceptLast);
                extension = segments[subtractedLength];
            }
            else
            {
                name = file.FileName;
                extension = null;
            }

            var safeName = RemoveNonLettersOrDigits(name)?.ToLowerInvariant();
            var safeExtension = RemoveNonLettersOrDigits(extension)?.ToLowerInvariant();

            return (safeName, safeExtension);
        }
        public string HtmlEncodeFileName(string fileName)
        {
            
            var trustedFileName = WebUtility.HtmlEncode(fileName);
            return trustedFileName;
        }

        // checking the file signature
        public bool IsValidSignature(string fileExtension, Stream data)
        {
            MemoryStream destination = new MemoryStream();
            data.CopyTo(destination);
            destination.Position = 0;
            data.Position = 0;
            using (var reader = new BinaryReader(destination))
            {
                var signatures = _fileSignature[fileExtension]; // pada, filextenzija je
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));
                //data.Position = 0;
                return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
            }
        }

    }
}