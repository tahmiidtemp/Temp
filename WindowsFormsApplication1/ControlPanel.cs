using System;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Temp
{
    public partial class ControlPanel : Form
    {

        public ControlPanel()
        {
            InitializeComponent();

            findProfiles();
        }

        private void findProfiles()
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            var profile = new BrowserController("8");
            profile.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var profile = new BrowserController("1");
            profile.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var profile = new BrowserController("2");
            profile.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var profile = new BrowserController("3");
            profile.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var profile = new BrowserController("4");
            profile.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var profile = new BrowserController("5");
            profile.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var profile = new BrowserController("6");
            profile.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var profile = new BrowserController("7");
            profile.Show();
        }
    }
}
