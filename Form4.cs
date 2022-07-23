using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelApp = Microsoft.Office.Interop.Excel;

namespace FinalProject
{

    public partial class Form4 : Form
    {
        String userName = null;
        String password = null;
        public ExcelApp.Application excelApp = null;
        public ExcelApp.Workbook excelBook = null;
        ExcelApp.Workbook sourceFile = null;
        public ExcelApp._Worksheet patientSheet = null;
        public ExcelApp.Range PatientRange = null;
        ExcelApp.Range sourceFileRange = null;
        ExcelApp._Worksheet UsersSheet = null;
        ExcelApp._Worksheet sourceFileSheet = null;
        ExcelApp.Range UsersRange = null;
        int patientRow = 1;
        int patientCol = 0;
        int UsersLine = 1;
        int UsersRow = 0;
        Dictionary<string, string> dict = new Dictionary<string, string>();
        public int[] deasArray = new int[26];
        public String currentLoginID = null;

        public Form4()
        {
            InitializeComponent();
            //The Dictionary of the Recommended treatments table.
            dict.Add("anemia", "Two 10 mg B12 pills a day for a month");//0
            dict.Add("diet", "Schedule an appointment with a nutritionist");//1
            dict.Add("bleeding", "To be rushed to the hospital urgently");//2
            dict.Add("hyperlipidemia", "Schedule an appointment with a nutritionist, a 5 mg pill of Simobil daily for a week");//3
            dict.Add("Disruption of Blood", "10 mg pill of B12 a day for a month 5 mg pill of folic acid a day for a month");//4
            dict.Add("Hematological disorder", "An injection of a hormone to encourage red blood cell production");//5
            dict.Add("Iron Poisoning", "To be evacuated to the hospital");//6
            dict.Add("Dehydration", "Complete rest while lying down, returning fluids to drinking");//7
            dict.Add("Infection", "Dedicated antibiotics");//8
            dict.Add("Vitamin Deficiency", "Referral for a blood test to identify the missing vitamins");//9
            dict.Add("Viral disease", "Rest at home");//10
            dict.Add("Diseases of the biliary tract", "Referral to surgical treatment");//11
            dict.Add("heart diseases", "Schedule an appointment with a nutritionist");//12
            dict.Add("Blood disease", "A combination of cyclophosphamide and corticosteroids");//13
            dict.Add("Liver disease", "A combination of cyclophosphamide and corticosteroids");//14
            dict.Add("Kidney disease", "Referral to a specific diagnosis for the purpose of determining treatment");//15
            dict.Add("Iron deficiency", "Balance blood sugar levels");//16
            dict.Add("Muscle diseases", "Two 10 mg B12 pills a day for a month");//17
            dict.Add("Smokers", "to stop smoking");//18
            dict.Add("Lung disease", "Stop smoking / refer to X-ray of the lungs");//19
            dict.Add("Overactive thyroid gland", "Propylthiouracil to reduce thyroid activity");//20
            dict.Add("Adult diabetes", "Insulin adjustment for the patient");//21
            dict.Add("Cancer", "Entrectinib");//22
            dict.Add("Increased consumption of meat", "Schedule an appointment with a nutritionist");//23
            dict.Add("Use of various medications", "Referral to a family doctor for a match between medications");//24
            dict.Add("Malnutrition", "Schedule an appointment with a nutritionist");//25
            dict.Add("You're healthy", "Don't Need for any Treatment.");//26
        }
        public Form4(String a)
        {
            InitializeComponent();
            //The Dictionary of the Recommended treatments table.
            dict.Add("anemia", "Two 10 mg B12 pills a day for a month");//0
            dict.Add("diet", "Schedule an appointment with a nutritionist");//1
            dict.Add("bleeding", "To be rushed to the hospital urgently");//2
            dict.Add("hyperlipidemia", "Schedule an appointment with a nutritionist, a 5 mg pill of Simobil daily for a week");//3
            dict.Add("Disruption of Blood", "10 mg pill of B12 a day for a month 5 mg pill of folic acid a day for a month");//4
            dict.Add("Hematological disorder", "An injection of a hormone to encourage red blood cell production");//5
            dict.Add("Iron Poisoning", "To be evacuated to the hospital");//6
            dict.Add("Dehydration", "Complete rest while lying down, returning fluids to drinking");//7
            dict.Add("Infection", "Dedicated antibiotics");//8
            dict.Add("Vitamin Deficiency", "Referral for a blood test to identify the missing vitamins");//9
            dict.Add("Viral disease", "Rest at home");//10
            dict.Add("Diseases of the biliary tract", "Referral to surgical treatment");//11
            dict.Add("heart diseases", "Schedule an appointment with a nutritionist");//12
            dict.Add("Blood disease", "A combination of cyclophosphamide and corticosteroids");//13
            dict.Add("Liver disease", "A combination of cyclophosphamide and corticosteroids");//14
            dict.Add("Kidney disease", "Referral to a specific diagnosis for the purpose of determining treatment");//15
            dict.Add("Iron deficiency", "Balance blood sugar levels");//16
            dict.Add("Muscle diseases", "Two 10 mg B12 pills a day for a month");//17
            dict.Add("Smokers", "to stop smoking");//18
            dict.Add("Lung disease", "Stop smoking / refer to X-ray of the lungs");//19
            dict.Add("Overactive thyroid gland", "Propylthiouracil to reduce thyroid activity");//20
            dict.Add("Adult diabetes", "Insulin adjustment for the patient");//21
            dict.Add("Cancer", "Entrectinib");//22
            dict.Add("Increased consumption of meat", "Schedule an appointment with a nutritionist");//23
            dict.Add("Use of various medications", "Referral to a family doctor for a match between medications");//24
            dict.Add("Malnutrition", "Schedule an appointment with a nutritionist");//25
            dict.Add("You're healthy", "Don't Need for any Treatment.");//26
            currentLoginID = a;
        }
        private void AddPatient_Click(object sender, EventArgs e)
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

            if (checkIfExist(1, 3, ID.Text) == false)
            {
                if (CheckNames(FirstName.Text) && CheckNames(LastName.Text) && CheckID(ID.Text) && CheckAge(Age.Text) && CheckHeight(height.Text) && CheckWeight(weight.Text) && CheckUnits(AP.Text) && CheckUnits(HDL.Text) && CheckUnits(IRON.Text) && CheckUnits(CRTN.Text) && CheckUnits(HB.Text) && CheckUnits(UREA.Text) && CheckUnits(HCT.Text) && CheckUnits(RBC.Text) && CheckUnits(LYMPH.Text) && CheckUnits(NEUT.Text) && CheckUnits(WBC.Text))
                {
                    //enter values to database
                    int i = ValidCell(1, 1);
                    //doctor owner
                    patientSheet.Cells[i, 26].Value2 = currentLoginID;
                    //first name
                    patientSheet.Cells[i, 1].Value2 = FirstName.Text;
                    //last name
                    patientSheet.Cells[i, 2].Value2 = LastName.Text;
                    //ID
                    patientSheet.Cells[i, 3].Value2 = ID.Text;
                    //Age
                    patientSheet.Cells[i, 4].Value2 = Age.Text;
                    //height
                    patientSheet.Cells[i, 5].Value2 = height.Text;
                    //weight
                    patientSheet.Cells[i, 6].Value2 = weight.Text;
                    //phone
                    patientSheet.Cells[i, 7].Value2 = phone.Text;
                    //gender
                    if (comboBox1.SelectedItem == null)
                    {
                        patientSheet.Cells[i, 21].Value2 = "";
                    }
                    else
                    {
                        patientSheet.Cells[i, 21].Value2 = comboBox1.SelectedItem;
                    }
                    //WBC
                    patientSheet.Cells[i, 8].Value2 = WBC.Text;
                    //NEUT
                    patientSheet.Cells[i, 9].Value2 = NEUT.Text + "%";
                    //LYMPH
                    patientSheet.Cells[i, 10].Value2 = LYMPH.Text + "%";
                    //RBC
                    patientSheet.Cells[i, 11].Value2 = RBC.Text;
                    //HCT
                    patientSheet.Cells[i, 12].Value2 = HCT.Text + "%";
                    //UREA
                    patientSheet.Cells[i, 13].Value2 = UREA.Text;
                    //HB
                    patientSheet.Cells[i, 14].Value2 = HB.Text;
                    //CRTN
                    patientSheet.Cells[i, 15].Value2 = CRTN.Text;
                    //IRON
                    patientSheet.Cells[i, 16].Value2 = IRON.Text;
                    //HDL
                    patientSheet.Cells[i, 17].Value2 = HDL.Text;
                    //AP
                    patientSheet.Cells[i, 18].Value2 = AP.Text;

                    //Ethiopian
                    if (Q1yes.Checked)
                    {
                        patientSheet.Cells[i, 22].Value2 = "YES";
                    }
                    else if (Q1No.Checked)
                    {
                        patientSheet.Cells[i, 22].Value2 = "NO";
                    }

                    //Easterens
                    if (Q2Yes.Checked)
                    {
                        patientSheet.Cells[i, 23].Value2 = "YES";
                    }
                    else if (Q2No.Checked)
                    {
                        patientSheet.Cells[i, 23].Value2 = "NO";
                    }
                    //smoking
                    if (Q3Yes.Checked)
                    {
                        patientSheet.Cells[i, 24].Value2 = "YES";
                    }
                    else if (Q3No.Checked)
                    {
                        patientSheet.Cells[i, 24].Value2 = "NO";
                    }
                    //pregnant
                    if (Q4Yes.Checked)
                    {
                        patientSheet.Cells[i, 25].Value2 = "YES";
                    }
                    else if (Q4No.Checked)
                    {
                        patientSheet.Cells[i, 25].Value2 = "NO";
                    }

                    MessageBox.Show("Patient data loaded successfully");

                    string keysSTR = recAlgoritherm();
                    string valuesSTR = "";
                    
                    for (int j = 0; j < 27; j++)
                    {
                        if (keysSTR.Contains(dict.ElementAt(j).Key))
                        {
                            valuesSTR += " - " + (dict.ElementAt(j).Value + "\n");
                        }
                    }

                    patientSheet.Cells[i, 19].Value2 = keysSTR;
                    patientSheet.Cells[i, 20].Value2 = valuesSTR;

                    //after reading, release the excel project
                    excelApp.Application.ActiveWorkbook.Save();
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                    Hide();
                }
                else
                {
                    MessageBox.Show("You have been entered data with mistakes");
                    //after reading, release the excel project
                    excelApp.Application.ActiveWorkbook.Save();
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
            }
            else
            {
                MessageBox.Show("Patient data already exist in database");
                //after reading, release the excel project
                excelApp.Application.ActiveWorkbook.Save();
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        private void ImportPetiantData_Click(object sender, EventArgs e)
        {
            importData();
        }

        private void FirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void LastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void ID_TextChanged(object sender, EventArgs e)
        {

        }

        private void Age_TextChanged(object sender, EventArgs e)
        {

        }

        private void weight_TextChanged(object sender, EventArgs e)
        {

        }

        private void height_TextChanged(object sender, EventArgs e)
        {

        }

        private void phone_TextChanged(object sender, EventArgs e)
        {

        }

        private void AP_TextChanged(object sender, EventArgs e)
        {

        }

        private void HDL_TextChanged(object sender, EventArgs e)
        {

        }

        private void IRON_TextChanged(object sender, EventArgs e)
        {

        }

        private void CRTN_TextChanged(object sender, EventArgs e)
        {

        }

        private void HB_TextChanged(object sender, EventArgs e)
        {

        }

        private void UREA_TextChanged(object sender, EventArgs e)
        {

        }

        private void HCT_TextChanged(object sender, EventArgs e)
        {

        }

        private void RBC_TextChanged(object sender, EventArgs e)
        {

        }

        private void LYMPH_TextChanged(object sender, EventArgs e)
        {

        }

        private void NEUT_TextChanged(object sender, EventArgs e)
        {

        }

        private void WBC_TextChanged(object sender, EventArgs e)
        {

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

        //import excel file with patient data to the database
        public void importData()
        {
            //createing a file dialog
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel files (*.xlsx)|*.xlsx|Excel files (*.xls)|*.xls";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;

            //if the user choose OK
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //create excel workbook object
                excelApp = new ExcelApp.Application();
                //check if excel installed on computer
                if (excelApp == null)
                {
                    Console.WriteLine("Excel is not installed!!");
                    this.Close();
                }

                //open the source file
                sourceFile = excelApp.Workbooks.Open(ofd.FileName.ToString());

                //open the target file - database
                string fileName = "Database.xlsx";
                string filePath = Path.Combine(Environment.CurrentDirectory, fileName);

                //connect to database
                excelBook = excelApp.Workbooks.Open(filePath);
                

                //patient sheets 
                sourceFileSheet = sourceFile.Sheets[1];
                sourceFileRange = sourceFileSheet.UsedRange;

                //patient worksheet 
                patientSheet = excelBook.Sheets[1];
                PatientRange = patientSheet.UsedRange;

                if (checkIfImportedFileIsValid(sourceFile) == false)
                {
                    MessageBox.Show("The imported file is no valid!");
                    this.Hide();
                }
                else
                {
                    //WBC
                    WBC.Text = sourceFileSheet.Cells[2, 1].Value2.ToString();
                    //NEUT
                    NEUT.Text = sourceFileSheet.Cells[2, 2].Value2.ToString();
                    //LYMPH
                    LYMPH.Text = sourceFileSheet.Cells[2, 3].Value2.ToString();
                    //RBC
                    RBC.Text = sourceFileSheet.Cells[2, 4].Value2.ToString();
                    //HCT
                    HCT.Text = sourceFileSheet.Cells[2, 5].Value2.ToString();
                    //UREA
                    UREA.Text = sourceFileSheet.Cells[2, 6].Value2.ToString();
                    //HB
                    HB.Text = sourceFileSheet.Cells[2, 7].Value2.ToString();
                    //CRTN
                    CRTN.Text = sourceFileSheet.Cells[2, 8].Value2.ToString();
                    //IRON
                    IRON.Text = sourceFileSheet.Cells[2, 9].Value2.ToString();
                    //HDL
                    HDL.Text = sourceFileSheet.Cells[2, 10].Value2.ToString();
                    //AP
                    AP.Text = sourceFileSheet.Cells[2, 11].Value2.ToString();
                }

                //after reading, release the excel project
                excelApp.Application.ActiveWorkbook.Save();
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

        }

        //check if the imporetd file is valid
        public Boolean checkIfImportedFileIsValid(ExcelApp.Workbook file)
        {
            if (!(file.Sheets[1].Cells[1, 1].Value2 == "WBC" || file.Sheets[1].Cells[1, 1].Value2 == "Wbc"))
                return false;
            if (!(file.Sheets[1].Cells[1, 2].Value2 == "Neut" || file.Sheets[1].Cells[1, 2].Value2 == "NEUT"))
                return false;
            if (!(file.Sheets[1].Cells[1, 3].Value2 == "LYMPH" || file.Sheets[1].Cells[1, 3].Value2 == "Lymph"))
                return false;
            if (!(file.Sheets[1].Cells[1, 4].Value2 == "RBC" || file.Sheets[1].Cells[1, 4].Value2 == "Rbc"))
                return false;
            if (!(file.Sheets[1].Cells[1, 5].Value2 == "HCT" || file.Sheets[1].Cells[1, 5].Value2 == "Hct"))
                return false;
            if (!(file.Sheets[1].Cells[1, 6].Value2 == "UREA" || file.Sheets[1].Cells[1, 6].Value2 == "Urea"))
                return false;
            if (!(file.Sheets[1].Cells[1, 7].Value2 == "HB" || file.Sheets[1].Cells[1, 7].Value2 == "Hb"))
                return false;
            if (!(file.Sheets[1].Cells[1, 8].Value2 == "CRTN" || file.Sheets[1].Cells[1, 8].Value2 == "Crtn"))
                return false;
            if (!(file.Sheets[1].Cells[1, 9].Value2 == "IRON" || file.Sheets[1].Cells[1, 9].Value2 == "Iron"))
                return false;
            if (!(file.Sheets[1].Cells[1, 10].Value2 == "HDL" || file.Sheets[1].Cells[1, 10].Value2 == "Hdl"))
                return false;
            if (!(file.Sheets[1].Cells[1, 11].Value2 == "AP" || file.Sheets[1].Cells[1, 11].Value2 == "Ap"))
                return false;
            return true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Man")
            {
                Q4Yes.Checked = false;
                Q4No.Checked = false;
            }
        }
    
        //check name
        public Boolean CheckNames(string name)
        {
            if (name.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < name.Length; i++)
            {

                if (!((name[i] >= 'a' && name[i] <= 'z') || (name[i] >= 'A' && name[i] <= 'Z')))
                {
                    return false;
                }

            }
            return true;
        }
        //check id
        public Boolean CheckID(String id)
        {
            if (id == null || id == "")
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
        //check age
        public Boolean CheckAge(string age)
        {
            if (age.Length == 0 || age.Length > 3)
            {
                return false;
            }
            for (int i = 0; i < age.Length; i++)
            {

                if (!(age[i] >= '0' && age[i] <= '9'))
                {
                    return false;
                }

            }
            return true;
        }
        //check height
        public Boolean CheckHeight(String height)
        {
            int numberCounter = 0;
            if (height.Length <= 3 && height.Length > 0)
            {
                for (int i = 0; i < height.Length; i++)
                {
                    if (height[i] >= '0' && height[i] <= '9')
                    {
                        numberCounter++;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (numberCounter == height.Length && Convert.ToInt32(height) > 0)
                {
                    return true;
                }
            }
            return false;
        }
        //check weight
        public Boolean CheckWeight
           (String weight)
        {
            int numberCounter = 0;
            if (weight.Length <= 3 && weight.Length > 0)
            {
                for (int i = 0; i < weight.Length; i++)
                {
                    if (weight[i] >= '0' && weight[i] <= '9')
                    {
                        numberCounter++;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (numberCounter == weight.Length && Convert.ToInt32(weight) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        //check patient data units
        public Boolean CheckUnits
             (String unit)
        {
            int numberCounter = 0;
            int dotCounter = 0;

            for (int i = 0; i < unit.Length; i++)
            {
                if (unit[i] >= '0' && unit[i] <= '9')
                {
                    numberCounter++;
                }
                else if (unit[i] == '.')
                {
                    dotCounter++;
                }
                else
                {
                    return false;
                }
            }

            if (numberCounter == unit.Length - dotCounter && dotCounter <= 1)
            {
                return true;
            }

            return false;
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            panel2.SendToBack();
        }

        private void Q1yes_CheckedChanged(object sender, EventArgs e)
        {
            Q1No.Checked = !Q1yes.Checked;
        }
        private void Q1No_CheckedChanged(object sender, EventArgs e)
        {
            Q1yes.Checked = !Q1No.Checked;
        }

        private void Q2Yes_CheckedChanged(object sender, EventArgs e)
        {
            Q2No.Checked = !Q2Yes.Checked;
        }

        private void Q2No_CheckedChanged(object sender, EventArgs e)
        {
            Q2Yes.Checked = !Q2No.Checked;
        }

        private void Q3Yes_CheckedChanged(object sender, EventArgs e)
        {
            Q3No.Checked = !Q3Yes.Checked;
        }

        private void Q3No_CheckedChanged(object sender, EventArgs e)
        {
            Q3Yes.Checked = !Q3No.Checked;
        }

        private void Q4Yes_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Man")
            {
                Q4Yes.Checked = false;
                Q4No.Checked = false;
            }
            else
            {
                Q4No.Checked = !Q4Yes.Checked;
            }
        }

        private void Q4No_CheckedChanged_1(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Man")
            {
                Q4Yes.Checked = false;
                Q4No.Checked = false;
            }
            else
            {
                Q4Yes.Checked = !Q4No.Checked;
            }
        }

        private void Back_Click(object sender, EventArgs e)
        {
            Hide();
        }

        //recommendation algoritherm
        public string recAlgoritherm()
        {
            //reset the array
            for(int i = 0; i < 26; i++)
            {
                deasArray[i] = 0;
            }
            //enter values to array
            Wbc();
            Neut();
            Lymph();
            Rbc();
            Hct();
            Urea();
            Hb();
            Crtn();
            Iron();
            Hdl();
            Ap();

            int maxvalue = deasArray.Max();
            string STRreturn = "";
            if (maxvalue == 0)
            {
                STRreturn += " - " + (dict.ElementAt(26).Key + "\n");
                return STRreturn;
                //return "";
            }
            for(int i = 0; i < 26; i++)
            {
                if(deasArray[i] == maxvalue)
                {
                    STRreturn += " - " + (dict.ElementAt(i).Key + "\n");
                }
            }
            return STRreturn;
        }
        
        private void Wbc()
        {
            if (WBC.Text == null || WBC.Text=="")
                return;
            //(WBC)white Blood Cells
            if (Convert.ToInt32(Age.Text) >= 18)
            {

                if (Convert.ToInt32(WBC.Text) > 11000)
                {
                    deasArray[8]+=2;
                    deasArray[13]++;
                    deasArray[22]++;
                }
                if (Convert.ToInt32(WBC.Text) < 4500)
                {
                    deasArray[10]+=2;
                    deasArray[22]++;
                }
            }

            if (Convert.ToInt32(Age.Text) >= 4 && Convert.ToInt32(Age.Text) <= 17)
            {

                if (Convert.ToInt32(WBC.Text) > 15500)
                {
                    deasArray[8]+=2;
                    deasArray[13]++;
                    deasArray[22]++;
                }
                if (Convert.ToInt32(WBC.Text) < 5500)
                {
                    deasArray[10]+=2;
                    deasArray[22]++;
                }
            }

            if (Convert.ToInt32(Age.Text) >= 0 && Convert.ToInt32(Age.Text) <= 3)
            {

                if (Convert.ToInt32(WBC.Text) > 17500)
                {
                    deasArray[8]+=2;
                    deasArray[13]++;
                    deasArray[22]++;
                }
                    

                if (Convert.ToInt32(WBC.Text) < 6000)
                {
                    deasArray[10]+=2;
                    deasArray[22]++;
                }
            }
        }

        private void Neut()
        {
            if (NEUT.Text == null || NEUT.Text == "")
                return;
            if (Convert.ToInt32(NEUT.Text) > 54)
            {
                deasArray[8]+=2;
            }
            if (Convert.ToInt32(NEUT.Text) < 28)
            {
                deasArray[4]+=2;
                deasArray[8]+=2;
                deasArray[22]++;
            }
        }

        private void Lymph()
        {
            if (LYMPH.Text == null || LYMPH.Text == "")
                return;
            if (Convert.ToInt32(LYMPH.Text) > 52)
            {
                deasArray[22]+=2;
                deasArray[8]+=2;
            }
            if (Convert.ToInt32(LYMPH.Text) < 36)
            {
                deasArray[4]+=2;
            }
        }

        private void Rbc()
        {
            if (RBC.Text == null || RBC.Text == "")
                return;

            if (Convert.ToDouble(RBC.Text) > 6)
            {
                if (Q3Yes.Checked)
                {
                    return;
                }
                deasArray[4]+=2;
                deasArray[18] += 2;
                deasArray[19] += 2;
            }
            if (Convert.ToDouble(RBC.Text) < 4.5)
            {
                deasArray[2] += 2;
                deasArray[0] += 2;
            }
        }

        private void Hct()
        {

            if (HCT.Text == null || HCT.Text == "")
                return;

            if (comboBox1.Text == "Man")
            {
                if (Convert.ToInt32(HCT.Text) > 54)
                {
                    if (Q3Yes.Checked)
                    {
                        return;
                    }
                    deasArray[18]++;
                }
                if (Convert.ToInt32(HCT.Text) < 37)
                {
                    deasArray[0] += 2;
                    deasArray[2] += 2;
                }  
            }
            else
            {
                if (Convert.ToInt32(HCT.Text) > 47)
                {
                    if (Q3Yes.Checked)
                    {
                        return;
                    }
                    deasArray[18] += 2;
                }
                    

                if (Convert.ToInt32(HCT.Text) < 33)
                {
                    deasArray[0] += 2;
                    deasArray[2] += 2;
                }
                    
            }
        }
        private void Urea()
        {
            if (UREA.Text == null || UREA.Text == "")
                return;

            if (Q2Yes.Checked == true)
            {
                
                if (Convert.ToInt32(UREA.Text) > (43 * 1.1))
                {
                    deasArray[7] += 2;
                    deasArray[15] += 2;
                    deasArray[25] += 2;
                        
                }
                if (Convert.ToInt32(UREA.Text) < (17 * 1.1))
                {
                    if (Q4Yes.Checked == true)
                    {
                        return;
                    }
                    deasArray[0] += 2;
                    deasArray[2] += 2;
                }
                
            }
            else
            {
                if (Convert.ToInt32(UREA.Text) > 43)
                {
                    deasArray[7] += 2;
                    deasArray[15] += 2;
                    deasArray[25] += 2;

                }
                if (Convert.ToInt32(UREA.Text) < 17)
                {
                    if (Q4Yes.Checked == true)
                    {
                        return;
                    }
                    deasArray[0] += 2;
                    deasArray[2] += 2;
                }
            }
        }

        private void Hb()
        {
            if (HB.Text == null || HB.Text == "")
                return;

            //(WBC)white Blood Cells
            if (comboBox1.Text == "Man")
            {
                if (Convert.ToInt32(HB.Text) < 12)
                {
                    deasArray[0] += 2;
                    deasArray[16] += 2;
                    deasArray[2] += 2;
                    return;
                }
            }

            if (comboBox1.Text == "Woman")
            {

                if (Convert.ToInt32(HB.Text) < 12)
                {
                    deasArray[0] += 2;
                    deasArray[16] += 2;
                    deasArray[2] += 2;
                    return;
                }

            }
            if (Convert.ToInt32(Age.Text) >= 0 && Convert.ToInt32(Age.Text) <= 17)
            {
                if (Convert.ToInt32(HB.Text) < 11.5)
                {
                    deasArray[0] += 2;
                    deasArray[16] += 2;
                    deasArray[2] += 2;
                    return;
                }
            }
        }

        private void Crtn()
        {
            if (CRTN.Text == null || CRTN.Text == "")
                return;

            if (Convert.ToInt32(Age.Text) >= 60)
            {
                if (Convert.ToDouble(CRTN.Text) > 1.2)
                {
                    deasArray[15]++;
                    deasArray[17]+=2;
                    deasArray[23]+=2;
                }
                if (Convert.ToDouble(CRTN.Text) < 0.6)
                {
                    deasArray[25] += 2;
                }
            }

            if (Convert.ToInt32(Age.Text) >= 18 && Convert.ToInt32(Age.Text) <= 59)
            {

                if (Convert.ToDouble(CRTN.Text) > 1)
                {
                    deasArray[15]++;
                    deasArray[17]+=2;
                    deasArray[23]+=2;
                }

                if (Convert.ToDouble(CRTN.Text) < 0.6)
                {
                    deasArray[25] += 2;
                }

            }

            if (Convert.ToInt32(Age.Text) >= 3 && Convert.ToInt32(Age.Text) <= 17)
            {

                if (Convert.ToDouble(CRTN.Text) > 1)
                {
                    deasArray[15]++;
                    deasArray[17]+=2;
                    deasArray[23]+=2;
                }

                if (Convert.ToDouble(CRTN.Text) < 0.5)
                {
                    deasArray[25] += 2;
                }

            }

            if (Convert.ToInt32(Age.Text) >= 0 && Convert.ToInt32(Age.Text) <= 2)
            {

                if (Convert.ToDouble(CRTN.Text) > 0.5)
                {
                    deasArray[15]++;
                    deasArray[17]+=2;
                    deasArray[23]+=2;
                }

                if (Convert.ToDouble(CRTN.Text) < 0.2)
                {
                    deasArray[25] += 2;
                }
            }
        }

        private void Iron()
        {
            if (IRON.Text == null || IRON.Text == "")
                return;

            if (comboBox1.Text == "Man")
            {
                if (Convert.ToInt32(IRON.Text) > 160)
                {
                    deasArray[6] += 2;
                }
                if (Convert.ToInt32(IRON.Text) < 60)
                {
                    deasArray[25] += 2;
                }
            }
            else
            {
                if (Convert.ToInt32(IRON.Text) > (160 * 0.8))
                {
                    deasArray[6] += 2;
                }

                if (Convert.ToInt32(IRON.Text) < (60 * 0.8))
                {
                    if (Q4Yes.Checked)
                    {
                        return;
                    }
                    deasArray[25] += 2;
                }
                    
            }
        }

        private void Hdl()
        {

            if (HDL.Text == null || HDL.Text == "")
                return;

            //Ethiopians higher by 20%
            if (Q1yes.Checked == true)
            {
                if (comboBox1.Text == "Man")
                {
                    if (Convert.ToDouble(HDL.Text) > 62 * 1.2)
                    {
                        return;
                    }
                       
                    if (Convert.ToDouble(HDL.Text) < 29 * 1.2)
                    {
                        deasArray[3] += 2;
                        deasArray[12] += 2;
                        deasArray[21] += 2;
                    }
                }
                else
                {

                    if (Convert.ToDouble(HDL.Text) > 82 * 1.2)
                    {
                        return;
                    }
                    if (Convert.ToDouble(HDL.Text) < 34 * 1.2)
                    {
                        deasArray[3] += 2;
                        deasArray[12] += 2;
                        deasArray[21] += 2;
                    }
                }
            }
            else
            {
                if (comboBox1.Text == "Man")
                {
                    if (Convert.ToDouble(HDL.Text) > 62)
                    {
                        return;
                    }
                    if (Convert.ToDouble(HDL.Text) < 29)
                    {
                        deasArray[3] += 2;
                        deasArray[12] += 2;
                        deasArray[21] += 2;
                    }
                }
                else
                {
                    if (Convert.ToDouble(HDL.Text) > 82)
                    {
                        return;
                    }
                    if (Convert.ToDouble(HDL.Text) < 34)
                    {
                        deasArray[3] += 2;
                        deasArray[12] += 2;
                        deasArray[21] += 2;
                    }
                }
            }
        }

        private void Ap()
        {
            if (AP.Text == null || AP.Text == "")
                return;

            //Easterners 60 - 120
            if (Q2Yes.Checked == true)
            {
                if (Convert.ToInt32(AP.Text) > 120)
                {
                    if (Q4Yes.Checked)
                    {
                        return;
                    }
                    deasArray[14] += 2;
                    deasArray[11] += 2;
                    deasArray[20] += 2;
                    deasArray[24] += 2;
                }
                if (Convert.ToInt32(AP.Text) < 60)
                {
                    deasArray[25] += 2;
                    deasArray[9] += 2;
                }
            }
            else
            {
                if (Convert.ToInt32(AP.Text) > 90)
                {
                    if (Q4Yes.Checked)
                    {
                        return;
                    }
                    deasArray[14] += 2;
                    deasArray[11] += 2;
                    deasArray[20] += 2;
                    deasArray[24] += 2;
                }
                if (Convert.ToInt32(AP.Text) < 30)
                {
                    deasArray[25] += 2;
                    deasArray[9] += 2;
                }
            }
        }
        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
