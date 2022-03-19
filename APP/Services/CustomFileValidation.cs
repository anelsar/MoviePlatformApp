using nClam;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APP.Services
{
    public class CustomFileValidation : ValidationAttribute
    {
        private readonly string[] SupportedFiles = new[] { "jpeg", "png", "jpg" };
        
        public override bool IsValid(object value)
        { 
            //var clam = new ClamClient("localhost", 3310);
            var type = (HttpPostedFileBase)value;
            //var scanResult = await clam.SendAndScanFileAsync();
            var fileSize = type.ContentLength;
            var fileExt = System.IO.Path.GetExtension(type.FileName).Substring(1);
            if (!SupportedFiles.Contains(fileExt))
            {
                ErrorMessage = "Only jpeg, jpg and png files allowed";
                return false;
            }
            if(fileSize > 5242880)
            {
                ErrorMessage = "The size of the file needs to be less than 5 MB";
                return false;
            }
                return true;
        }
        
    }
}