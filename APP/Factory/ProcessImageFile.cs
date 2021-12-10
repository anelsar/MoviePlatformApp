using System.IO;
using System.Web;

namespace APP.Factory
{
    public class ProcessImageFile : IProcessFile
    {
        public string SetPsyhicalPath(HttpPostedFileBase file, string fileName)
        {
            var psyhicalPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Images"), fileName);
            return psyhicalPath;
        }
        public void SaveFile(HttpPostedFileBase file, string psyhicalPath)
        {
            file.SaveAs(psyhicalPath);
        }     
        public string GetFileName(HttpPostedFileBase file)
        {
            return Path.GetFileName(file.FileName);
        }
    }
}