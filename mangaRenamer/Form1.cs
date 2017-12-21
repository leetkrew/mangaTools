using iTextSharp.text;
using iTextSharp.text.pdf;
using mangaRenamer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace mangaRenamer
{
    public partial class Form1 : Form
    {
        List<filesModel> fileList = new List<filesModel>();
        List<directoryModel> directoryList = new List<directoryModel>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            directoryList.Clear();
            fileList.Clear();
            txtPathFrom.Text = mangaRenamer.Properties.Settings.Default.src;
            txtPathTo.Text = mangaRenamer.Properties.Settings.Default.dest;
            txtPathFrom.ReadOnly = true;
            txtPathTo.ReadOnly = true;
            cboRegEx.Items.Add("(.*)");
            cboRegEx.SelectedIndex = 0;

        }

        private void btnAddQueue_Click(object sender, EventArgs e)
        {
            try
            {

                btnAddQueue.Enabled = false;
                btnClear.Enabled = false;
                btnCopy.Enabled = false;
                btnExportPdf.Enabled = false;
                btnBrowseFrom.Enabled = false;
                btnBrowseTo.Enabled = false;
                cboRegEx.Enabled = false;
                txtGroup.Enabled = false;
                btnGenerate.Enabled = false;

                if (txtPathFrom.Text == txtPathTo.Text)
                {
                    throw new Exception("Source and destination directory must be different");
                }

                if (string.IsNullOrEmpty(txtPathFrom.Text))
                {
                    throw new Exception("Source directory is empty");
                }

                if (string.IsNullOrEmpty(txtPathTo.Text))
                {
                    throw new Exception("Destination directory is empty");
                }

                dataGridView1.DataSource = null;
                fileList.Clear();
                directoryList.Clear();

                




                string regexMatch = ((cboRegEx.SelectedIndex == -1) ? cboRegEx.Text : cboRegEx.SelectedItem.ToString());
                
                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerReportsProgress = true;
                bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {

                    var directory_tmp = new directoryModel();
                    foreach (var item in Directory.GetDirectories(txtPathFrom.Text))
                    {
                        try
                        {
                            var directoryName = item.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Last();
                            var regexMatches_temp = Regex.Matches(directoryName, regexMatch);
                            directory_tmp.sorting = regexMatches_temp[0].Groups[Convert.ToInt32(txtGroup.Text)].Value;
                        }
                        catch
                        {
                            continue;
                        }

                        directory_tmp.directoryName = item;
                        directoryList.Add(directory_tmp);
                        directory_tmp = new directoryModel();
                    }

                    fileList.Clear();

                    int fileCounter = 1;

                    directoryList = directoryList.OrderBy(x => x.sorting, new NaturalStringComparer()).ToList();
                    //directoryList = directoryList.OrderBy(x => x.directoryName, new NaturalStringComparer()).ToList();

                    var file_tmp = new filesModel();
                    foreach (var directory_item in directoryList)
                    {
                        int fileCounter_sort = 1;
                        foreach (var file_item in Directory.GetFiles(directory_item.directoryName))
                        {
                            file_tmp.sorting = string.Format("{0}.{1}", directory_item.sorting, fileCounter_sort);
                            file_tmp.from = file_item;
                            file_tmp.to = string.Format("{0}\\{1}{2}", txtPathTo.Text, fileCounter.ToString().PadLeft(7, '0'), Path.GetExtension(file_item));
                            file_tmp.pageNo = fileCounter;
                            fileCounter++;
                            fileCounter_sort++;
                            fileList.Add(file_tmp);
                            file_tmp = new filesModel();
                        }
                    }

                    fileList = fileList.OrderBy(x => x.sorting, new NaturalStringComparer()).ToList();
                });

                bw.ProgressChanged += new ProgressChangedEventHandler(delegate (object o, ProgressChangedEventArgs args)
                {

                    //progressBar1.Value = args.ProgressPercentage;
                });

                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate (object o, RunWorkerCompletedEventArgs args)
                {

                    btnAddQueue.Enabled = true;
                    btnClear.Enabled = true;
                    btnCopy.Enabled = true;
                    btnExportPdf.Enabled = true;
                    btnBrowseFrom.Enabled = true;
                    btnBrowseTo.Enabled = true;
                    cboRegEx.Enabled = true;
                    txtGroup.Enabled = true;
                    btnGenerate.Enabled = true;


                    if (args.Error != null)
                    {
                        fileList.Clear();
                        directoryList.Clear();
                        dataGridView1.DataSource = null;
                        MessageBox.Show(args.Error.Message, "Manga Renamer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        dataGridView1.DataSource = fileList;
                        dataGridView1.AutoResizeColumns();
                        progressBar1.Minimum = 0;
                        progressBar1.Maximum = fileList.Count;

                    }

                });

                bw.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                directoryList.Clear();
                fileList.Clear();
                dataGridView1.DataSource = null;
                MessageBox.Show(ex.Message, "Manga Renamer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                btnAddQueue.Enabled = false;
                btnClear.Enabled = false;
                btnCopy.Enabled = false;
                btnExportPdf.Enabled = false;
                btnBrowseFrom.Enabled = false;
                btnBrowseTo.Enabled = false;
                cboRegEx.Enabled = false;
                txtGroup.Enabled = false;
                btnGenerate.Enabled = false;

                if (!Directory.Exists(txtPathTo.Text))
                {
                    Directory.CreateDirectory(txtPathTo.Text);
                }

                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerReportsProgress = true;
                bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    int cnt = 0;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        try
                        {
                            File.Copy(row.Cells["from"].Value.ToString(), row.Cells["to"].Value.ToString());
                            cnt++;
                            bw.ReportProgress(cnt);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                });

                bw.ProgressChanged += new ProgressChangedEventHandler(delegate (object o, ProgressChangedEventArgs args)
                {
                    progressBar1.Value = args.ProgressPercentage;
                });

                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate (object o, RunWorkerCompletedEventArgs args)
                {
                    btnAddQueue.Enabled = true;
                    btnClear.Enabled = true;
                    btnCopy.Enabled = true;
                    btnExportPdf.Enabled = true;
                    btnBrowseFrom.Enabled = true;
                    btnBrowseTo.Enabled = true;
                    cboRegEx.Enabled = true;
                    txtGroup.Enabled = true;
                    btnGenerate.Enabled = true;

                    progressBar1.Value = 0;
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.Message, "Manga Renamer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Completed", "Manga Renamer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                });

                bw.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Manga Renamer" + ex.HResult, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            fileList.Clear();
            directoryList.Clear();
            dataGridView1.DataSource = null;

        }

        private void btnBrowseFrom_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.SelectedPath = mangaRenamer.Properties.Settings.Default.src;
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    mangaRenamer.Properties.Settings.Default.src = folderDialog.SelectedPath;
                    mangaRenamer.Properties.Settings.Default.Save();
                    txtPathFrom.Text = folderDialog.SelectedPath;
                    btnAddQueue_Click(this.btnBrowseFrom, null);
                }
            }
        }

        private void btnBrowseTo_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.SelectedPath = mangaRenamer.Properties.Settings.Default.dest;
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    mangaRenamer.Properties.Settings.Default.dest = folderDialog.SelectedPath;
                    mangaRenamer.Properties.Settings.Default.Save();
                    txtPathTo.Text = folderDialog.SelectedPath;
                    btnAddQueue_Click(null, null);
                }
            }
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            btnAddQueue.Enabled = false;
            btnClear.Enabled = false;
            btnCopy.Enabled = false;
            btnExportPdf.Enabled = false;
            btnBrowseFrom.Enabled = false;
            btnBrowseTo.Enabled = false;
            cboRegEx.Enabled = false;
            txtGroup.Enabled = false;
            btnGenerate.Enabled = false;

            var param = new List<string>();
            foreach (var item in fileList)
            {
                param.Add(item.from);
            }

            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                try
                {
                    var byteArray = ConvertIntoSinglePDF(param, ref bw);
                    byteArray = AddPageNumbers(byteArray, ref bw);
                    using (var fs = new FileStream(txtPathTo.Text + @"\out" + DateTime.Now.Ticks.ToString() + ".pdf", FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(byteArray, 0, byteArray.Length);
                    }

                }
                catch
                {
                    throw;
                }
            });

            bw.ProgressChanged += new ProgressChangedEventHandler(delegate (object o, ProgressChangedEventArgs args)
            {
                progressBar1.Value = args.ProgressPercentage;
            });

            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate (object o, RunWorkerCompletedEventArgs args)
            {
                btnAddQueue.Enabled = true;
                btnClear.Enabled = true;
                btnCopy.Enabled = true;
                btnExportPdf.Enabled = true;
                btnBrowseFrom.Enabled = true;
                btnBrowseTo.Enabled = true;
                cboRegEx.Enabled = true;
                txtGroup.Enabled = true;
                btnGenerate.Enabled = true;

                progressBar1.Value = 0;

                if (args.Error != null)
                {
                    MessageBox.Show(args.Error.Message, "Manga Renamer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Completed", "Manga Renamer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });

            bw.RunWorkerAsync();

        }



        public static byte[] ConvertIntoSinglePDF(List<string> filePaths, ref BackgroundWorker bw)
        {
            Document doc = new Document();
            Document imageDocument = null;
            PdfWriter imageDocumentWriter = null;

            doc.SetPageSize(PageSize.A4);
            doc.SetMargins(0, 0, 0, 0);

            var ms = new MemoryStream();

            PdfCopy pdf = new PdfCopy(doc, ms);
            doc.Open();


            for (int i = 0; i <= filePaths.Count() - 1; i++)
            {
                bw.ReportProgress(i);

                byte[] data = File.ReadAllBytes(filePaths[i]);
                doc.NewPage();
                imageDocument = null;
                imageDocumentWriter = null;

                if (Path.GetExtension(filePaths[i]).ToLower().Trim('.') == "jpg")
                {
                    Paragraph para = new Paragraph("rjregalado.com");
                    para.Alignment = Element.ALIGN_CENTER;
                    doc.Add(para);
                    imageDocument = new Document();
                    using (var imageMS = new MemoryStream())
                    {
                        imageDocumentWriter = PdfWriter.GetInstance(imageDocument, imageMS);

                        imageDocument.Open();
                        if (imageDocument.NewPage())
                        {
                            var image = iTextSharp.text.Image.GetInstance(data);

                            if (!image.IsJpeg())
                            {
                                continue;
                            }

                            image.Alignment = Element.ALIGN_CENTER;
                            image.UseVariableBorders = true;
                            image.CompressionLevel = PdfStream.BEST_COMPRESSION;

                            iTextSharp.text.Rectangle defaultPageSize = PageSize.A4;


                            float width = defaultPageSize.Width - doc.RightMargin - doc.LeftMargin;
                            float height = defaultPageSize.Height - doc.TopMargin - doc.BottomMargin;


                            float h = image.ScaledHeight;
                            float w = image.ScaledWidth;
                            float scalePercent;
                            float scaleReduction = 0.08F;

                            if (h > w)
                            {
                                if (h > doc.PageSize.Height)
                                {
                                    scalePercent = height / h;
                                    image.ScaleAbsolute((w * scalePercent) - (float)((w * scalePercent) * scaleReduction),
                                        (h * scalePercent) - (float)((h * scalePercent) * scaleReduction));

                                }
                            }
                            else
                            {
                                if (w > doc.PageSize.Width)
                                {
                                    scalePercent = width / w;
                                    image.ScaleAbsolute((w * scalePercent) - (float)((w * scalePercent) * scaleReduction),
                                        (h * scalePercent) - (float)((h * scalePercent) * scaleReduction));
                                }
                            }

                            if (!imageDocument.Add(image))
                            {
                                throw new Exception("Unable to add image to page!");
                            }

                            imageDocument.Close();
                            imageDocumentWriter.SetFullCompression();

                            imageDocumentWriter.Close();
                            PdfReader imageDocumentReader = new PdfReader(imageMS.ToArray());
                            var page = pdf.GetImportedPage(imageDocumentReader, 1);
                            pdf.AddPage(page);
                            imageDocumentReader.Close();
                        }
                    }

                }
            }


            if (doc.IsOpen()) doc.Close();

            return ms.ToArray();


        }

        public static BitmapSource ConvertBitmap(Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                            source.GetHbitmap(),
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
        }

        public static Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }

        private void btnExploreFrom_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", txtPathFrom.Text);
        }

        private void btnExploreTo_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", txtPathTo.Text);
        }

        public static byte[] AddPageNumbers(byte[] pdf, ref BackgroundWorker bw)
        {


            MemoryStream ms = new MemoryStream();
            PdfReader reader = new PdfReader(pdf);
            int n = reader.NumberOfPages;
            iTextSharp.text.Rectangle psize = reader.GetPageSize(1);

            Document document = new Document(psize, 50, 50, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);

            document.Open();
            PdfContentByte cb = writer.DirectContent;

            int p = 0;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                bw.ReportProgress(page);
                document.NewPage();
                p++;

                PdfImportedPage importedPage = writer.GetImportedPage(reader, page);
                cb.AddTemplate(importedPage, 0, 0);

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, string.Format("{0}", p), document.PageSize.Width / 2,

                    (float)
                    (
                        (document.PageSize.Height * 0.01)
                    )

                    , 0);
                cb.EndText();
            }
            document.Close();
            return ms.ToArray();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            btnAddQueue.Enabled = false;
            btnClear.Enabled = false;
            btnCopy.Enabled = false;
            btnExportPdf.Enabled = false;
            btnBrowseFrom.Enabled = false;
            btnBrowseTo.Enabled = false;
            cboRegEx.Enabled = false;
            txtGroup.Enabled = false;
            btnGenerate.Enabled = false;

            var generatedRegExs = new List<string>();
            BackgroundWorker bw_generateRegEx = new BackgroundWorker();
            bw_generateRegEx.WorkerReportsProgress = true;
            bw_generateRegEx.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                foreach (var item in Directory.GetDirectories(txtPathFrom.Text))
                {
                    var directoryName = item.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Last();
                    string regex1 = directoryName.Replace("[", "\\[")
                        .Replace("(", "\\(")
                        .Replace(")", "\\)")
                        .Replace("]", "\\]")
                        .Replace("{", "\\{")
                        .Replace("}", "\\}")
                        .Replace("?", "\\?")
                        .Replace("=", "\\=")
                        .Replace("*", "\\*")
                        .Replace(".", "\\.")
                        ;
                        generatedRegExs.Add(Regex.Replace(regex1, "(=?\\d{1,})", "(.*)"));
                }
            });

            bw_generateRegEx.ProgressChanged += new ProgressChangedEventHandler(delegate (object o, ProgressChangedEventArgs args)
            {
                //progressBar1.Value = args.ProgressPercentage;
            });

            bw_generateRegEx.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate (object o, RunWorkerCompletedEventArgs args)
            {
                foreach (var item in generatedRegExs)
                {
                    if (!cboRegEx.Items.Contains(item))
                    {
                        cboRegEx.Items.Add(item);
                    }
                }
                
                if (args.Error != null)
                {
                    MessageBox.Show(args.Error.Message, "Manga Renamer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Completed", "Manga Renamer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                btnAddQueue.Enabled = true;
                btnClear.Enabled = true;
                btnCopy.Enabled = true;
                btnExportPdf.Enabled = true;
                btnBrowseFrom.Enabled = true;
                btnBrowseTo.Enabled = true;
                cboRegEx.Enabled = true;
                txtGroup.Enabled = true;
                btnGenerate.Enabled = true;

            });

            bw_generateRegEx.RunWorkerAsync();
        }

        private void cboRegEx_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtGroup.Text = populateRegExGroup().ToString();
        }

        private int populateRegExGroup()
        {
            string regexMatch = ((cboRegEx.SelectedIndex == -1) ? cboRegEx.Text : cboRegEx.SelectedItem.ToString());
            return Regex.Matches(regexMatch, "(=?\\(\\.\\*\\))").Count;
        }

    }
}
