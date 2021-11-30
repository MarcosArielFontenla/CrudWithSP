using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudWithSP
{
    public partial class frmEmployee : Form
    {
        string connectionString = "Server=VARITECH\\SQLEXPRESS; Database=CrudWithSP; Integrated security=True";
        string EmployeeId = "";
        SqlConnection conn;
        SqlCommand command;

        public frmEmployee()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
            conn.Open();
        }

        private void frmEmployee_Load(object sender, EventArgs e)
        {
            dgvEmployee.AutoGenerateColumns = true;
            dgvEmployee.DataSource = FetchEmpDetails();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Enter the Employee Name!");
                txtName.Select();
            }
            else if (string.IsNullOrWhiteSpace(txtCity.Text))
            {
                MessageBox.Show("Enter the Current City!");
                txtCity.Select();
            }
            else if (string.IsNullOrWhiteSpace(txtDepartment.Text))
            {
                MessageBox.Show("Enter the Department!");
                txtDepartment.Select();
            }
            else if (cboGender.SelectedIndex <= -1)
            {
                MessageBox.Show("Enter the Gender!");
                cboGender.Select();
            }
            else
            {
                try
                {
                    if (conn == null || conn.State == ConnectionState.Closed)
                        conn.Open();

                    DataTable dt = new DataTable();
                    command = new SqlCommand("spEmployee", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ActionType", "SaveData");
                    command.Parameters.AddWithValue("@EmployeeId", EmployeeId);
                    command.Parameters.AddWithValue("@Name", txtName.Text);
                    command.Parameters.AddWithValue("@City", txtCity.Text);
                    command.Parameters.AddWithValue("@Department", txtDepartment.Text);
                    command.Parameters.AddWithValue("@Gender", cboGender.Text);

                    int numRes = command.ExecuteNonQuery();

                    if (numRes > 0)
                    {
                        MessageBox.Show("Record saved Successfully!!");
                        ClearAllData();
                    }
                    else
                    {
                        MessageBox.Show("Please try again!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:-" + ex.Message);
                }
            }
        }

        private void dgvEmployee_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnSave.Text = "Update";
                EmployeeId = dgvEmployee.Rows[e.RowIndex].Cells[0].Value.ToString();
                DataTable dt = FetchEmpRecords(EmployeeId);

                if (dt.Rows.Count > 0)
                {
                    EmployeeId = dt.Rows[0][0].ToString();
                    txtName.Text = dt.Rows[0][1].ToString();
                    txtCity.Text = dt.Rows[0][2].ToString();
                    txtDepartment.Text = dt.Rows[0][3].ToString();
                    cboGender.Text = dt.Rows[0][4].ToString();
                }
                else
                {
                    ClearAllData();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(EmployeeId))
            {
                try
                {
                    if (conn == null || conn.State == ConnectionState.Closed)
                        conn.Open();

                    DataTable dt = new DataTable();
                    command = new SqlCommand("spEmployee", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ActionType", "DeleteData");
                    command.Parameters.AddWithValue("@EmployeeId", EmployeeId);

                    int numRes = command.ExecuteNonQuery();

                    if (numRes > 0)
                    {
                        MessageBox.Show("Record deleted Successfully !!");
                        ClearAllData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:-" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a Record!");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAllData();
        }

        private DataTable FetchEmpRecords(string employeeId)
        {
            if (conn == null || conn.State == ConnectionState.Closed)
                conn.Open();

            DataTable dt = new DataTable();
            command = new SqlCommand("spEmployee", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ActionType", "FetchRecord");
            command.Parameters.AddWithValue("@EmployeeId", employeeId);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(dt);
            return dt;
        }

        private DataTable FetchEmpDetails()
        {
            if (conn == null || conn.State == ConnectionState.Closed)
                conn.Open();

            DataTable dtData = new DataTable();
            command = new SqlCommand("spEmployee", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ActionType", "FetchData");
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(dtData);
            return dtData;
        }

        private void ClearAllData()
        {
            btnSave.Text = "Save";
            txtName.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtDepartment.Text = string.Empty;
            cboGender.SelectedIndex = -1;
            EmployeeId = string.Empty;
            dgvEmployee.AutoGenerateColumns = false;
            dgvEmployee.DataSource = FetchEmpDetails();
        }
    }
}
