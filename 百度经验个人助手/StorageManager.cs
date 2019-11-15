﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Windows.Networking;
using Windows.Foundation;
using System.Text;
using Windows.Storage.Streams;

using System.IO;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.UI;

namespace 百度经验个人助手
{
    [XmlType(TypeName = "ContentExpEntry")]
    public class ContentExpEntry
    {
        public ContentExpEntry(string name, string url, int view, int vote, int collect, string date)
        {
            ExpName = StorageManager.RemoveInvalidXmlChars(name);
            View = view;
            Vote = vote;
            Collect = collect;
            Date = date;
            Url = url;
            ViewIncrease = -1;
        }

        public ContentExpEntry()
        {
            //do nothing.
            ViewIncrease = -1;
        }

        [XmlAttribute("ExpName")]
        public string ExpName { get; set; }
        [XmlElement("View")]
        public int View { get; set; }
        [XmlElement("Vote")]
        public int Vote { get; set; }
        [XmlElement("Collect")]
        public int Collect { get; set; }
        [XmlElement("Date")]
        public string Date { get; set; }
        [XmlElement("Url")]
        public string Url { get; set; }

        [XmlIgnore] public int ViewIncrease { get; set; }

        [XmlIgnore]
        public string ViewIncState
        {
            get
            {
                if (ViewIncrease == -1) return "无对应";
                if (ViewIncrease == 0) return "0";
                else return "↑ " + ViewIncrease;
            }
        }

        [XmlIgnore]
        public string ViewIncColor
        {
            get
            {
                if (ViewIncrease == -1) return Color.FromArgb(120, 150, 150, 150).ToString();
                if (ViewIncrease == 0) return "DarkOrange";
                else return "LimeGreen";
            }
        }


    }

    [XmlRoot("DataPack")]
    public class DataPack
    {
        #region 要序列化的字符串

        [XmlElement("mainUserName")]
        public string mainUserName;
        [XmlElement("mainIndexHuiXiang")]
        public string mainIndexHuiXiang;
        [XmlElement("mainIndexYiuZhi")]
        public string mainIndexYiuZhi;
        [XmlElement("mainIndexYuanChuang")]
        public string mainIndexYuanChuang;
        [XmlElement("mainIndexHuoYue")]
        public string mainIndexHuoYue;
        [XmlElement("mainIndexHuDong")]
        public string mainIndexHuDong;
        [XmlElement("mainExpCount")]
        public string mainExpCount;
        [XmlElement("mainPortraitUrl")]
        public string mainPortraitUrl;
        #endregion

        #region 要序列化的整数
        [XmlElement("contentExpsCount")]
        public int contentExpsCount;
        [XmlElement("contentPagesCount")]
        public int contentPagesCount;
        [XmlElement("contentExpsViewSum")]
        public int contentExpsViewSum;
        [XmlElement("contentExpsVoteSum")]
        public int contentExpsVoteSum;
        [XmlElement("contentExpsCollectSum")]
        public int contentExpsCollectSum;
        [XmlElement("contentExpsView20")]
        public int contentExpsView20;
        #endregion

        [XmlElement("date")] public DateTime date;

        [XmlArray("contentExps")]
        public ObservableCollection<ContentExpEntry> contentExps;

        [XmlIgnore]
        public string MainUserNameDecoded
        {
            get
            {
                return Uri.UnescapeDataString(mainUserName);
            }
        }

        public void SafeSetUserName(string uname)
        {
            mainUserName = StorageManager.RemoveInvalidXmlChars(uname);
        }
    }


    [XmlRoot("Settings")]
    public class Settings
    {
        public Settings()
        {
            isFirstIn = true;
            version = "0";
        }

        [XmlElement("Version")] public string version;
        [XmlElement("isFirstIn")] public bool isFirstIn;
    }

    [XmlRoot("EditSettings")]
    public class EditSettings
    {
        public EditSettings()
        {
            //TODO
            strTitle2Brief = "本经验介绍在\\3开发中，如何\\4。示例：标题是\\0。应用场景如：";
            strTitle2Tool = "极品飞车17=电脑\n极品飞车17=极品飞车17最高通缉\nPython=PyCharm\n关键词=工具";
            strAttention = "如果遇到问题，可以在下面提出疑问。";
            strTitle2Category = "win=2 1 5\nPhotoshop=2 1 5\n狗=4 1\n\n=2 1 5 默认分类";
            strSteps = "<步骤1>\n这里是步骤1的内容\n</步骤1>\n<步骤2>\n这里是步骤2，\n后边还可以添加步骤3\n</步骤2>";
            ifSteps = false;
            ifCheckOrigin = true;
            ifAddStep = true;
            addStepCount = 3;

        }

        [XmlIgnore] public string strTitle2Brief;
        [XmlIgnore] public string strTitle2Tool;
        [XmlIgnore] public string strAttention;
        [XmlIgnore] public string strTitle2Category;
        [XmlIgnore] public string strSteps;

        [XmlElement("StoreStrTitle2Brief")]
        public string StoreStrTitle2Brief
        {
            get { return Utility.Transferred(strTitle2Brief); }
            set { strTitle2Brief = Utility.DecodeTransferred(value); }
        }

        [XmlElement("StoreStrTitle2Tool")]
        public string StoreStrTitle2Tool
        {
            get { return Utility.Transferred(strTitle2Tool); }
            set { strTitle2Tool = Utility.DecodeTransferred(value); }
        }

        [XmlElement("StoreStrAttention")]
        public string StoreStrAttention
        {
            get { return Utility.Transferred(strAttention); }
            set { strAttention = Utility.DecodeTransferred(value); }
        }

        [XmlElement("StoreStrSteps")]
        public string StoreStrSteps
        {
            get { return Utility.Transferred(strSteps); }
            set { strSteps = Utility.DecodeTransferred(value); }
        }


        [XmlElement("StoreStrTitle2Category")]
        public string StoreStrTitle2Category
        {
            get { return Utility.Transferred(strTitle2Category); }
            set { strTitle2Category = Utility.DecodeTransferred(value); }
        }

        [XmlElement("ifSteps")] public bool ifSteps;
        [XmlElement("ifCheckOrigin")] public bool ifCheckOrigin;
        [XmlElement("ifAddStep")] public bool ifAddStep;
        [XmlElement("addStepCount")] public int addStepCount;


    }


    [XmlType(TypeName = "DIYTool")]
    public class DIYTool
    {
        public DIYTool(string name, string targetUrl, string trigType, string note, string code)
        {
            Name = name;
            TargetUrl = targetUrl;
            TrigType = trigType;
            Note = note;
            Code = code;
            IsActivate = false;
        }

        public DIYTool()
        {
            Name = TargetUrl = "";
            TrigType = "click";
            Note = "";
            Code = "";
            IsActivate = false;
        }

        [XmlElement("Name")] public string Name { get; set; }
        [XmlElement("TargetUrl")] public string TargetUrl { get; set; }
        [XmlElement("TrigType")] public string TrigType { get; set; }
        [XmlElement("Note")] public string Note { get; set; }
        [XmlElement("Code")] public string Code { get; set; }

        [XmlIgnore]
        public bool IsActivate { get; set; }

        [XmlIgnore]
        public bool IsClickTrig
        {
            get
            {
                return TrigType == "click";
            }
        }

        [XmlIgnore]
        public string ShowTrigType
        {
            get
            {
                if (TrigType == "click") return "🖱";
                else if (IsActivate) return "🔗 激活中";
                else return "🔗";
            }
        }

        [XmlIgnore]
        public string ShowNote
        {
            get
            {
                if (Note.Trim() == "") return "(请编辑该功能的描述)";
                if (Note.Length < 40) return Note;
                else return Note.Substring(0, 35) + "...";
            }
        }

        [XmlIgnore]
        public string StateColor1
        {
            get
            {
                if (IsActivate) return "White";
                else return "Black";
            }
        }

        [XmlIgnore]
        public string ToolSymbol
        {
            get
            {
                if (IsClickTrig)
                {
                    return "TouchPointer";
                }
                else if (IsActivate)
                {
                    return "Pause";
                }
                else
                {
                    return "Play";
                }
            }
        }
    }

    [XmlRoot("DIYToolsSettings")]
    public class DIYToolsSettings
    {
        [XmlArray("DIYTools")]
        public ObservableCollection<DIYTool> DIYTools;

        public DIYToolsSettings()
        {
            DIYTools = new ObservableCollection<DIYTool>();
        }

        public void Init(bool allClear = false)
        {
            //准备自带的功能
            var tempTools = new ObservableCollection<DIYTool>();

            DIYTool dt1 = new DIYTool(
                "开宝箱",
                "https://jingyan.baidu.com/usersign",
                "navigate",
                "打开签到日历页面，激活此工具，会一直开宝箱，直到所有宝箱都开启.",
                "var openb = document.getElementById('openBoxBtn'); if(openb) openb.click();");

            DIYTool dt2 = new DIYTool(
                "开老虎机",
                "https://jingyan.baidu.com/user/nuc",
                "click",
                "（还没写）打开老虎机，点击此工具，会一直开老虎机直到开完.",
                "window.external.notify('NOTIFY: 开发者还没写这个功能 | 如果你已经写了，可以告知开发者 | WARN');\r\n//var zp = document.getElementsByClassName(\"zhuanpan\")[0];\r\n//var try10 = zp.getElementsByClassName(\"try10\")[0];\r\n//if(!try10.classList.contains(\"disable\") try10.click();");

            DIYTool dt3 = new DIYTool(
               "查看未读消息-触发器",
               "https://jingyan.baidu.com/user/nuc",
               "click",
               "先激活 “查看未读消息” 功能，然后点击这个进入工作页面。",
               "window.external.notify('GOTO: https://jingyan.baidu.com/user/nucpage/message FROM: https://jingyan.baidu.com/user/nuc/');");

            DIYTool dt4 = new DIYTool(
                 "查看未读消息",
                 "https://jingyan.baidu.com/user/nucpage/message",
                 "navigate",
                 "先激活此功能，然后点击 查看未读消息-触发器",
                 "var cks = document.getElementsByClassName('msg-more-btn'); var tcount = 200; for(let ck of cks) {setTimeout(function(){ck.click()}, tcount); tcount += 200;}");

            tempTools.Add(dt1);
            tempTools.Add(dt2);
            tempTools.Add(dt3);
            tempTools.Add(dt4);

            if (allClear)
            {
                DIYTools.Clear();
                foreach(var tool in tempTools)
                {
                    DIYTools.Add(tool);
                }
            }
            else
            {
                var midTools = new ObservableCollection<DIYTool>();
                foreach (var utool in DIYTools)
                {
                    bool isDefault = false;
                    foreach (var tool in tempTools)
                    {
                        if (tool.Name == utool.Name) isDefault = true;
                    }
                    if (!isDefault) midTools.Add(utool);
                }

                DIYTools.Clear();
                foreach (var tool in tempTools)
                {
                    DIYTools.Add(tool);
                }
                foreach (var tool in midTools)
                {
                    DIYTools.Add(tool);
                }
            }
        }
    }

    public static class StorageManager
    {
        private static StorageFolder _storageFolder = ApplicationData.Current.LocalFolder;
        private static StorageFolder _currentUserFolder;
        private static StorageFolder _currentUserRecentFolder;

        public const string VER = "1.5.5";

        private static string _editSettingsFileName = "EditSettings.xml";
        private static string _settingsFileName = "Settings.xml";
        private static string _dIYToolsSettingsFileName = "DIYToolsSettingsV1.xml";

        public static StorageFolder StorageFolder
        {
            get { return _storageFolder; }
        }
        public static StorageFolder CurrentUserFolder
        {
            get { return _currentUserFolder; }
        }
        public static StorageFolder CurrentUserRecentFolder
        {
            get { return _currentUserRecentFolder; }
        }


        public static Settings appSettings;
        public static EditSettings editSettings;
        public static DIYToolsSettings dIYToolsSettings;


        private static Regex _invalidXmlChars = new Regex(
            @"(?<![\uD800-\uDBFF])[\uDC00-\uDFFF]|[\uD800-\uDBFF](?![\uDC00-\uDFFF])|[\x00-\x08\x0B\x0C\x0E-\x1F\x7F-\x9F\uFEFF\uFFFE\uFFFF]",
            RegexOptions.Compiled);

        /// <summary>
        /// 移除特殊的unicode字符
        /// </summary>
        public static string RemoveInvalidXmlChars(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return _invalidXmlChars.Replace(text, "");
        }
        private static string GetValidFileName(string input)
        {
            input = input.Replace("_", "_b");

            input = input.Replace("\\", "_s");
            input = input.Replace("/", "_d");
            input = input.Replace(":", "_c");
            input = input.Replace("*", "_x");
            input = input.Replace("?", "_q");
            input = input.Replace("\"", "_y");
            input = input.Replace("<", "_l");
            input = input.Replace(">", "_r");
            input = input.Replace("|", "_v");
            return input;
        }
        private static string GetFolderName(string id)
        {
            char[] forbiddens = { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };
            bool isIdValid = true;

            if (id == null)
            {
                Utility.ShowMessageDialog("奇异情况", "百度id不存在。请询问开发者（1223989563@qq.com）此问题。"
                    + "\n此时用户名：" + ExpManager.newMainUserName
                    + "\n此时经验数：" + ExpManager.newMainExpCount);
                return "临时用户";
            }

            return GetValidFileName("USER-" + id);

        }

        private static async Task _handleSerializeExceptions(Exception e)
        {
            await Utility.ShowMessageDialog("设置未保存。序列化Xml发生问题",

                    "错误类型：" + e.GetType() + "\n错误编码：" + string.Format("{0:X}", e.HResult) +
                    "\nXmlSerializer.Serialize函数出错，数据无法保存，看到此错误可截图发送给开发者。其他功能继续。" + e.Message);
            if (e.InnerException != null)
            {
                await Utility.ShowMessageDialog(
                    "e.InnerException", "信息：" + e.Message
                                        + "类型：" + e.GetType() + "\n调用栈："
                                        + e.InnerException.StackTrace);
            }
        }

        //不需要用户初始化。一开始就能用
        public static async Task<bool> ReadSettings()
        {
            appSettings = new Settings();

            XmlSerializer serializer =
                new XmlSerializer(typeof(Settings));
            StorageFile f;

            try
            {
                f = await _storageFolder.GetFileAsync(_settingsFileName);
            }
            catch (Exception e)
            {
                return false;
            }
            Stream fs = await f.OpenStreamForWriteAsync();
            XmlReader reader = XmlReader.Create(fs);
            Settings tempSets;
            try
            {
                tempSets = (Settings)serializer.Deserialize(reader);
            }
            catch (InvalidOperationException e)
            {
                await Utility.ShowMessageDialog("遇到格式错误的设置文件", "非关键问题，一切继续。看到此消息可截图给开发者以解决问题。\n设置文件：" + f.Name + "\n" + e.Message);
                reader.Dispose();
                fs.Dispose();
                return false;
            }

            reader.Dispose();
            fs.Dispose();
            appSettings = tempSets;
            return true;
        }

        public static async Task<bool> SaveSettings()
        {
            string filename = _settingsFileName;

            XmlSerializer serializer =
                new XmlSerializer(typeof(Settings));

            StorageFile file =
                await _storageFolder.CreateFileAsync(
                    filename,
                    CreationCollisionOption.ReplaceExisting
                );

            Stream fs = await file.OpenStreamForWriteAsync();

            try
            {
                serializer.Serialize(fs, appSettings);
            }
            catch (Exception e)
            {
                await _handleSerializeExceptions(e);
                fs.Dispose();
                return false;
            }
            fs.Dispose();
            return true;
        }

        public static async Task<bool> ReadEditSettings()
        {
            editSettings = new EditSettings();

            XmlSerializer serializer =
                new XmlSerializer(typeof(EditSettings));
            StorageFile f;

            try
            {
                f = await _storageFolder.GetFileAsync(_editSettingsFileName);
            }
            catch (Exception e)
            {
                return false;
            }
            Stream fs = await f.OpenStreamForWriteAsync();
            XmlReader reader = XmlReader.Create(fs);
            EditSettings tempSets;
            try
            {
                tempSets = (EditSettings)serializer.Deserialize(reader);
            }
            catch (InvalidOperationException e)
            {
                await Utility.ShowMessageDialog("遇到格式错误的设置文件", "非关键问题，一切继续。看到此消息可截图给开发者以解决问题。\n设置文件：" + f.Name + "\n" + e.Message);
                reader.Dispose();
                fs.Dispose();
                return false;
            }

            reader.Dispose();
            fs.Dispose();
            editSettings = tempSets;
            return true;
        }

        public static async Task<bool> SaveEditSettings()
        {
            string filename = _editSettingsFileName;

            XmlSerializer serializer =
                new XmlSerializer(typeof(EditSettings));

            StorageFile file =
                await _storageFolder.CreateFileAsync(
                    filename,
                    CreationCollisionOption.ReplaceExisting
                );

            Stream fs = await file.OpenStreamForWriteAsync();

            try
            {
                serializer.Serialize(fs, editSettings);
            }
            catch (Exception e)
            {
                await _handleSerializeExceptions(e);
                fs.Dispose();
                return false;
            }
            fs.Dispose();
            return true;
        }

        public static async Task<bool> ReadDIYToolsSettings()
        {
            dIYToolsSettings = new DIYToolsSettings();

            XmlSerializer serializer =
                new XmlSerializer(typeof(DIYToolsSettings));
            StorageFile f;

            try
            {
                f = await _storageFolder.GetFileAsync(_dIYToolsSettingsFileName);
            }
            catch (Exception e)
            {
                dIYToolsSettings.Init();
                return false;
            }
            Stream fs = await f.OpenStreamForWriteAsync();
            XmlReader reader = XmlReader.Create(fs);
            DIYToolsSettings tempSets;
            try
            {
                tempSets = (DIYToolsSettings)serializer.Deserialize(reader);
            }
            catch (InvalidOperationException e)
            {
                await Utility.ShowMessageDialog("遇到格式错误的设置文件", "非关键问题，一切继续。看到此消息可截图给开发者以解决问题。\n设置文件：" + f.Name + "\n" + e.Message);
                reader.Dispose();
                fs.Dispose();
                return false;
            }

            reader.Dispose();
            fs.Dispose();
            dIYToolsSettings = tempSets;
            return true;
        }

        public static async Task<bool> SaveDIYToolsSettings()
        {
            string filename = _dIYToolsSettingsFileName;

            XmlSerializer serializer =
                new XmlSerializer(typeof(DIYToolsSettings));

            StorageFile file =
                await _storageFolder.CreateFileAsync(
                    filename,
                    CreationCollisionOption.ReplaceExisting
                );

            Stream fs = await file.OpenStreamForWriteAsync();

            try
            {
                serializer.Serialize(fs, dIYToolsSettings);
            }
            catch (Exception e)
            {
                await _handleSerializeExceptions(e);
                fs.Dispose();
                return false;
            }
            fs.Dispose();
            return true;
        }

        public static async Task<int> InitUserFolder(string id)
        {
            //Do nothing
            bool needCreateFolder = false;
            string folderName = GetFolderName(id);
            try
            {
                _currentUserFolder = await _storageFolder.GetFolderAsync(folderName);
            }
            catch (Exception e)
            {
                needCreateFolder = true;
            }

            if (needCreateFolder)
            {
                try
                {
                    _currentUserFolder = await _storageFolder.CreateFolderAsync(folderName);
                }
                catch (Exception e)
                {
                    await Utility.ShowMessageDialog("创建文件夹出错。可以截图发送给开发者", e.InnerException.ToString() + "\n" + e.StackTrace);
                    return 0;
                }
            }

            bool needCreateRecentFolder = false;
            string recentfolderName = "NewestData";
            try
            {
                _currentUserRecentFolder = await _currentUserFolder.GetFolderAsync(recentfolderName);
            }
            catch (Exception e)
            {
                needCreateRecentFolder = true;
            }

            if (needCreateRecentFolder)
            {
                try
                {
                    _currentUserRecentFolder = await _currentUserFolder.CreateFolderAsync(recentfolderName);
                }
                catch (Exception e)
                {
                    await Utility.ShowMessageDialog("创建最新文件夹出错。可以截图发送给开发者", e.InnerException.ToString() + "\n" + e.StackTrace);
                    return 0;
                }
            }

            return 0;
            //int updateCount = 0;
            //try
            //{
            //    updateCount = await UpdateSavedDataPacks();
            //}
            //catch (Exception e)
            //{
            //    return -1;
            //}
            //return updateCount;

        }

        public static async Task<string> GetCookieTry()
        {
            Stream fs;
            string storedCookie;
            try
            {
                fs = await _storageFolder.OpenStreamForReadAsync("Cookie.txt") as Stream;
                StreamReader sw = new StreamReader(fs);
                string cookie = await sw.ReadToEndAsync();
                storedCookie = cookie;
                sw.Dispose();
                fs.Dispose();
            }
            catch (Exception)
            {
                return null;
            }

            return storedCookie;
        }

        public static async Task SaveCookie(string cookie)
        {
            StorageFile file =
                await _storageFolder.CreateFileAsync("Cookie.txt", CreationCollisionOption.ReplaceExisting);

            Stream fs = await file.OpenStreamForWriteAsync();
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(cookie);

            sw.Dispose();
            fs?.Dispose();
        }

        public static async Task SaveDataPack(DataPack dp)
        {
            //TODO
            string filename = string.Format("DailyDataVer2_{0:d}.xml", dp.date).Replace("/", "-");

            if (_currentUserFolder == null)
            {
                await Utility.ShowMessageDialog("目录不存在，无法保存数据", "要解决此问题请将相关信息提供给开发者。");
                return;
            }
            StorageFile file =
                await _currentUserFolder.CreateFileAsync(
                    filename,
                    CreationCollisionOption.ReplaceExisting
                );
            StorageFile file2 =
                await _currentUserRecentFolder.CreateFileAsync(
                    "newest.xml",
                    CreationCollisionOption.ReplaceExisting
                );

            XmlSerializer serializer =
                new XmlSerializer(typeof(DataPack));


            Stream fs = await file.OpenStreamForWriteAsync();
            Stream fs2 = await file2.OpenStreamForWriteAsync();

            try
            {
                serializer.Serialize(fs, dp);
                serializer.Serialize(fs2, dp);
            }
            catch (Exception e)
            {
                await Utility.ShowMessageDialog("序列化Xml发生问题（System.Xml.Serialization）",

                    "错误类型：" + e.GetType() + "\n错误编码：" + string.Format("{0:X}", e.HResult) +
                    "\nXmlSerializer.Serialize函数出错，数据无法保存，看到此错误可截图发送给开发者。其他功能继续。" + e.Message);
                if (e.InnerException != null)
                {
                    await Utility.ShowMessageDialog(
                        "e.InnerException", "信息：" + e.Message
                        + "类型：" + e.GetType() + "\n调用栈："
                        + e.InnerException.StackTrace);
                }
            }
            //mys.Dispose();
            fs.Dispose();
            fs2.Dispose();
        }


        public static async Task<DataPack> ReadRecentDataPack()
        {
            XmlSerializer serializer =
                new XmlSerializer(typeof(DataPack));
            StorageFile f;

            try
            {
                f = await _currentUserRecentFolder.GetFileAsync("newest.xml");
            }
            catch (Exception e)
            {
                return null;
            }
            Stream fs = await f.OpenStreamForWriteAsync();
            XmlReader reader = XmlReader.Create(fs);
            DataPack tempDp;
            try
            {
                tempDp = (DataPack)serializer.Deserialize(reader);
            }
            catch (InvalidOperationException e)
            {
                await Utility.ShowMessageDialog("遇到格式错误的数据文件", "非关键问题，一切继续。看到此消息可截图给开发者以解决问题。\n文件名是：" + f.Name + "\n" + e.Message);
                reader.Dispose();
                fs.Dispose();
                return null;
            }

            reader.Dispose();
            fs.Dispose();
            return tempDp;


        }

        public static async Task<IReadOnlyList<StorageFile>> GetDataPackFiles()
        {
            return await _currentUserFolder.GetFilesAsync();
        }


        public static async Task<DataPack> ReadHistoryDataPackSingle(StorageFile file)
        {
            ObservableCollection<StorageFile> files = new ObservableCollection<StorageFile>();
            files.Add(file);
            ObservableCollection<DataPack> tempDataPacks = await ReadHistoryDataPacks(files);
            return tempDataPacks[0];
        }

        //读出历史数据包返回 (数据分析调用)
        public static async Task<ObservableCollection<DataPack>> ReadHistoryDataPacks(ObservableCollection<StorageFile> files)
        {
            ObservableCollection<DataPack> tempDataPacks = new ObservableCollection<DataPack>();

            XmlSerializer serializer =
                new XmlSerializer(typeof(DataPack));



            foreach (StorageFile sf in files)
            {

                Stream fs = await sf.OpenStreamForWriteAsync();
                XmlReader reader = XmlReader.Create(fs);
                try
                {
                    DataPack tempDp = (DataPack)serializer.Deserialize(reader);
                    tempDataPacks.Add(tempDp);
                }
                catch (InvalidOperationException e)
                {
                    await Utility.ShowMessageDialog("遇到格式错误的数据文件",
                        "看到此消息可截图给开发者。\n文件名是：" + sf.Name + "\n" + e.Message);
                    throw e;
                }

                reader.Dispose();
                fs.Dispose();
            }

            if (tempDataPacks.Count == 0)
            {
                await Utility.ShowMessageDialog("无成功读取",
                    "没有成功读取的历史数据包");
                throw new InvalidDataException("没有成功读取的历史数据包");
            }
            return tempDataPacks;
        }



        #region AutoComplete

        public static async Task<StorageFolder> GetSubFolderAsync(StorageFolder sfd, string folderName)
        {
            StorageFolder fd = null;
            IReadOnlyList<StorageFolder> fds = await sfd.GetFoldersAsync();
            foreach (StorageFolder tfd in fds)
            {
                if (tfd.Name == folderName)
                {
                    fd = tfd;
                    break;
                }
            }
            if (fd == null)
            {
                fd = await sfd.CreateFolderAsync(folderName);
            }
            return fd;
        }

        public static StorageFile FindFile(IReadOnlyList<StorageFile> sfs, string fileName)
        {
            foreach (StorageFile sf in sfs)
            {
                if (sf.Name == fileName)
                {
                    return sf;
                }
            }
            return null;
        }

        public static async Task SaveAutoCompleteData(string filename, string data)
        {

            StorageFolder sf = await GetSubFolderAsync(StorageFolder, "AutoCompleteData");
            StorageFile f = await sf.CreateFileAsync(
                filename == "" ? "default.json" : filename,
                CreationCollisionOption.ReplaceExisting);
            Stream fs = await f.OpenStreamForWriteAsync();
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(data);
            sw.Dispose();
            fs?.Dispose();
        }

        public static async Task<string> ReadAutoCompleteData(string filename)
        {
            StorageFolder sf = await GetSubFolderAsync(StorageFolder, "AutoCompleteData");
            IReadOnlyList<StorageFile> sfs = await sf.GetFilesAsync();
            StorageFile f = FindFile(sfs, filename == "" ? "default.json" : filename);
            if (f == null) return "";
            Stream fs = await f.OpenStreamForReadAsync();
            StreamReader sw = new StreamReader(fs);
            string data = await sw.ReadToEndAsync();
            sw.Dispose();
            fs?.Dispose();
            return data;
        }

        #endregion


        /// <summary>
        /// 获取一个数据包的描述（时间）
        /// </summary>
        /// <param name="dp">数据包</param>
        /// <returns>描述字符串</returns>
        public static string GetDataPackDescribe(DataPack dp)
        {
            string when;
            if (DateTime.Today.Date - ExpManager.currentDataPack.date.Date == TimeSpan.FromDays(0))
                when = "今天";
            else if (DateTime.Today.Date - ExpManager.currentDataPack.date.Date == TimeSpan.FromDays(1))
                when = "昨天";
            else if (DateTime.Today.Date - ExpManager.currentDataPack.date.Date == TimeSpan.FromDays(2))
                when = "2天前";
            else if (DateTime.Today.Date - ExpManager.currentDataPack.date.Date == TimeSpan.FromDays(2))
                when = "3天前";
            else when = "很久以前";

            return when;
        }
    }
}


