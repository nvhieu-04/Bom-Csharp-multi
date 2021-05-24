using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BombermanMultiplayer
{
    public partial class Mode : Form
    {
        public Mode()
        {
            InitializeComponent();
        }

        private void btnLocalGame_Click(object sender, EventArgs e)
        {
            using (GameWindow f = new GameWindow())
            {
                f.ShowDialog();
            }
        }

        private void btnLanMode_Click(object sender, EventArgs e)
        {
            using (Lobby f = new Lobby())
            {
                f.ShowDialog();
            }
        }

        private void lbTitle_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            MainMenu f = new MainMenu();
            f.Show();
        }
    }
}
