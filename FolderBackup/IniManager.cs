using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderBackup
{
    public class IniManager
    {
        // ファイル名
        private string filePath = string.Empty;

        private List<iniSection> sections = new List<iniSection>();

        #region "iniItem"

        internal class iniItem
        {
            /// <summary>
            /// キー
            /// </summary>
            public string key { get; set; }

            /// <summary>
            /// 値
            /// </summary>
            public string value { get; set; }

            /// <summary>
            /// コメント
            /// </summary>
            public string comment { get; set; }

            public iniItem(string line)
            {
                key = value = comment = string.Empty;

                // Tabを半角空白にする
                line = line.Replace("\t", " ");

                // keyとそれ以外を分ける
                var wk = line.Split('=');
                if(wk.Length > 1)
                {
                    // =の左側がキー
                    key = wk[0].Trim();
                    // valueとそれ以外を分ける
                    var wk2 = wk[1].Split("//");
                    if (wk2.Length > 1)
                    {
                        value = wk2[0].Trim();
                        comment = wk2[1].Trim();
                    }
                    else
                    {
                        value = wk[1].Trim();
                        comment = string.Empty;
                    }
                }
            }

            public iniItem(string key, string value, string comment = "")
            {
                this.key = key;
                this.value = value;
                this.comment = comment;
            }

            public string GetLine()
            {
                if(this.comment.Length > 0)
                {
                    return string.Format("{0} = {1}\t{2}",this.key,this.value,this.comment);
                }
                return string.Format("{0} = {1}", this.key, this.value);
            }

        }
        #endregion

        #region "iniSection"

        internal class iniSection
        {
            public string sectionName { get; set; }

            private List<iniItem> iniItems = new List<iniItem>();

            public iniSection(string sectinName)
            {
                this.sectionName = sectinName;
            }

            public void addItem(string line)
            {
                iniItem item = new iniItem(line);
                if(item.key.Length > 0)
                {
                    iniItems.Add(item);
                }
            }

            public void addItem(string key,string value, string comment = "")
            {
                iniItems.Add(new iniItem(key,value,comment));
            }

            public string GetValue(string key)
            {
                foreach(var item in iniItems){
                    if(item.key.Equals(key)) return item.value; 
                }
                return string.Empty;
            }

            public iniItem? GetItem(string key)
            {
                foreach (var item in iniItems)
                {
                    if (item.key.Equals(key)) return item;
                }
                return null;
            }

            public void SetValue(string key, string value)
            {
                foreach (var item in iniItems)
                {
                    if (item.key.Equals(key))
                    {
                        item.value = value;
                        return;
                    }
                }
                iniItems.Add(new iniItem(key, value));
            }

            public List<string> GetLines()
            { 
                var lines = new List<string>();
                foreach(var item in iniItems)
                {
                    lines.Add(item.GetLine());
                }
                return lines;
            }

        }

        #endregion

        // コンストラクタ
        public IniManager(string fileName)
        {
            // Path.GetFileNameでfileNameが返ってきたらフォルダ指定なし。
            if (Path.GetFileName(fileName).Equals(fileName))
            {
                // 実行ファイルと同じフォルダに作成する。
                filePath = Path.Combine(Path.GetDirectoryName( Application.ExecutablePath),fileName);
            } 
            else
            {
                filePath = fileName;
            }

            if(File.Exists(filePath))
            {
                using(var fs = new FileStream(filePath,FileMode.Open))
                {
                    StreamReader sr = new StreamReader(fs);
                    iniSection section = null;
                    
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        if(line == null) continue;
                        if (line.Trim().StartsWith(@"[")){
                            section = new iniSection(line.Trim().Substring(1, line.Trim().Length-2));
                            sections.Add(section);
                        }
                        else
                        {
                            if(section != null)
                            {
                                section.addItem(line.Trim());
                            }
                        }
                    }
                }
            }
        }

        public string GetValue(string sectionName, string key)
        {
            foreach(var section in sections)
            {
                if(section.sectionName.Equals(sectionName, StringComparison.OrdinalIgnoreCase))
                {
                    return section.GetValue(key);
                }
            }
            return string.Empty;
        }

        public void SetValue(string sectionName, string key, string value)
        {
            try
            {
                foreach (var section in sections)
                {
                    if (section.sectionName.Equals(sectionName, StringComparison.OrdinalIgnoreCase))
                    {
                        section.SetValue(key, value);
                        return;
                    }
                }

                var nsection = new iniSection(sectionName);
                nsection.addItem(key, value);
                sections.Add(nsection);
            }
            finally
            {
                save();
            }
        }

        public void SetValue(string sectionName, string key, bool value)
        {
            SetValue(sectionName, key, value.ToString());
        }

        public void SetValue(string sectionName, string key, decimal value)
        {
            SetValue(sectionName, key, value.ToString());
        }

        public List<string> GetSectionNames()
        {
            var ret = new List<string>();
            foreach(var section in sections)
            {
                ret.Add(section.sectionName);
            }
            return ret;
        }

        public void save()
        {
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                using(StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (var section in sections)
                    {
                        sw.WriteLine(string.Format("[{0}]", section.sectionName));
                        List<string> lines = section.GetLines();
                        foreach (var line in lines)
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
            }
        }

        public int GetValueInt(string sectionName, string key,int defaultValue = 0)
        {
            int ret;
            string value = GetValue(sectionName, key);
            if(!int.TryParse(value, out ret))
            {
                ret = defaultValue;
            }

            return ret;
        }

        public decimal GetValueDecimal(string sectionName, string key, decimal defaultValue = 0)
        {
            decimal ret;
            string value = GetValue(sectionName, key);
            if (!decimal.TryParse(value, out ret))
            {
                ret = defaultValue;
            }

            return ret;
        }

        public bool GetValueBool(string sectionName, string key, bool defaultValue = false)
        {
            bool ret;
            string value = GetValue(sectionName, key);
            if (!bool.TryParse(value, out ret))
            {
                ret = defaultValue;
            }

            return ret;
        }



    }
}
