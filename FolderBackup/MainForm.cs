//using FolderBackup.Properties;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SevenZip;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using Microsoft.VisualBasic;

namespace FolderBackup
{
    public partial class MainForm : Form
    {
        // クラス変数、定数
        private const string AppTitle = "FolderBackup";         //  フォームタイトル
        private const string StartWatchdog = "監視開始";        //  待機中ボタン表示名
        private const string StopWatchdog = "監視中";           //  監視中ボタン表示名
        private DateTime SaveFolderStart = DateTime.MinValue;   //  バックアップ開始日時
        private DateTime LastBackupDt = DateTime.MinValue;      //  最終バックアップ日時
        private bool FileChangedFlag_Main = false;
        private bool FileChangedFlag_Sub = false;

        SevenZipCompressor? compressor = null;

        private const string defSetting = "Setting";

        private IniManager iniManager;

        /// <summary>
        /// 初期設定
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            iniManager = new IniManager("Setting.ini");

            var sections = iniManager.GetSectionNames();
            var sectionName = iniManager.GetValue(defSetting, "Section");

            foreach (var section in sections)
            {
                if (!section.Equals(defSetting))
                {
                    cmbSection.Items.Add(section);
                    if (sectionName.Length > 0 && section.Equals(sectionName))
                    {
                        cmbSection.SelectedIndex = cmbSection.Items.Count - 1;
                    }
                }
            }

            if (cmbSection.Items.Count > 0 && cmbSection.SelectedIndex < 0)
            {
                cmbSection.SelectedValue = cmbSection.Items[0];
                cmbSection.SelectedIndex = 0;
            }

            chkAutoStart.Checked = iniManager.GetValueBool(defSetting, "AutoStart");

            if (chkAutoStart.Checked)
            {
                StartBackup(false);
            }

            if (!string.IsNullOrEmpty(iniManager.GetValue(defSetting, "FormPosition")) && Control.ModifierKeys != Keys.Control)
            {
                var strFormPosition = iniManager.GetValue(defSetting, "FormPosition").Split(",");
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(int.Parse(strFormPosition[0].Trim()), int.Parse(strFormPosition[1].Trim()));
                this.Size = new Size(int.Parse(strFormPosition[2].Trim()), int.Parse(strFormPosition[3].Trim()));
            }

            chkUse7z_CheckedChanged(null, EventArgs.Empty);

            if (!string.IsNullOrEmpty(iniManager.GetValue(defSetting, "Intarval")))
            {
                int interval;
                if (int.TryParse(iniManager.GetValue(defSetting, "Intarval"), out interval))
                    tmrSave.Interval = interval;
            }

        }

        /// <summary>
        /// 監視フォルダの選択
        /// </summary>
        private void btnTargetFolder_Click(object sender, EventArgs e)
        {
            dlgSelectFolder.SelectedPath = lblTargetFolder.Text;
            if (dlgSelectFolder.ShowDialog() == DialogResult.OK)
            {
                lblTargetFolder.Text = dlgSelectFolder.SelectedPath;
                iniManager.SetValue(cmbSection.Text, "TargetFolder", dlgSelectFolder.SelectedPath);
            }
        }

        /// <summary>
        /// 保存フォルダの選択
        /// </summary>
        private void btnSaveFolder_Click(object sender, EventArgs e)
        {
            dlgSelectFolder.SelectedPath = lblSaveFolder.Text;
            if (dlgSelectFolder.ShowDialog() == DialogResult.OK)
            {
                lblSaveFolder.Text = dlgSelectFolder.SelectedPath;
                iniManager.SetValue(cmbSection.Text, "SaveFolder", dlgSelectFolder.SelectedPath);
            }
        }

        /// <summary>
        /// 監視開始/中止
        /// </summary>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text.Equals(StartWatchdog))
            {
                StartBackup(true);
            }
            else
            {
                fsWatcher.EnableRaisingEvents = false;
                if (!string.IsNullOrEmpty(lblTargetFolderSub.Text))
                {
                    fsWatcherSub.EnableRaisingEvents = false;
                }
                btnStart.Text = StartWatchdog;
                SaveFolderStart = DateTime.MinValue;
                btnSaveFolder.Enabled = true;
                btnTargetFolder.Enabled = true;
                btnTargetFolderSub.Enabled = true;
                cmbSection.Enabled = true;
                btnAddApplication.Enabled = true;
                SetSubTitle(string.Empty);
            }
        }

        /// <summary>
        /// バックアップ開始待ち
        /// </summary>
        private void tmrSave_Tick(object sender, EventArgs e)
        {
            if (SaveFolderStart.Equals(DateTime.MinValue))
            {
                tmrSave.Stop();
                return;
            }

            var nw = DateTime.Now;
            var lw = LastBackupDt.AddMinutes((double)numInterval.Value);

            if (SaveFolderStart < nw && lw < nw)
            {
                // BackupStart
                tmrSave.Stop();

                // フォルダの変化監視を中断する
                fsWatcher.EnableRaisingEvents = false;
                if (!string.IsNullOrEmpty(lblTargetFolderSub.Text))
                {
                    fsWatcherSub.EnableRaisingEvents = false;
                }
                SaveFolderStart = DateTime.MinValue;

                if (Directory.Exists(lblSaveFolder.Text) && FileChangedFlag_Main)
                {
                    SetSubTitle("保存中");
                    Application.DoEvents();
                    var tmp = Path.Combine(lblSaveFolder.Text, nw.ToString("yyyyMMdd_HHmmss"));

                    SaveFolder(lblTargetFolder.Text, lblSaveFolder.Text, tmp);

                    FileChangedFlag_Main = false;
                }

                if (!string.IsNullOrEmpty(lblTargetFolderSub.Text) && FileChangedFlag_Sub)
                {
                    if (Directory.Exists(lblSaveFolder.Text))
                    {
                        SetSubTitle("保存中");
                        Application.DoEvents();
                        var tmp = Path.Combine(lblSaveFolder.Text, nw.ToString("yyyyMMdd_HHmmss")) + "_sub";

                        SaveFolder(lblTargetFolderSub.Text, lblSaveFolder.Text, tmp);

                        FileChangedFlag_Sub = false;
                    }
                }

                // 最終保存日時を記録する
                LastBackupDt = nw;

                // フォルダの変化監視を再開する
                SetSubTitle("監視中");
                fsWatcher.EnableRaisingEvents = true;
                if (!string.IsNullOrEmpty(lblTargetFolderSub.Text))
                {
                    fsWatcherSub.EnableRaisingEvents = true;
                }
            }
            else
            {
                DateTime now = DateTime.Now;
                TimeSpan diff1 = SaveFolderStart - now;
                TimeSpan diff2 = LastBackupDt.AddMinutes((double)numInterval.Value) - now;

                SetSubTitle("監視中(保存待ち)" + string.Format("{0}秒 : {0}秒", diff1, diff2));
                // SetSubTitle("監視中(保存待ち)" + string.Format("{0}秒",(diff1 > diff2 ? diff1 : diff2).Seconds ));
            }
        }

        private void SaveFolder(string targetPath, string folderPath, string archivePath)
        {
            // 圧縮中は別のファイル圧縮は出来ない
            if (compressor != null)
            {
                return;
            }

            try
            {
                if (chkUse7z.Checked)
                {
                    // 7z.dllが見つかった場合は7z形式で圧縮保存する
                    compressor = new SevenZipCompressor();
                    compressor.CompressionFinished += Compressor_CompressionFinished;
                    compressor.CompressDirectoryAsync(targetPath, archivePath + ".7z");
                    while (compressor != null)
                    {
                        Application.DoEvents();
                        Thread.Sleep(500);
                    }
                }
                else
                {
                    // 7z.dllが見つからなかった場合はフォルダをコピーする
                    CopyDirectory(targetPath, archivePath);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (MessageBox.Show("バックアップに失敗しました。\r\nもう一度バックアップしますか？", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    tmrSave.Start();
                    SaveFolderStart = DateTime.Now;
                }
                else
                {
                    return;
                }
            }
        }

        private void Compressor_CompressionFinished(object? sender, EventArgs e)
        {
            compressor = null;
        }

        /// <summary>
        /// 設定に問題がなければバックアップを開始する
        /// </summary>
        /// <param name="swDialog">設定に問題があった場合はメッセージボックスを表示する</param>
        private void StartBackup(bool swDialog)
        {
            // テキストボックスの内容を取得する
            var txtTargetFolder = lblTargetFolder.Text.Trim();
            var txtSaveFolder = lblSaveFolder.Text.Trim();

            // 設定のエラーチェック
            if (string.IsNullOrEmpty(txtTargetFolder))
            {
                if (swDialog)
                {
                    MessageBox.Show("TargetFolderを設定してください。");
                }
                return;
            }
            if (!Directory.Exists(txtTargetFolder))
            {
                if (swDialog)
                {
                    MessageBox.Show(string.Format("[{0}]が存在しません。", txtTargetFolder));
                }
                return;
            }
            if (txtSaveFolder.Length == 0)
            {
                if (swDialog)
                {
                    MessageBox.Show("SaveFolderを設定してください。");
                }
                return;
            }
            if (!Directory.Exists(txtSaveFolder))
            {
                if (swDialog)
                {
                    MessageBox.Show(string.Format("[{0}]が存在しません。", txtSaveFolder));
                }
                return;
            }
            // バックアップ先が監視対象フォルダ内の場合、延々とコピーしファイルが増殖するのでエラーとする
            if (txtSaveFolder.Length >= txtTargetFolder.Length)
            {
                if (txtSaveFolder.Substring(0, txtTargetFolder.Length).Equals(txtTargetFolder, StringComparison.OrdinalIgnoreCase))
                {
                    if (txtSaveFolder.Length > txtTargetFolder.Length && txtSaveFolder.Substring(txtTargetFolder.Length, 1).Equals("\\"))
                    {
                        if (swDialog)
                        {
                            MessageBox.Show("循環参照されています。\r\nSaveFolderはTargetFolder配下に置かないでください。");
                        }
                        return;
                    }
                }
            }

            // 監視を開始する
            FileChangedFlag_Main = false;
            FileChangedFlag_Sub = false;
            fsWatcher.Path = lblTargetFolder.Text.Trim();
            fsWatcher.EnableRaisingEvents = true;
            if (!string.IsNullOrEmpty(lblTargetFolderSub.Text))
            {
                fsWatcherSub.Path = lblTargetFolderSub.Text.Trim();
                fsWatcherSub.EnableRaisingEvents = true;
            }

            // 監視開始ボタンの表示を監視中に変更する
            btnStart.Text = StopWatchdog;

            // 監視中は監視フォルダ、保存フォルダの変更を禁止する
            btnSaveFolder.Enabled = false;
            btnTargetFolder.Enabled = false;
            btnTargetFolderSub.Enabled = false;
            cmbSection.Enabled = false;
            btnAddApplication.Enabled = false;

            // フォームタイトルに監視中を表示する
            SetSubTitle("監視中");
        }

        /// <summary>
        /// フォームタイトルを設定する
        /// </summary>
        /// <param name="subTitle">状態</param>
        private void SetSubTitle(string subTitle)
        {
            if (subTitle.Length > 0)
            {
                ((MainForm)this).Text = string.Format("{0} - {1}", AppTitle, subTitle);
            }
            else
            {
                ((MainForm)this).Text = AppTitle;
            }
        }

        /// <summary>
        /// ディレクトリをコピーする。
        /// </summary>
        /// <param name="sourceDirName">コピーするディレクトリ</param>
        /// <param name="destDirName">コピー先のディレクトリ</param>
        private static void CopyDirectory(string sourceDirName, string destDirName)
        {
            // コピー先のディレクトリがないかどうか判定する
            if (!Directory.Exists(destDirName))
            {
                // コピー先のディレクトリを作成する
                Directory.CreateDirectory(destDirName);
            }

            // コピー元のディレクトリの属性をコピー先のディレクトリに反映する
            File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));

            // ディレクトリパスの末尾が「\」でないかどうかを判定する
            if (!destDirName.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                // コピー先のディレクトリ名の末尾に「\」を付加する
                destDirName = destDirName + Path.DirectorySeparatorChar;
            }

            // コピー元のディレクトリ内のファイルを取得する
            string[] files = Directory.GetFiles(sourceDirName);
            foreach (string file in files)
            {
                // コピー元のディレクトリにあるファイルをコピー先のディレクトリにコピーする
                File.Copy(file, destDirName + Path.GetFileName(file), true);
            }

            // コピー元のディレクトリのサブディレクトリを取得する
            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                // コピー元のディレクトリのサブディレクトリで自メソッド（CopyDirectory）を再帰的に呼び出す
                CopyDirectory(dir, destDirName + Path.GetFileName(dir));
            }
        }

        /// <summary>
        /// WaitSecが変化した時はSettingsに保存する
        /// </summary>
        private void numWaitSec_ValueChanged(object sender, EventArgs e)
        {
            iniManager.SetValue(cmbSection.Text, "WaitSec", numWaitSec.Value);
        }

        /// <summary>
        /// 自動開始チェックボックスが変化した時はSettingsに保存する
        /// </summary>
        private void chkAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            iniManager.SetValue(defSetting, "AutoStart", chkAutoStart.Checked);
        }

        /// <summary>
        /// フォームの位置、大きさが変わった時はSettingsに保存する
        /// </summary>
        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            if (iniManager != null)
            {
                iniManager.SetValue(defSetting, "FormPosition",
                    string.Format("{0},{1},{2},{3}", this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height));
            }
        }

        #region "バックアップ開始イベント"

        /// <summary>
        /// 監視フォルダ内のファイル、フォルダに変化が発生した場合、バックアップ開始日時を設定し、タイマーを開始する
        /// </summary>
        private void fsWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            SaveFolderStart = DateTime.Now.AddSeconds((double)numWaitSec.Value);

            DateTime now = DateTime.Now;
            TimeSpan diff1 = SaveFolderStart - now;
            TimeSpan diff2 = LastBackupDt.AddMinutes((double)numInterval.Value) - now;

            SetSubTitle("監視中(保存待ち)" + string.Format("{0}秒 : {0}秒", diff1, diff2));

            if (sender == fsWatcher)
            {
                FileChangedFlag_Main = true;
            }
            if (sender == fsWatcherSub)
            {
                FileChangedFlag_Sub = true;
            }
            tmrSave.Start();
        }

        /// <summary>
        /// 監視フォルダ内でリネームが発生した場合、バックアップ開始日時を設定し、タイマーを開始する
        /// </summary>
        private void fsWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            SaveFolderStart = DateTime.Now.AddSeconds((double)numWaitSec.Value);

            DateTime now = DateTime.Now;
            TimeSpan diff1 = SaveFolderStart - now;
            TimeSpan diff2 = LastBackupDt.AddMinutes((double)numInterval.Value) - now;

            SetSubTitle("監視中(保存待ち)" + string.Format("{0}秒 : {0}秒", diff1, diff2));

            if (sender == fsWatcher)
            {
                FileChangedFlag_Main = true;
            }
            if (sender == fsWatcherSub)
            {
                FileChangedFlag_Sub = true;
            }
            tmrSave.Start();
        }

        /// <summary>
        /// Intervalが変化した時はSettingsに保存する
        /// </summary>
        private void numInterval_ValueChanged(object sender, EventArgs e)
        {
            iniManager.SetValue(cmbSection.Text, "IntervalMinutes", numInterval.Value);
        }

        #endregion

        /// <summary>
        /// 7z.dllを使用するしないを切り替える
        /// </summary>
        private void chkUse7z_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUse7z.Checked)
            {
                string path = string.Empty;
                try
                {
                    if (string.IsNullOrEmpty(iniManager.GetValue(defSetting, "DllPath")))
                    {
                        path = Path.Combine(AppContext.BaseDirectory, "7z.dll");
                    }
                    else
                    {
                        path = iniManager.GetValue(defSetting, "DllPath");
                    }

                    SevenZipBase.SetLibraryPath(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(path);
                    MessageBox.Show("7z.dllをFolderBackup.exeと同じフォルダに配置してください。");
                    chkUse7z.Checked = false;
                }
            }

            if (!string.IsNullOrEmpty(cmbSection.Text))
            {
                iniManager.SetValue(cmbSection.Text, "Use7z", chkUse7z.Checked);
            }
        }

        private void btnForce_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("バックアップを行いますか？", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SaveFolderStart = DateTime.Now;
                FileChangedFlag_Main = true;
                FileChangedFlag_Sub = true;
                LastBackupDt = DateTime.MinValue;
                tmrSave.Start();
            }
        }

        private void btnAddApplication_Click(object sender, EventArgs e)
        {
            var inputText = Interaction.InputBox("アプリケーション名を入力してください。", "追加", "", 200, 100);
            if (!string.IsNullOrEmpty(inputText))
            {
                foreach (var appName in cmbSection.Items)
                {
                    if (inputText.Equals(appName))
                    {
                        cmbSection.SelectedItem = appName;
                        return;
                    }
                }

                cmbSection.Items.Add(inputText);
                cmbSection.SelectedItem = inputText;

            }
        }

        private void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSection.SelectedIndex >= 0)
            {
                var selectedText = cmbSection.Items[cmbSection.SelectedIndex].ToString();
                cmbSection.Text = selectedText;

                iniManager.SetValue(defSetting, "Section", selectedText);

                lblTargetFolder.Text = iniManager.GetValue(selectedText, "TargetFolder");
                lblTargetFolderSub.Text = iniManager.GetValue(selectedText, "TargetFolderSub");
                lblSaveFolder.Text = iniManager.GetValue(selectedText, "SaveFolder");
                numWaitSec.Value = iniManager.GetValueDecimal(selectedText, "WaitSec", (decimal)1.0);
                numInterval.Value = iniManager.GetValueDecimal(selectedText, "IntervalMinutes");
                chkUse7z.Checked = iniManager.GetValueBool(selectedText, "Use7z");

            }
        }

        private void btnTargetFolderSub_Click(object sender, EventArgs e)
        {
            dlgSelectFolder.SelectedPath = lblTargetFolderSub.Text;
            if (dlgSelectFolder.ShowDialog() == DialogResult.OK)
            {
                lblTargetFolderSub.Text = dlgSelectFolder.SelectedPath;
                iniManager.SetValue(cmbSection.Text, "TargetFolderSub", dlgSelectFolder.SelectedPath);
            }
        }

        private void folderLabel_DoubleClick(object sender, EventArgs e)
        {
            Label target = (Label)sender;
            if (Directory.Exists(target.Text))
            {
                System.Diagnostics.Process.Start("EXPLORER.EXE", target.Text);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (compressor != null)
            {
                MessageBox.Show("圧縮中は終了できません。");
                e.Cancel = true;
            }
            if (!SaveFolderStart.Equals(DateTime.MinValue))
            {
                if (MessageBox.Show("バックアップ待機中です。終了しますか？", "確認", MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    e.Cancel = true;
                }
            }

        }

    }
}
