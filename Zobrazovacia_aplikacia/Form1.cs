using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using S7.Net;

namespace Zobrazovacia_aplikacia
{
    public partial class Form1 : Form
    {
        private ushort OK_L = 0, NOK_L =0, OK_P=0, NOK_P = 0, plan_L = 0, plan_P = 0, trend_L = 0, trend_P = 0;

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private bool trend_plus_L = false, trend_plus_P = false, chyba_1RB1 = false, chyba_2KT = false, chyba_3RP1 = false, chyba_3LD1 = false, chyba_4RB2 = false, chyba_5RP24 = false,
                       chyba_5RP23 = false, chyba_5RP22 = false, chyba_5RP21 = false, chyba_5LD2 = false, chyba_6VK = false;


        private bool upozornenie_1RB1 = false, upozornenie_2KT = false, upozornenie_3RP1 = false, upozornenie_3LD1 = false, upozornenie_4RB2 = false, upozornenie_5RP24 = false,
                       upozornenie_5RP23 = false, upozornenie_5RP22 = false, upozornenie_5RP21 = false, upozornenie_5LD2 = false, upozornenie_6VK = false;

        public Form1()
        {
            InitializeComponent();

            Timer Timer1 = new Timer();
            Timer Timer2 = new Timer();
            Timer Timer3 = new Timer();
            Timer2.Interval = 10000;
            Timer1.Interval = 1000;
            Timer3.Interval = 500;
            Timer2.Tick += new EventHandler(Timer2_Tick);
            Timer2.Enabled = true;
            Timer1.Tick += new EventHandler(Timer1_Tick);
            Timer1.Enabled = true;
            Timer3.Tick += new EventHandler(Timer3_Tick);
            Timer3.Enabled = true;
        }


        private void Timer1_Tick(object Sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\VesconSettings");
           
            string ip = key.GetValue("IP_ADRESA").ToString();
            short rack = short.Parse(key.GetValue("RACK_PLC").ToString());
            short slot = short.Parse(key.GetValue("SLOT_PLC").ToString());
            string rada_plc = key.GetValue("RADA_PLC").ToString();

            time.Text = DateTime.Now.ToString("HH:mm", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            date.Text = DateTime.Now.ToShortDateString();

            String display = key.GetValue("DISPLAY").ToString();
            showOnScreen(display, true);


            try
            {
                Plc plc = new Plc(ConverterCPU(rada_plc), ip, rack, slot);
                plc.Open();
                OK_L = (ushort)plc.Read("DB616.DBX0.0");
                NOK_L = (ushort)plc.Read("DB616.DBX2.0");
                OK_P = (ushort)plc.Read("DB616.DBX4.0");
                NOK_P = (ushort)plc.Read("DB616.DBX6.0");
                plan_L = (ushort)plc.Read("DB616.DBX8.0");
                plan_P = (ushort)plc.Read("DB616.DBX10.0");
                trend_L = (ushort)plc.Read("DB616.DBX12.0");
                trend_P = (ushort)plc.Read("DB616.DBX14.0");

                trend_plus_L = (bool)plc.Read("DB616.DBX16.0");
                trend_plus_P = (bool)plc.Read("DB616.DBX16.1");
                chyba_1RB1 = (bool)plc.Read("DB616.DBX28.0");
                chyba_2KT = (bool)plc.Read("DB616.DBX28.1");
                chyba_3RP1 = (bool)plc.Read("DB616.DBX28.2");
                chyba_3LD1 = (bool)plc.Read("DB616.DBX28.3");
                chyba_4RB2 = (bool)plc.Read("DB616.DBX28.4");
                chyba_5RP24 = (bool)plc.Read("DB616.DBX28.5");//prava
                chyba_5RP23 = (bool)plc.Read("DB616.DBX28.6");//prava
                chyba_5RP22 = (bool)plc.Read("DB616.DBX28.7");//lava
                chyba_5RP21 = (bool)plc.Read("DB616.DBX29.0");//lava
                chyba_5LD2 = (bool)plc.Read("DB616.DBX29.1");
                chyba_6VK = (bool)plc.Read("DB616.DBX29.2");

                upozornenie_1RB1 = (bool)plc.Read("DB616.DBX29.3");
                upozornenie_2KT = (bool)plc.Read("DB616.DBX29.4");
                upozornenie_3RP1 = (bool)plc.Read("DB616.DBX29.5");
                upozornenie_3LD1 = (bool)plc.Read("DB616.DBX29.6");
                upozornenie_4RB2 = (bool)plc.Read("DB616.DBX29.7");
                upozornenie_5RP24 = (bool)plc.Read("DB616.DBX30.0");
                upozornenie_5RP23 = (bool)plc.Read("DB616.DBX30.1");
                upozornenie_5RP22 = (bool)plc.Read("DB616.DBX30.2");
                upozornenie_5RP21 = (bool)plc.Read("DB616.DBX30.3");
                upozornenie_5LD2 = (bool)plc.Read("DB616.DBX30.4");
                upozornenie_6VK = (bool)plc.Read("DB616.DBX30.5");
                plc.Close();
                pripojenie.Visible = false;
            }
            catch (Exception)
            {
                pripojenie.Visible = true;
            }

            AK_LS_OK.Text = OK_L.ToString("D3");
            AK_LS_NOK.Text = NOK_L.ToString("D3");
            AK_PS_OK.Text = OK_P.ToString("D3");
            AK_PS_NOK.Text = NOK_P.ToString("D3");
            PL_LS.Text = plan_L.ToString("D3");
            PL_PS.Text = plan_P.ToString("D3");
            Trend_L.Text = trend_L.ToString("D2");
            Trend_P.Text = trend_P.ToString("D2");
            if (trend_plus_L == false) Trend_L.ForeColor = Color.Red;
            else Trend_L.ForeColor = Color.Green;

            if (trend_plus_P == false) Trend_P.ForeColor = Color.Red;
            else Trend_P.ForeColor = Color.Green;

        }
        private void Timer2_Tick(object Sender, EventArgs e)
        {
            if (panel1.Visible == false)
            {
                panel1.Visible = true;
            }
            else
            {
                panel1.Visible = false;
            }
        }

        private void Timer3_Tick(object Sender, EventArgs e)
        {
            if (panel1.Visible == true)
            {
                blikanie(error_VK6, warningVK6, chyba_6VK, upozornenie_6VK);
                blikanie(error_4RB2, warning_4RB2, chyba_4RB2, upozornenie_4RB2);
                blikanie(error_2KT, warning_2KT, chyba_2KT, upozornenie_2KT);
                blikanie(error1RB1, warning1RB1, chyba_1RB1, upozornenie_1RB1);
                blikanie(error_3LD1, warning_3LD1, chyba_3LD1, upozornenie_3LD1);
                blikanie(error_3RP1, warning_3RP1, chyba_3RP1, upozornenie_3RP1);

                blikanie(error_5LD2, warning_5LD2, chyba_5LD2, upozornenie_5LD2);
                blikanie(error_5LP2, warning_5LP2, chyba_5RP21, upozornenie_5RP21);
                blikanie(error_5LP2, warning_5LP2, chyba_5RP22, upozornenie_5RP22);

                blikanie(error_5RP2, warning_5PR2, chyba_5RP23, upozornenie_5RP23);
                blikanie(error_5RP2, warning_5PR2, chyba_5RP24, upozornenie_5RP24);


            }

        }




        void showOnScreen(string screenName, bool maximised = false)
        {
            try
            {
                Screen res = Screen.AllScreens.FirstOrDefault(s => s.DeviceName == screenName);
                Location = res.WorkingArea.Location;
                if (maximised)
                {
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    TopMost = true;
                }
                else
                {
                    FormBorderStyle = FormBorderStyle.FixedSingle;
                    WindowState = FormWindowState.Normal;
                    TopMost = false;
                }
            }
            catch
            {
                FormBorderStyle = FormBorderStyle.FixedSingle;
                WindowState = FormWindowState.Normal;
                TopMost = false;
                //MessageBox.Show("DISPLAY NIE JE PRIPOJENY");
            }


        }
        void blikanie(PictureBox blikanie_error, PictureBox blikanie_warning, bool error, bool warning)
        {
            if ((blikanie_error.Visible == false) && (blikanie_warning.Visible == false))
            {
                if (error)
                {


                    blikanie_error.Visible = true;
                }
                else if (warning)
                {


                    blikanie_warning.Visible = true;
                }
                else
                {

                }

            }
            else
            {
                blikanie_error.Visible = false;
                blikanie_warning.Visible = false;
            }

        }

        private static CpuType ConverterCPU(string CPU)
        {

            CpuType cpuType = CpuType.S71500;
            if (CPU == "SIEMENS 200") { cpuType = CpuType.S7200; }
            else if (CPU == "SIEMENS 300") { cpuType = CpuType.S7300; }
            else if (CPU == "SIEMENS 400") { cpuType = CpuType.S7400; }
            else if (CPU == "SIEMENS 1200") { cpuType = CpuType.S71200; }
            else if (CPU == "SIEMENS 1500") { cpuType = CpuType.S71500; }
            return cpuType;
        }


    }
}
