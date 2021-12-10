using System.Web;

namespace APP.Factory
{
    public interface IProcessFile
    {
        string GetFileName(HttpPostedFileBase file);
        string SetPsyhicalPath(HttpPostedFileBase file, string fileName);
        void SaveFile(HttpPostedFileBase file, string psyhicalPath);       
    }
}
