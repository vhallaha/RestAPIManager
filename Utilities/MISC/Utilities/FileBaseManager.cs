using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace Utilities
{
    /// <summary>
    /// File Base Manager
    /// </summary>
    public class FileBaseManager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FileBaseManager()
        {
            //DEFAULT CONSTRUCTOR
            Errors = new List<Error>();
            FileUrls = new List<string>();
            ThumbUrls = new List<string>();
        }

        #region Save

        /// <summary>
        /// Saves file to the server.
        /// </summary>
        /// <param name="oFile">Posted FileBase</param>
        /// <param name="sUrl">Relative Save Path</param>
        /// <param name="bOverwrite">Overwrite</param>
        /// <returns>bool</returns>
        public bool SaveFile(HttpPostedFileBase oFile, string sUrl, bool bOverwrite)
        {
            try
            {
                bool bStatus = false;
                string sServerFileUrl = String.Empty;

                //Check if the posted filebase has file
                if (oFile != null)
                {
                    if (bOverwrite)
                    {
                        //Gets the filename of the file.
                        string sFileName = Path.GetFileName(oFile.FileName);

                        //Combines the url and the new filename.
                        string sPath = Path.Combine(HttpContext.Current.Server.MapPath(sUrl), sFileName);

                        //Save the file with a new filename.
                        if (File.Exists(sPath))
                            File.Delete(sPath);

                        oFile.SaveAs(sPath);

                        //Add a new url so the user can get the server url of the file.
                        sServerFileUrl = sUrl + "/" + oFile.FileName;
                        FileUrls.Add(sServerFileUrl);

                        bStatus = true;
                    }
                    else
                    {
                        //Gets the extension of the file.
                        string sExtension = Path.GetExtension(oFile.FileName);

                        //Combines the url and the new filename.
                        string sName = Guid.NewGuid().ToString("N") + sExtension;
                        string sPath = Path.Combine(HttpContext.Current.Server.MapPath(sUrl), sName);

                        //Save the file with a new filename.
                        oFile.SaveAs(sPath);

                        //Add a new url so the user can get the server url of the file.
                        sServerFileUrl = sUrl + "/" + sName;
                        FileUrls.Add(sServerFileUrl);

                        bStatus = true;
                    }
                }

                return bStatus;
            }
            catch (Exception ex)
            {
                Errors.Add(new Error
                {
                    Message = ex.Message,
                    Exception = ex
                });

                return false;
            }
        }

        /// <summary>
        /// Saves file to the server.
        /// </summary>
        /// <param name="oFile">Posted FileBase</param>
        /// <param name="sUrl">Relative Save Path</param>
        /// <param name="iSize">Image Size</param>
        /// <param name="bOverwrite">OverWrite</param>
        /// <param name="bThumb">Make it as a thumbnail</param>
        /// <returns>bool</returns>
        public bool SaveFile(HttpPostedFileBase oFile, string sUrl, int iSize, bool bOverwrite, bool bThumb, string sPrefix = "", string sSuffix = "")
        {
            try
            {
                bool sStatus = false;
                string sServerFileUrl = String.Empty;
                //Check if the filebase has content.
                if (oFile != null)
                {
                    //Create the image from filebase to Image Drawing.
                    var vImage = Image.FromStream(oFile.InputStream);

                    int iNewWidth;
                    int iNewHeight;

                    //Check if the Width is greater than its Height.
                    if (vImage.Width > vImage.Height)
                    {
                        iNewWidth = iSize;
                        iNewHeight = vImage.Height * iSize / vImage.Width;
                    }
                    else
                    {
                        iNewWidth = vImage.Width * iSize / vImage.Height;
                        iNewHeight = iSize;
                    }

                    //Start creating new bitmap with the new dimension.
                    var vImageBithmap = new Bitmap(iNewWidth, iNewHeight);

                    //Start creating graphic effect for smoothing the file.
                    var vImageGraph = Graphics.FromImage(vImageBithmap);
                    vImageGraph.CompositingQuality = CompositingQuality.HighQuality;
                    vImageGraph.SmoothingMode = SmoothingMode.HighQuality;

                    //Start creating Rectangle to contain the image.
                    var vImageRec = new Rectangle(0, 0, iNewWidth, iNewHeight);
                    vImageGraph.DrawImage(vImage, vImageRec);

                    //Save the image with the new Size.
                    string sFileUrl = String.Empty;
                    string sFileName = String.Empty;
                    if (bOverwrite)
                    {
                        sFileName = sPrefix + Path.GetFileNameWithoutExtension(oFile.FileName) + sSuffix + Path.GetExtension(oFile.FileName);
                        sFileUrl = Path.Combine(HttpContext.Current.Server.MapPath(sUrl), sFileName);
                        if (File.Exists(sFileUrl))
                            File.Delete(sFileUrl);

                        vImageBithmap.Save(sFileUrl, vImage.RawFormat);

                        //Add a new url so the user can get the server url of the file.
                        sServerFileUrl = sUrl + "/" + oFile.FileName;
                    }
                    else
                    {
                        sFileName = sPrefix + Guid.NewGuid().ToString("N") + sSuffix + Path.GetExtension(oFile.FileName);
                        sFileUrl = Path.Combine(HttpContext.Current.Server.MapPath(sUrl), sFileName);
                        vImageBithmap.Save(sFileUrl, vImage.RawFormat);

                        //Add a new url so the user can get the server url of the file.
                        sServerFileUrl = sUrl + "/" + sFileName;
                    }

                    if (bThumb)
                        ThumbUrls.Add(sServerFileUrl);
                    else
                        FileUrls.Add(sServerFileUrl);

                    //Dispose all used objects.
                    vImageGraph.Dispose();
                    vImageBithmap.Dispose();
                    vImage.Dispose();

                    sStatus = true;
                }

                return sStatus;
            }
            catch (Exception ex)
            {
                Errors.Add(new Error()
                {
                    Message = ex.Message,
                    Exception = ex
                });

                return false;
            }
        }

        /// <summary>
        /// Saves file to the server.
        /// </summary>
        /// <param name="oFile">Posted FileBase</param>
        /// <param name="sUrl">Relative Save Path</param>
        /// <param name="sPrefix">Prefix</param>
        /// <param name="sSuffix">Suffix</param>
        /// <param name="iSize">Image Size</param>
        /// <param name="bOverwrite">OverWrite</param>
        /// <param name="bThumb">Is Thumbnail</param>
        /// <returns>bool</returns>
        public bool SaveFile(HttpPostedFileBase oFile, string sUrl, string sPrefix, string sSuffix, int iSize, bool bOverwrite, bool bThumb)
        {
            try
            {
                bool sStatus = false;
                string sServerFileUrl = String.Empty;

                //Check if the filebase has content.
                if (oFile != null)
                {
                    //Create the image from filebase to Image Drawing.
                    var vImage = Image.FromStream(oFile.InputStream);

                    int iNewWidth;
                    int iNewHeight;

                    //Check if the Width is greater than its Height.
                    if (vImage.Width > vImage.Height)
                    {
                        iNewWidth = iSize;
                        iNewHeight = vImage.Height * iSize / vImage.Width;
                    }
                    else
                    {
                        iNewWidth = vImage.Width * iSize / vImage.Height;
                        iNewHeight = iSize;
                    }

                    //Start creating new bitmap with the new dimension.
                    var vImageBithmap = new Bitmap(iNewWidth, iNewHeight);

                    //Start creating graphic effect for smoothing the file.
                    var vImageGraph = Graphics.FromImage(vImageBithmap);
                    vImageGraph.CompositingQuality = CompositingQuality.HighQuality;
                    vImageGraph.SmoothingMode = SmoothingMode.HighQuality;

                    //Start creating Rectangle to contain the image.
                    var vImageRec = new Rectangle(0, 0, iNewWidth, iNewHeight);
                    vImageGraph.DrawImage(vImage, vImageRec);

                    //Save the image with the new Size.
                    string sFileUrl = String.Empty;
                    string sName = String.Empty;
                    if (bOverwrite)
                    {
                        sName = sPrefix + Path.GetFileNameWithoutExtension(oFile.FileName) + sSuffix + Path.GetExtension(oFile.FileName);
                        sFileUrl = Path.Combine(HttpContext.Current.Server.MapPath(sUrl), sName);
                        if (File.Exists(sFileUrl))
                            File.Delete(sFileUrl);

                        vImageBithmap.Save(sFileUrl, vImage.RawFormat);
                    }
                    else
                    {
                        sName = sPrefix + Guid.NewGuid().ToString("N") + sSuffix + Path.GetExtension(oFile.FileName);
                        sFileUrl = Path.Combine(HttpContext.Current.Server.MapPath(sUrl), sName);
                        vImageBithmap.Save(sFileUrl, vImage.RawFormat);
                    }

                    //Add a new url so the user can get the server url of the file.
                    sServerFileUrl = sUrl + "/" + sName;

                    if (bThumb)
                        ThumbUrls.Add(sServerFileUrl);
                    else
                        FileUrls.Add(sServerFileUrl);

                    //Dispose all used objects.
                    vImageGraph.Dispose();
                    vImageBithmap.Dispose();
                    vImage.Dispose();

                    sStatus = true;
                }

                return sStatus;
            }
            catch (Exception ex)
            {
                Errors.Add(new Error()
                {
                    Message = ex.Message,
                    Exception = ex
                });

                return false;
            }
        }

        /// <summary>
        /// Saves file to the server.
        /// </summary>
        /// <param name="oFile">Posted FileBase</param>
        /// <param name="sUrl">Relative Save Path</param>
        /// <param name="iWidth">Image Width Size</param>
        /// <param name="iHeight">Image Height Size</param>
        /// <param name="bOverwrite">Overwrite</param>
        /// <param name="bThumb">Make it as a thumbnail</param>
        /// <returns>bool</returns>
        public bool SaveFile(HttpPostedFileBase oFile, string sUrl, int iWidth, int iHeight, bool bOverwrite, bool bThumb, string sPrefix = "", string sSuffix = "")
        {
            try
            {
                bool sStatus = false;
                string sServerFileUrl = String.Empty;
                //Check if the filebase has content.
                if (oFile != null)
                {
                    //Create the image from filebase to Image Drawing.
                    var vImage = Image.FromStream(oFile.InputStream);

                    //Start creating new bitmap with the new dimension.
                    var vImageBithmap = new Bitmap(iWidth, iHeight);

                    //Start creating graphic effect for smoothing the file.
                    var vImageGraph = Graphics.FromImage(vImageBithmap);
                    vImageGraph.CompositingQuality = CompositingQuality.HighQuality;
                    vImageGraph.SmoothingMode = SmoothingMode.HighQuality;

                    //Start creating Rectangle to contain the image.
                    var vImageRec = new Rectangle(0, 0, iWidth, iHeight);
                    vImageGraph.DrawImage(vImage, vImageRec);

                    //Save the image with the new Size.
                    string sFileUrl = String.Empty;
                    string sFileName = String.Empty;
                    if (bOverwrite)
                    {
                        sFileName = sPrefix + Path.GetFileNameWithoutExtension(oFile.FileName) + sSuffix + Path.GetExtension(oFile.FileName);
                        sFileUrl = Path.Combine(HttpContext.Current.Server.MapPath(sUrl), sFileName);
                        if (File.Exists(sFileUrl))
                            File.Delete(sFileUrl);

                        vImageBithmap.Save(sFileUrl, vImage.RawFormat);
                    }
                    else
                    {
                        sFileName = sPrefix + Guid.NewGuid().ToString("N") + sSuffix + Path.GetExtension(oFile.FileName);
                        sFileUrl = Path.Combine(HttpContext.Current.Server.MapPath(sUrl), sFileName);
                        vImageBithmap.Save(sFileUrl, vImage.RawFormat);
                    }

                    //Add a new url so the user can get the server url of the file.
                    sServerFileUrl = sUrl + "/" + sFileName;

                    if (bThumb)
                        ThumbUrls.Add(sServerFileUrl);
                    else
                        FileUrls.Add(sServerFileUrl);

                    //Dispose all used objects.
                    vImageGraph.Dispose();
                    vImageBithmap.Dispose();
                    vImage.Dispose();

                    sStatus = true;
                }

                return sStatus;
            }
            catch (Exception ex)
            {
                Errors.Add(new Error()
                {
                    Message = ex.Message,
                    Exception = ex
                });

                return false;
            }
        }

        /// <summary>
        /// Saves file to the server.
        /// </summary>
        /// <param name="sDataString">Posted 64 Bit data string</param>
        /// <param name="sFileName">File Name</param>
        /// <param name="sUrl">Prefix</param>
        /// <param name="bOverwrite">OverWrite</param>
        /// <param name="sPrefix">Prefix</param>
        /// <param name="sSuffix">Suffix</param>
        /// <param name="iSize">Image Size</param>
        /// <param name="bThumb">Is Thumbnail</param>
        /// <returns>bool</returns>
        public bool SaveBase64File(string sDataString, string sFileName, string sUrl, bool bOverwrite, string sPrefix, string sSuffix, int iSize = 0, bool bThumb = false)
        {
            // copied from https://gist.github.com/vbfox/484643
            var base64Data = Regex.Match(sDataString, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);

            bool sStatus = false;
            string sServerFileUrl = String.Empty;
            using (var stream = new MemoryStream(binData))
            {
                var vImage = Image.FromStream(stream);

                int iNewWidth = vImage.Width;
                int iNewHeight = vImage.Height;

                if (iSize != 0)
                {
                    //Check if the Width is greater than its Height.
                    if (vImage.Width > vImage.Height)
                    {
                        iNewWidth = iSize;
                        iNewHeight = vImage.Height * iSize / vImage.Width;
                    }
                    else
                    {
                        iNewWidth = vImage.Width * iSize / vImage.Height;
                        iNewHeight = iSize;
                    }
                }

                //Start creating new bitmap with the new dimension.
                var vImageBithmap = new Bitmap(iNewWidth, iNewHeight);

                //Start creating graphic effect for smoothing the file.
                var vImageGraph = Graphics.FromImage(vImageBithmap);
                vImageGraph.CompositingQuality = CompositingQuality.HighQuality;
                vImageGraph.SmoothingMode = SmoothingMode.HighQuality;

                //Start creating Rectangle to contain the image.
                var vImageRec = new Rectangle(0, 0, iNewWidth, iNewHeight);
                vImageGraph.DrawImage(vImage, vImageRec);

                //Save the image with the new Size.
                string sFileUrl = String.Empty;
                string sNFileName = String.Empty;
                if (bOverwrite)
                {
                    sNFileName = sPrefix + Path.GetFileNameWithoutExtension(sFileName) + sSuffix + Path.GetExtension(sFileName);
                    sFileUrl = Path.Combine(HttpContext.Current.Server.MapPath(sUrl), sNFileName);
                    if (File.Exists(sFileUrl))
                        File.Delete(sFileUrl);

                    vImageBithmap.Save(sFileUrl, vImage.RawFormat);
                }
                else
                {
                    sNFileName = sPrefix + Guid.NewGuid().ToString("N") + sSuffix + Path.GetExtension(sFileName);
                    sFileUrl = Path.Combine(HttpContext.Current.Server.MapPath(sUrl), sNFileName);
                    vImageBithmap.Save(sFileUrl, vImage.RawFormat);
                }

                //Add a new url so the user can get the server url of the file.
                sServerFileUrl = sUrl + "/" + sNFileName;

                if (bThumb)
                    ThumbUrls.Add(sServerFileUrl);
                else
                    FileUrls.Add(sServerFileUrl);

                //Dispose all used objects.
                vImageGraph.Dispose();
                vImageBithmap.Dispose();
                vImage.Dispose();

                sStatus = true;
            }

            return sStatus;
        }

        /// <summary>
        /// Saves file to the server.
        /// </summary>
        /// <param name="sDataString">Posted 64 Bit data string</param>
        /// <param name="sFileName">File Name</param>
        /// <param name="sUrl">Prefix</param>
        /// <param name="bOverwrite">OverWrite</param>
        /// <param name="sPrefix">Prefix</param>
        /// <param name="sSuffix">Suffix</param>
        /// <param name="iWidth">Image Width Size</param>
        /// <param name="iHeight">Image Height Size</param>
        /// <param name="bThumb">Is Thumbnail</param>
        /// <returns>bool</returns>
        public bool SaveBase64File(string sDataString, string sFileName, string sUrl, bool bOverwrite, string sPrefix, string sSuffix, int iWidth = 0, int iHeight = 0, bool bThumb = false)
        {
            // copied from https://gist.github.com/vbfox/484643
            var base64Data = Regex.Match(sDataString, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);

            bool sStatus = false;
            string sServerFileUrl = String.Empty;
            using (var stream = new MemoryStream(binData))
            {
                var vImage = Image.FromStream(stream);

                int iNewWidth = vImage.Width;
                int iNewHeight = vImage.Height;

                //Start creating new bitmap with the new dimension.
                var vImageBithmap = new Bitmap(iWidth, iHeight);

                //Start creating graphic effect for smoothing the file.
                var vImageGraph = Graphics.FromImage(vImageBithmap);
                vImageGraph.CompositingQuality = CompositingQuality.HighQuality;
                vImageGraph.SmoothingMode = SmoothingMode.HighQuality;

                //Start creating Rectangle to contain the image.
                var vImageRec = new Rectangle(0, 0, iWidth, iHeight);
                vImageGraph.DrawImage(vImage, vImageRec);

                //Save the image with the new Size.
                string sFileUrl = String.Empty;
                string sNFileName = String.Empty;
                if (bOverwrite)
                {
                    sNFileName = sPrefix + Path.GetFileNameWithoutExtension(sFileName) + sSuffix + Path.GetExtension(sFileName);
                    sFileUrl = Path.Combine(HttpContext.Current.Server.MapPath(sUrl), sNFileName);
                    if (File.Exists(sFileUrl))
                        File.Delete(sFileUrl);

                    vImageBithmap.Save(sFileUrl, vImage.RawFormat);
                }
                else
                {
                    sNFileName = sPrefix + Guid.NewGuid().ToString("N") + sSuffix + Path.GetExtension(sFileName);
                    sFileUrl = Path.Combine(HttpContext.Current.Server.MapPath(sUrl), sNFileName);
                    vImageBithmap.Save(sFileUrl, vImage.RawFormat);
                }

                //Add a new url so the user can get the server url of the file.
                sServerFileUrl = sUrl + "/" + sNFileName;

                if (bThumb)
                    ThumbUrls.Add(sServerFileUrl);
                else
                    FileUrls.Add(sServerFileUrl);

                //Dispose all used objects.
                vImageGraph.Dispose();
                vImageBithmap.Dispose();
                vImage.Dispose();

                sStatus = true;
            }

            return sStatus;
        }

        #endregion Save

        #region Move

        /// <summary>
        /// Moves the file toa different location
        /// </summary>
        /// <param name="sFrom">Relative Path Source</param>
        /// <param name="sTo">Relative Path Destination</param>
        /// <returns>string</returns>
        public bool Move(string sFrom, string sTo)
        {
            try
            {
                sFrom = HttpContext.Current.Server.MapPath(sFrom);
                sTo = HttpContext.Current.Server.MapPath(sTo);
                Directory.Move(sFrom, sTo);

                return true;
            }
            catch (Exception ex)
            {
                Errors.Add(new Error()
                {
                    Message = ex.Message,
                    Exception = ex
                });

                return false;
            }
        }

        #endregion Move

        #region Delete

        public bool Delete(string sUrl, string sPrefix = "", string sSuffix = "")
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(sSuffix) || !String.IsNullOrWhiteSpace(sSuffix))
                {
                    string sFileName = sPrefix + Path.GetFileNameWithoutExtension(sUrl) + sSuffix + Path.GetExtension(sUrl);
                    string sPath = sUrl.Replace(Path.GetFileName(sUrl), sFileName);

                    sPath = HttpContext.Current.Server.MapPath(sPath);
                    if (File.Exists(sPath))
                        File.Delete(sPath);
                }

                sUrl = HttpContext.Current.Server.MapPath(sUrl);
                File.Delete(sUrl);

                return true;
            }
            catch (Exception ex)
            {
                Errors.Add(new Error()
                {
                    Message = ex.Message,
                    Exception = ex
                });

                return false;
            }
        }

        public bool DeleteFolder(string sUrl)
        {
            try
            {
                sUrl = HttpContext.Current.Server.MapPath(sUrl);
                DirectoryInfo oDir = new DirectoryInfo(sUrl);
                oDir.Delete(true);

                return true;
            }
            catch (Exception ex)
            {
                Errors.Add(new Error()
                {
                    Message = ex.Message,
                    Exception = ex
                });

                return false;
            }
        }

        #endregion Delete

        #region Create Folder

        /// <summary>
        /// Creates a new directory, also check if the directoy exist and rename the new folder accordingly
        /// </summary>
        /// <param name="sRootUrl">Root Url</param>
        /// <param name="sFolderName">Folder Name</param>
        /// <returns>string</returns>
        public string CreateFolder(string sRootUrl, string sFolderName)
        {
            try
            {
                //Declare variables and ge the actual Root Url path
                string sServerPath = HttpContext.Current.Server.MapPath(sRootUrl);
                string sName = CheckName(sRootUrl, sFolderName);
                string sPath = Path.Combine(sServerPath, sName);

                // Create a new Directory
                Directory.CreateDirectory(sPath);

                return sName;
            }
            catch (Exception ex)
            {
                Errors.Add(new Error()
                {
                    Message = ex.Message,
                    Exception = ex
                });

                return null;
            }
        }

        /// <summary>
        /// Creates a new directoy, allows the user to replace existing folder with the new folder
        /// </summary>
        /// <param name="sRootUrl">Root Url</param>
        /// <param name="sFolderName">Folder Name</param>
        /// <param name="bOverwrite">Overwrite</param>
        /// <returns>string</returns>
        public string CreateFolder(string sRootUrl, string sFolderName, bool bOverwrite)
        {
            try
            {
                //Combines the url and the new folder name.
                string sPath = Path.Combine(HttpContext.Current.Server.MapPath(sRootUrl), sFolderName);

                if (bOverwrite)
                {
                    if (Directory.Exists(sPath))
                        Directory.Delete(sPath);

                    Directory.CreateDirectory(sPath);
                    return sFolderName;
                }
                else
                {
                    if (!Directory.Exists(sPath))
                    {
                        Directory.CreateDirectory(sPath);
                        return sFolderName;
                    }
                    else
                    {
                        if (sRootUrl[sRootUrl.Length - 1] != '/')
                        {
                            return sRootUrl + "/" + sFolderName;
                        }
                        else
                        {
                            return sRootUrl + sFolderName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Add(new Error()
                {
                    Message = ex.Message,
                    Exception = ex
                });

                return null;
            }
        }

        #endregion Create Folder

        #region Replicate Folder

        /// <summary>
        /// Replicate a folder tree structure to a new folder
        /// </summary>
        /// <param name="sRootUrl">Root Url</param>
        /// <param name="sSourceFolderName">Source Folder Name</param>
        /// <param name="sNewFolderName">New Folder Name</param>
        /// <returns>string</returns>
        public string ReplicateFolderTree(string sRootUrl, string sSourceFolderName, string sNewFolderName)
        {
            try
            {
                sSourceFolderName = sSourceFolderName.ToLower();
                sNewFolderName = CheckName(sRootUrl, sNewFolderName);

                string sServerSourcePath = Path.Combine(HttpContext.Current.Server.MapPath(sRootUrl), sSourceFolderName);
                string sServerNewPath = Path.Combine(HttpContext.Current.Server.MapPath(sRootUrl), sNewFolderName);

                DirectoryInfo diInfo = new DirectoryInfo(sServerSourcePath);
                if (diInfo.Exists)
                {
                    DirectoryInfo diInfoDest = new DirectoryInfo(sServerNewPath);

                    if (!diInfoDest.Exists)
                        diInfoDest.Create();
                    diInfoDest = null;

                    // Copy folder structure
                    foreach (DirectoryInfo di in diInfo.GetDirectories("*", SearchOption.AllDirectories))
                    {
                        if (!System.IO.Directory.Exists(di.FullName.ToLower().Replace(sSourceFolderName, sNewFolderName)))
                            System.IO.Directory.CreateDirectory(di.FullName.ToLower().Replace(sSourceFolderName, sNewFolderName));
                    }
                }
                diInfo = null;

                return sNewFolderName;
            }
            catch (Exception ex)
            {
                Errors.Add(new Error()
                {
                    Message = ex.Message,
                    Exception = ex
                });

                return null;
            }
        }

        /// <summary>
        /// Replicate a folder tree structure to a new folder that also allows to includes all the files
        /// </summary>
        /// <param name="sRootUrl">Root Url</param>
        /// <param name="sSourceFolderName">Source Folder Name</param>
        /// <param name="sNewFolderName">New Folder Name</param>
        /// <param name="bIncludeFiles">Include all the files</param>
        /// <returns>string</returns>
        public string ReplicateFolderTree(string sRootUrl, string sSourceFolderName, string sNewFolderName, bool bIncludeFiles)
        {
            try
            {
                sSourceFolderName = sSourceFolderName.ToLower();
                sNewFolderName = CheckName(sRootUrl, sNewFolderName);

                string sServerSourcePath = Path.Combine(HttpContext.Current.Server.MapPath(sRootUrl), sSourceFolderName);
                string sServerNewPath = Path.Combine(HttpContext.Current.Server.MapPath(sRootUrl), sNewFolderName);

                DirectoryInfo diInfo = new DirectoryInfo(sServerSourcePath);
                if (diInfo.Exists)
                {
                    DirectoryInfo diInfoDest = new DirectoryInfo(sServerNewPath);

                    // Check if the folder exists exit the creation of the folder.
                    if (diInfoDest.Exists)
                        return null;

                    diInfoDest.Create();
                    diInfoDest = null;

                    // Copy folder structure
                    foreach (DirectoryInfo di in diInfo.GetDirectories("*", SearchOption.AllDirectories))
                    {
                        if (!Directory.Exists(di.FullName.ToLower().Replace(sSourceFolderName, sNewFolderName)))
                            Directory.CreateDirectory(di.FullName.ToLower().Replace(sSourceFolderName, sNewFolderName));
                    }

                    if (bIncludeFiles)
                    {
                        // Copy files in main and sub folders
                        foreach (string newPath in Directory.GetFiles(sServerSourcePath, "*.*", SearchOption.AllDirectories))
                        {
                            if (File.Exists(newPath.ToLower().Replace(sServerSourcePath, sServerNewPath)))
                                File.Delete(newPath.ToLower().Replace(sServerSourcePath, sServerNewPath));

                            File.Copy(newPath.ToLower(), newPath.ToLower().Replace(sServerSourcePath, sServerNewPath));
                        }
                    }
                }

                diInfo = null;

                return sNewFolderName;
            }
            catch (Exception ex)
            {
                Errors.Add(new Error
                {
                    Message = ex.Message,
                    Exception = ex
                });

                return null;
            }
        }

        /// <summary>
        /// Replicate a folder tree structure to a new folder that also allows to includes all the files and overwrite the folder
        /// </summary>
        /// <param name="sRootUrl">Root Url</param>
        /// <param name="sSourceFolderName">Source Folder Name</param>
        /// <param name="sNewFolderName">New Folder Name</param>
        /// <param name="bIncludeFiles">Include all the files</param>
        /// <param name="bOverwrite">Overwrite</param>
        /// <returns>string</returns>
        public string ReplicateFolderTree(string sRootUrl, string sSourceFolderName, string sNewFolderName, bool bIncludeFiles, bool bOverwrite)
        {
            try
            {
                sSourceFolderName = sSourceFolderName.ToLower();

                if (bOverwrite)
                    sNewFolderName = sNewFolderName.ToLower();
                else
                    sNewFolderName = CheckName(sRootUrl, sNewFolderName);

                string sServerSourcePath = Path.Combine(HttpContext.Current.Server.MapPath(sRootUrl), sSourceFolderName);
                string sServerNewPath = Path.Combine(HttpContext.Current.Server.MapPath(sRootUrl), sNewFolderName);

                DirectoryInfo diInfo = new DirectoryInfo(sServerSourcePath);
                if (diInfo.Exists)
                {
                    DirectoryInfo diInfoDest = new DirectoryInfo(sServerNewPath);

                    // Check if the folder exists delete the current folder
                    if (diInfoDest.Exists)
                        diInfoDest.Delete();

                    diInfoDest.Create();
                    diInfoDest = null;

                    // Copy folder structure
                    foreach (DirectoryInfo di in diInfo.GetDirectories("*", SearchOption.AllDirectories))
                    {
                        if (!Directory.Exists(di.FullName.ToLower().Replace(sSourceFolderName, sNewFolderName)))
                            Directory.CreateDirectory(di.FullName.ToLower().Replace(sSourceFolderName, sNewFolderName));
                    }

                    if (bIncludeFiles)
                    {
                        // Copy files in main and sub folders
                        foreach (string newPath in Directory.GetFiles(sServerSourcePath, "*.*", SearchOption.AllDirectories))
                        {
                            if (File.Exists(newPath.ToLower().Replace(sServerSourcePath, sServerNewPath)))
                                File.Delete(newPath.ToLower().Replace(sServerSourcePath, sServerNewPath));

                            File.Copy(newPath.ToLower(), newPath.ToLower().Replace(sServerSourcePath, sServerNewPath));
                        }
                    }
                }
                diInfo = null;

                return sNewFolderName;
            }
            catch (Exception ex)
            {
                Errors.Add(new Error()
                {
                    Message = ex.Message,
                    Exception = ex
                });

                return null;
            }
        }

        #endregion Replicate Folder

        #region Methods

        /// <summary>
        /// Check for the folder name of the new folder if exist and return a new folder name
        /// </summary>
        /// <param name="sRootUrl">Root Url</param>
        /// <param name="sFolderName">Folder Name</param>
        /// <returns>string</returns>
        internal static string CheckName(string sRootUrl, string sFolderName)
        {
            // Counter to change the folder name
            int iCounter = 0;
            bool bContinue = true;
            string sName = sFolderName;

            while (bContinue)
            {
                // Combine the Url
                string sUrl = Path.Combine(sRootUrl, sName);

                // Check if the folder already exist
                if (!Directory.Exists(sUrl))
                    bContinue = false; // end while loop
                else
                    // Append the count to the folder to make it unique
                    sName = sFolderName + '-' + iCounter.ToString();

                iCounter++;
            }

            return sName;
        }

        #endregion Methods

        /// <summary>
        /// List of errors occured when uploading the file.
        /// </summary>
        public List<Error> Errors { get; set; }

        /// <summary>
        /// Gives the url of the file uploaded
        /// to the server.
        /// </summary>
        public List<string> FileUrls { get; set; }

        /// <summary>
        /// Gives the url of the thumbnail file uploaded
        /// to the server.
        /// </summary>
        public List<string> ThumbUrls { get; set; }
    }
}