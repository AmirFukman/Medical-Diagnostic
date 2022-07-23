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
    public partial class Form2 : Form
    {
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

        public Form2()
        {
            InitializeComponent();

        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            //create excel workbook object
            excelApp = new ExcelApp.Application();
            //check if excel installed on computer
            if (excelApp == null)
            {
                Console.WriteLine("Excel is not installed!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //connect to database
            string fileName = "Database.xlsx";
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

            //connect to database
            excelBook = excelApp.Workbooks.Open(filePath);
            
            
            //Users + password worksheet 
            UsersSheet = excelBook.Sheets[2];
            UsersRange = UsersSheet.UsedRange;

            //ID
            if (checkIfExist(2,3,textBox3.Text) == false)
            {
                //user name
                if(checkIfExist(2, 1, textBox1.Text) == false)
                {
                    //password
                    if(textBox2.Text != null)
                    {
                        //check if ID and User name are valid
                        if (CheckID(textBox3.Text) && CheckUserName(textBox1.Text) && CheckPassword(textBox2.Text))
                        {
                            //user name insertion
                            UsersSheet.Cells[ValidCell(2,1), 1].Value = textBox1.Text;
                            //password name insertion
                            UsersSheet.Cells[ValidCell(2, 2), 2].Value = textBox2.Text;
                            //ID insertion
                            UsersSheet.Cells[ValidCell(2, 3), 3].Value = textBox3.Text;

                            //after reading, release the excel project
                            excelApp.Application.ActiveWorkbook.Save();
                            excelApp.Quit();
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                            MessageBox.Show("You have been successfully registered", "Congratulation", MessageBoxButtons.OK, MessageBoxIcon.None);
                            Hide();

                        }
                        else
                        {
                            if (CheckID(textBox3.Text) == false)
                            {
                                MessageBox.Show("ID is not valid, please insert a valid ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //after reading, release the excel project
                                excelApp.Application.ActiveWorkbook.Save();
                                excelApp.Quit();
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                            }
                            else if (CheckUserName(textBox1.Text) == false)
                            {
                                MessageBox.Show("User name is not valid, please insert a valid user name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //after reading, release the excel project
                                excelApp.Application.ActiveWorkbook.Save();
                                excelApp.Quit();
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                            }
                            else
                            {
                                MessageBox.Show("Password combination is not valid, please insert a valid password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //after reading, release the excel project
                                excelApp.Application.ActiveWorkbook.Save();
                                excelApp.Quit();
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("User name already exist, please insert a different user name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //after reading, release the excel project
                    excelApp.Application.ActiveWorkbook.Save();
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
            }
            else
            {
                MessageBox.Show("ID Already Exist, Please Insert ID Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //after reading, release the excel project
                excelApp.Application.ActiveWorkbook.Save();
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
                
        }

        //check password validation
        public Boolean CheckPassword(string password)
        {
            int numberCounter = 0, letterCounter = 0, specialCounter = 0;
            if (password.Length >= 8 && password.Length <= 10)
            {
                for (int i = 0; i < password.Length; i++)
                {
                    if (password[i] >= '0' && password[i] <= '9')
                    {
                        numberCounter++;
                    }
                    if (((password[i] >= 'a' && password[i] <= 'z') || (password[i] >= 'A' && password[i] <= 'Z')))
                    {
                        letterCounter++;
                    }
                    if (password[i] == '!' || password[i] == '@' || password[i] == '#' || password[i] == '$' || 
                        password[i] == '%' || password[i] == '^' || password[i] == '&' || password[i] == '*' ||
                        password[i] == '(' || password[i] == ')' || password[i] == '-' || password[i] == '_' ||
                        password[i] == '=' || password[i] == '+' || password[i] == ',' || password[i] == '.' ||
                        password[i] == '[' || password[i] == ']' || password[i] == ';' || password[i] == '.' ||
                        password[i] == '/' || password[i] == '?')
                    {
                        specialCounter++;
                    }
                }
                if (numberCounter < 1 || letterCounter < 1 || specialCounter < 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        
        //check user name validation
        public Boolean CheckUserName(string userName)
        {
            if (userName == null || userName == "")
            {
                return false;
            }
            int numberCounter = 0;
            if (userName.Length >= 6 && userName.Length <= 8)
            {
                for (int i = 0; i < userName.Length; i++)
                {
                    if (userName[i] >= '0' && userName[i] <= '9')
                    {
                        numberCounter++;
                    }
                    else if (!((userName[i] >= 'a' && userName[i] <= 'z') || (userName[i] >= 'A' && userName[i] <= 'Z')))
                    {
                        return false;
                    }
                }
                if (numberCounter > 2)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        //get sheet number and return the first empty cell selected col
        public int ValidCell(int sheetNumber , int col)
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
            for (i = 1; i < ValidCell(sheet,col); i++)
            {
                
                if (excelBook.Sheets[sheet].Cells[i, col].Value.ToString() == value)
                {
                    return true;
                }
            }
            return false;
        }

        //check ID validation
        public Boolean CheckID(String id)
        {
            if(id == null || id == "")
            {
                return false;
            }
            int numberCounter = 0; ;
            if (id.Length == 9)
            {
                for (int i = 0; i < id.Length; i++)
                {
                    if (id[i] >= '0' && id[i] <= '9')
                    {
                        numberCounter++;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (numberCounter == 9)
                {
                    return true;
                }
            }
            return false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;
            label2.Parent = pictureBox1;
            label2.BackColor = Color.Transparent;
            label3.Parent = pictureBox1;
            label3.BackColor = Color.Transparent;
            label4.Parent = pictureBox1;
            label4.BackColor = Color.Transparent;
            label5.Parent = pictureBox1;
            label5.BackColor = Color.Transparent;
            label6.Parent = pictureBox1;
            label6.BackColor = Color.Transparent;
            label7.Parent = pictureBox1;
            label7.BackColor = Color.Transparent;
            toolTip1.SetToolTip(button1, "- Length between 6 to 8 charachters.\n- At most two numbers\n- English charachters only!");
            toolTip2.SetToolTip(button3, "- Length between 8 to 10 charachters.\n- At least one charachter is a number\n- At least one charachter is a letter\n- At least one charachter is special");
            toolTip3.SetToolTip(button4, "- Numbers only.\n- Length Has to be 9 charachters");
            toolTip4.SetToolTip(button6, "Hide Password");
            toolTip5.SetToolTip(button7, "Show Password");

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Hide();
            
      

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox2.UseSystemPasswordChar == false)
            {
                textBox2.UseSystemPasswordChar = true;
                button7.BringToFront();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox2.UseSystemPasswordChar == true)
            {
                textBox2.UseSystemPasswordChar = false;
                button6.BringToFront();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
    


