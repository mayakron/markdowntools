using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarkdownPreviewer
{
    public partial class MainWindow : Form
    {
        private DateTime CssFileLastWriteTime = DateTime.MinValue;

        private string CssFilePath;

        private string DocumentTitle;

        private DateTime MarkdownFileLastWriteTime = DateTime.MinValue;

        private string MarkdownFilePath;

        private string ScrollY;

        private Timer Timer;

        private WebView2 WebView;

        public MainWindow()
        {
            InitializeComponent();

            Icon = IconUtility.GetExeIcon();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1180, 800);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Markdown Previewer";

            Load += MainWindowLoad;

            WebView = new WebView2
            {
                Dock = DockStyle.Fill
            };

            WebView.EnsureCoreWebView2Async(null);

            WebView.NavigationCompleted += WebViewNavigationCompleted;

            Timer = new Timer
            {
                Enabled = false,
                Interval = 1000
            };

            Timer.Tick += new EventHandler(this.TimerTick);

            Controls.Add(WebView);

            ResumeLayout(false);
        }

        private void MainWindowLoad(object sender, EventArgs e)
        {
            if (Program.Args.Length > 0)
            {
                MarkdownFilePath = Program.Args[0];
            }
            else
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Markdown file (*.md)|*.md"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    MarkdownFilePath = openFileDialog.FileName;
                }
            }

            if (Program.Args.Length > 1)
            {
                CssFilePath = Program.Args[1];
            }
            else
            {
                CssFilePath = Path.Combine(Application.StartupPath, "Styles", "Default.min.css");
            }

            if (Program.Args.Length > 2)
            {
                DocumentTitle = Program.Args[2];
            }
            else
            {
                DocumentTitle = Path.GetFileNameWithoutExtension(MarkdownFilePath);
            }

            if (!string.IsNullOrWhiteSpace(DocumentTitle))
            {
                Text = DocumentTitle;
            }

            this.TimerTick(sender, e);

            if (Program.Args.Length > 3)
            {
                var timerEnabled = Program.Args[3].Equals("True", StringComparison.OrdinalIgnoreCase);
                
                Timer.Enabled = timerEnabled;
            }
            else
            {
                Timer.Enabled = true;
            }
        }

        private async void TimerTick(object sender, EventArgs e)
        {
            try
            {
                var markdownFileLastWriteTime = new FileInfo(MarkdownFilePath).LastWriteTime;
                var cssFileLastWriteTime = new FileInfo(CssFilePath).LastWriteTime;

                if ((markdownFileLastWriteTime > MarkdownFileLastWriteTime) || (cssFileLastWriteTime > CssFileLastWriteTime))
                {
                    await UpdatePreviewAsync();

                    MarkdownFileLastWriteTime = markdownFileLastWriteTime;
                    CssFileLastWriteTime = cssFileLastWriteTime;
                }
            }
            catch
            {
            }
        }

        private async Task UpdatePreviewAsync()
        {
            ScrollY = await WebView.ExecuteScriptAsync("window.scrollY;");

            var html = MarkdownLibrary.Facade.MarkdownToHtml(MarkdownFilePath, CssFilePath, DocumentTitle);

            WebView.NavigateToString(html);
        }

        private async void WebViewNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (string.IsNullOrEmpty(ScrollY)) return;

            await WebView.ExecuteScriptAsync($@"requestAnimationFrame(() => {{ requestAnimationFrame(() => {{ window.scrollTo(0, {ScrollY}); }}); }});");
        }
    }
}