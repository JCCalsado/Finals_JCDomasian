using Finals_JCDomasian.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Finals_JCDomasian
{
    public partial class ParticipantsListForm : Form
    {
        private DbHelper dbHelper;

        public ParticipantsListForm()
        {
            InitializeComponent();
            dbHelper = new DbHelper();
        }

        // Form Load - runs when form opens
        private void ParticipantsListForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Load marathon types into ComboBox
                LoadMarathonTypes();

                // Load all participants into DataGridView
                LoadParticipants();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load all participants
        private void LoadParticipants()
        {
            try
            {
                DataTable dt = dbHelper.GetAllParticipants();
                dgvParticipants.DataSource = dt;

                // Format columns
                FormatDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading participants: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load marathon types into ComboBox
        private void LoadMarathonTypes()
        {
            try
            {
                cmbMarathonType.Items.Clear();
                cmbMarathonType.Items.Add("All");
                cmbMarathonType.Items.Add("5km");
                cmbMarathonType.Items.Add("10km");
                cmbMarathonType.Items.Add("25km");
                cmbMarathonType.SelectedIndex = 0; // Select "All" by default
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading marathon types: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Format DataGridView columns
        private void FormatDataGridView()
        {
            if (dgvParticipants.Columns.Count > 0)
            {
                // Hide ParticipantID column
                if (dgvParticipants.Columns.Contains("ParticipantID"))
                    dgvParticipants.Columns["ParticipantID"].Visible = false;

                // Set column headers
                if (dgvParticipants.Columns.Contains("FullName"))
                    dgvParticipants.Columns["FullName"].HeaderText = "Name";

                if (dgvParticipants.Columns.Contains("BirthDate"))
                {
                    dgvParticipants.Columns["BirthDate"].HeaderText = "Birthday";
                    dgvParticipants.Columns["BirthDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                }

                if (dgvParticipants.Columns.Contains("Phone"))
                    dgvParticipants.Columns["Phone"].HeaderText = "Phone";

                if (dgvParticipants.Columns.Contains("Address"))
                    dgvParticipants.Columns["Address"].HeaderText = "Address";

                if (dgvParticipants.Columns.Contains("BibNumber"))
                    dgvParticipants.Columns["BibNumber"].HeaderText = "Bib #";

                if (dgvParticipants.Columns.Contains("MarathonType"))
                    dgvParticipants.Columns["MarathonType"].HeaderText = "Marathon Type";

                if (dgvParticipants.Columns.Contains("Price"))
                {
                    dgvParticipants.Columns["Price"].HeaderText = "Price";
                    dgvParticipants.Columns["Price"].DefaultCellStyle.Format = "₱#,##0.00";
                }

                if (dgvParticipants.Columns.Contains("PaymentStatus"))
                    dgvParticipants.Columns["PaymentStatus"].HeaderText = "Status";
            }
        }

        // Search button click
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string nameFilter = txtSearchName.Text.Trim();
                string marathonType = cmbMarathonType.SelectedItem?.ToString() ?? "All";

                DataTable dt = dbHelper.SearchParticipants(nameFilter, marathonType);
                dgvParticipants.DataSource = dt;
                FormatDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Refresh button click
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear filters
                txtSearchName.Clear();
                cmbMarathonType.SelectedIndex = 0;

                // Reload all data
                LoadParticipants();

                MessageBox.Show("Data refreshed successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Add New Participant button click
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                Finals_JCDomasian.Forms.RegistrationForm regForm = new Finals_JCDomasian.Forms.RegistrationForm();
                regForm.ShowDialog(); // Open as modal dialog

                // Refresh data after registration form closes
                LoadParticipants();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening registration form: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {

        }

        private void btnAddNew_Click_1(object sender, EventArgs e)
        {

        }
    }
}
