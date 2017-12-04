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
            //txtPathFrom.Text = @"D:\Profile\Documents\Mangas\Tantei Gakuen Q";
            //txtPathTo.Text = @"D:\Profile\Documents\Mangas\out";
            dataGridView1.DataSource = null;
            directoryList.Clear();
            fileList.Clear();
            txtPathFrom.Text = mangaRenamer.Properties.Settings.Default.src;
            txtPathTo.Text = mangaRenamer.Properties.Settings.Default.dest;
            txtPathFrom.ReadOnly = true;
            txtPathTo.ReadOnly = true;

        }

        private void btnAddQueue_Click(object sender, EventArgs e)
        {
            try
            {
                


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

                var directory_tmp = new directoryModel();
                foreach (var item in Directory.GetDirectories(txtPathFrom.Text))
                {
                    directory_tmp.directoryName = item;
                    directoryList.Add(directory_tmp);

                    directory_tmp = new directoryModel();
                }

                fileList.Clear();

                int fileCounter = 1;

                directoryList = directoryList.OrderBy(x => x.directoryName, new NaturalStringComparer()).ToList();

                var file_tmp = new filesModel();
                foreach (var directory_item in directoryList)
                {
                    foreach (var file_item in Directory.GetFiles(directory_item.directoryName))
                    {
                        file_tmp.from = file_item;
                        file_tmp.to = string.Format("{0}\\{1}{2}", txtPathTo.Text, fileCounter.ToString().PadLeft(7, '0'), Path.GetExtension(file_item));
                        file_tmp.pageNo = fileCounter;
                        fileCounter++;
                        fileList.Add(file_tmp);
                        file_tmp = new filesModel();
                    }
                }

                fileList = fileList.OrderBy(x => x.from, new NaturalStringComparer()).ToList();

                dataGridView1.DataSource = fileList;
                dataGridView1.AutoResizeColumns();

                progressBar1.Minimum = 0;
                progressBar1.Maximum = fileList.Count;
            } catch (Exception ex)
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
                    using (var fs = new FileStream(txtPathTo.Text + @"\out" + DateTime.Now.Ticks.ToString() + ".pdf", FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(byteArray, 0, byteArray.Length);
                    }

                } catch
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
            doc.SetPageSize(PageSize.A4);
            doc.SetMargins(0, 0, 0, 0);

            var ms = new MemoryStream();

            PdfCopy pdf = new PdfCopy(doc, ms);
            doc.Open();
            int cnt = 0;


            foreach (string path in filePaths)
            {
                bw.ReportProgress(cnt);
                cnt++;

                byte[] data = File.ReadAllBytes(path);
                doc.NewPage();
                Document imageDocument = null;
                PdfWriter imageDocumentWriter = null;

                if (Path.GetExtension(path).ToLower().Trim('.') == "jpg")
                {
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

                            //todo: resize page to image size
                            //bug: only affects the next page
                            //iTextSharp.text.Rectangle r = (width > defaultPageSize.Width
                            //                                || height > defaultPageSize.Height)
                            //                                  ? new iTextSharp.text.Rectangle(width, height)
                            //                                  : defaultPageSize;
                            //doc.SetPageSize(new iTextSharp.text.Rectangle(w, h));
                            //doc.NewPage();


                            //image.ScaleToFit(doc.PageSize.Width - 10, doc.PageSize.Height - 10);
                            //image.ScaleToFit(doc.PageSize.Width - 20, doc.PageSize.Height);

                            if (!imageDocument.Add(image))
                            {
                                throw new Exception("Unable to add image to page!");
                            }
                            imageDocument.Close();
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
    }
}
