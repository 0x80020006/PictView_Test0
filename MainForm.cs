using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PictView_Test0
{
    class MainForm : Form
    {
        //変数置き場
        //ファイル読み込み変数
        //ファイルまでのフルパス
        public string openFilePath;

        //読み込んだファイル一覧
        List<string> filesList;
        public int loadFileNum;

        //メニューバー
        MenuStrip menuStrip;
        OpenFileDialog ofd;

        //画像表示
        PictureBox pictMain;

        static readonly int MS_WIDTH  = 500;
        static readonly int MS_HEIGHT = 470;
        static readonly int CS_WIDTH  = 800;
        static readonly int CS_HEIGHT = 770;

        Bitmap imgScreen = new Bitmap(CS_WIDTH, CS_HEIGHT);

        public MainForm()
        {
            //アプリケーションタイトル
            Text = "PictView";

            //メニューバー表示
            menuStrip = new MenuStrip();

            //クライアント最小サイズの設定
            MinimumSize = new Size(MS_WIDTH, MS_HEIGHT);

            //クライアント起動時サイズ
            Size = new Size(CS_WIDTH, CS_HEIGHT);

            //画像表示枠
            pictMain = new PictureBox();
            pictMain.Location = new Point(0, 25);
            pictMain.BackColor = Color.Blue;

            //アプリケーションのロード処理
            //実行時の処理は以下で行う
            Load += new EventHandler(MainForm_Load);
            KeyDown += new KeyEventHandler(MainForm_KeyDown);
            Controls.Add(menuStrip);
            Controls.Add(pictMain);
            pictMain.Dock = DockStyle.Fill;
        }

        //メニューバー
        private void MainForm_Load(object sender, EventArgs e)
        {
            //ウィンドウサイズが変更されたときに呼び出す
            SizeChanged += Window_SizeChanged;

            //フラグの初期化
            Flags.fileFlag = 0;
            Console.WriteLine($"フラグ：{Flags.fileFlag}");
            //メニューにファイルを表示する
            ToolStripMenuItem menuFile = new ToolStripMenuItem();
            menuFile.Text = "ファイル(&F)";
            menuStrip.Items.Add(menuFile);

            //メニュー内容            
            ToolStripMenuItem menuFileOpen = new ToolStripMenuItem();
            
            menuFileOpen.Text = "開く(&O)";
            //「ファイルを開く」の実行
            menuFileOpen.Click += new EventHandler(Open_Click);
            menuFile.DropDownItems.Add(menuFileOpen);
            ToolStripMenuItem menuFileEnd = new ToolStripMenuItem();
            
            menuFileEnd.Text = "終了(&X)";
            //「アプリケーションの終了」の実行
            menuFileEnd.Click += new EventHandler(Close_Click);
            menuFile.DropDownItems.Add(menuFileEnd);
        }

        //ファイルを開く
        private void Open_Click(object sender, EventArgs e)
        {
            ofd = new OpenFileDialog();
            //読み込み許可ファイルの種類の設定 
            ofd.Filter = "Image File(*.bmp,*.jpg,*.png)|*.bmp;*.jpg;*.png|Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg)|*.jpg|PNG(*.png)|*.png";
            //ファイルを選択してOKしたときの処理
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine($"読み込みファイル:{ofd.FileName}");
                string folderPath = Path.GetDirectoryName(ofd.FileName);
                IEnumerable<string> files = Directory.EnumerateFiles(folderPath).Where(str => str.EndsWith(".bmp") || str.EndsWith(".jpg") || str.EndsWith(".png"));

                //フォルダ内部のファイルを全て読み込んでリストに格納
                filesList = files.ToList();

                //デバッグ用出力(読み込みファイル)
                foreach (string str in files)
                {
                    Console.WriteLine($"読み込みファイル:{str}");
                }
                //デバッグ用出力（読み込みファイル数）
                Console.WriteLine($"読み込みファイル数:{filesList.Count}");

                //リスト内をソート処理
                var sortQuery = filesList.OrderBy(s => s.Length);
                filesList = sortQuery.ToList();
                loadFileNum = filesList.IndexOf(ofd.FileName);
                //ソート後のリスト内を出力
                filesList.ForEach(Console.WriteLine);

                //ファイル読み込みフラグオン
                Flags.fileFlag |= Flags.fileFlags.FILE_LOAD;
                Console.WriteLine($"{Flags.fileFlag}");

                //「画像読み込み」の実行
                LoadImage();                
            }

            Console.WriteLine("開く処理終了");

        }

        private void MainForm_Action()
        {
            if (loadFileNum >= filesList.Count)
            {
                loadFileNum = 0;
            }
            else if (loadFileNum < 0)
            {
                loadFileNum = filesList.Count - 1;
            }

            //「loadFileNo」のファイルを描画する
            if (File.Exists(filesList[loadFileNum]))
            {
                LoadImage();
            }
        }

        //キーボードが押されたときの処理
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            var k = e.KeyCode;
            Console.WriteLine($"{k}");

            //ファイルが読み込まれいる場合
            if (Flags.fileFlag.HasFlag(Flags.fileFlags.FILE_LOAD))
            {

                switch (k)
                {
                case Keys.Up:
                    loadFileNum -= 1;
                    break;

                case Keys.Down:
                    loadFileNum += 1;
                    break;

                case Keys.Left:
                    loadFileNum -= 1;
                    break;

                case Keys.Right:
                    loadFileNum += 1;
                    break;

                default:
                    break;
                }

                MainForm_Action();
            }
        }

        //画像読み込み
        private void LoadImage()
        {

            //画像が読み込まれていた場合
            if (pictMain.Image != null)
            {
                //リソースを解放する
                pictMain.Image.Dispose();
                //pictureBox[pictFrame]の中を空にする
                pictMain.Image = null;

            }

            //読み込む画像
            openFilePath = filesList[loadFileNum];
            Console.WriteLine($"{loadFileNum},{openFilePath}");

            imgScreen = new Bitmap(Width, Height);
            var g = Graphics.FromImage(imgScreen);
            Image img = Image.FromFile(openFilePath);

            //画像サイズ
            float imgWidth = img.Width;
            float imgHeight = img.Height;

            //描画枠と画像の比率
            float rWidth = Width / imgWidth;
            float rHeight = Height / imgHeight;
            float r = Math.Min(rWidth, rHeight);

            g.DrawImage(img, Width / 2 - imgWidth * r / 2, Height / 2 - imgHeight * r /2, img.Width * r, img.Height * r);
            pictMain.Image = imgScreen;

            img.Dispose();
            g.Dispose();

            //読み込んだファイル名を表示
            string Title = "PictViewer - ";
            Title += openFilePath;
            Text = Title;

        }

        private void Window_SizeChanged(object sender, EventArgs e)
        {
            if(Flags.fileFlag.HasFlag(Flags.fileFlags.FILE_LOAD))
            {
                LoadImage();

                Console.WriteLine($"画像表示処理[Window_SizeChanged]");
            }
        }


        //終了時のメモリ解放
        private IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        //アプリケーション終了
        private void Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
