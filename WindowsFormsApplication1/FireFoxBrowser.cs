using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Temp
{
    class FireFoxBrowser
    {
        private readonly IWebDriver _driver;
        private string _index;
        private string _tfc;
        private string _ivr;
        private string _ivr1;
        private string _ivr2;
        private string _sf;
        private string _ivrd;
        private string _vsbgd;
        private string _newTab;


        public FireFoxBrowser()
        {
            _driver = GetNewChromeInstance("New User");
        }

        public FireFoxBrowser(string p)
        {
            _driver = GetNewChromeInstance(p);
        }

        private static IWebDriver GetNewChromeInstance(string name)
        {
            //            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            //            chromeDriverService.HideCommandPromptWindow = true;
            //            var options = new ChromeOptions();
            //
            //            options.AddArguments("user-data-dir=C:/Users/" + Environment.UserName + "/AppData/Local/Google/Chrome/" + name);
            //            var driver = new ChromeDriver(chromeDriverService, options);
            //            return driver;


            /*            var ffpath = @"F:\febeprof.TJBase";
                        var binary = new FirefoxBinary();
                            var profile = new FirefoxProfile(ffpath);
                                                    var desiredCapabilities = new DesiredCapabilities();
                                                    desiredCapabilities.SetCapability("nativeEvents", false);
                            profile.SetPreference("browser.tabs.loadInBackground", false);
            //                //                var port = RNumber.GetRandomNumber(10, 7100, 9500);
            //                //                profile.Port = port;
                            profile.EnableNativeEvents = false;
                            return new FirefoxDriver(binary, profile, new TimeSpan(0, 7, 0));*/

            try
            {
//                var profile = new FirefoxProfile(@"F:\Profiles\" + name);
                var profile = new FirefoxProfile(@"F:\Profiles\TJ1");
                var desiredCapabilities = new DesiredCapabilities();
                desiredCapabilities.SetCapability("nativeEvents", false);
                profile.SetPreference("browser.tabs.loadInBackground", false);
                var port = GetRandomNumber(10, 7100, 9500);
                profile.Port = port;
                profile.EnableNativeEvents = false;
                var firefox = new FirefoxDriver(new FirefoxBinary(), profile, new TimeSpan(0, 7, 0));

                return firefox;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static int GetRandomNumber(int gLabel, int min, int max)
        {
            var rand = new Random();
            var randNumber = rand.Next(min, max);
            var rm = randNumber % gLabel;
            return randNumber - rm;
        }


        internal bool LoadIndex(string p)
        {
            try
            {
                _driver.SwitchTo().Window(_driver.WindowHandles.LastOrDefault());
                _driver.Url = p;
                _driver.Navigate();
                Thread.Sleep(1000);
                var success = WaitUnltilPageLoaded();
                if(!success)
                    return false;
                if (_driver.Title != "Indian Visa Application")
                {
                    return false;
                }
                _index = _driver.CurrentWindowHandle;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public bool LoadTfc()
        {
            try
            {
                _driver.SwitchTo().Window(_index);
                var partiallyFilledFormButton = _driver.FindElement(By.ClassName("frt_bt1")).FindElement(By.TagName("a"));
                if (partiallyFilledFormButton.Displayed && partiallyFilledFormButton.Enabled)
                {
                    partiallyFilledFormButton.Click();
                    _driver.SwitchTo().Window(_driver.WindowHandles.LastOrDefault());
                    if (_driver.CurrentWindowHandle == _index)
                        return false;
                    _newTab = _driver.CurrentWindowHandle;
                    while (_driver.Title == "")
                    {
                        Thread.Sleep(1000);
                    }
                    var success = WaitUnltilPageLoaded();
                    if (_driver.Title != "Visa Partial Filled Application" || !success)
                    {
                        if (_newTab != _index)
                            _driver.Close();
                        return false;
                    }
                    _tfc = _driver.CurrentWindowHandle;
                    IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                    js.ExecuteScript("document.getElementsByTagName(\"form\")[0].setAttribute(\"target\", \"_blank\");");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        internal bool PutTemporaryApplicationId(string p)
        {
            try
            {
                _driver.SwitchTo().Window(_tfc);
                var temporaryApplicationIdField = _driver.FindElement(By.Id("fileno"));
                if (temporaryApplicationIdField.Displayed && temporaryApplicationIdField.Enabled)
                {
                    temporaryApplicationIdField.Click();
                    temporaryApplicationIdField.SendKeys(p);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        internal bool PutCaptcha(string p)
        {
            try
            {
                _driver.SwitchTo().Window(_tfc);
                var captchaField = _driver.FindElement(By.Id("ImgNum"));
                if (captchaField.Displayed && captchaField.Enabled)
                {
                    captchaField.Click();
                    captchaField.SendKeys(p);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public bool LoadIvr()
        {
            try
            {
                _driver.SwitchTo().Window(_tfc);
                var submitPartiallyFilledFormButton = _driver.FindElements(By.Name("TmpFile")).FirstOrDefault();
                if (submitPartiallyFilledFormButton.Displayed && submitPartiallyFilledFormButton.Enabled)
                {
                    try
                    {
                        submitPartiallyFilledFormButton.Click();

                    }
                    catch (Exception e)
                    {
                        _driver.SwitchTo().Alert().Accept(); // prepares Selenium to handle alert 
                        _driver.SwitchTo().Window(_driver.WindowHandles.LastOrDefault());
                        return false;
                    }

                    _driver.SwitchTo().Window(_driver.WindowHandles.LastOrDefault());

                    while (_driver.Title == "")
                    {
                        Thread.Sleep(1000);
                    }

                    var success = WaitUnltilPageLoaded();

                    Thread.Sleep(1000);

                    if (_driver.Title != "Online Indian Visa Form" && _driver.Title != "Visa Partial Filled Application" || !success)
                    {
                        _driver.Close();
                        return false;
                    }
                    else
                    {
                        _ivr = _driver.CurrentWindowHandle;
                        IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                        js.ExecuteScript("document.getElementsByTagName(\"form\")[0].setAttribute(\"target\", \"_blank\");");
                        return true;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        internal bool IvrCheckOtherPassport()
        {

            try
            {
                _driver.SwitchTo().Window(_ivr);
                var other = _driver.FindElement(By.Id("oth_ppt_no"));
                if (other == null)
                    return false;

                if (other.Text == "")
                {
                    var otherNo = _driver.FindElements(By.Name("oth_ppt")).LastOrDefault();
                    if (otherNo.Displayed && otherNo.Enabled)
                    {
                        otherNo.Click();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public bool LoadIvr1()
        {
            try
            {
                _driver.SwitchTo().Window(_ivr);
                var ivRegContinueButton = _driver.FindElement(By.Id("continue"));
                if (ivRegContinueButton.Displayed && ivRegContinueButton.Enabled)
                {
                    ivRegContinueButton.Click();
                    _driver.SwitchTo().Window(_driver.WindowHandles.LastOrDefault());
                    while (_driver.Title == "")
                    {
                        Thread.Sleep(1000);
                    }
                    var success = WaitUnltilPageLoaded();
                    if (_driver.Title != "Online Indian Visa Form" && _driver.Title != "Visa Partial Filled Application" || !success)
                    {
                        _driver.Close();
                        return false;
                    }
                    else
                    {
                        _ivr1 = _driver.CurrentWindowHandle;
                        IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                        js.ExecuteScript("document.getElementsByTagName(\"form\")[0].setAttribute(\"target\", \"_blank\");");
                        return true;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        internal bool Ivr1SelectOccupationOf(string p)
        {
            try
            {
                _driver.SwitchTo().Window(_ivr1);
                var occupationOf = _driver.FindElements(By.Name("occ_flag")).FirstOrDefault();
                if (occupationOf.Displayed && occupationOf.Enabled)
                {
                    var clickThis = new SelectElement(occupationOf);
                    clickThis.SelectByText(p);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public bool LoadIvr2()
        {
            try
            {
                _driver.SwitchTo().Window(_ivr1);
                var ivReg1SaveAndContinueButton = _driver.FindElements(By.Name("sc")).FirstOrDefault();
                if (ivReg1SaveAndContinueButton.Displayed && ivReg1SaveAndContinueButton.Enabled)
                {
                    ivReg1SaveAndContinueButton.Click();
                    _driver.SwitchTo().Window(_driver.WindowHandles.LastOrDefault());
                    while (_driver.Title == "")
                    {
                        Thread.Sleep(1000);
                    }
                    var success = WaitUnltilPageLoaded();

                    if (_driver.Title != "Online Indian Visa Form" && _driver.Title != "Visa Partial Filled Application" || !success)
                    {
                        _driver.Close();
                        return false;
                    }
                    else
                    {
                        _ivr2 = _driver.CurrentWindowHandle;
                        IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                        js.ExecuteScript("document.getElementsByTagName(\"form\")[0].setAttribute(\"target\", \"_blank\");");
                        return true;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        internal bool Ivr2CheckPrevious()
        {

            try
            {
                _driver.SwitchTo().Window(_ivr2);
                var previousAddress = _driver.FindElement(By.Id("prv_visit_add1"));
                if (previousAddress == null)
                    return false;

                if (previousAddress.Text == "")
                {
                    var previousAddressRadioNo = _driver.FindElements(By.Name("oldvisa")).LastOrDefault();
                    if (previousAddressRadioNo.Displayed && previousAddressRadioNo.Enabled)
                    {
                        previousAddressRadioNo.Click();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        internal bool Ivr2CheckRefused()
        {

            try
            {
                _driver.SwitchTo().Window(_ivr2);
                var refused = _driver.FindElement(By.Id("refuse_details"));
                if (refused == null)
                    return false;

                if (refused.Text == "")
                {
                    var refusedRadioNo = _driver.FindElements(By.Name("PermStay")).LastOrDefault();
                    if (refusedRadioNo.Displayed && refusedRadioNo.Enabled)
                    {
                        refusedRadioNo.Click();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        internal bool Ivr2CheckSaarc()
        {

            try
            {
                _driver.SwitchTo().Window(_ivr2);
                var saarc = _driver.FindElement(By.Id("saarcVisit1"));
                if (saarc == null)
                    return false;

                if (saarc.Text == "")
                {
                    var saarcRadioNo = _driver.FindElements(By.Name("saarc")).LastOrDefault();
                    if (saarcRadioNo.Displayed && saarcRadioNo.Enabled)
                    {
                        saarcRadioNo.Click();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public bool LoadSf()
        {
            try
            {
                _driver.SwitchTo().Window(_ivr2);
                var ivReg2SaveAndContinueButton = _driver.FindElements(By.Name("sc")).FirstOrDefault();
                if (ivReg2SaveAndContinueButton.Displayed && ivReg2SaveAndContinueButton.Enabled)
                {
                    ivReg2SaveAndContinueButton.Click();
                    //                _driver.SwitchTo().Alert().Accept();
                    _driver.SwitchTo().Window(_driver.WindowHandles.LastOrDefault());
                    while (_driver.Title == "")
                    {
                        Thread.Sleep(1000);
                    }
                    var success = WaitUnltilPageLoaded();

                    if (_driver.Title != "Online Indian Visa Form" && _driver.Title != "Visa Partial Filled Application" || !success)
                    {
                        _driver.Close();
                        return false;
                    }
                    else
                    {
                        _sf = _driver.CurrentWindowHandle;
                        IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                        js.ExecuteScript("document.getElementsByTagName(\"form\")[0].setAttribute(\"target\", \"_blank\");");
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool SfRefresh()
        {
            try
            {
                _driver.SwitchTo().Window(_sf);
                _driver.Navigate().Refresh();
                while (_driver.Title == "")
                {
                    Thread.Sleep(1000);
                }
                var success = WaitUnltilPageLoaded();
                if (_driver.Title != "Online Indian Visa Form" || !success)
                {
                    return false;
                }
                else
                {
                    _sf = _driver.CurrentWindowHandle;
                    IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                    js.ExecuteScript("document.getElementsByTagName(\"form\")[0].setAttribute(\"target\", \"_blank\");");
                    return true;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        internal bool LoadIvrd()
        {
            try
            {
                _driver.SwitchTo().Window(_sf);
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                js.ExecuteScript("document.getElementById(\"upload_button_tr\").removeAttribute(\"style\"); document.getElementById(\"submit_btn\").disabled = false;");

                var varifyandContinueButton = _driver.FindElement(By.Id("submit_btn"));
                if (varifyandContinueButton.Displayed && varifyandContinueButton.Enabled)
                {

                    varifyandContinueButton.Click();
                    Thread.Sleep(1000);
                    _driver.SwitchTo().Alert().Accept();
                    _driver.SwitchTo().Window(_driver.WindowHandles.LastOrDefault());
                    while (_driver.Title == "")
                    {
                        Thread.Sleep(1000);
                    }
                    var success = WaitUnltilPageLoaded();

                    if (_driver.Title != "Online Indian Visa Form" || !success)
                    {
                        _driver.Close();
                        return false;
                    }
                    else
                    {
                        _ivrd = _driver.CurrentWindowHandle;
                        js = (IJavaScriptExecutor)_driver;
                        js.ExecuteScript("document.getElementsByTagName(\"form\")[0].setAttribute(\"target\", \"_blank\");");


                        js.ExecuteScript("s = function refreshCaptcha()		{try	{	 	var d = new Date();	 	var n=d.getTime();	document.getElementById('capt').src='Rimage.jsp?rand='+n;  	}	catch(e) {alert(e);} return false;		}; "
                                        + "script = document.createElement('script');"
                                        + "script.innerHTML = s;"
                                        + "document.head.appendChild(script);");


                        js.ExecuteScript("document.getElementsByTagName('table')[2].children[0].children[7].innerHTML = \"<tr><td colspan='2'><img src='Rimage.jsp' id='capt' alt='Develped by NIC, New Delhi' width='150' height='35' border='0'><a href='#' onclick='return refreshCaptcha();'><img src='images/refresh.png' style='width:25px;height:25px'></a></td></tr>\";");

                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        internal bool IvrdHotel(string p)
        {
            try
            {
                _driver.SwitchTo().Window(_ivrd);

                var hotelBox = _driver.FindElement(By.Id("place_of_stay1"));
                if (hotelBox.Displayed && hotelBox.Enabled)
                {
                    hotelBox.Click();
                    hotelBox.SendKeys(p);
                    Thread.Sleep(1000);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        internal bool IvrdAddress(string p)
        {
            try
            {
                _driver.SwitchTo().Window(_ivrd);
                var addressBox = _driver.FindElement(By.Id("pos_address1"));
                if (addressBox.Displayed && addressBox.Enabled)
                {
                    addressBox.Click();
                    addressBox.SendKeys(p);
                    Thread.Sleep(1000);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        internal bool IvrdPhone(string p)
        {
            try
            {
                _driver.SwitchTo().Window(_ivrd);
                var phoneBox = _driver.FindElement(By.Id("pos_phone1"));
                if (phoneBox.Displayed && phoneBox.Enabled)
                {
                    phoneBox.Click();
                    phoneBox.SendKeys(p);
                    Thread.Sleep(1000);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        internal bool IvrdState(string p)
        {
            try
            {
                _driver.SwitchTo().Window(_ivrd);
                var state = _driver.FindElement(By.Id("pos_state_id1"));
                if (state.Displayed && state.Enabled)
                {
                    var js = (IJavaScriptExecutor)_driver;
                    js.ExecuteScript("document.getElementById('pos_state_id1').setAttribute('onchange', '');");

                    var clickThis = new SelectElement(state);
                    clickThis.SelectByText(p);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        internal bool IvrdDistrict(string p, string state)
        {
            try
            {
                _driver.SwitchTo().Window(_ivrd);
                var js = (IJavaScriptExecutor)_driver;
                //                js.ExecuteScript(" x = document.getElementsByName('DIST1')[0].firstChild; "
                //                               + " x.setAttribute('value', 'KOLKATA'); "
                //                               + " y.text = 'KOLKATA'; "
                //                               + " y.setAttribute('selected', 'selected'); ");

                js.ExecuteScript("document.getElementById('DIST1').innerHTML = \" <select name='DIST1' class='combo2' onchange='callme()'><option selected='selected' value='" + p + "'>" + p + "</option></select><input id='state_id1' name='state_id1' value='" + state + " ' type='hidden'> \";");

                return true;
                //
                //                var dist = _driver.FindElement(By.Name("DIST1"));
                //                if (dist.Displayed && dist.Enabled)
                //                {
                //                    var clickThis = new SelectElement(dist);
                //                    clickThis.SelectByText(p);
                //                    return true;
                //                }
                //                else
                //                {
                //                    return false;
                //                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        internal bool IvrdCaptchaRefresh()
        {
            try
            {
                _driver.SwitchTo().Window(_ivrd);
                var captchaRefreshButton = _driver.FindElements(By.TagName("a")).FirstOrDefault();
                if (captchaRefreshButton.Displayed && captchaRefreshButton.Enabled)
                {
                    captchaRefreshButton.Click();
                    //Thread.Sleep(5);
                    //var captchaHeight = (String)((IJavaScriptExecutor)_driver).ExecuteScript("document.getElementById('capt').height");
                    //if(int.Parse(captchaHeight)<20)
                    //    return false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        internal bool IvrdJsOff()
        {
            //TODO manual
            /*var xyz = _driver.FindElementById("fileno");
            xyz.SendKeys(Keys.Alt + "q");*/
            return true;
        }

        internal bool LoadVsbgd(string date)
        {
            try
            {
                _driver.SwitchTo().Window(_ivrd);
                var vsbgdSubmit = _driver.FindElements(By.Name("Submit")).FirstOrDefault();
                if (vsbgdSubmit.Displayed && vsbgdSubmit.Enabled)
                {
                    vsbgdSubmit.Click();
                    //                _driver.SwitchTo().Alert().Accept();
                    _driver.SwitchTo().Window(_driver.WindowHandles.LastOrDefault());
                    while (_driver.Title == "")
                    {
                        Thread.Sleep(1000);
                    }
                    var success = WaitUnltilPageLoaded();

                    if (_driver.Title != "Online Registration" || !success)
                    {
                        _driver.Close();
                        return false;
                    }
                    else
                    {
                        _vsbgd = _driver.CurrentWindowHandle;
                        IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                        js.ExecuteScript("document.getElementsByTagName(\"form\")[0].setAttribute(\"target\", \"_blank\");");


                        js.ExecuteScript("document.getElementsByClassName('style11')[0].parentNode.innerHTML = "
                            + "\""
                            + "<td align='left' valign='middle'><div align='right'><font size='3' face='Arial, Helvetica, sans-serif'><font color='#990000'><strong>Select &nbsp;Date</strong></font></font></div></td><td align='right' valign='middle'><div align='left'><font size='3' face='Arial, Helvetica, sans-serif'><font color='#990000'><select name='DATE' size='1'><option value=''>Select</option>null<option selected='selected' value='" + date + "' title='" + date + "'> " + date + "</option></select> </font></font></div> </td><td><div align='right'><input type='submit' name='SAVE' class='btn btn-primary' value='Confirm The Appointment'></div></td><td colspan='2'></td>"
                            + "\";");

                        //js.ExecuteScript("var tr = document.getElementsByClassName('style11')[0].parentNode");
                        //js.ExecuteScript("document.getElementsByClassName('style11')[0].parentNode.innerHTML = " + s);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }




        private bool WaitUnltilPageLoaded()
        {
            Console.Write("Page Load status: " + _driver.Title);
      
            
            var cnt = 0;
            while (cnt < 60)
            {
                cnt++;
                var status = CheckIfPageLoaded(500);
                if (status)
                {
                    Console.WriteLine("\nPage Loaded " + _driver.Title);
                    break;
                }
                Console.Write("No ");
                if (_driver.Title == "Problem loading page" ) ;
                return false;
            }
            return true;
        }

        private bool CheckIfPageLoaded(int timeSpan)
        {
            try
            {

                //            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(30.00));

                //            wait.Until(driver1 => ((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState").Equals("complete"));
                var wait = new WebDriverWait(new SystemClock(), _driver, TimeSpan.FromMilliseconds(timeSpan), TimeSpan.FromMilliseconds(100));
                wait.Until(driver1 => ((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState").Equals("complete"));
                var status = (String)((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState");
                return !string.IsNullOrEmpty(status) && status.Equals("complete");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public void Close()
        {
            _driver.Quit();
        }

        internal void GoToLastTab()
        {
            _driver.SwitchTo().Window(_driver.WindowHandles.LastOrDefault());
        }



        internal bool Click()
        {
            try
            {
                _driver.SwitchTo().Window(_vsbgd);
                var ivReg2SaveAndContinueButton = _driver.FindElements(By.Name("SAVE")).FirstOrDefault();
                if (ivReg2SaveAndContinueButton.Displayed && ivReg2SaveAndContinueButton.Enabled)
                {
                    ivReg2SaveAndContinueButton.Click();
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        internal void SaveLastTabAsSf()
        {
            _sf = _driver.WindowHandles.LastOrDefault();
        }

        internal void SaveLastTabAsVsbgd()
        {
            _vsbgd = _driver.WindowHandles.LastOrDefault();
        }

        internal bool TfcCaptchaRefresh()
        {
            try
            {
                _driver.SwitchTo().Window(_tfc);
                var captchaRefreshButton = _driver.FindElements(By.TagName("a")).FirstOrDefault();
                if (captchaRefreshButton.Displayed && captchaRefreshButton.Enabled)
                {
                    captchaRefreshButton.Click();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        internal bool VsbgdSubmitNow()
        {
            try
            {
                _driver.SwitchTo().Window(_vsbgd);
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;

                js.ExecuteScript("document.getElementsByName('SAVE')[0].removeAttribute('disabled');");

                var submitNowButton = _driver.FindElement(By.Name("SAVE"));
                if (submitNowButton.Displayed && submitNowButton.Enabled)
                {

                    submitNowButton.Click();
                    //_driver.SwitchTo().Window(_driver.WindowHandles.LastOrDefault());
                    //while (_driver.Title == "")
                    //{
                    //    Thread.Sleep(50);
                    //}

                    //if (_driver.Title.EndsWith("not available"))
                    //{
                    //    return false;
                    //}
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
