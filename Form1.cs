using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

//Izveidot atskaiti:

//2. Connection string jānolasa no faila „C:\Temp\ConnS.txt” vai jālieto viens connection string, kas norādīts App.config failā. Connection string kodā iešūt nedrīkst!!!!!.
//----> Izmantoju OpenFileDialog.
//4. Pirmās lapas augšā (tikai pirmās lapas) tiek drukāts logo un atskaites nosaukums.
//----> Atskaitē pie logo un atskaites nosaukuma properties redzamību uzstādiju kā izteiksmi: =Iif(Globals.PageNumber>1, True, False)
//5. Izdrukāt tabulu, kas satur izdevniecību, grāmatu un autoru sarakstu.
//Izdevniecībai jānorāda nosaukums.
//Grāmatas jāgrupē pa izdevniecībām. 
//----> Tabulai pievienoju, ka tiek grupēts pēc izdevniecībām (Report1.rdlc [Design])
//Grāmatai jānorāda, tips, cena un autoru saraksts
//----> Autoru sarakstu dabūju izmantotjot SQL query, skatīt DataTable1TableAdapter SQL Statement.
//Autoram jānorāda Vārds un Uzvārds.
//6. Jānorāda grāmatu skaits katrā izdevniecībā un kopējais grāmatu skaits sarakstā (tabulā).
//---->Pievienoju pēc katras izdevniecības grupas kopējo grāmatu skaitu un tabulas beigās kopējo grāmatu skaitu sarakstā.
//7. Pievienot vēl kādu paša izvēlētu apkopojuma skatu.Piemēram (viens no šiem):
//---->grafiks, kur attēlots grāmatu skaits pa tipiem;
//---->grafiks, kur attēlota grāmatas vidējā cena izdevniecībā;
//8. Jānodrošina atbilstoša datu sadalīšana pa lapām. (Pievērsiet uzmanību, lai beigās nerastos tukša lapa.Atcerieties, ka ierakstu skaits datubāzē var mainīties.)
//---->ReportViewer pats sadala.
//9. Izveidot iespēju atskaiti aplūkot nedrukājot.
//---->ReportViewer pats piedāvā iespēju aplūkot nedrukājot.
//10. Izveidot iespēju atskaiti izdrukāt. (Drukāšanu ieteicams testēt izmantojot pdf printerus.)
//---->ReportViewer pats piedāvā iespēju izdrukāt.

namespace ReportMD
{
    public partial class Form1 : Form
    {
        string failasaturs;
        SqlConnection myConnection;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = openFileDialog1.ShowDialog(); // Atver dialogu, kur var izvēlēties failu
                if (result == DialogResult.OK) //Lai nospiežot cancel programma nenosprāgtu
                {
                    string file = openFileDialog1.FileName;     //Saglabā faila atrašanās vietu
                    failasaturs = File.ReadAllText(file);   // Simbolu virknē failasaturs saglabā visu faila tekstu
                }

                myConnection = new SqlConnection();
                myConnection.ConnectionString = failasaturs; //Data Source=USER;initial catalog=md2;Integrated Security = True
                myConnection.Open();
                
                DataTable1TableAdapter.Connection = myConnection; //Norādu, ka jāizmanto myConnection, nevis tas, kas saglabāts pie Properties->Connection
                DataTable1TableAdapter.Fill(DataSet1.DataTable1); //Aizpildu DataSetā tabulu DataTable

                reportViewer1.RefreshReport();
                myConnection.Close();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Cannot open a connection without specifying a data source or server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (SqlException ex)
            {
                if (ex.Number == 53)
                {
                    MessageBox.Show("Error: " + ex.Number.ToString() + "\nAn error has occurred while establishing a connection to the server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (ex.Number == -2)
                {
                    MessageBox.Show("Error: " + ex.Number.ToString() + "\nTimeout expired. The timeout period elapsed prior to completion of the operation or the server is not responding", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (ex.Number == 17)
                {
                    MessageBox.Show("Error:" + ex.Number.ToString() + "\nInvalid server name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (ex.Number == 4060)
                {
                    MessageBox.Show("Error: " + ex.Number.ToString() + "\nInvalid database name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (ex.Number == 18456)
                {
                    MessageBox.Show("Error: " + ex.Number.ToString() + "\nInvalid user name or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Error: " + ex.Number.ToString() + "\n" + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Some error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
    }

    }
}
