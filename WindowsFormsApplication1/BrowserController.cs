using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Temp;

namespace WindowsFormsApplication1
{
    public partial class BrowserController : Form
    {
        private FireFoxBrowser _fireFoxBrowser;

        public BrowserController()
        {
            InitializeComponent();

            LaunchBrowser();
        }

        public BrowserController(string p)
        {
            InitializeComponent();

            LaunchBrowser("");
        }

        private void LaunchBrowser(string p)
        {
            if (_fireFoxBrowser != null)
            {
                _fireFoxBrowser.Close();
                _fireFoxBrowser = null;

            }
            _fireFoxBrowser = new FireFoxBrowser(p);
        }

        private void LaunchBrowser()
        {
            if (_fireFoxBrowser != null)
            {
                _fireFoxBrowser.Close();
                _fireFoxBrowser = null;

            }
            _fireFoxBrowser = new FireFoxBrowser();
        }

        private void indexButton_Click(object sender, EventArgs e)
        {
            new Thread(delegate()
            {
                LoadIndex();
            }).Start();
        }

        private bool LoadIndex()
        {
            try
            {
                int count = 0;
                bool run = true;
                while (run)
                {
                    this.Invoke(new MethodInvoker(() => run = indexCheckBox.Checked));
                    this.Invoke(new MethodInvoker(() => indexCheckBox.Text = "" + count));

                    if (_fireFoxBrowser.LoadIndex("http://indianvisa-bangladesh.nic.in/visa/index.html"))
                        break;
                    count++;

                    Thread.Sleep(1000);
                }

                this.Invoke(new MethodInvoker(() => indexCheckBox.ForeColor = Color.Blue));

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        private void tfcButton_Click(object sender, EventArgs e)
        {
            new Thread(delegate()
            {
                LoadTfc();
            }).Start();

        }

        private bool LoadTfc()
        {
            try
            {
                int count = 0;
                bool run = true;
                while (run)
                {
                    count++;
                    this.Invoke(new MethodInvoker(() => tfcCheckBox.Text = "" + count));

                    if (_fireFoxBrowser.LoadTfc())
                        break;

                    Thread.Sleep(1000);
                    this.Invoke(new MethodInvoker(() => run = tfcCheckBox.Checked));
                }

                this.Invoke(new MethodInvoker(() => tfcCheckBox.ForeColor = Color.Blue));

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void ivrButton_Click(object sender, EventArgs e)
        {
            TfcFill();
            new Thread(delegate()
            {
                LoadIvr();
            }).Start();
        }

        private bool TfcFill()
        {
            var a = _fireFoxBrowser.PutTemporaryApplicationId(tempIdTextBox.Text);
            var b = _fireFoxBrowser.PutCaptcha(captchaTextBox.Text);

            if (a && b)
            {
                return true;
            }
            return false;
        }

        private bool LoadIvr()
        {
            try
            {
                int count = 0;
                bool run = true;
                while (run)
                {
                    count++;
                    this.Invoke(new MethodInvoker(() => ivrCheckBox.Text = "" + count));

                    if (_fireFoxBrowser.LoadIvr())
                        break;

//                    Thread.Sleep(1000);
                    this.Invoke(new MethodInvoker(() => run = ivrCheckBox.Checked));
                }

                this.Invoke(new MethodInvoker(() => ivrCheckBox.ForeColor = Color.Blue));

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void ivr1Button_Click(object sender, EventArgs e)
        {
            IvrFill();
            new Thread(delegate()
            {
                LoadIvr1();
            }).Start();

        }

        private bool IvrFill()
        {
            return _fireFoxBrowser.IvrCheckOtherPassport();
        }

        private bool LoadIvr1()
        {
            try
            {
                int count = 0;
                bool run = true;
                while (run)
                {
                    count++;
                    this.Invoke(new MethodInvoker(() => ivr1CheckBox.Text = "" + count));

                    if (_fireFoxBrowser.LoadIvr1())
                        break;

//                    Thread.Sleep(1000);
                    this.Invoke(new MethodInvoker(() => run = ivr1CheckBox.Checked));
                }

                this.Invoke(new MethodInvoker(() => ivr1CheckBox.ForeColor = Color.Blue));

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void ivr2Button_Click(object sender, EventArgs e)
        {
            Ivr1Fill();
            new Thread(delegate()
            {
                LoadIvr2();
            }).Start();

        }

        private bool Ivr1Fill()
        {
            return _fireFoxBrowser.Ivr1SelectOccupationOf(OccupationOftextBox.Text);
        }

        private bool LoadIvr2()
        {
            try
            {
                int count = 0;
                bool run = true;
                while (run)
                {
                    count++;
                    this.Invoke(new MethodInvoker(() => ivr2CheckBox.Text = "" + count));

                    if (_fireFoxBrowser.LoadIvr2())
                        break;

//                    Thread.Sleep(1000);
                    this.Invoke(new MethodInvoker(() => run = ivr2CheckBox.Checked));
                }

                this.Invoke(new MethodInvoker(() => ivr2CheckBox.ForeColor = Color.Blue));

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void sfButton_Click(object sender, EventArgs e)
        {
            Ivr2Fill();
            new Thread(delegate()
            {
                LoadSf();
            }).Start();
        }

        private bool Ivr2Fill()
        {
            var a = _fireFoxBrowser.Ivr2CheckPrevious();
            var b = _fireFoxBrowser.Ivr2CheckRefused();
            var c = _fireFoxBrowser.Ivr2CheckSaarc();

            if (a && b && c)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool LoadSf()
        {
            try
            {
                int count = 0;
                bool run = true;
                while (run)
                {
                    count++;
                    this.Invoke(new MethodInvoker(() => sfCheckBox.Text = "" + count));

                    if (_fireFoxBrowser.LoadSf())
                        break;

                    Thread.Sleep(2000);
                    this.Invoke(new MethodInvoker(() => run = sfCheckBox.Checked));
                }

                this.Invoke(new MethodInvoker(() => sfCheckBox.ForeColor = Color.Blue));

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        private void ivrdButton_Click(object sender, EventArgs e)
        {
            new Thread(delegate()
            {
                LoadIvrd();
            }).Start();
        }
        
        private bool LoadIvrd()
        {
            try
            {
                int count = 0;
                bool run = true;
                while (run)
                {
                    count++;
                    this.Invoke(new MethodInvoker(() => ivrdCheckBox.Text = "" + count));

                    if (_fireFoxBrowser.LoadIvrd())
                        break;

//                    Thread.Sleep(1000);
                    this.Invoke(new MethodInvoker(() => run = ivrdCheckBox.Checked));
                }

                this.Invoke(new MethodInvoker(() => ivrdCheckBox.ForeColor = Color.Blue));

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void vsbgdButton_Click(object sender, EventArgs e)
        {
            var date = dateTextBox.Text;
            new Thread(delegate()
            {
                LoadVsbgd(date);
            }).Start();
        }

        private bool LoadVsbgd(string date)
        {
            try
            {
                int count = 0;
                bool run = true;
                while (run)
                {
                    count++;
                    this.Invoke(new MethodInvoker(() => vsbgdCheckBox.Text = "" + count));

                    if (_fireFoxBrowser.LoadVsbgd(date))
                        break;

//                    Thread.Sleep(1000);
                    this.Invoke(new MethodInvoker(() => run = vsbgdCheckBox.Checked));
                }

                this.Invoke(new MethodInvoker(() => vsbgdCheckBox.ForeColor = Color.Blue));

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool IvrdFill()
        {
            var d = _fireFoxBrowser.IvrdState(stateTextBox.Text);
            var a = _fireFoxBrowser.IvrdHotel(hotelTextBox.Text);
            var b = _fireFoxBrowser.IvrdAddress(addressTextBox.Text);
            var c = _fireFoxBrowser.IvrdPhone(phoneTextBox.Text);
            var e = _fireFoxBrowser.IvrdDistrict(districtTextBox.Text, stateTextBox.Text);
            var f = _fireFoxBrowser.IvrdJsOff();

            return a && b && c && d && e && f;
        }

        private void toLastTabButton_Click(object sender, EventArgs e)
        {
            _fireFoxBrowser.GoToLastTab();

        }

        private void JsButton_Click(object sender, EventArgs e)
        {
            _fireFoxBrowser.IvrdJsOff();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            while (true)
            {
                _fireFoxBrowser.Click();
                Thread.Sleep(800);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                _fireFoxBrowser.SaveLastTabAsSf();
            }
            catch(Exception ex)
            {

            }
        }

        private void captchaRefreshButton_Click(object sender, EventArgs e)
        {
            new Thread(delegate()
            {
                ReloadCaptcha();
            }).Start();
        }

        private bool ReloadCaptcha()
        {
            try
            {
                int count = 0;
                bool run = true;
                while (run)
                {
                    count++;
                    this.Invoke(new MethodInvoker(() => captchaRefreshCheckBox.Text = "" + count));
                    //_fireFoxBrowser.TfcCaptchaRefresh();
                    if (!_fireFoxBrowser.IvrdCaptchaRefresh())
                    {
                        continue;
                    }

                    int i = 0;
                    while (i < captchaUpDown.Value)
                    {
                        Thread.Sleep(1000);
                        if (captchaRefreshCheckBox.Checked == false)
                            break;
                        i++;
                    }
                    this.Invoke(new MethodInvoker(() => run = captchaRefreshCheckBox.Checked));
                }
                this.Invoke(new MethodInvoker(() => captchaRefreshCheckBox.ForeColor = Color.Blue));

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void BrowserController_Load(object sender, EventArgs e)
        {

        }

        private void ivrdFillButton_Click(object sender, EventArgs e)
        {
            IvrdFill();
        }

        private void LastTabVsbgdButton_Click(object sender, EventArgs e)
        {
            try
            {
                _fireFoxBrowser.SaveLastTabAsVsbgd();
            }
            catch (Exception ex)
            {

            }
        }

        private void VsbgdSubmitButton_Click(object sender, EventArgs e)
        {
            var submissionTime = vsbgdSubmitTimePicker.Value;
            new Thread(delegate()
            {
                VsbgdSubmitOnTime(submissionTime);
            }).Start();
        }

        private bool VsbgdSubmitOnTime(DateTime submissionTime)
        {
            try
            {
                int count = 0;
                bool run = true;
                while (run)
                {
                    count++;
                    this.Invoke(new MethodInvoker(() => vsbgdSubmitCheckBox.Text = "" + count));

                    int i = 0;
                    while (i < (5))
                    {
                        Thread.Sleep(200);
                        var time = DateTime.Now;
                        if (time.CompareTo(submissionTime) >= 0)
                        {
                            var done =_fireFoxBrowser.VsbgdSubmitNow();

                            if (!done)
                            {
                                continue;
                            }
                            else
                            {
                                this.Invoke(new MethodInvoker(() => vsbgdSubmitCheckBox.Checked = false));
                                break;
                            }
                        }
                        if (vsbgdSubmitCheckBox.Checked == false)
                            break;

                        i++;
                    }
                    this.Invoke(new MethodInvoker(() => run = vsbgdSubmitCheckBox.Checked));
                }
                this.Invoke(new MethodInvoker(() => vsbgdSubmitCheckBox.ForeColor = Color.Blue));

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void tempIdTextBox_TextChanged(object sender, EventArgs e)
        {
            this.Text = tempIdTextBox.Text;
        }


    }
}
