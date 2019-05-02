using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Data;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace HomeoMedicinLabelingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static void ReadFromExcel(string medicin_file, string search_medicine = "", string search_dose = "")
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWB = null;
            
            List<string> filter = new List<string>();

            // Chheck for null value in search_dose
            if (search_dose.Length > 0)
            {
                int start = search_dose[0] - 48;
                Int32.TryParse(search_dose.Substring(2), out int end);

                for (int i = start; i <= end; i++)
                {
                    filter.Add(search_medicine + " M/" +  i.ToString());
                }
            }

            try
            {
                xlWB = xlApp.Workbooks.Open(medicin_file);
            } 
            catch (Exception)
            {
                MessageBox.Show("Source file not found in " + medicin_file);
                return;
            }

            // Visible the XlApp
            xlApp.Visible = true;

            // Get the 1st WoerkSheet
            Excel._Worksheet xlWS = (Excel._Worksheet)xlWB.Sheets[1];
            
            // Select paper Size A4
            xlWS.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;

            Excel.Range xlRange = xlWS.UsedRange;

            // Filter
            if (filter.Count > 0) {
                xlRange.AutoFilter(1, filter.ToArray(), Excel.XlAutoFilterOperator.xlFilterValues, false, false);
            }
        }
        
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            //string medicin_file = @"E:\Programms\Shuvasis\HomeoMedicinLabelingApp\MedicinList.xlsx";
            string medicin_file = fileName.Text;
            string medicine = medicineName.Text;
            string dose = Regex.Replace(medicineDose.Text, @"s", "");

            ReadFromExcel(medicin_file, medicine, dose);
        }

        private void Btn_browseFile_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            
            // formatting openDialog options 
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            // Launch Openfiledialog using ShowDialog method
            Nullable<bool> result = openFileDialog.ShowDialog();
            
            if(result == true)
            {
                fileName.Text = openFileDialog.FileName;
            }

        }
    }

    public class Medicin
    {
        private string _name;
        private int _dose;

        public Medicin(string name, int dose)
        {
            this._name = name;
            this._dose = dose;
        }

        public string name
        {
            get {return this._name; }

            set { this._name = name; }
        }

        public int dose
        {
            get { return this._dose; }

            set { this._dose = dose; }
        }
    }
}
