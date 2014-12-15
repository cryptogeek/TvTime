using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tvtime
{
    public partial class Form1 : Form
    {
        string curItem;
        public Form1()
        {
            InitializeComponent();
            
            groupBox1.Enabled = false;

            button3.Enabled = false;

            generateListbox();

            listBox1.Sorted = true;
        }
        //generate list
        public void generateListbox()
        {
            listBox1.Items.Clear(); 
            
            string line;
            string[] tvshow;
            if (!System.IO.File.Exists(Application.StartupPath + @"\tvshow.txt"))
            {
                System.IO.File.Create(Application.StartupPath + @"\tvshow.txt").Dispose();
            }
            System.IO.StreamReader file = new System.IO.StreamReader(Application.StartupPath + @"\tvshow.txt");
            while ((line = file.ReadLine()) != null)
            {
                tvshow = line.Split('|');
                listBox1.Items.Add(tvshow[0]);
                System.Console.WriteLine(tvshow[0]);
            }
            file.Close();
          
        }
        public void deleteSelected()
        {    
            if (curItem != "new tv-show")
            {
                string[] tvshow;
                string line;
                System.IO.StreamReader reader = new System.IO.StreamReader(Application.StartupPath + @"\tvshow.txt");
                System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.StartupPath + @"\tvshow.txt.temp");
                while ((line = reader.ReadLine()) != null)
                {
                    tvshow = line.Split('|');
                    if (tvshow[0] == curItem)
                        continue;
                    writer.WriteLine(line);
                }
                reader.Close();
                writer.Close();
                System.IO.File.Delete(Application.StartupPath + @"\tvshow.txt");
                System.IO.File.Move(Application.StartupPath + @"\tvshow.txt.temp", Application.StartupPath + @"\tvshow.txt");
            }
            
            groupBox1.Enabled = false;
            
            //disable delete button
            button3.Enabled = false;
        }
        //save tv-show data button clicked
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                //deleting old data
                deleteSelected();
            
                //writing new data
                string[] tvshow = { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text };
                string tvshowString = string.Join("|", tvshow);
                System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.StartupPath + @"\tvshow.txt",true);
                writer.WriteLine(tvshowString);
                writer.Close();

                generateListbox();

                //selecting newly created tv-show
                int index = listBox1.FindString(textBox1.Text);
                listBox1.SetSelected(index, true);

                //show and hide saved label
                label6.Visible = true;
                hideLabel();
            }
        }
        public async Task hideLabel()
        {
            await Task.Delay(1000);
            label6.Visible = false;
        }
        //new tv-show button clicked
        private void button2_Click(object sender, EventArgs e)
        {
            int index = listBox1.FindString("new tv-show");
            if (index == -1)
            {
                listBox1.Items.Add("new tv-show");
                index = listBox1.FindString("new tv-show");
                listBox1.SetSelected(index, true);
                textBox1.Text = "The 100";
                textBox2.Text = "https://google.com/search?q=the+100+season+sss+episode+eee";
                textBox3.Text = "1";
                textBox4.Text = "1";
            }
        }
        //item selected in listbox
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                curItem = listBox1.SelectedItem.ToString();

                if (curItem != "new tv-show")
                {
                    button2.Enabled = true;
                    button3.Enabled = true;
                }
                else
                {
                    button2.Enabled = false;
                    button3.Enabled = false;
                }

                groupBox1.Enabled = true;

                string line;
                string[] tvshow;
                System.IO.StreamReader file = new System.IO.StreamReader(Application.StartupPath + @"\tvshow.txt");
                while ((line = file.ReadLine()) != null)
                {
                    tvshow = line.Split('|');
                    if (tvshow[0] == curItem)
                    {
                        textBox1.Text = curItem;
                        textBox2.Text = tvshow[1];
                        textBox3.Text = tvshow[2];
                        textBox4.Text = tvshow[3];
                    }
                }
                file.Close();

                //textBox1.Focus();
            }
        }
        //delete button clicked
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (MessageBox.Show("Are you sure you want to delete the selected tv-show ?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    deleteSelected();
                    generateListbox();
                }       
            } 
        }
        //item double clicked
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                curItem = listBox1.SelectedItem.ToString();

                string line;
                string[] tvshow;
                System.IO.StreamReader file = new System.IO.StreamReader(Application.StartupPath + @"\tvshow.txt");
                while ((line = file.ReadLine()) != null)
                {
                    tvshow = line.Split('|');
                    if (tvshow[0] == curItem)
                    {
                        string season = tvshow[2].PadLeft(2,'0');
                        string episode = tvshow[3].PadLeft(2, '0');
                        string url = tvshow[1].Replace("sss", season).Replace("eee", episode);
                        if (url != "")
                        {
                            System.Diagnostics.Process.Start(url);
                        }
                    }
                }
                file.Close();       
            }
        }
        //open all
        private void button4_Click(object sender, EventArgs e)
        {
            string line;
            string[] tvshow;
            System.IO.StreamReader file = new System.IO.StreamReader(Application.StartupPath + @"\tvshow.txt");
            while ((line = file.ReadLine()) != null)
            {
                tvshow = line.Split('|');
                string season = tvshow[2].PadLeft(2, '0');
                string episode = tvshow[3].PadLeft(2, '0');
                string url = tvshow[1].Replace("sss", season).Replace("eee", episode);
                if (url != "")
                {
                    System.Diagnostics.Process.Start(url);
                }
            }
            file.Close();      
        }
        private void button5_Click(object sender, EventArgs e)
        {
            int nb = Convert.ToInt32(textBox3.Text);
            nb++;
            textBox3.Text = nb.ToString();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            int nb = Convert.ToInt32(textBox3.Text);
            nb--;
            textBox3.Text = nb.ToString();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            int nb = Convert.ToInt32(textBox4.Text);
            nb++;
            textBox4.Text = nb.ToString();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            int nb = Convert.ToInt32(textBox4.Text);
            nb--;
            textBox4.Text = nb.ToString();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://cryptogeek.ninja/");
        }
    }
}
