using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using OfficeOpenXml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace csgofast
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(Start) { IsBackground = true };
            thread.Start();
        }
        //https://csgofast123.com/#r/797zkg
        private void Start()
        {
            IWebElement myField;
            int proxchet1 = 0;
            string refsil = textBox4.Text;
            string putbalans = textBox1.Text;
            string putlogin = textBox2.Text;
            string putprox = textBox3.Text;
            StreamReader objReader = new StreamReader(putlogin);
            string sLine = "";
            ArrayList login = new ArrayList();
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                    login.Add(sLine);
            }
            objReader.Close();
            StreamReader objReader1 = new StreamReader(putprox);
            string sLine1 = "";
            ArrayList proxy = new ArrayList();
            while (sLine1 != null)
            {
                sLine1 = objReader1.ReadLine();
                if (sLine1 != null)
                    proxy.Add(sLine1);
            }
            objReader1.Close();
            string[] username = new string[login.Count];
            string[] password = new string[login.Count];
            for (int i = 0; i < login.Count; i++)
            {
                username[i] = Regex.Match(login[i].ToString(), ".*(?=:)").Value;
                password[i] = Regex.Match(login[i].ToString(), "(?<=:).*").Value;
            }
            ExcelPackage package = new ExcelPackage(new FileInfo(putbalans));
            ExcelWorksheet sheet = package.Workbook.Worksheets[1];
            for (int logchet = 0; logchet < login.Count; logchet++)
            {
                for (int proxchet = proxchet1; proxchet < proxy.Count; proxchet++)
                {
                    proxchet1++;
                    IWebElement bal;
                    ChromeOptions options = new ChromeOptions();
                    options.AddArguments("--proxy-server="+textBox5.Text+"://"+proxy[proxchet]);
                    var driver = new ChromeDriver(options);
                    try
                    {
                        driver.Navigate().GoToUrl(refsil);
                        myField = driver.FindElement(By.XPath("/html/body/div[3]/div[1]/div[2]/nav/a[1]"));
                    }
                    catch (NoSuchElementException)
                    {
                        driver.Close();
                        driver.Dispose();
                        continue;
                    }
                    catch (WebDriverTimeoutException)
                    {
                        driver.Close();
                        driver.Dispose();
                        continue;
                    }
                    catch (WebDriverException)
                    {
                        driver.Dispose();
                        driver.Dispose();
                        continue;
                    }
                    for (int x = 0; x < 1; x++)
                    {
                        try
                        {
                            myField = driver.FindElement(By.Id("adroll_banner_close"));
                            myField.Click();
                        }
                        catch (NoSuchElementException)
                        {
                            continue;
                        }
                        catch (WebDriverException)
                        {
                            driver.Close();
                            driver.Dispose();
                            continue;
                        }
                    }
                    for (int x = 0; x < 1; x++)
                    {
                        try
                        {
                            myField = driver.FindElement(By.Id("onesignal-popover-cancel-button"));
                            myField.Click();
                        }
                        catch (NoSuchElementException)
                        {
                            continue;
                        }
                        catch (WebDriverException)
                        {
                            driver.Close();
                            driver.Dispose();
                            continue;
                        }
                    }
                    myField = driver.FindElement(By.XPath("/html/body/div[3]/div[1]/div[2]/nav/a[1]"));
                    myField.Click();
                    myField = driver.FindElement(By.XPath("/html/body/div[3]/div[6]/div/div/div[2]/ul/li[1]/button"));
                    myField.Click();
                    String parentHandle = driver.WindowHandles.First();
                    String childHandle = driver.WindowHandles.ElementAt(2);
                    driver.SwitchTo().Window(childHandle);
                    try
                    {
                        IWebElement captcha = driver.FindElement(By.Id("captchagid"));
                    }
                    catch (NoSuchElementException)
                    {
                        driver.Close();
                        driver.Dispose();
                        continue;
                    }
                    string value = Regex.Match(driver.PageSource, "(?<=id\" value=\").*(?=\" )").Value;
                    if (value == "-1")
                    {
                        IWebElement user = driver.FindElement(By.Name("username"));
                        user.SendKeys(username[logchet]);
                        IWebElement pass = driver.FindElement(By.Name("password"));
                        pass.SendKeys(password[logchet]);
                        IWebElement vhodsteam = driver.FindElement(By.Id("imageLogin"));
                        vhodsteam.Click();
                        Thread.Sleep(10000);
                        driver.SwitchTo().Window(parentHandle);
                        driver.Navigate().GoToUrl("https://csgofast.com/game/classic/");
                        try
                        {
                            bal = driver.FindElement(By.XPath("/html/body/div[3]/header/div[2]/div[1]/div/div/div/span"));
                        }
                        catch (WebDriverException)
                        {
                            driver.Dispose();
                            driver.Dispose();
                            continue;
                        }
                        string balans = bal.Text;
                        sheet.Cells[logchet + 1, 1].Value = username[logchet];
                        sheet.Cells[logchet + 1, 2].Value = balans;
                        sheet.Cells[logchet + 1, 3].Value = proxy[proxchet];
                        driver.Close();
                        driver.Dispose();
                        package.Save();
                        break;
                    }
                    else
                    {
                        driver.Close();
                        driver.Dispose();
                    }
                }
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "EXCEL|*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.FileName;
                textBox1.Text = path;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "TXT|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.FileName;
                textBox2.Text = path;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "EXCEL|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.FileName;
                textBox3.Text = path;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
                textBox5.Text = "socks4";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                textBox5.Text = "socks5";
        }
    }
}
