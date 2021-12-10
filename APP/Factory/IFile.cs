using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Factory
{
    public interface IFile
    {
        string FileName { get; set; }
        string PsyhicalPath { get; set; }
        
    }
}
