using System;
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
        //[変数]ファイルまでのフルパス
        public string openFilePath;

        //[変数]読み込んだファイル一覧
        //public string[] loadFiles = new string[]{ };
        List<string> filesList;
        public int loadFileNum;
        public int numForSlider;

        //[変数]メニューバー
        MenuStrip menuStrip;
        OpenFileDialog ofd;

        static readonly int MS_WIDTH  = 500;
        static readonly int MS_HEIGHT = 470;
        static readonly int CS_WIDTH  = 800;
        static readonly int CS_HEIGHT = 770;


        public MainForm()
        {
            //メニューバー表示
            menuStrip = new MenuStrip();

            //クライアント最小サイズの設定
            MinimumSize = new Size(MS_WIDTH, MS_HEIGHT);

            //クライアント起動時サイズ
            Size = new Size(CS_WIDTH, CS_HEIGHT);

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
    }
}
