using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace User_Info_dump
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string accounts_raw;
        List<string> accounts_list = new List<string>();
        List<string> accounts_list_final = new List<string>();
        List<string> dumped_account_details = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        public string runcommand(object command)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("powershell", "/c " + command);


                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;

                procStartInfo.CreateNoWindow = true;

                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                string result = proc.StandardOutput.ReadToEnd();

                Console.WriteLine(result);
                return result;
            }
            catch (Exception objException)
            {
                return $@"failed {objException}";
            }
        }

        private void Dump_Local_accounts(object sender, RoutedEventArgs e)
        {
            string command_result = runcommand("net user");
            result_output.Text = command_result;
            try
            {
                accounts_raw = command_result.Split("-------------------------------------------------------------------------------")[1].Split("The command completed successfully.")[0];
                accounts_list = accounts_raw.Split(" ").ToList();
                foreach (string account in accounts_list)
                {
                    if (!(account == " " || account == "" || account == "\r\n"))
                    {
                        try
                        {
                            string test = account.Replace("\n", "").Replace("\r", "");
                            if (test != "")
                            {
                                accounts_list_final.Add(test);
                            }
                        }
                        catch
                        {
                            accounts_list_final.Add(account);
                        }
                    }

                }

            }
            catch
            { }
        }

        private void Dump_Domain_accounts(object sender, RoutedEventArgs e)
        {
            string command_result = runcommand("net user /domain");
            result_output.Text = command_result;
            try
            {
                accounts_raw = command_result.Split("-------------------------------------------------------------------------------")[1].Split("The command completed successfully.")[0];
                accounts_list = accounts_raw.Split(" ").ToList();
                foreach (string account in accounts_list)
                {
                    if (!(account == " " || account == "" || account == "\r\n"))
                    {
                        try
                        {
                            string test = account.Replace("\n", "").Replace("\r", "");
                            if (test != "")
                            {
                                accounts_list_final.Add(test);
                            }
                        }
                        catch
                        {
                            accounts_list_final.Add(account);
                        }
                    }

                }

            }
            catch
            { }
        }

        private void Dump_Account_Info_local(object sender, RoutedEventArgs e)
        {
            if (accounts_list_final.Count != 0)
            {
                new Thread(() => {
                    foreach (string account in accounts_list_final)
                    {
                        dumped_account_details.Add(runcommand($@"net user {account}"));
                    }
                    MessageBox.Show("Done");
                }).Start();
            }
        }
        private void Dump_Account_Info_domain(object sender, RoutedEventArgs e)
        {
            if (accounts_list_final.Count != 0)
            {
                new Thread(() => {
                    foreach (string account in accounts_list_final)
                    {
                        dumped_account_details.Add(runcommand($@"net user /domain {account}"));
                    }
                    MessageBox.Show("Done");
                }).Start();
            }
        }

        private void Save_Dumped_Account_Info_To_File_button(object sender, RoutedEventArgs e)
        {
            if(System.IO.File.Exists("dumped_account_info.txt"))
            {
                System.IO.File.Delete("dumped_account_info.txt");
            }
            System.IO.File.WriteAllLines("dumped_account_info.txt", dumped_account_details);
        }

        private void Save_Accounts_To_File_Button(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists("accounts_list.txt"))
            {
                System.IO.File.Delete("accounts_list.txt");
            }
            System.IO.File.WriteAllLines("accounts_list.txt", accounts_list_final);
        }
    }
}
