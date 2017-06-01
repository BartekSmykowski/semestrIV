using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;

namespace Lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Tasks_Click(object sender, RoutedEventArgs e)
        {
            int N = Int32.Parse(NTextField.Text);
            int K = Int32.Parse(KTextField.Text);
            var tuple = new Tuple<int, int>(N, K);
            Task<int> taskLicznik = licznikAsync(tuple);

            Task<int> taskMianownik = mianownikAsync(K);

            int licznik = taskLicznik.Result;
            int mianownik = taskMianownik.Result;

            TasksTextField.Text = (licznik / mianownik).ToString();


        }

        private void Delegates_Click(object sender, RoutedEventArgs e)
        {
            int N = Int32.Parse(NTextField.Text);
            int K = Int32.Parse(KTextField.Text);
            Func<int, int, int> licznikDelegate = NewtonResolver.licznik;
            Func<int, int> mianownikDelegate = NewtonResolver.mianownik;
            IAsyncResult resultLicznik = licznikDelegate.BeginInvoke(N, K, null, null);
            IAsyncResult resultMianownik = mianownikDelegate.BeginInvoke(K, null, null);
            //while(resultLicznik.IsCompleted == false || resultMianownik.IsCompleted == false)
            //{

            //}

            int licznik = licznikDelegate.EndInvoke(resultLicznik);
            int mianownik = mianownikDelegate.EndInvoke(resultMianownik);

            DelegatesTextField.Text = (licznik / mianownik).ToString();

        }

        private async void AsyncAwait_Click(object sender, RoutedEventArgs e)
        {
            int N = Int32.Parse(NTextField.Text);
            int K = Int32.Parse(KTextField.Text);

            int licznik = await licznikAsync(new Tuple<int, int>(N,K));
            int mianownik = await mianownikAsync(K);

            AsyncAwaitTextField.Text = (licznik / mianownik).ToString();
        }

        private Task<int> licznikAsync(Tuple<int, int> input)
        {
            Task<int> taskLicznik = Task.Factory.StartNew<int>((param) =>
            {
                Tuple<int, int> paramIn = (Tuple<int, int>)param;
                return NewtonResolver.licznik(paramIn.Item1, paramIn.Item2);
            }, input);
            return taskLicznik;
        }

        private Task<int> mianownikAsync(int K)
        {
            return Task.Factory.StartNew<int>((param) => NewtonResolver.mianownik((int)param), K);
        }

        private void GET_Click(object s, RoutedEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += ((object sender, DoWorkEventArgs args) =>
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                int n = (int)args.Argument; //Extract the argument 
                int result = 1; //Performlongrunningprocess 
                int resultPrev = 0;
                for (int i = 1; i < n; i++)
                {
                    int tmp = result;
                    result += resultPrev;
                    resultPrev = tmp;
                    worker.ReportProgress((int)((double)(i+1)/n*100));
                    Thread.Sleep(20);
                }
                args.Result = result;
            });
            bw.ProgressChanged += ((object sender, ProgressChangedEventArgs args) =>
             { //UpdatelabelinUI withprogress 
                FibonacciProgressBar.Value = args.ProgressPercentage;
             });
            bw.RunWorkerCompleted += ((object sender, RunWorkerCompletedEventArgs args) =>
             { //Updatetheuserinterface 
                FibonacciResultTextField.Text = args.Result.ToString();
             });
            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync(Int32.Parse(iTextField.Text));
        }

        private void Compress_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            string selectedPath = folderBrowserDialog.SelectedPath;
            DirectoryInfo selectedDir = new DirectoryInfo(selectedPath);
            Parallel.ForEach(selectedDir.GetFiles(), (toCompress) =>
            {
                using (FileStream toCompressStream = toCompress.OpenRead())
                {
                    using (FileStream compressedFileStream = File.Create(toCompress.FullName + ".gz"))
                    {
                        using (GZipStream compressionStream = new GZipStream(
                            compressedFileStream, CompressionMode.Compress))
                        {
                            toCompressStream.CopyTo(compressionStream);
                        }
                    }
                }
            });
        }

        private void Decompress_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            string selectedPath = folderBrowserDialog.SelectedPath;
            DirectoryInfo selectedDir = new DirectoryInfo(selectedPath);
            Parallel.ForEach(selectedDir.GetFiles(), (toDecompress) => {
                if (toDecompress.Extension == ".gz")
                {
                    using (FileStream compressedStream = toDecompress.OpenRead())
                    {
                        string name = toDecompress.FullName;
                        string newName = name.Remove(name.Length - toDecompress.Extension.Length);

                        using (FileStream decompressedStream = File.Create(newName))
                        {
                            using (GZipStream decompressionStream = new GZipStream(
                                compressedStream, CompressionMode.Decompress))
                            {
                                decompressionStream.CopyTo(decompressedStream);
                            }
                        }
                    }
                }
            });
        }

        private void ResolveDNS_Click(object sender, RoutedEventArgs e)
        {
            string[] hostNames = {
                "www.microsoft.com",
                "www.apple.com", "www.google.com",
                "www.ibm.com", "cisco.netacad.net",
                "www.oracle.com", "www.nokia.com",
                "www.hp.com", "www.dell.com",
                "www.samsung.com", "www.toshiba.com",
                "www.siemens.com", "www.amazon.com",
                "www.sony.com", "www.canon.com",
                "www.alcatel-lucent.com", "www.acer.com",
                "www.motorola.com"
            };

            DNSTextBlock.Text = hostNames.AsParallel()
                .Select(h => new { Host = h, IP = Dns.GetHostAddresses(h)[0] })
                .Aggregate("", (accum, host) => accum + host.Host + " => " + host.IP + "\r\n");
        }

        private void CheckResponsibility_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
