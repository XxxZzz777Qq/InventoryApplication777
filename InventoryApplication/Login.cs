using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApplication
{
    public partial class Login : Form
    {

        private string responseMessage;

        public Login()
        {
            InitializeComponent();
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (txtLoginName.Text != "" && txtPassword.Text != "")
            {
                // Создайте соединение.
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
                {
                    // Создайте SqlКоманду и идентифицируйте ее как хранимую процедуру.
                    using (SqlCommand sqlCommand = new SqlCommand("dbo.uspLogin", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        // Добавьте входной параметр для хранимой процедуры и укажите, что использовать в качестве его значения.
                        sqlCommand.Parameters.Add(new SqlParameter("@pLoginName", SqlDbType.NVarChar, 254));
                        sqlCommand.Parameters["@pLoginName"].Value = txtLoginName.Text;

                        // Добавьте входной параметр для хранимой процедуры и укажите, что использовать в качестве его значения.
                        sqlCommand.Parameters.Add(new SqlParameter("@pPassword", SqlDbType.NVarChar, 50));
                        sqlCommand.Parameters["@pPassword"].Value = txtPassword.Text;

                        // Добавьте выходной параметр.
                        sqlCommand.Parameters.Add(new SqlParameter("@responseMessage", SqlDbType.NVarChar, 250));
                        sqlCommand.Parameters["@responseMessage"].Direction = ParameterDirection.Output;

                        try
                        {
                            connection.Open();
                            sqlCommand.ExecuteNonQuery();
                            var response = sqlCommand.Parameters["@responseMessage"].Value;
                            responseMessage = response.ToString();
                            //MessageBox.Show(responseMessage);
                        }
                        catch
                        {
                            MessageBox.Show("Где-то произошла ошибка :(");
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, Заполните Все Поля");
            }
            validateResponseMessage();
        }

        // Проверка входа в систему
        private void validateResponseMessage()
        {
            if (responseMessage == "Успешный вход в систему" || responseMessage.Equals("Успешный вход в систему"))
            {
                this.Hide();
                Dashboard dashboard = new Dashboard();
                dashboard.Closed += (s, args) => this.Close();
                dashboard.Show();
            }
            else
            {

            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
