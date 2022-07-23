using Microsoft.Office.Interop.Excel;
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
    public partial class Form3 : Form
    {
        
        public ExcelApp.Application excelApp = null;
        public ExcelApp.Workbook excelBook = null;
        ExcelApp.Workbook sourceFile = null;
        public ExcelApp._Worksheet patientSheet = null;
        public ExcelApp.Range PatientRange = null;
        public String currentLoginID = null;
        public Form3()
        {
            InitializeComponent();
           
        }
        public Form3(String a)
        {
            InitializeComponent();
            currentLoginID = a;
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(currentLoginID);
            form4.Closed += (s, args) => Close();
            form4.Show();
        }

        private void ShowPatient_Click(object sender, EventArgs e)
        {
            panel1.SendToBack();
            ShowPatient.SendToBack();
            HidePatient.BringToFront();
        }

        private void HidePatient_Click(object sender, EventArgs e)
        {
            panel1.BringToFront();
            HidePatient.SendToBack();
            ShowPatient.BringToFront();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //create excel workbook object
            excelApp = new ExcelApp.Application();
            //check if excel installed on computer
            if (excelApp == null)
            {
                Console.WriteLine("Excel is not installed!!");
                return;
            }
            //connect to database
            string fileName = "Database.xlsx";
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

            //connect to database
            excelBook = excelApp.Workbooks.Open(filePath);
            

            //patient worksheet 
            patientSheet = excelBook.Sheets[1];
            PatientRange = patientSheet.UsedRange;

            ListViewItem[] listOfItems = new ListViewItem[0];
            listView1.Items.Clear();

            for (int i = 2; i < ValidCell(1, 1); i++)
            {
                if(patientSheet.Cells[i, 26].Value2.ToString() == currentLoginID)
                {
                    string[] row = { patientSheet.Cells[i, 1].Value2.ToString() + " " + patientSheet.Cells[i, 2].Value2.ToString(), patientSheet.Cells[i, 3].Value2.ToString(), patientSheet.Cells[i, 4].Value2.ToString() };
                    var listViewItem = new ListViewItem(row);
                    //resize the array with one
                    Array.Resize(ref listOfItems, listOfItems.Length + 1);
                    //resize the array with one
                    Array.Resize(ref listOfItems, listOfItems.Length + 1);
                    //add the new worker object to array
                    listOfItems[listOfItems.Length - 1] = listViewItem;
                    //show thw worker on board
                    listView1.Items.Add(listOfItems[listOfItems.Length - 1]);
                }
            }
            //after reading, release the excel project
            excelApp.Application.ActiveWorkbook.Save();
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }

        //return first empty cell index in colume
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            //check if the user mark a patient
            if (listView1.SelectedIndices.Count <= 0)
            {
                MessageBox.Show("Please choose a patient first from the patients list");
            }
            else
            {
                //create excel workbook object
                excelApp = new ExcelApp.Application();
                //check if excel installed on computer
                if (excelApp == null)
                {
                    Console.WriteLine("Excel is not installed!!");
                    return;
                }

                //connect to database
                string fileName = "Database.xlsx";
                string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

                //connect to database
                excelBook = excelApp.Workbooks.Open(filePath);
                

                //patient worksheet 
                patientSheet = excelBook.Sheets[1];
                PatientRange = patientSheet.UsedRange;

                Boolean flag = false;
                int counter = 0;
                int check = listView1.SelectedIndices[0] + 2;
                for (int i = 1; i < ValidCell(1, 1); i++)
                {
                    if(excelBook.Sheets[1].Cells[i, 26].Value.ToString() == currentLoginID)
                    {
                        flag = true;
                    }
                }
                if (flag != true)
                {
                    MessageBox.Show("Patient dont have diagnostics yet.");
                }
                else
                {
                    for(int j = 1; j < ValidCell(1,1); j++)
                    {
                        if (excelBook.Sheets[1].Cells[j, 26].Value.ToString() == currentLoginID)
                        {
                            counter++;
                        }
                        if(counter == listView1.SelectedIndices[0] + 1)
                        {
                            MessageBox.Show("Diagnostics: \n" + patientSheet.Cells[j, 19].Value2.ToString() + "\nTreatment recommendations: \n" + patientSheet.Cells[j, 20].Value2.ToString());
                            break;
                        }
                    }
                    
                    
                }
                
                //after reading, release the excel project
                excelApp.Application.ActiveWorkbook.Save();
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }
        //log out button
        private void LogOut_Click(object sender, EventArgs e)
        {
            Hide();
            var form1 = new Form1();
            form1.Closed += (s, args) => Close();
            form1.Show();

        }

        private void Form3_Load_1(object sender, EventArgs e)
        {
            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;
            label1.Text = Form1.username;
            label2.Parent = pictureBox1;
            label2.BackColor = Color.Transparent;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
