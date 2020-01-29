using System;
using System.IO;

namespace DocxJsonConverter.Services
{
    public class FileValidator
    {
        private readonly string extension;

        public FileValidator(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                throw new ArgumentNullException("the extension parameter must be valid.");
            }

            // confirm there is a period at the beginning of this extension
            if (extension[0] != '.')
            {
                extension = "." + extension;
            }

            this.extension = extension;
        }

        public bool IsValid(string fullPath)
        {
            bool result = true;

            // confirm there is a path
            if (string.IsNullOrEmpty(fullPath) || string.IsNullOrEmpty(extension))
            {
                return false;
            }

            // Check to see if the is the correct type
            if (!fullPath.ToLowerInvariant().EndsWith(extension))
            {
                return false;
            }

            //verify that the file exists
            if (!File.Exists(fullPath))
            {
                return false;
            }

            return result;
        }
    }
}
