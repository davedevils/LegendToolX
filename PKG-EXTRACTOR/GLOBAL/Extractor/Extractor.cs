using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Extractor.Extractor
{
    public partial class Extractor : Form
    {
		private delegate void SetTextCallback(string text);

		private bool done;

		private bool uirun;

		public int uifcount;

		public int uinum;

		public string uipath;

		public string uiname;

		public string basepath = "out\\";

		public int fid = 1;

		public byte[] _offset;

		public byte[] zsize;

		public byte[] fsize;

		public byte[] fname;

		public byte[] fdir;

		public byte[] pkg;

		private List<int> fileoffset = new List<int>();

		private List<int> filezsize = new List<int>();

		private List<int> filesize = new List<int>();

		private List<string> filename = new List<string>();

		private List<string> filedir = new List<string>();

		private List<int> pkgfile = new List<int>();

		private List<string> filepath = new List<string>();


		public Extractor()
		{
			InitializeComponent();
		}

		private void Extractor_Load(object sender, EventArgs e)
		{
			Thread thread = new Thread(getidx);
			thread.IsBackground = true;
			thread.Start();
			thread.Join();
			uirun = true;
			Thread thread2 = new Thread(uiwork);
			thread2.IsBackground = true;
			thread2.Start();
		}

		public void getidx()
		{
			Array.Resize(ref _offset, 4);
			Array.Resize(ref zsize, 4);
			Array.Resize(ref fsize, 4);
			Array.Resize(ref fname, 260);
			Array.Resize(ref fdir, 260);
			Array.Resize(ref pkg, 4);
			try
			{
				FileStream fileStream = File.Open("pkg\\pkg.idx", FileMode.Open);
				fileStream.Seek(296L, SeekOrigin.Begin);
				while (fileStream.Position < fileStream.Length)
				{
					fileStream.Read(_offset, 0, _offset.Length);
					fileStream.Seek(4L, SeekOrigin.Current);
					foffset(_offset);
					fileStream.Read(zsize, 0, zsize.Length);
					fileStream.Seek(40L, SeekOrigin.Current);
					fileStream.Read(fsize, 0, fsize.Length);
					fileStream.Read(fname, 0, fname.Length);
					fileStream.Read(fdir, 0, fdir.Length);
					fileStream.Seek(4L, SeekOrigin.Current);
					fileStream.Read(pkg, 0, pkg.Length);
					fileStream.Seek(8L, SeekOrigin.Current);
					fzsize(zsize);
					ffsize(fsize);
					ffname(fname);
					ffdir(fdir);
					filep(fdir, fname);
					ffpkg(pkg);
					fid++;
				}
				fileStream.Close();
				listTodata();
			}
			catch (Exception ex)
			{
				using (StreamWriter w = File.AppendText("extract.log"))
				{
					Log(ex.Message, w);
				}
			}
		}

		private void foffset(byte[] ioffset)
		{
			int item = BitConverter.ToInt32(ioffset, 0);
			fileoffset.Add(item);
		}

		private void fzsize(byte[] izsize)
		{
			int item = BitConverter.ToInt32(izsize, 0);
			filezsize.Add(item);
		}

		private void ffsize(byte[] ifsize)
		{
			int item = BitConverter.ToInt32(ifsize, 0);
			filesize.Add(item);
		}

		private void ffname(byte[] ifname)
		{
			string @string = Encoding.GetEncoding(950).GetString(ifname);
			List<string> list = filename;
			char[] trimChars = new char[1];
			list.Add(@string.Trim(trimChars));
		}

		private void ffdir(byte[] ifdir)
		{
			string @string = Encoding.GetEncoding(950).GetString(ifdir);
			List<string> list = filedir;
			char[] trimChars = new char[1];
			list.Add(@string.Trim(trimChars));
		}

		private void ffpkg(byte[] ifpkg)
		{
			int item = BitConverter.ToInt32(ifpkg, 0);
			pkgfile.Add(item);
		}

		private void filep(byte[] fd, byte[] fn)
		{
			string @string = Encoding.GetEncoding(950).GetString(fd);
			string string2 = Encoding.GetEncoding(950).GetString(fn);
			List<string> list = filepath;
			char[] trimChars = new char[1];
			string str = @string.Trim(trimChars);
			char[] trimChars2 = new char[1];
			list.Add(str + string2.Trim(trimChars2));
		}

		private void listTodata()
		{
			mdata = new DataTable();
			mdata.Columns.Add("id");
			mdata.Columns.Add("offset");
			mdata.Columns.Add("zsize");
			mdata.Columns.Add("size");
			mdata.Columns.Add("fileName");
			mdata.Columns.Add("fileDir");
			mdata.Columns.Add("pkg");
			mdata.Columns.Add("path");
			int count = fileoffset.Count;

			for (int i = 0; i < count; i++)
			{
				filename[i] = CutWhenUnknowChar(filename[i]);

				mdata.Rows.Add(i.ToString(), fileoffset[i].ToString(), filezsize[i].ToString(), filesize[i].ToString(), filename[i], filedir[i], pkgfile[i].ToString(), filepath[i]);
			}
			dataGridView1.DataSource = mdata;
			uifcount = count;
			uinum = 0;
			button1.Enabled = true;
		}

		private string fileidx(int idx)
		{
			if (idx < 10)
			{
				return "00" + idx;
			}
			if (idx < 100)
			{
				return "0" + idx;
			}
			return idx.ToString();
		}

		public static byte[] deCompress(byte[] raw, bool version)
		{
			if (version)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (MemoryStream stream = new MemoryStream(raw))
					{
						using (System.IO.Compression.DeflateStream deflateStream = new System.IO.Compression.DeflateStream(stream, System.IO.Compression.CompressionMode.Decompress, leaveOpen: true))
						{
							deflateStream.CopyTo(memoryStream);
						}
						return memoryStream.ToArray();
					}
				}
			}
			using (MemoryStream memoryStream2 = new MemoryStream())
			{
				using (ZlibStream zlibStream = new ZlibStream(memoryStream2, Ionic.Zlib.CompressionMode.Decompress, leaveOpen: true))
				{
					zlibStream.Write(raw, 0, raw.Length);
				}
				return memoryStream2.ToArray();
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Thread thread = new Thread(extracter);
			thread.IsBackground = true;
			thread.Start();
		}


		public static void Log(string logMessage, TextWriter w)
		{

			w.Write($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
			w.Write("  :");
			w.Write("\r\n");
			w.Write($"  {logMessage}");
			w.Write("\r\n");
			w.WriteLine("-------------------------------");
		}


		private void extracter()
		{
			for (int i = 0; i < fileoffset.Count; i++)
			{
				try
				{
					string curFile = "pkg\\pkg" + fileidx(pkgfile[i]) + ".pkg";

					if (!File.Exists(curFile))
					{
						using (StreamWriter w = File.AppendText("extract.log"))
						{
							Log("Can't open " + curFile + "! - " + filedir[i] + "" + filename[i] + " will be skiped.", w);
						}
						//MessageBox.Show("Can't open " + curFile + "!\n" + filename[i] +" will be skiped.");
						continue;
					}
					FileStream fileStream = File.Open(curFile, FileMode.Open);
					fileStream.Seek(fileoffset[i], SeekOrigin.Begin);
					byte[] array = new byte[filezsize[i]];
					fileStream.Read(array, 0, array.Length);
					bool flag = (array[0] == 136) ? true : false;

					string finalpath = ReturnCleanASCII(basepath + filedir[i]);
					string finalfilepath = ReturnCleanASCII(finalpath + CutWhenUnknowChar(filename[i]));

					if (flag)
					{
						byte[] raw = array.Skip(2).ToArray();
						byte[] array2 = deCompress(raw, flag);
						uipath = filedir[i];
						uiname = filename[i];
						if (!Directory.Exists(finalpath))
						{
							Directory.CreateDirectory(finalpath);
						}
						FileStream fileStream2 = File.Create(finalfilepath);
						for (int j = 0; j < array2.Length; j++)
						{
							fileStream2.WriteByte(array2[j]);
						}
						fileStream2.Close();
						fileStream.Close();
						uinum = i + 1;
					}
					else
					{
						byte[] array3 = deCompress(array, flag);
						uipath = filedir[i];
						uiname = filename[i];
						if (!Directory.Exists(finalpath))
						{
							Directory.CreateDirectory(finalpath);
						}
						FileStream fileStream3 = File.Create(finalfilepath);
						for (int k = 0; k < array3.Length; k++)
						{
							fileStream3.WriteByte(array3[k]);
						}
						fileStream3.Close();
						fileStream.Close();
						uinum = i + 1;
					}
					Thread.Sleep(10);

					done = true;
				}
				catch (Exception ex)
				{
					using (StreamWriter w = File.AppendText("extract.log"))
					{
						Log(ex.Message, w);
					}
					continue;
				}
			}
		}

		public string ReturnCleanASCII(string s)
		{
			StringBuilder sb = new StringBuilder(s.Length);
			foreach (char c in s)
			{

				// remove crap
				if ((int)c > 127) 
					continue;
				if ((int)c < 32) 
					continue;
				if (c == ',')
					continue;
				if (c == '"')
					continue;
				sb.Append(c);
			}
			return sb.ToString();
		}


		public string CutWhenUnknowChar(string s)
		{
			StringBuilder sb = new StringBuilder(s.Length);
			foreach (char c in s)
			{
				if ((int)c > 127)
					return sb.ToString();
				if ((int)c < 32)
					return sb.ToString();
				if (c == ',')
					return sb.ToString();
				if (c == '"')
					return sb.ToString();
				sb.Append(c);
			}
			return sb.ToString();
		}
		private void uiwork()
		{
			while (uirun)
			{
				Thread.Sleep(10);
				updateui();
			}
		}

		private void updateui()
		{
			SetText1(basepath + uipath + uiname);
			SetText2(uinum + " / " + uifcount);
			if (done)
			{
				SetText2("Done!");
			}
		}

		private void SetText1(string text)
		{
			if (label1.InvokeRequired)
			{
				SetTextCallback method = SetText1;
				Invoke(method, text);
			}
			else
			{
				label1.Text = text;
			}
		}

		private void SetText2(string text)
		{
			if (label2.InvokeRequired)
			{
				SetTextCallback method = SetText2;
				Invoke(method, text);
			}
			else
			{
				label2.Text = text;
			}
		}
		
		private void button2_Click(object sender, EventArgs e)
        {
			Thread thread = new Thread(extracter);
			thread.IsBackground = true;
			thread.Start();
		}

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
