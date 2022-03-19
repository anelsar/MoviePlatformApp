using System;
using System.IO;
using System.Web;

namespace APP.Factory
{
    public interface IProcessFile
    {
        (string Name, string Extension) GetSafeFileName(HttpPostedFileBase file);
        string SetPsyhicalPath(HttpPostedFileBase file, string fileName);
        void SaveFile(HttpPostedFileBase file, string psyhicalPath);
        string HtmlEncodeFileName(string fileName);
        bool IsValidSignature(string fileExtension, Stream data);
    }
}
