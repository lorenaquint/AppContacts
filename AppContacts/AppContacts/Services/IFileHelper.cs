using System;
using System.Collections.Generic;
using System.Text;

namespace AppContacts.Services
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string fileName);
    }
}
