﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = Cryptor.Encrypt(textBox1.Text);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.KeyCode);
        }
    }
}
