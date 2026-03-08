using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PkgManager.PkgManager
{
    public partial class PkgManager : Form
    {
		private const int MaxPackageSize = 10485760;
		private delegate void SetTextCallback(string text);

		private bool done;

		private Dictionary<string, string> hashMap = new Dictionary<string, string>();

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


		public PkgManager()
		{
			InitializeComponent();
		}

		private void PkgManager_Load(object sender, EventArgs e)
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

		private string DecodeString(byte[] data)
		{
			string big5 = Encoding.GetEncoding(950).GetString(data);
			char[] trimChars = new char[1];
			big5 = big5.Trim(trimChars);
			foreach (char c in big5)
			{
				if (c == '\uFFFD')
				{
					string utf8 = Encoding.UTF8.GetString(data);
					return utf8.Trim(trimChars);
				}
			}
			return big5;
		}

		private void ffname(byte[] ifname)
		{
			filename.Add(DecodeString(ifname));
		}

		private void ffdir(byte[] ifdir)
		{
			filedir.Add(DecodeString(ifdir));
		}

		private void ffpkg(byte[] ifpkg)
		{
			int item = BitConverter.ToInt32(ifpkg, 0);
			pkgfile.Add(item);
		}

		private void filep(byte[] fd, byte[] fn)
		{
			filepath.Add(DecodeString(fd) + DecodeString(fn));
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

		private void UnpackAll_Click(object sender, EventArgs e)
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
		
		private void UnpackSelected_Click(object sender, EventArgs e)
        {
			Thread thread = new Thread(extracter);
			thread.IsBackground = true;
			thread.Start();
		}

		private void PackAll_Click(object sender, EventArgs e)
		{
			done = false;
			Thread thread = new Thread(packer);
			thread.IsBackground = true;
			thread.Start();
		}

		private class IdxEntry
		{
			public int Index;
			public int Offset;
			public int IndexOffset;
			public int CompressedSize;
			public int FileSize;
			public string FileName;
			public string FilePath;
			public int PackageId;
			public string CompressedFilePath;
		}

		private string ComputeHash(string filePath)
		{
			using (var md5 = System.Security.Cryptography.MD5.Create())
			using (var stream = File.OpenRead(filePath))
			{
				var hash = md5.ComputeHash(stream);
				return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
			}
		}

		private void LoadMD5Map()
		{
			hashMap.Clear();
			if (!File.Exists("pkg.md5")) return;
			foreach (var line in File.ReadAllLines("pkg.md5"))
			{
				int sep = line.LastIndexOf('|');
				if (sep > 0)
					hashMap[line.Substring(0, sep)] = line.Substring(sep + 1);
			}
		}

		private void SaveMD5Map()
		{
			using (var w = new StreamWriter("pkg.md5", false, Encoding.UTF8))
			{
				foreach (var kvp in hashMap)
					w.WriteLine(kvp.Key + "|" + kvp.Value);
			}
		}

		private void packer()
		{
			string sourceDir = "source";
			string outputDir = "output";

			if (!Directory.Exists(sourceDir)) { Directory.CreateDirectory(sourceDir); return; }
			if (!Directory.Exists(outputDir)) { Directory.CreateDirectory(outputDir); }

			LoadMD5Map();

			var files = Directory.EnumerateFiles(sourceDir, "*", SearchOption.AllDirectories)
				.Where(f => !f.EndsWith("_Z_", StringComparison.OrdinalIgnoreCase))
				.ToList();

			uifcount = files.Count;
			uinum = 0;
			done = false;

			int currentPackageSize = 0;
			int packageId = 1;
			var idxEntries = new List<IdxEntry>();

			for (int i = 0; i < files.Count; i++)
			{
				var fp = files[i];
				string fileName = Path.GetFileName(fp);
				string relativePath = fp.Substring(sourceDir.Length + 1);
				string fileDir = Path.GetDirectoryName(relativePath);
				if (!string.IsNullOrEmpty(fileDir)) fileDir += "\\";
				else fileDir = "";
				string fileKey = fileDir + fileName;

				bool needCompress = false;
				string compressedPath = fp + "_Z_";

				if (hashMap.ContainsKey(fileKey))
				{
					string newHash = ComputeHash(fp);
					if (hashMap[fileKey] != newHash)
					{
						hashMap[fileKey] = newHash;
						needCompress = true;
					}
				}
				else
				{
					hashMap[fileKey] = ComputeHash(fp);
					needCompress = true;
				}

				if (needCompress || !File.Exists(compressedPath))
					File.WriteAllBytes(compressedPath, ZlibStream.CompressBuffer(File.ReadAllBytes(fp)));

				int compressedSize = (int)(new FileInfo(compressedPath).Length);
				int fileSize = (int)(new FileInfo(fp).Length);

				if (currentPackageSize >= MaxPackageSize)
				{
					currentPackageSize = 0;
					packageId++;
				}
				currentPackageSize += compressedSize;

				idxEntries.Add(new IdxEntry
				{
					Index = i,
					Offset = 0,
					CompressedSize = compressedSize,
					FileSize = fileSize,
					FileName = fileName.ToLower(),
					FilePath = fileDir.ToLower(),
					PackageId = packageId,
					CompressedFilePath = compressedPath
				});

				uipath = fileDir;
				uiname = fileName;
				uinum = i + 1;
			}

			WriteIdxFile(Path.Combine(outputDir, "pkg.idx"), idxEntries);
			WritePkgFiles(outputDir, idxEntries);
			WriteSpFile(outputDir);
			SaveMD5Map();
			done = true;
		}

		private void WriteIdxFile(string idxPath, List<IdxEntry> entries)
		{
			using (var fs = new FileStream(idxPath, FileMode.Create, FileAccess.ReadWrite))
			{
				fs.Write(new byte[292], 0, 292);

				for (int i = 0; i < entries.Count; i++)
				{
					var e = entries[i];
					e.IndexOffset = (int)fs.Position;

					fs.Write(BitConverter.GetBytes(e.Index), 0, 4);
					fs.Write(BitConverter.GetBytes(e.Offset), 0, 4);
					fs.Write(BitConverter.GetBytes(e.IndexOffset), 0, 4);
					fs.Write(BitConverter.GetBytes(e.CompressedSize), 0, 4);
					fs.Write(new byte[8], 0, 8);
					fs.Write(new byte[8], 0, 8);
					fs.Write(new byte[8], 0, 8);
					fs.Write(new byte[8], 0, 8);
					fs.Write(new byte[4], 0, 4);
					fs.Write(BitConverter.GetBytes((uint)1), 0, 4);
					fs.Write(BitConverter.GetBytes(e.FileSize), 0, 4);

					byte[] fnBytes = Encoding.UTF8.GetBytes(e.FileName);
					fs.Write(fnBytes, 0, fnBytes.Length);
					fs.Write(new byte[260 - fnBytes.Length], 0, 260 - fnBytes.Length);

					byte[] fpBytes = Encoding.UTF8.GetBytes(e.FilePath);
					fs.Write(fpBytes, 0, fpBytes.Length);
					fs.Write(new byte[260 - fpBytes.Length], 0, 260 - fpBytes.Length);

					fs.Write(new byte[4], 0, 4);
					fs.Write(BitConverter.GetBytes(e.PackageId), 0, 4);
					fs.Write(new byte[4], 0, 4);
				}
			}
		}

		private void WritePkgFiles(string outputDir, List<IdxEntry> entries)
		{
			string idxPath = Path.Combine(outputDir, "pkg.idx");
			var grouped = entries.GroupBy(e => e.PackageId).OrderBy(g => g.Key);

			using (var idxFs = new FileStream(idxPath, FileMode.Open, FileAccess.ReadWrite))
			{
				foreach (var group in grouped)
				{
					string pkgPath = Path.Combine(outputDir, GetPackageFileName(group.Key));
					using (var pkgFs = new FileStream(pkgPath, FileMode.Create, FileAccess.Write))
					{
						foreach (var entry in group)
						{
							byte[] data = File.ReadAllBytes(entry.CompressedFilePath);
							int realOffset = (int)pkgFs.Position;
							pkgFs.Write(data, 0, data.Length);

							idxFs.Seek(entry.IndexOffset + 4, SeekOrigin.Begin);
							idxFs.Write(BitConverter.GetBytes(realOffset), 0, 4);
							entry.Offset = realOffset;
						}
					}
				}
			}
		}

		private string GetPackageFileName(int packageId)
		{
			if (packageId < 10) return "pkg00" + packageId + ".pkg";
			if (packageId < 100) return "pkg0" + packageId + ".pkg";
			return "pkg" + packageId + ".pkg";
		}

		private void WriteSpFile(string outputDir)
		{
			string spPath = Path.Combine(outputDir, "pkg.sp");
			string[] pkgFiles = Directory.GetFiles(outputDir, "pkg???.pkg");

			using (var fs = new FileStream(spPath, FileMode.Create, FileAccess.Write))
			{
				for (int i = 0; i < pkgFiles.Length; i++)
				{
					int id = i + 1;
					int size = (int)(new FileInfo(pkgFiles[i]).Length);
					fs.Write(BitConverter.GetBytes(id), 0, 4);
					fs.Write(BitConverter.GetBytes(size), 0, 4);
					fs.Write(new byte[4], 0, 4);
				}
			}
		}
}
}
