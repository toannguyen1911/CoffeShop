using QuanlyCafe.DAO;
using QuanlyCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanlyCafe
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btlogin_Click(object sender, EventArgs e)
        {
            string userName = txbusername.Text;
            string passWord = txbpassword.Text;
            if(Login1(userName, passWord))
            {
                Account loginAccount = AccountDAO.Instance.GetAccountByUserName(userName);
                Quanly f = new Quanly(loginAccount);
                MessageBox.Show("Đăng nhập thành công!");
                this.Hide();
                f.ShowDialog();
                this.Show(); 
                txbpassword.Clear();
                txbusername.Focus();

            }
            else
            {
                MessageBox.Show("The User name or Password is incorrect!");
                txbusername.Clear();
                txbpassword.Clear();
                txbusername.Focus();
            }    
        }

        bool Login1(string userName, string passWord)
        {
            return AccountDAO.Instance.Login(userName, passWord);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
