using iTextSharp.text;
using iTextSharp.text.pdf;
using mangaRenamer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
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
        //public string mangaName { get; set; }
        private string mangaName = "out"; 

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            System.Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = string.Format("Manga Tools {0}.{1}.{2} (by RJ Regalado)", version.Major, version.Minor, version.Build);

            dataGridView1.DataSource = null;
            directoryList.Clear();
            fileList.Clear();
            txtPathFrom.Text = mangaRenamer.Properties.Settings.Default.src;
            txtPathTo.Text = mangaRenamer.Properties.Settings.Default.dest;
            txtPathFrom.ReadOnly = true;
            txtPathTo.ReadOnly = true;
            cboChapterNumberRegEx.Items.Add("(.*)");
            cboChapterTitleRegEx.Items.Add("(.*)");
            cboChapterNumberRegEx.SelectedIndex = 0;
            cboChapterTitleRegEx.SelectedIndex = 0;

            string[] pageSizeArray =
            {
                "LETTER", "ARCH_B",  "ARCH_A", "FLSA",  "FLSE",  "HALFLETTER",  "_11X17",  "ID_1",  "ID_2",  "ID_3",  "LEDGER",
                "CROWN_QUARTO",  "ARCH_C",  "LARGE_CROWN_QUARTO",  "ROYAL_QUARTO",  "CROWN_OCTAVO",  "LARGE_CROWN_OCTAVO",
                "DEMY_OCTAVO",  "ROYAL_OCTAVO",  "SMALL_PAPERBACK",  "PENGUIN_SMALL_PAPERBACK",  "PENGUIN_LARGE_PAPERBACK",
                "LETTER_LANDSCAPE",  "LEGAL_LANDSCAPE",  "A4_LANDSCAPE",  "DEMY_QUARTO",  "ARCH_D",  "ARCH_E",  "A7",  "TABLOID", 
                "EXECUTIVE",  "POSTCARD",  "A0",  "A1",  "A2",  "A3",  "A4",  "A5",  "A6",  "B10",  "LEGAL",  "A8",  "A10", 
                 "B0",  "B1",  "B2",  "B3",  "B4",  "B5",  "B6",  "B7",  "B8",  "B9",  "A9",  "NOTE"
            };

            Array.Sort(pageSizeArray);

            cboPaperSize.Items.AddRange(pageSizeArray);
            cboPaperSize.SelectedIndex = 6; // A4


#if DEBUG
            {
                txtPathFrom.Text = @"D:\Profile\Documents\Mangas\Shokugeki no Soma";
                txtPathTo.Text = @"D:\Profile\Documents\Mangas\out";
            }
#endif

        }

        private void btnAddQueue_Click(object sender, EventArgs e)
        {
            try
            {
                toggleUI(false);

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

                this.mangaName = txtPathFrom.Text.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Last();
                
                dataGridView1.DataSource = null;
                fileList.Clear();
                directoryList.Clear();

                string regexMatch_sorting = ((cboChapterNumberRegEx.SelectedIndex == -1) ? cboChapterNumberRegEx.Text : cboChapterNumberRegEx.SelectedItem.ToString());
                string regexMatch_chapterTitle = ((cboChapterTitleRegEx.SelectedIndex == -1) ? cboChapterTitleRegEx.Text : cboChapterTitleRegEx.SelectedItem.ToString());

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
                            var regexMatches_temp = Regex.Matches(directoryName, regexMatch_sorting);
                            directory_tmp.sorting = regexMatches_temp[0].Groups[Convert.ToInt32(txtChapterNumberRegExGroup.Text)].Value.Trim();

                            
                            if (chkChapterTitleEnabled.Enabled)
                            {
                                regexMatches_temp = Regex.Matches(directoryName, regexMatch_chapterTitle);
                                try
                                {
                                    directory_tmp.chapterTitle = regexMatches_temp[0].Groups[Convert.ToInt32(txtChapterTitleRegExGroup.Text)].Value.Trim();
                                } catch
                                {
                                    directory_tmp.chapterTitle = null;
                                }
                            }
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

                    var file_tmp = new filesModel();
                    foreach (var directory_item in directoryList)
                    {
                        int fileCounter_sort = 1;
                        foreach (var file_item in Directory.GetFiles(directory_item.directoryName))
                        {
                            if (chkChapterTitleEnabled.Enabled)
                            {
                                file_tmp.chapterTitle = directory_item.chapterTitle;
                            }
                            file_tmp.sorting = string.Format("Chapter {0} {1}", directory_item.sorting, fileCounter_sort);
                            file_tmp.from = file_item;
                            //file_tmp.to = string.Format("{0}\\{1}{2}", txtPathTo.Text, fileCounter.ToString().PadLeft(7, '0'), Path.GetExtension(file_item));
                            file_tmp.to = string.Format("{0}\\{1}\\{2}{3}", txtPathTo.Text, this.mangaName, fileCounter.ToString().PadLeft(7, '0'), Path.GetExtension(file_item));
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

                });

                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate (object o, RunWorkerCompletedEventArgs args)
                {

                    toggleUI(true);

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
                toggleUI(false);

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
                            if (!Directory.Exists(Path.GetDirectoryName(row.Cells["to"].Value.ToString())))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(row.Cells["to"].Value.ToString()));
                            }

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
                    toggleUI(true);

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
                    btnGenerate_Click("hideAlert", null);
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
                    btnGenerate_Click("hideAlert", null);
                    btnAddQueue_Click(null, null);
                }
            }
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            toggleUI(false);

            var param = new List<string>();
            foreach (var item in fileList)
            {
                param.Add(item.from);
            }

            string paperSize = cboPaperSize.Text;

            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                try
                {
                    var byteArray = ConvertIntoSinglePDF(param, ref bw, paperSize);

                    if (chkAddPageNumber.Checked)
                    {
                        byteArray = AddPageNumbers(byteArray, ref bw);
                    }
                    
                    //using (var fs = new FileStream(txtPathTo.Text + @"\out" + DateTime.Now.Ticks.ToString() + ".pdf", FileMode.Create, FileAccess.Write))

                    if (string.IsNullOrEmpty(this.mangaName))
                    {
                        this.mangaName = "out";
                    }

                    using (var fs = new FileStream(string.Format("{0}\\{1}_{2}.pdf", txtPathTo.Text, this.mangaName, DateTime.Now.Ticks.ToString()), FileMode.Create, FileAccess.Write))
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
                toggleUI(true);

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

        private Document setPageSize(ref  Document doc, string paperSize)
        {
            //doc.SetPageSize(PageSize.A4);

            if (paperSize == "LETTER") { doc.SetPageSize(PageSize.LETTER); }
            if (paperSize == "ARCH_B") { doc.SetPageSize(PageSize.ARCH_B); }
            if (paperSize == "ARCH_A") { doc.SetPageSize(PageSize.ARCH_A); }
            if (paperSize == "FLSA") { doc.SetPageSize(PageSize.FLSA); }
            if (paperSize == "FLSE") { doc.SetPageSize(PageSize.FLSE); }
            if (paperSize == "HALFLETTER") { doc.SetPageSize(PageSize.HALFLETTER); }
            if (paperSize == "_11X17") { doc.SetPageSize(PageSize._11X17); }
            if (paperSize == "ID_1") { doc.SetPageSize(PageSize.ID_1); }
            if (paperSize == "ID_2") { doc.SetPageSize(PageSize.ID_2); }
            if (paperSize == "ID_3") { doc.SetPageSize(PageSize.ID_3); }
            if (paperSize == "LEDGER") { doc.SetPageSize(PageSize.LEDGER); }
            if (paperSize == "CROWN_QUARTO") { doc.SetPageSize(PageSize.CROWN_QUARTO); }
            if (paperSize == "ARCH_C") { doc.SetPageSize(PageSize.ARCH_C); }
            if (paperSize == "LARGE_CROWN_QUARTO") { doc.SetPageSize(PageSize.LARGE_CROWN_QUARTO); }
            if (paperSize == "CROWN_OCTAVO") { doc.SetPageSize(PageSize.CROWN_OCTAVO); }
            if (paperSize == "LARGE_CROWN_OCTAVO") { doc.SetPageSize(PageSize.LARGE_CROWN_OCTAVO); }
            if (paperSize == "DEMY_OCTAVO") { doc.SetPageSize(PageSize.DEMY_OCTAVO); }
            if (paperSize == "ROYAL_OCTAVO") { doc.SetPageSize(PageSize.ROYAL_OCTAVO); }
            if (paperSize == "SMALL_PAPERBACK") { doc.SetPageSize(PageSize.SMALL_PAPERBACK); }
            if (paperSize == "PENGUIN_SMALL_PAPERBACK") { doc.SetPageSize(PageSize.PENGUIN_SMALL_PAPERBACK); }
            if (paperSize == "PENGUIN_LARGE_PAPERBACK") { doc.SetPageSize(PageSize.PENGUIN_LARGE_PAPERBACK); }
            if (paperSize == "LETTER_LANDSCAPE") { doc.SetPageSize(PageSize.LETTER_LANDSCAPE); }
            if (paperSize == "LEGAL_LANDSCAPE") { doc.SetPageSize(PageSize.LEGAL_LANDSCAPE); }
            if (paperSize == "A4_LANDSCAPE") { doc.SetPageSize(PageSize.A4_LANDSCAPE); }
            if (paperSize == "DEMY_QUARTO") { doc.SetPageSize(PageSize.DEMY_QUARTO); }
            if (paperSize == "ARCH_D") { doc.SetPageSize(PageSize.ARCH_D); }
            if (paperSize == "ARCH_E") { doc.SetPageSize(PageSize.ARCH_E); }
            if (paperSize == "A7") { doc.SetPageSize(PageSize.A7); }
            if (paperSize == "TABLOID") { doc.SetPageSize(PageSize.TABLOID); }
            if (paperSize == "EXECUTIVE") { doc.SetPageSize(PageSize.EXECUTIVE); }
            if (paperSize == "POSTCARD") { doc.SetPageSize(PageSize.POSTCARD); }
            if (paperSize == "A0") { doc.SetPageSize(PageSize.A0); }
            if (paperSize == "A1") { doc.SetPageSize(PageSize.A1); }
            if (paperSize == "A2") { doc.SetPageSize(PageSize.A2); }
            if (paperSize == "A3") { doc.SetPageSize(PageSize.A3); }
            if (paperSize == "A4") { doc.SetPageSize(PageSize.A4); }
            if (paperSize == "A5") { doc.SetPageSize(PageSize.A5); }
            if (paperSize == "A6") { doc.SetPageSize(PageSize.A6); }
            if (paperSize == "B10") { doc.SetPageSize(PageSize.B10); }
            if (paperSize == "LEGAL") { doc.SetPageSize(PageSize.LEGAL); }
            if (paperSize == "A8") { doc.SetPageSize(PageSize.A8); }
            if (paperSize == "A10") { doc.SetPageSize(PageSize.A10); }
            if (paperSize == "B0") { doc.SetPageSize(PageSize.B0); }
            if (paperSize == "B1") { doc.SetPageSize(PageSize.B1); }
            if (paperSize == "B2") { doc.SetPageSize(PageSize.B2); }
            if (paperSize == "B3") { doc.SetPageSize(PageSize.B3); }
            if (paperSize == "B4") { doc.SetPageSize(PageSize.B4); }
            if (paperSize == "B5") { doc.SetPageSize(PageSize.B5); }
            if (paperSize == "B6") { doc.SetPageSize(PageSize.B6); }
            if (paperSize == "B7") { doc.SetPageSize(PageSize.B7); }
            if (paperSize == "B8") { doc.SetPageSize(PageSize.B8); }
            if (paperSize == "B9") { doc.SetPageSize(PageSize.B9); }
            if (paperSize == "A9") { doc.SetPageSize(PageSize.A9); }
            if (paperSize == "NOTE") { doc.SetPageSize(PageSize.NOTE); }

            return doc;
        }

        public byte[] ConvertIntoSinglePDF(List<string> filePaths, ref BackgroundWorker bw, string paperSize)
        {
            Document doc = new Document();
            Document imageDocument = null;
            PdfWriter imageDocumentWriter = null;

            doc = setPageSize(ref doc, paperSize);

            //doc.SetPageSize(PageSize.A4);


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

                if (
                    (Path.GetExtension(filePaths[i]).ToLower().Trim('.') == "jpg")
                    || (Path.GetExtension(filePaths[i]).ToLower().Trim('.') == "jpeg")
                    || (Path.GetExtension(filePaths[i]).ToLower().Trim('.') == "png")
                    || (Path.GetExtension(filePaths[i]).ToLower().Trim('.') == "gif")
                    || (Path.GetExtension(filePaths[i]).ToLower().Trim('.') == "tif")
                    || (Path.GetExtension(filePaths[i]).ToLower().Trim('.') == "tiff")
                    )
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
                                try
                                {
                                    using (System.Drawing.Image img = System.Drawing.Image.FromStream(new MemoryStream(data)))
                                    {
                                        var convertedImageMS = new MemoryStream();
                                        img.Save(convertedImageMS, ImageFormat.Jpeg);
                                        data = convertedImageMS.ToArray();
                                        convertedImageMS.Dispose();
                                    }
                                    image = iTextSharp.text.Image.GetInstance(data);
                                }
                                catch
                                {
                                    continue;
                                }
                            }

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
                    (float)((document.PageSize.Height * 0.01))
                    , 0);
                cb.EndText();
            }
            document.Close();
            return ms.ToArray();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            toggleUI(false);

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
                    generatedRegExs.Add(Regex.Replace(regex1, "(=?\\d{1,})", "(=?\\d{1,})"));
                }
            });

            bw_generateRegEx.ProgressChanged += new ProgressChangedEventHandler(delegate (object o, ProgressChangedEventArgs args)
            {

            });

            bw_generateRegEx.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate (object o, RunWorkerCompletedEventArgs args)
            {
                foreach (var item in generatedRegExs)
                {
                    if (!cboChapterNumberRegEx.Items.Contains(item))
                    {
                        cboChapterNumberRegEx.Items.Add(item);
                    }
                }

                string regEx2 = "";
                foreach (var item in cboChapterNumberRegEx.Items)
                {
                    if ((string)item == "(.*)")
                    {
                        continue;
                    }
                    regEx2 = string.Format("{0}|{1}", regEx2, item);
                }
                cboChapterNumberRegEx.Items.Add(regEx2.Substring(1));

                if (args.Error != null)
                {
                    MessageBox.Show(args.Error.Message, "Manga Renamer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (sender.ToString() != "hideAlert")
                    {
                        MessageBox.Show("Completed", "Manga Renamer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                toggleUI(true);

            });

            bw_generateRegEx.RunWorkerAsync();
        }

        private void cboRegEx_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtChapterNumberRegExGroup.Text = populateRegExGroup().ToString();
        }

        private int populateRegExGroup()
        {
            string selectedRegEx = ((cboChapterNumberRegEx.SelectedIndex == -1) ? cboChapterNumberRegEx.Text : cboChapterNumberRegEx.SelectedItem.ToString());

            string regexPatern = "(=?\\(\\.\\*\\))";
            regexPatern += "|(=?\\(\\=\\?\\\\d\\{1\\,\\}\\))";
            regexPatern += "|(=?\\(\\=\\?\\\\d\\{1\\,\\d\\}\\))";

            return Regex.Matches(selectedRegEx, regexPatern).Count;
        }

        private void chkChapterTitleEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChapterTitleEnabled.Checked)
            {
                cboChapterTitleRegEx.Enabled = true;
                txtChapterTitleRegExGroup.Enabled = true;
                btnGenerateTitleRegEx.Enabled = true;
            } else
            {
                cboChapterTitleRegEx.Enabled = false;
                txtChapterTitleRegExGroup.Enabled = false;
                btnGenerateTitleRegEx.Enabled = false;
            }
        }

        private void btnGenerateTitleRegEx_Click(object sender, EventArgs e)
        {

        }

        private void cboChapterTitleRegEx_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtChapterTitleRegExGroup.Text = populateRegExGroup().ToString();
        }

        private void toggleUI(bool trigger)
        {
            btnAddQueue.Enabled = trigger;
            btnClear.Enabled = trigger;
            btnCopy.Enabled = trigger;
            btnExportPdf.Enabled = trigger;
            btnBrowseFrom.Enabled = trigger;
            btnBrowseTo.Enabled = trigger;
            cboChapterNumberRegEx.Enabled = trigger;
            txtChapterNumberRegExGroup.Enabled = trigger;
            btnGenerate.Enabled = trigger;
            cboPaperSize.Enabled = trigger;

            chkChapterTitleEnabled.Enabled = trigger;

            //cboChapterTitleRegEx.Enabled = trigger;
            //txtChapterTitleRegExGroup.Enabled = trigger;
            //btnGenerateTitleRegEx.Enabled = trigger;

            cboChapterTitleRegEx.Enabled = false;
            txtChapterTitleRegExGroup.Enabled = false;
            btnGenerateTitleRegEx.Enabled = false;

            if (trigger)
            {
                if (chkChapterTitleEnabled.Checked)
                {
                    cboChapterTitleRegEx.Enabled = true;
                    txtChapterTitleRegExGroup.Enabled = true;
                    btnGenerateTitleRegEx.Enabled = true;
                }
            }
        }
    }
}
