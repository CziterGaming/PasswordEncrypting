using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace PasswordEncrypting
{
    public partial class Form1 : Form
    {
        string localappdata = Environment.GetEnvironmentVariable("LOCALAPPDATA");

        public static string pass;
        bool hidden = true;

        public Form1()
        {
            mainPassword mp = new mainPassword();
            mp.ShowDialog();

            InitializeComponent();
            Directory.CreateDirectory(localappdata + "\\CziterGaming");
            Directory.CreateDirectory(localappdata + "\\CziterGaming\\PasswordEncrypting");

            richTextBox1.Text = new String('*', pass.Length);
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            if(textBox2.Text.Contains("=") || textBox2.Text.Contains("-") || textBox3.Text.Contains("=") || textBox3.Text.Contains("-"))
            {
                MessageBox.Show("Currently descriptions and passwords can't contain letters: '=' and '-'.");
                    return;
            }

            int passconv = 0;

            foreach(char h in pass)
            {
                passconv = passconv + (int)h;
            }

            int[] test = new int[textBox3.Text.Length];

            int i = 0;
            string encrypted = null;
            foreach(char ch in textBox3.Text)
            {
                test[i] = (int)ch;
                Console.Write(test[i]);

                test[i] = test[i] * passconv;

                encrypted = encrypted + test[i] + "-";

                Console.WriteLine(" - " + test[i] + " - " + (test[i] / passconv) + " - " + Convert.ToChar((test[i] / passconv)));
                i++;
            }

            encrypted = encrypted.Remove(encrypted.Length - 1);

            using(StreamWriter sw = File.AppendText(localappdata + "\\CziterGaming\\PasswordEncrypting\\passwords.ini"))
            {
                sw.WriteLine(textBox2.Text + "=" + encrypted);
            }
            textBox1.AppendText(textBox2.Text + " - " + textBox3.Text + Environment.NewLine);
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            int passconv = 0;

            foreach (char h in pass)
            {
                passconv = passconv + (int)h;
            }

            var fileContent = string.Empty;
            var filePath = string.Empty;

            string line;
            using (StreamReader sr = new StreamReader(localappdata + "\\CziterGaming\\PasswordEncrypting\\passwords.ini"))
            {
                List<string> passwords = new List<string>();
                int i = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    passwords.Add(line);
                    i++;
                }

                Console.WriteLine("---");
                string readytoexport;

                foreach (string a in passwords)
                {
                    string[] temp = a.Split('=');
                    string[] temp2 = temp[1].Split('-');
                    readytoexport = temp[0] +  " - ";
                    foreach(var b in temp2)
                    {
                        int number = Convert.ToInt32(b) / passconv;
                        char letter = ((char)number);
                        readytoexport = readytoexport + letter;
                    }

                    textBox1.AppendText(readytoexport + Environment.NewLine);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(hidden)
            {
                button1.Text = "hide";
                richTextBox1.Text = pass;
                hidden = false;
            }
            else
            {
                button1.Text = "show";
                richTextBox1.Text = new String('*', pass.Length);
                hidden = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start(localappdata + "\\CziterGaming\\PasswordEncrypting");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}