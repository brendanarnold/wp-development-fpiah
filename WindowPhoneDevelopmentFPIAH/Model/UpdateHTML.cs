using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Windows.Resources;
using System.IO;
using System.Collections.Generic;

namespace WindowPhoneDevelopmentFPIAH.Model
{
    public static class UpdateHtml
    {


        //public delegate void HtmlUpdatedHandler();
        public static event EventHandler HtmlUpdated = delegate { };

        //public delegate void HtmlUpdatingHandler();
        public static event EventHandler HtmlUpdating = delegate { };


        public static int StoredHtmlVersion
        {
            get {
                return 0;
                //return (int)IsolatedStorageSettings.ApplicationSettings["HtmlVersion"]; 
            }
            set {
                //IsolatedStorageSettings.ApplicationSettings["HtmlVersion"] = value; 
            }
        }

        public static int PackagedHtmlVersion
        {
            get
            {
                // Read in the version from a version file in the HTML folder
                StreamResourceInfo sri = Application.GetResourceStream(new Uri(AppConstants.HTML_VERSION_FILE, UriKind.Relative));
                using (StreamReader sr = new StreamReader(sri.Stream))
                {
                    string line = sr.ReadToEnd();
                    return Convert.ToInt32(line.Trim());
                }
            }
        }


        public static void PerformHTMLUpdate()
        {
            // Use equal to be safe (in case of database corruption), assume newer version always installed
            if (PackagedHtmlVersion == StoredHtmlVersion)
                return;

            // Raise the updating HTML event
            HtmlUpdating(null, EventArgs.Empty);

            // Clear the current files
            ClearDirectory(AppConstants.HTML_BASE_DIR);

            // Get filenames from manifest file
            List<string> fns = new List<string>();
            using (StreamReader sr = new StreamReader(Application.GetResourceStream(
                new Uri(AppConstants.HTML_MANIFEST_FILE, UriKind.Relative)).Stream))
            {
                string line;
                while (sr.Peek() >= 0)
                {
                    line = sr.ReadLine();
                    if (line.Trim() != String.Empty)
                    {
                        fns.Add(System.IO.Path.Combine(AppConstants.HTML_BASE_DIR, line));
                    }
                }
            }

            // Read into storage
            SaveFilesToIsoStore(fns.ToArray());

            // Raise the done event
            HtmlUpdated(null, EventArgs.Empty);

        }


        public static void SaveFilesToIsoStore(string[] fns)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                foreach (string fn in fns)
                {
                    StreamResourceInfo sri = Application.GetResourceStream(new Uri(fn, UriKind.Relative));
                    using (BinaryReader br = new BinaryReader(sri.Stream))
                    {
                        byte[] data = br.ReadBytes((int)sri.Stream.Length);
                        string[] pathElements = fn.Split(System.IO.Path.DirectorySeparatorChar);

                        // Create dir structure
                        string dir = String.Empty;
                        for (int i = 0; i < pathElements.Length - 1; i++)
                        {
                            dir = System.IO.Path.Combine(dir, pathElements[i]);
                            if (!isoStore.DirectoryExists(dir))
                            {
                                isoStore.CreateDirectory(dir);
                            }
                        }

                        using (BinaryWriter bw = new BinaryWriter(isoStore.CreateFile(fn)))
                        {
                            bw.Write(data);
                            bw.Close();
                        }
                    }
                }
            }
        }


        public static void ClearDirectory(string dirName)
        {
            IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();

            if (!isoFile.DirectoryExists(dirName)) return;

            string[] dirNames = isoFile.GetDirectoryNames(dirName + "\\*");
            string[] fileNames = isoFile.GetFileNames(dirName + "\\*");

            if (fileNames.Length > 0)
            {
                for (int i = 0; i < fileNames.Length; i++)
                {
                    isoFile.DeleteFile(System.IO.Path.Combine(dirName, fileNames[i]));
                }
            }

            if (dirNames.Length > 0)
            {
                for (int i = 0; i < dirNames.Length; i++)
                {
                    ClearDirectory(System.IO.Path.Combine(dirName, dirNames[i]));
                    isoFile.DeleteDirectory(System.IO.Path.Combine(dirName, dirNames[i]));
                }
            }
            
        }



    }
}
