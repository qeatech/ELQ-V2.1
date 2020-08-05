using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Dialogs;

using Flir.Atlas.Image;

namespace ELQ_V2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Global Variables
        public string SourceFolder = "";



        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        public void Init()
        {
            // Load this function upon loading the form
            label_palette_high.Text = label_palette_low.Text = "";

            textBox_log.Text = "Log Console Initiated at ";




        }



        public void ResetEverything()
        {
            Init();
        }


        private void metroButton2_Click(object sender, EventArgs e)
        {
            string path = "C:\\Users\\Babak\\Desktop\\TBE\\new _name\\Test2.jpg";
            DisplayImage(path);
        }

        public void DisplayImage(string path)
        {
            // Load the thermal image
            ThermalImageFile ti = new ThermalImageFile(path);
            Image img = ti.Image;

            // Update the thermal image in the picture box
            pictureBox_mainimg.Image = img;

            // Upload the palette bar 
            var bmp = new Bitmap(10, 100);
            var scaleImage = ti.Scale.Image;
            using (var g = Graphics.FromImage(bmp))
            {
                //Use gdi+ to interpolate with nearest neighbor
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(scaleImage, 0, 0, bmp.Width * 2, bmp.Height);
            }
            pictureBox_palette.Image = bmp;    // Update the palette bar 

            double scale_min_forlabel = Math.Round(ti.Scale.Range.Minimum, 1);
            double scale_max_forlabel = Math.Round(ti.Scale.Range.Maximum, 1);
            label_palette_high.Text = Convert.ToString(scale_max_forlabel) + "°C";
            label_palette_low.Text = Convert.ToString(scale_min_forlabel) + "°C";

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadFolderDialog();
        }

        public void LoadFolderDialog()
        {
            // Load files into the ListBox 
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            dialog.IsFolderPicker = true;
            
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Console.WriteLine(dialog.FileName);
            }

        }

        private void bunifuCards_Images_SizeChanged(object sender, EventArgs e)
        {
            // If the size of this Card changes, we want the size of height of the ListBox inside the card change as well. 
            listBox_images.Height = Form1.ActiveForm.Height - bunifuCards_ThermalTuning.Height - 100;

            ////// DEBUG
            //Console.WriteLine("Listbox Height : " +
            //    Convert.ToString(listBox_images.Height) + " and Cards height is: " + Convert.ToString(Form1.ActiveForm.Height));
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel_bottom.Visible = !panel_bottom.Visible;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GC.Collect();
            Application.Exit();
        }
    }
}
