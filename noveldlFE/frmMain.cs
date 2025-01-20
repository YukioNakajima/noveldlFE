using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace noveldlFE
{
	[StructLayout(LayoutKind.Explicit)]
	public struct COPYDATASTRUCT32
	{
		[FieldOffset(0)] public UInt32 dwData;
		[FieldOffset(4)] public UInt32 cbData;
		[FieldOffset(8)] public IntPtr lpData;
	}

	public enum NOVEL_STATUS
	{
		None = 0,
		Running = 1,
		Stopped = 2,
		complete = 9,
	}

	public partial class frmMain : Form
	{
		[DllImport("KERNEL32.DLL")]
		public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);
		[DllImport("KERNEL32.DLL")]
		public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);
		[DllImport("kernel32.dll")]
		public static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpstring, string lpFileName);

		struct URLParam_t
		{
			public string novelBaseFolder;
			public string downloaderName;
			public string[] urlTopParts;
			public string[] extProg;
		};

		// WM_COPYDATAのメッセージID
		private const int WM_COPYDATA = 0x004A;
		private const int WM_USER = 0x400;
		private const int WM_DLINFO = WM_USER + 30;

		private UInt32 TotalChap = 0;
		private UInt32 ChapCount = 0;
		private string NovelFName = "";
		private string WriterName = "";
		public string exePath = "";
		public string exeDirName = "";
		public string iniPath = "";
		public string iniPathFE = "";
		private string logPath = "";

		//private DateTime latestDLDateTime;
		//private DateTime nextEveryDay;
		//private DateTime nextEveryWeek;
		//private DateTime nextEveryMon;
		//private string sStatus = "";
		//private int novelTotal = 0;
		//private int novelCount = 0;
		public bool busy = false;
		private IntPtr hWnd = IntPtr.Zero;
		private string dlAfterOpeNovel1st = "";
		private string dlAfterOpeNovel1Later = "";
		private string[] extProg = new string[3];

		public NOVEL_STATUS novelSt = NOVEL_STATUS.None;
		private DateTime StartTime = DateTime.Now;

		private URLParam_t[] urlParam;

		public frmMain()
		{
			InitializeComponent();
		}
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			switch (m.Msg)
			{
				case WM_COPYDATA:
					{
						COPYDATASTRUCT32 cds = (COPYDATASTRUCT32)Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT32));
						TotalChap = cds.dwData;
						string novelname = Marshal.PtrToStringAuto(cds.lpData);
						lblNovelTitle.Text = novelname;
						switch (novelname.Substring(1, novelname.IndexOf('】')))
						{
							case "連載中": novelSt = NOVEL_STATUS.Running; break;
							case "中断": novelSt = NOVEL_STATUS.Stopped; break;
							case "短編":
							case "完結": novelSt = NOVEL_STATUS.complete; break;
							default: novelSt = NOVEL_STATUS.None; break;
						}
						string[] strs = novelname.Split(',');
						NovelFName = strs[0];
						NovelFName = NovelFName.Substring(novelname.IndexOf('】'));
						WriterName = strs[1];
						foreach (char ch in Path.GetInvalidPathChars())
						{
							NovelFName.Replace(ch, '_');
							WriterName.Replace(ch, '_');
						}
					}
					break;
				case WM_DLINFO:
					{
						ChapCount = (UInt32)m.WParam;
						lblProgress.Text = $"{(int)(ChapCount * 100 / TotalChap)}".PadLeft(3) + $@"% ({ChapCount}/{TotalChap})";
					}
					break;
			}
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			Assembly myAssembly = Assembly.GetEntryAssembly();
			exePath = myAssembly.Location;
			exeDirName = Path.GetDirectoryName(exePath);
			iniPath = exeDirName + @"\" + Path.GetFileNameWithoutExtension(exePath) + ".ini";
			logPath = exeDirName + @"\Log";
			if (Directory.Exists(logPath) == false)
			{
				Directory.CreateDirectory(logPath);
			}
			logPath += $@"\{DateTime.Now.ToString("yyyyMMdd")}.log";

			lblStatus.Text = "";

			StringBuilder wk = new StringBuilder(512);
			GetPrivateProfileString("Common", "TargetPath", "", wk, 512, iniPath);
			iniPathFE = wk.ToString();

			if (File.Exists(iniPathFE) == false)
			{
				if (MessageBox.Show(this, "noveldlFromtEndのINIファイルが見つかりませんでした。\nINIファイルを選択しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
				{
					this.Close();
				}
				OpenFileDialog ofd = new OpenFileDialog();
				ofd.FileName = Path.GetFileName(iniPathFE);
				ofd.InitialDirectory = Path.GetDirectoryName(exeDirName);
				ofd.Filter = "iniファイル(*.ini)|*.ini|全てのファイル(*.*)|*.*";
				ofd.FilterIndex = 1;
				ofd.Title = "noveldlFrontEndのiniファイルを選択してください";
				ofd.RestoreDirectory = true;
				ofd.CheckFileExists = true;
				ofd.CheckPathExists = true;
				//ダイアログを表示する
				if (ofd.ShowDialog() != DialogResult.OK)
				{
					this.Close();
				}
				iniPathFE = ofd.FileName;
				WritePrivateProfileString("Common", "TargetPath", iniPathFE, iniPath);

			}
			lblIniPath.Text = iniPathFE;

			GetPrivateProfileString("DownloadAfterOperation", "Novel1st", "", wk, 512, iniPathFE);
			dlAfterOpeNovel1st = wk.ToString();
			GetPrivateProfileString("DownloadAfterOperation", "Novel1Later", "", wk, 512, iniPathFE);
			dlAfterOpeNovel1Later = wk.ToString();

			GetPrivateProfileString("DownloadAfterOperation", "ExtProgram1", "", wk, 512, iniPathFE);
			extProg[0] = wk.ToString();
			GetPrivateProfileString("DownloadAfterOperation", "ExtProgram2", "", wk, 512, iniPathFE);
			extProg[1] = wk.ToString();
			GetPrivateProfileString("DownloadAfterOperation", "ExtProgram3", "", wk, 512, iniPathFE);
			extProg[2] = wk.ToString();

			hWnd = this.Handle;

			int num = (int)GetPrivateProfileInt("ListItems", "Count", -1, iniPathFE);
			urlParam = new URLParam_t[num];
			int cnt = 0;
			exeDirName = Path.GetDirectoryName(iniPathFE);
			string findstr = "Novel Folder:";
			for (int i = 0; i < num; i++)
			{
				GetPrivateProfileString("ListItems", $"Item{i + 1}", "", wk, 256, iniPathFE);
				string item = wk.ToString();
				if (item != "")
				{
					cboxListSelect.Items.Add(item);
					getUrlParam(cnt++, item);
				}
			}
			if (cboxListSelect.Items.Count > 0) cboxListSelect.SelectedIndex = 0;
			//30日以上経過したログの消去
			try
			{
				string fpath = exeDirName;
				fpath += @"\Log\";
				if (Directory.Exists(fpath) == false)
				{
					Directory.CreateDirectory(fpath);
				}
				string[] files = System.IO.Directory.GetFiles(fpath, "*.log"); //, System.IO.SearchOption.AllDirectories);
				foreach (string path in files)
				{
					string fname = Path.GetFileNameWithoutExtension(path);
					int iy, im, id;
					if ((fname.Length == 6)
					&& (int.TryParse(fname.Substring(0, 2), out iy))
					&& (int.TryParse(fname.Substring(2, 2), out im))
					&& (int.TryParse(fname.Substring(4, 2), out id)))
					{
						DateTime fdate = new DateTime(iy + 2000, im, id);
						if ((DateTime.Now - fdate).TotalDays > 30)
						{
							System.IO.File.Delete(path);
						}
					}
				}
			}
			catch (Exception ex)
			{
				string msg = ex.Message;
			}
		}

		private void getUrlParam(int index, string listName)
		{
			string[] Lines = File.ReadAllLines(exeDirName + @"\" + listName);
			string findstr = "Type:";
			string[] res = Array.FindAll(Lines, delegate (string s) { return s.IndexOf(findstr) != -1; });
			if (res.Length > 0)
			{
				string urltype = res[0].Substring(findstr.Length);
				StringBuilder wk = new StringBuilder(512);
				GetPrivateProfileString(urltype, "Downloader", "", wk, 512, iniPathFE);
				urlParam[index].downloaderName = wk.ToString();
				GetPrivateProfileString(urltype, "URLTop", "", wk, 512, iniPathFE);
				urlParam[index].urlTopParts = wk.ToString().Split(',');
				urlParam[index].extProg = new string[3];
				GetPrivateProfileString(urltype, "ExtProgram1", "", wk, 512, iniPathFE);
				urlParam[index].extProg[0] = wk.ToString();
				GetPrivateProfileString(urltype, "ExtProgram2", "", wk, 512, iniPathFE);
				urlParam[index].extProg[1] = wk.ToString();
				GetPrivateProfileString(urltype, "ExtProgram3", "", wk, 512, iniPathFE);
				urlParam[index].extProg[2] = wk.ToString();

			}
			findstr = "Novel Folder:";
			res = Array.FindAll(Lines, delegate (string s) { return s.IndexOf(findstr) != -1; });
			if (res.Length > 0)
			{
				urlParam[index].novelBaseFolder = res[0].Substring(findstr.Length);
			}
		}


		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				if (MessageBox.Show(this, "終了しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					e.Cancel = true;
				}
			}
		}

		private bool chkUrl(int index, string URL)
		{
			if ((urlParam[index].urlTopParts == null) || (urlParam[index].urlTopParts.Length <= 0)) return false;
			foreach (string str in urlParam[index].urlTopParts)
			{
				if (URL.Contains(str)) return true;
			}
			return false;
		}

		private void btnDL_Click(object sender, EventArgs e)
		{
			lblStatus.Text = "ダウンロード中";
			StartTime = DateTime.Now;
			string URL = tbURL.Text;
			if (string.IsNullOrWhiteSpace(URL) || (URL.Substring(0, 8) != "https://"))
			{
				MessageBox.Show(this, $"URL[{URL}]は無効です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			timer1.Enabled = true;
			string filepath = tbNovelPath.Text;
			if (string.IsNullOrEmpty(filepath))
			{
				DownloadNovel(URL);
			}
			else
			{
				string fext = $"{Path.GetExtension(filepath)}";

				//この時点では次の２つは不明
				//filepath = filepath.Replace("%W", 作者名).Replace("%w", 作者名);
				//filepath = filepath.Replace("%T", NovelFName).Replace("%t", NovelFName);

				int index = cboxListSelect.SelectedIndex;
				string fullpath = urlParam[index].novelBaseFolder + @"\" + filepath;
				string fname = Path.GetFileNameWithoutExtension(fullpath);
				//小説を格納するフォルダがなければ作成
				string novelDir = Path.GetDirectoryName(fullpath);
				if (Directory.Exists(novelDir) == false)
				{
					Directory.CreateDirectory(novelDir);
				}
				//小説一つをダウンロード
				DownloadNovel(URL, novelDir, fname);
			}
			timer1.Enabled = false;
		}

		private void LogOut(String logMsg)
		{
			string fpath = exeDirName + @"\Log\";
			if (!Directory.Exists(fpath))
			{
				Directory.CreateDirectory(fpath);
			}
			using (StreamWriter sw = new StreamWriter(fpath + DateTime.Now.ToString("yyMMdd") + @".log", true, Encoding.UTF8))
			{
				sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ":" + logMsg);
			}
		}

		/// <summary>
		/// 小説をダウンロード
		/// </summary>
		/// <param name="UrlAdr">URL</param>
		/// <param name="DirName">格納するディレクトリ名</param>
		/// <param name="fname">ファイル名</param>
		/// <param name="fext">ファイル拡張子</param>
		private void DownloadNovel(string UrlAdr, string DirName = "", string fname = "", string fext = ".txt")
		{
			try
			{
				string tbNovelPathText = tbNovelPath.Text;
				string dirname = ((string.IsNullOrEmpty(DirName) ? exeDirName : DirName));
				string infopath = $@"{dirname}\{fname}Info.txt";
				string filepath = (string.IsNullOrEmpty(fname) ? "" : $@"{dirname}\{fname}{fext}");
				int startPage = 1;
				ChapCount = 0;
				TotalChap = 0;
				lblProgress.Text = "";
				DateTime latestDate = DateTime.Parse("2000/1/1");
				List<string> infoLines = null;
				if (File.Exists(infopath))
				{
					infoLines = File.ReadAllLines(infopath).ToList<string>();
					if (infoLines.Count > 0)
					{
						foreach (string ldata in infoLines)
						{
							string[] infos = ldata.Split(',');
							if (infos.Length >= 2)
							{
								if ((DateTime.TryParse(infos[0], out latestDate))
								&& (int.TryParse(infos[1], out startPage)))
								{
									startPage++;
									infoLines.Remove(ldata);
									break;
								}
							}
						}
					}
					//if (DlAbort) return;
					numericUpDown1.Value = startPage;
				}
				else
				{
					startPage = (int)numericUpDown1.Value;
				}
				string tmppath = $@"{dirname}\__tmp.txt";
				Process proc = null;
				if (startPage > 1)
				{
					//途中までダウンロードできていれば続きをダウンロードし、マージする
					if (File.Exists(tmppath)) File.Delete(tmppath);
					//小説を続きの章から最新章までダウンロード
					proc = noveldlDownload(hWnd, UrlAdr, tmppath, startPage);
					proc.WaitForExit();

					//小説ファイルをマージする
					if (File.Exists(tmppath))
					{
						//リンクの図を検索してリンクのみの文字列配列を取得し、情報ファイルの内容に追加・重複削除する
						getFigLink(File.ReadAllLines(tmppath), ref infoLines);
						if (File.Exists(tmppath))
						{
							if (dlAfterOpeNovel1Later != "")
							{
								exeAfterOperation(dlAfterOpeNovel1Later, tmppath);
							}
							int index = cboxListSelect.SelectedIndex;
							foreach (string str in urlParam[index].extProg)
							{
								if (str == "") break;
								exeAfterOperation(str, tmppath);
							}
							foreach (string str in extProg)
							{
								if (str == "") break;
								exeAfterOperation(str, tmppath);
							}
							//List<string> buff = File.ReadAllLines(tmppath).ToList<string>();
							string[] buff = File.ReadAllLines(tmppath);
							using (FileStream fs = File.Open(filepath, FileMode.Open))
							using (StreamWriter sw = new StreamWriter(fs, new UTF8Encoding()))
							{
								//int len = buff.Count;
								int len = buff.Length;
								fs.Seek(0, SeekOrigin.End);
								int idx = 0;
								for (; idx < len; idx++)
								{
									if (buff[idx].IndexOf("［＃中見出し］") >= 0) break;
								}
								for (; idx < len; idx++)
								{
									sw.WriteLine(buff[idx]);
								}
							}
							File.Delete(tmppath);
							File.Delete($@"{dirname}\__tmp.log");
						}
						LogOut($"{fname}、{UrlAdr}、開始章{startPage}、読込章数{ChapCount}");
					}
				}
				else
				{
					//小説を最初から最新章までダウンロード
					proc = noveldlDownload(hWnd, UrlAdr, filepath);
					proc.WaitForExit();
					if (File.Exists(filepath))
					{
						infoLines = new List<string>();
						//リンクの図を検索してリンクのみの文字列配列を取得し、情報ファイルの内容に追加・重複削除する
						getFigLink(File.ReadAllLines(filepath), ref infoLines);
						if (dlAfterOpeNovel1st != "")
						{
							exeAfterOperation(dlAfterOpeNovel1st, filepath);
						}
						int index = cboxListSelect.SelectedIndex;
						foreach (string str in urlParam[index].extProg)
						{
							if (str == "") break;
							exeAfterOperation(str, filepath);
						}
						foreach (string str in extProg)
						{
							if (str == "") break;
							exeAfterOperation(str, filepath);
						}
						ChapCount = TotalChap;
						LogOut($"{fname}、{UrlAdr}、開始章{startPage}、読込章数{ChapCount}");
					}
					else
					{
						//MessageBox.Show($"[{fname}]がダウンロードできませんでした", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						LogOut($"{fname}、{UrlAdr}、消滅");
					}
				}
				if ((File.Exists(filepath)) && (ChapCount > 0))
				{
					//小説情報ファイルを書き込む
					using (StreamWriter sw = new StreamWriter(File.Create(infopath), new UTF8Encoding()))
					{
						sw.WriteLine($"{DateTime.Now}, {ChapCount + startPage -1}");
						if (infoLines.Count > 0)
						{
							foreach (string str in infoLines)
							{
								sw.WriteLine(str);
							}
						}
					}
					if(novelSt != NOVEL_STATUS.complete)
					{
						NovelListAdd("#毎週", UrlAdr, tbNovelPathText);
					}
					else
					{
						NovelListAdd("#完結", UrlAdr, tbNovelPathText);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				//listBoxAdd(frmErrSt.lboxErrStatus, $"DL Error:[{fname}], {UrlAdr}");
			}
		}

		private void NovelListAdd(string section, string URL, string filePath)
		{
			//指定されたURLがリストファイルに無ければ追加する
			string listfilepath = exeDirName + @"\" + cboxListSelect.Items[cboxListSelect.SelectedIndex];
			List<string> Lines = File.ReadAllLines(listfilepath).ToList<string>();
			int idx = Lines.IndexOf(URL);
			if (idx < 0)
			{
				//URLが無いのでリストの毎週の最後に追加する
				idx = Lines.IndexOf(section);
				int pos = -1;
				for (int i = idx + 1; i < Lines.Count; i++)
				{
					if (string.IsNullOrEmpty(Lines[i].Trim()) == false)
					{
						if (Lines[i].Trim().Substring(0, 1) == "#") break;
						pos = i;
					}
				}
				if (pos > 0)
				{
					Lines.InsertRange(pos + 1, new List<string>() { filePath, URL });
					File.WriteAllLines(listfilepath, Lines, Encoding.UTF8);
				}
			}
		}


		private Process noveldlDownload(IntPtr hWnd, string URL, string filePath = null, int startChap = 1)
		{
			int index = cboxListSelect.SelectedIndex;
			if (chkUrl(index, URL) == false)
			{
				return null;
			}

			Process proc = new Process();
			proc.StartInfo.FileName = urlParam[index].downloaderName;
			string arg = $" \"-h {hWnd}\"";
			if (string.IsNullOrEmpty(filePath) == false)
			{
				arg += $" \"{filePath}\"";
			}
			if (startChap > 0)
			{
				arg += $" \"-s {startChap}\"";
			}
			arg += $" {URL}";
			proc.StartInfo.Arguments = arg;
			proc.StartInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
			proc.StartInfo.UseShellExecute = false; // シェル機能を使用しない
			proc.SynchronizingObject = this;
			proc.Exited += new EventHandler(proc_Exited);//終了イベントを登録
			proc.EnableRaisingEvents = true;
			//起動する
			proc.Start();
			return proc;
		}

		private void proc_Exited(object sender, EventArgs e)
		{
			//プロセスが終了したときに実行される
			lblStatus.Text = "ダウンロード終了しました。";
			tbURL.Text = "";
			tbNovelPath.Text = "";
			lblProgress.Text = $@"100% ({TotalChap}/{TotalChap})";
		}

		/// <summary>
		/// 外部プログラムの実行
		/// 
		/// iniファイルには以下が設定されている
		/// //初回ダウンロード後処理
		/// Novel1st=""C:\Program Files(x86)\sakura\sakura.exe" "-M=%A\na6dl用変換-自動.vbs" "%F""
		/// //2回目以降の差分ダウンロード後の差分ファイルに対しての処理
		/// Novel1Later=""C:\Program Files(x86)\sakura\sakura.exe" "-M=%A\na6dl用章追加変換-自動.vbs" "%F""
		/// </summary>
		/// <param name="cmdLine">コマンドライン、%Fは小説ファイル名に、%Aはexeフォルダパスに置き換え</param>
		/// <param name="filepath">小説のダウンロード保存先パス</param>
		private void exeAfterOperation(string cmdline, string filepath)
		{
			int pos = 0;
			string arg = "";
			string filename = "";
			cmdline = cmdline.Trim();
			if (cmdline[0] == '"')
			{
				pos = cmdline.IndexOf('\"', 1);
				filename = cmdline.Substring(1, pos - 1);//.Trim(new char[] { '"', ' ' });
			}
			else
			{
				pos = cmdline.IndexOf(' ', 1);
				filename = cmdline.Substring(0, pos);//.Trim(new char[] { '"', ' ' });
			}
			//引数を確定し、特定文字を置き換える
			arg = cmdline.Substring(pos + 1);//.Trim(new char[] { '"', ' ' });
			arg = arg.Replace("%F", filepath).Replace("%f", filepath);
			arg = arg.Replace("%A", exeDirName).Replace("%a", exeDirName);
			//プロセスを作成し、実行する
			ProcessStartInfo pInfo = new ProcessStartInfo();
			pInfo.FileName = filename;
			pInfo.Arguments = arg;
			//意味がない
			//pInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
			//pInfo.UseShellExecute = false; // シェル機能を使用しない
			Process p = Process.Start(pInfo);
			//終了待ち
			p.WaitForExit();
			//MessageBox.Show("終了しました");
		}

		/// <summary>
		/// "［＃リンクの図（//41743.mitemin.net/userpageimage/viewimagebig/icode/i813181/）入る］"と同様の文字列を含む行を抽出、
		/// リンクのみにして指定のリストにマージ、
		/// 重複を削除する
		/// </summary>
		/// <param name="strSrray">抽出元の文字列配列￥</param>
		/// <param name="destlist">マージする文字列のリスト</param>
		private void getFigLink(string[] strSrray, ref List<string> destlist)
		{
			//［＃リンクの図（//41743.mitemin.net/userpageimage/viewimagebig/icode/i813181/）入る］
			string[] strs = getFigLink(strSrray);
			destlist.AddRange(strs);
			destlist.Distinct();
		}

		private string[] getFigLink(string[] strSrray)
		{
			//return strSrray.Where(str => str.Contains("リンクの図")).Select(str => Regex.Replace(str, @"^.*リンクの図（", "https:")).Select(str => Regex.Replace(str, @"）入る］.*", "")).ToArray();
			return strSrray.Where(str => str.Contains("リンクの図")).Select(str => Regex.Replace(str, @"^.*リンクの図（", "https:")).Select(str => Regex.Replace(str, @"/userpageimage/viewimagebig/icode/", @"/")).Select(str => Regex.Replace(str, @"）入る］.*", "")).ToArray();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			lblTimeCount.Text = $"{DateTime.Now - StartTime}";
		}

		private void btnIniSelect_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.FileName = Path.GetFileName(iniPathFE);
			ofd.InitialDirectory = Path.GetDirectoryName(iniPathFE);
			ofd.Filter = "iniファイル(*.ini)|*.ini|全てのファイル(*.*)|*.*";
			ofd.FilterIndex = 1;
			ofd.Title = "noveldlFrontEndのiniファイルを選択してください";
			ofd.RestoreDirectory = true;
			ofd.CheckFileExists = true;
			ofd.CheckPathExists = true;
			//ダイアログを表示する
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				iniPathFE = ofd.FileName;
				lblIniPath.Text = iniPathFE;
				WritePrivateProfileString("Common", "TargetPath", iniPathFE, iniPath);

				exeDirName = Path.GetDirectoryName(iniPathFE);
				StringBuilder wk = new StringBuilder(512);
				int cnt = 0;
				string findstr = "Novel Folder:";

				int num = (int)GetPrivateProfileInt("ListItems", "Count", -1, iniPathFE);
				urlParam = new URLParam_t[num];
				cboxListSelect.Items.Clear();
				for (int i = 0; i < num; i++)
				{
					GetPrivateProfileString("ListItems", $"Item{i + 1}", "", wk, 256, iniPathFE);
					string item = wk.ToString();
					if (item != "")
					{
						cboxListSelect.Items.Add(item);
						getUrlParam(cnt++, item);
					}
				}
				if (cboxListSelect.Items.Count > 0) cboxListSelect.SelectedIndex = 0;
			}
		}

		private void btnNovelOpen_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			int index = cboxListSelect.SelectedIndex;
			String basedir = urlParam[index].novelBaseFolder;
			ofd.InitialDirectory = basedir;
			ofd.Filter = "テキストファイル(*.txt)|*.txt|全てのファイル(*.*)|*.*";
			ofd.FilterIndex = 1;
			ofd.Title = "小説を書き込むファイルを選択してください";
			ofd.RestoreDirectory = true;
			ofd.CheckFileExists = false;
			ofd.CheckPathExists = false;
			//sfd.CheckWriteAccess = true;
			//ダイアログを表示する
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				String fpath = ofd.FileName;
				if (fpath.Contains(basedir))
				{
					tbNovelPath.Text = fpath.Substring(basedir.Length + 1);
				}
			}
		}
	}
}
