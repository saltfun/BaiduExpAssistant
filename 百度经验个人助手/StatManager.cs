﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 百度经验个人助手
{
    public static class StatManager
    {

        private static ObservableCollection<DataPack> _dataPacksSelected;
        public static ObservableCollection<DataPack> DataPacksSelected //ObservableCollection支持排序
        {
            get { return _dataPacksSelected; }
            set
            {
                if (value != null && value.Count > 0)
                {
                    _dataPacksSelected = new ObservableCollection<DataPack>(value.OrderBy(t => t.date));
                    
                }   
            }
        }

        public static DataPack LastDateDataPack //BUG To Fix TODO:
        {
            get
            {
                if (_dataPacksSelected.Count > 0)
                    return _dataPacksSelected.Last();

                return null;
            }
        }

        //需要总共datapack计数和索引
        //需要构建主数据绘图数据
        //每篇经验采用什么方式呢？ExpName做键值（低耦合）
        //单篇浏览量趋势
        //总体趋势
        //统计信息是否可用标志位
        private static int _deltaExpsOneYearInc;
        public static int DeltaExpsOneYearInc
        {
            get { return _deltaExpsOneYearInc; }
        }

        private static double _deltaExpsRecentAverage;
        public static double DeltaExpsRecentAverage
        {
            get { return _deltaExpsRecentAverage; }
        }

        private static ObservableCollection<ContentExpEntry> _deltaExps;
        public static ObservableCollection<ContentExpEntry> DeltaExps
        {
            get { return _deltaExps; }
        }

        private static ObservableCollection<ContentExpEntry> _condensedExps;
        public static ObservableCollection<ContentExpEntry> CondensedExps
        {
            get { return _condensedExps; }
        }

        private static ObservableCollection<ContentExpEntry> _condensedDeltaExps;
        public static ObservableCollection<ContentExpEntry> CondensedDeltaExps
        {
            get { return _condensedDeltaExps; }
        }


        public static float _progress;

        public static float Progress
        {
            get { return _progress; }
        }

        public static void Init()
        {
            _deltaExps = new ObservableCollection<ContentExpEntry>();
            _condensedExps = new ObservableCollection<ContentExpEntry>();
            _condensedDeltaExps = new ObservableCollection<ContentExpEntry>();
        }

        public static async Task Calc(
            ObservableCollection<ContentExpEntry> expsOld,
            ObservableCollection<ContentExpEntry> expsNew)
        {
            _expsNew = expsNew;
            _expsOld = expsOld;
            CalcDeltaExpsDelegate d = CalcDeltaExps;
            await Task.Run(new Action(d));
        }

        private static DateTime Datestr2Date(string datestr)
        {
            string[] ymd = datestr.Split('-');
            int year = Convert.ToInt32(ymd[0]);
            int month = Convert.ToInt32(ymd[1]);
            int day = Convert.ToInt32(ymd[2]);
            var date = new DateTime(year, month, day);
            return date;
        }

        private static bool IsInOneYear(DateTime newDate, DateTime oldDate)
        {
            if ((newDate - oldDate).Days < 365) return true;
            return false;
        }

        public static void CalcDeltaExpsOneYearIncrease()
        {
            var nowDate = DateTime.Now;
            _deltaExpsOneYearInc = 0;
            foreach(ContentExpEntry ets in _deltaExps)
            {
                string datestr = ets.Date;
                var oldDate = Datestr2Date(datestr);
                if(IsInOneYear(nowDate, oldDate))
                {
                    _deltaExpsOneYearInc += ets.View;
                }
            }
        }

        public static void CalcDeltaExpsRecentAverage(int count)
        {
            int currentCount = 0;
            int sum = 0;
            foreach(ContentExpEntry ets in _deltaExps)
            {
                sum += ets.View;
                currentCount++;
                if (currentCount >= count) break;
            }
            if (currentCount == 0) _deltaExpsRecentAverage = 0;
            _deltaExpsRecentAverage = sum / (double)currentCount;
        }

        public static void CondenseExps(
            ObservableCollection<ContentExpEntry> expsNew)
        {
            int parts = 40;
            int newExpLength = expsNew.Count();
            _condensedExps.Clear();
            if(newExpLength < parts)
            {
                foreach (ContentExpEntry ets in expsNew)
                {
                    _condensedExps.Add(ets);
                }
            }
            else
            {
                int plen = newExpLength / parts;
                int currentp = 0;
                int totalCount = 0;
                int maxView = 0;
                string maxTitle = "";
                ContentExpEntry currentExpC = null;
                foreach (ContentExpEntry ets in expsNew)
                {
                    if (currentp == 0)
                    {
                        currentExpC = new ContentExpEntry("", "", 0, 0, 0, "");
                        maxView = ets.View;
                        maxTitle = ets.ExpName;
                        currentExpC.Date = ets.Date;
                    }

                    currentExpC.Vote += ets.Vote;
                    currentExpC.View += ets.View;
                    currentExpC.Collect += ets.Collect;

                    if (ets.View > maxView)
                    {
                        maxView = ets.View;
                        maxTitle = ets.ExpName;
                    }

                    currentp++;
                    totalCount++;

                    if (currentp == plen || totalCount == newExpLength)
                    {
                        currentExpC.Date = currentExpC.Date + "~" + ets.Date;
                        currentExpC.ExpName = currentExpC.Date + " " + currentp + "篇, 合计:";
                        _condensedExps.Add(currentExpC);
                        currentp = 0;
                    }
                }
            }

        }

        public static void CondenseDeltaExps()
        {
            int parts = 40;
            int newExpLength = DeltaExps.Count();
            _condensedDeltaExps.Clear();
            if (newExpLength < parts)
            {
                foreach (ContentExpEntry ets in DeltaExps)
                {
                    _condensedDeltaExps.Add(ets);
                }
            }
            else
            {
                int plen = newExpLength / parts;
                int currentp = 0;
                int totalCount = 0;
                int maxView = 0;
                string maxTitle = "";
                ContentExpEntry currentExpC = null;
                foreach (ContentExpEntry ets in DeltaExps)
                {
                    if (currentp == 0)
                    {
                        currentExpC = new ContentExpEntry("", "", 0, 0, 0, "");
                        maxView = ets.View;
                        maxTitle = ets.ExpName;
                        currentExpC.Date = ets.Date;
                    }

                    currentExpC.Vote += ets.Vote;
                    currentExpC.View += ets.View;
                    currentExpC.Collect += ets.Collect;

                    if (ets.View > maxView)
                    {
                        maxView = ets.View;
                        maxTitle = ets.ExpName;
                    }

                    currentp++;
                    totalCount++;

                    if (currentp == plen || totalCount == newExpLength)
                    {
                        currentExpC.Date = currentExpC.Date + "~" + ets.Date;
                        currentExpC.ExpName = currentExpC.Date + " " + currentp + "篇";
                        _condensedDeltaExps.Add(currentExpC);
                        currentp = 0;
                    }
                }
            }
        }

        private static ObservableCollection<ContentExpEntry> _expsOld;
        private static ObservableCollection<ContentExpEntry> _expsNew;
        private delegate void CalcDeltaExpsDelegate();

        private static void CalcDeltaExps()
        {
            _deltaExps.Clear();
            _progress = 0;
            float len = _expsOld.Count;
            int count = 0;

            string[] urlSeperator = { ".com/" };

            foreach (ContentExpEntry ets in _expsOld)   //对于历史数据中的每个经验条目
            {
                IEnumerable<ContentExpEntry> ic = _expsNew.Where(
                    t => t.Url.Split(urlSeperator, StringSplitOptions.None)[1] == ets.Url.Split(urlSeperator, StringSplitOptions.None)[1] 
                    && t.ExpName == ets.ExpName); //寻找经验名称相等的条目
                    //&& t.Date == ets.Date

                if (ic.Any())
                {
                    string shortName;
                    ContentExpEntry ft = ic.First();    //ft即所寻获的条目
                    if (ft.ExpName.Length > 10)
                        shortName = ft.ExpName.Substring(0, 10) + "...";
                    else shortName = ft.ExpName;
                    ContentExpEntry eet = new ContentExpEntry(shortName, "", ft.View - ets.View, ft.Vote - ets.Vote, 0, ets.Date);  //浏览量做差
                    _deltaExps.Add(eet);
                    ft.ViewIncrease = ft.View - ets.View; //浏览量做差，保存
                }
                count++;
                _progress = count / len;
            }
        }
    }
}
