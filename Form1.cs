using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelApp = Microsoft.Office.Interop.Excel;

namespace FinalProject
{

    public partial class Form1 : Form
    {

        public static string username;

        String userName = null;
        String password = null;
        ExcelApp.Application excelApp = null;
        ExcelApp.Workbook excelBook = null;
        ExcelApp._Worksheet patientSheet = null;
        ExcelApp.Range PatientRange = null;
        ExcelApp._Worksheet UsersSheet = null;
        ExcelApp.Range UsersRange = null;
        int patientRow = 1;
        int patientCol = 0;
        int UsersLine = 1;
        int UsersRow = 0;
       

        //constructor
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;
            label3.Parent = pictureBox1;
            label3.BackColor = Color.Transparent;
            label4.Parent = pictureBox1;
            label4.BackColor = Color.Transparent;
            label5.Parent = pictureBox1;
            label5.BackColor = Color.Transparent;
            toolTip1.SetToolTip(button3,"Hide Password");
            toolTip2.SetToolTip(button4,"Show Password");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            var form2 = new Form2();
            form2.Closed += (s, args) => Close();
            form2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //create excel workbook object
            excelApp = new ExcelApp.Application();
            //check if excel installed on computer
            if (excelApp == null)
            {
                Console.WriteLine("Excel is not installed!!");
                return;
            }
            string fileName = "Database.xlsx";
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

            //connect to database
            excelBook = excelApp.Workbooks.Open(filePath);
            

            //Users + password worksheet 
            UsersSheet = excelBook.Sheets[2];
            UsersRange = UsersSheet.UsedRange;

            if (checkIfExist(2, 1, textBox1.Text))
            {
                int i = 1;
                while (excelBook.Sheets[2].Cells[i, 1].Value.ToString() != textBox1.Text)
                {
                    i++;
                }

                if (excelBook.Sheets[2].Cells[i, 2].Value.ToString() == textBox2.Text)
                {
                    Hide();
                    username = textBox1.Text;
                    var form3 = new Form3(excelBook.Sheets[2].Cells[i, 3].Value.ToString());
                    form3.Closed += (s, args) => Close();
                    form3.Show();
                    
                }
                else
                {
                    MessageBox.Show("Wrong password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("User name does not exist","Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
   
            }

            //after reading, release the excel project
            excelApp.Application.ActiveWorkbook.Save();
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }

        //get sheet number and return the first empty cell selected col
        public int ValidCell(int sheetNumber, int col)
        {
            int i = 1;
            for (i = 1; i < excelBook.Sheets[sheetNumber].Rows.Count; i++)
            {
                if (excelBook.Sheets[sheetNumber].Cells[i, col].Value == null)
                {
                    break;
                }
            }
            return i;
        }

        //check if value exist in col
        public Boolean checkIfExist(int sheet, int col, string value)
        {
            int i = 1;
            for (i = 1; i < ValidCell(sheet, col); i++)
            {

                if (excelBook.Sheets[sheet].Cells[i, col].Value.ToString() == value)
                {
                    return true;
                }
            }
            return false;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (textBox2.UseSystemPasswordChar == false)
            {
                textBox2.UseSystemPasswordChar = true;
                button4.BringToFront();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.UseSystemPasswordChar == true)
            {
                textBox2.UseSystemPasswordChar = false;
                button3.BringToFront();
            }

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

