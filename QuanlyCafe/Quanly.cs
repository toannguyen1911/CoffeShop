using QuanlyCafe.DAO;
using QuanlyCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace QuanlyCafe
{
    public partial class Quanly : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        { 
            get => loginAccount;
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }

        public Quanly(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTable();

            LoadCategory();

            LoadItemFood();

            LoadComboboxTable(cbChuyenban);
        }

        private void Quanly_Load(object sender, EventArgs e)
        {

        }

        #region Method
        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }
        void LoadItemFood()
        {
            //string filename = @"C:\Users\Kin\source\repos\QuanlyCafe\QuanlyCafe\Resources\";
            string filename = System.IO.Path.GetDirectoryName(Application.ExecutablePath).ToString().Replace(@"\bin\Debug\netcoreapp3.1", @"\Resources\");



            List<QuanlyCafe.DTO.ItemFood> listItemFood = ItemFoodDAO.Instance.GetListItemFood();
            foreach (ItemFood i in listItemFood)
            {
                UserControl user = new UserControl();
                user.Size = new Size(200, 250);
                user.BackColor = Color.FromArgb(255, 232, 232);

                Label lbFood = new Label() { Size = new Size(200, 150), BackgroundImage = Image.FromFile(filename + i.Id.ToString() + @".jpg") };
                Button btnAdd = new Button() { Size = new Size(30, 30), Location = new Point(170, 210), BackgroundImage = Image.FromFile(filename + @"plus.png") };
                Button btnMinus = new Button() { Size = new Size(30, 30), Location = new Point(135, 210), BackgroundImage = Image.FromFile(filename + @"minus.png") };
                Label lbName = new Label() { Size = new Size(250, 23), TextAlign = ContentAlignment.MiddleLeft, Location = new Point(30, 170), Text = i.FoodName, Font = new Font("Arial", 15, FontStyle.Bold) };
                Label lbGia = new Label() { Location = new Point(30, 220), Text = i.Price.ToString() + "vnd", Font = new Font("Arial", 10, FontStyle.Bold) };

                btnAdd.Click += (sender, EventArgs) => { btnItemFood_Click(sender, EventArgs, i.Id, 1); };


                btnMinus.Click += (sender, EventArgs) => { btnItemFood_Click(sender, EventArgs, i.Id, -1); };

                user.Controls.Add(lbFood);
                user.Controls.Add(btnAdd);
                user.Controls.Add(btnMinus);
                user.Controls.Add(lbGia);
                user.Controls.Add(lbName);
                FLP.Controls.Add(user);

            }
            void btnItemFood_Click(object sender, EventArgs e, int id, int c)
            {
                Table table = lsvHoadon.Tag as Table;

                if (table == null)
                {
                    MessageBox.Show("Hãy chọn bàn");
                    return;
                }

                int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
                int foodID = id;
                int count = c;

                if (idBill == -1)
                {
                    BillDAO.Instance.InsertBill(table.ID);
                    BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
                }
                else
                {
                    BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
                }

                ShowBill(table.ID);
            }
        }
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.LightGreen;
                        break;
                    default:
                        btn.BackColor = Color.Red;
                        break;
                }
                 
                flpTable.Controls.Add(btn);
            }
        }

        void ShowBill(int id)
        {
            lsvHoadon.Items.Clear();
            List<QuanlyCafe.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;

            foreach (QuanlyCafe.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvHoadon.Items.Add(lsvItem);
            }

            //CultureInfo culture = new CultureInfo("vi-VN");

            txbTotalPrice.Text = totalPrice.ToString();
            LoadTable();
        }
        #endregion

        #region Event
        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvHoadon.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin f = new Admin();
            f.loginAccount = LoginAccount;
            f.InsertFood += f_InsertFood;
            f.DeleteFood += f_DeleteFood;
            f.UpdateFood += f_UpdateFood;
            f.ShowDialog();
        }
        void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvHoadon.Tag != null)
                ShowBill((lsvHoadon.Tag as Table).ID);
        }

        void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvHoadon.Tag != null)
                ShowBill((lsvHoadon.Tag as Table).ID);
            LoadTable();
        }

        void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvHoadon.Tag != null)
                ShowBill((lsvHoadon.Tag as Table).ID);
        }

        private void thôngTinTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountInfo f = new AccountInfo(loginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }
        void f_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        private void đăngXuấtToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất? " , "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                this.Close();
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            Table table = lsvHoadon.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDiscount.Value;
            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;
            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho: {0}\n\nTổng tiền - (Tổng tiền / 100) x Giảm giá\n\nTổng = {1} - ({1} / 100) x {2} = {3}", table.Name, totalPrice, discount, finalTotalPrice), "Thanh toán hóa đơn", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    ShowBill(table.ID);

                    LoadTable();
                }
            }
        }
        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvHoadon.Tag as Table ;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value;

            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }

            ShowBill(table.ID);
        }
        private void btnChuyenban_Click(object sender, EventArgs e)
        {
            int id1 = (lsvHoadon.Tag as Table).ID;

            int id2 = (cbChuyenban.SelectedItem as Table).ID;
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển bàn {0} qua bàn {1}", (lsvHoadon.Tag as Table).Name, (cbChuyenban.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);

                LoadTable();
            }
        }
        #endregion
        #region trash
        private void tPMenu_Click(object sender, EventArgs e)
        {

        }

        private void txbTotalPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void nmDiscount_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        #endregion     
    }
}
