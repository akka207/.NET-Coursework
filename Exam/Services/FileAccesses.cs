using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Services
{
    public class FileAccesses
    {
        public static bool HasPathAccessable(string path)
        {
            DirectorySecurity sec = new DirectoryInfo(path).GetAccessControl();
            WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
            foreach (FileSystemAccessRule acr in sec.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)))
            {
                if (acr.IdentityReference.Value == currentIdentity.Name)
                {
                    if (acr.FileSystemRights == FileSystemRights.FullControl && acr.AccessControlType == AccessControlType.Allow)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void ChangePathAccessControlCurrentUser(string path, bool isFile)
        {
            WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();

            if (isFile)
            {
                var pathAccessControl = new FileInfo(path).GetAccessControl();
                pathAccessControl.AddAccessRule(new FileSystemAccessRule(currentIdentity.Name, FileSystemRights.FullControl, AccessControlType.Allow));
                new FileInfo(path).SetAccessControl(pathAccessControl);
            }
            else
            {
                var pathAccessControl = new DirectoryInfo(path).GetAccessControl();
                pathAccessControl.AddAccessRule(new FileSystemAccessRule(currentIdentity.Name, FileSystemRights.FullControl, AccessControlType.Allow));
                new DirectoryInfo(path).SetAccessControl(pathAccessControl);
            }
        }
    }
}
