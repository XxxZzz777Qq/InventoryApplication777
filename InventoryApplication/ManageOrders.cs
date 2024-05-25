using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApplication
{
    public partial class ManageOrders : Form
    {
        public ManageOrders()
        {
            InitializeComponent();
        }

        DataTable ordersDataTable = new DataTable("Orders");
        DataTable salesDataTable = new DataTable("Sales");

        // Событие загрузки формы
        private void ManageOrders_Load(object sender, EventArgs e)
        {
            loadOrdersFromDatabase();
            loadSalesFromDatabase();
        }

        // Загружает данные заказа из базы данных и заполняет datatable
        private void loadOrdersFromDatabase()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                String query = "SELECT * FROM orders";
                using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        adapter.Fill(ordersDataTable);
                        dataGridOrders.DataSource = ordersDataTable;
                    }
                    catch
                    {
                        MessageBox.Show("ERROR while loading orders from database");
                    }
                    finally
                    {
                        connection.Close();
                    }

                }
            }
        }

        // Загружает данные о продажах из базы данных и заполняет datatable
        private void loadSalesFromDatabase()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                String query = "SELECT * FROM sales";
                using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        adapter.Fill(salesDataTable);
                        dataGridSales.DataSource = salesDataTable;
                    }
                    catch
                    {
                        MessageBox.Show("ERROR while loading orders from database");
                    }
                    finally
                    {
                        connection.Close();
                    }

                }
            }
        }

        // Выбирает OrderID и заполняет текстовое поле
        private void dataGridOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridOrders.SelectedRows.Count == 1)
            {
                //txtSearchFirstNames.Text = dataGridOrders.SelectedCells[0].Value.ToString();
            }
            else
            {
                MessageBox.Show("ERROR");
            }
        }

        // Поиск - OrderID в таблице заказов
        private void txtOrderID_TextChanged(object sender, EventArgs e)
        {
            salesDataTable.DefaultView.RowFilter = string.Format("convert(OrderID, 'System.String') LIKE '%{0}%'", txtSearchOrderIDs.Text);
            //salesDataTable.DefaultView.RowFilter = string.Format("FirstName LIKE '%{0}%'", txtSearchFirstName.Text);
        }

        // Поиск - OrderID в таблице продаж
        private void txtSearchOrderID_TextChanged(object sender, EventArgs e)
        {
            ordersDataTable.DefaultView.RowFilter = string.Format("convert(OrderID, 'System.String') LIKE '%{0}%'", txtSearchOrderID.Text);
            //DV.RowFilter = string.Format("convert(JobNumber, 'System.String') Like '%{0}%' ", textBox1.Text);
        }
    }
}
