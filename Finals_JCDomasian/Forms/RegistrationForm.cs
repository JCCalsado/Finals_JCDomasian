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

namespace Finals_JCDomasian.Forms
{
    public partial class RegistrationForm : Form
    {
        private DbHelper dbHelper;

        public RegistrationForm()
        {
            InitializeComponent();
            dbHelper = new DbHelper();
        }

        // Form Load Event
        private void RegistrationForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Load Marathon Types
                cmbMarathonType.Items.Clear();
                cmbMarathonType.Items.Add("5km");
                cmbMarathonType.Items.Add("10km");
                cmbMarathonType.Items.Add("25km");

                // Load Payment Status
                cmbPaymentStatus.Items.Clear();
                cmbPaymentStatus.Items.Add("Paid");
                cmbPaymentStatus.Items.Add("Unpaid");
                cmbPaymentStatus.SelectedIndex = 1; // Default to Unpaid

                // Set default date to 18 years ago (minimum age suggestion)
                dtpBirthday.Value = DateTime.Now.AddYears(-18);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading form: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Register Button Click
        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (!ValidateInputs())
                {
                    return;
                }

                // Check if bib number already exists
                if (dbHelper.BibNumberExists(txtBibNumber.Text.Trim()))
                {
                    MessageBox.Show("Bib number already exists! Please use a different bib number.",
                        "Duplicate Bib Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBibNumber.Focus();
                    return;
                }

                // Get TypeID from marathon type
                int typeID = dbHelper.GetTypeIDByName(cmbMarathonType.SelectedItem.ToString());

                if (typeID == 0)
                {
                    MessageBox.Show("Invalid marathon type selected.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Insert participant
                bool success = dbHelper.InsertParticipant(
                    txtFullName.Text.Trim(),
                    dtpBirthday.Value,
                    txtPhone.Text.Trim(),
                    txtAddress.Text.Trim(),
                    txtBibNumber.Text.Trim(),
                    typeID,
                    cmbPaymentStatus.SelectedItem.ToString()
                );

                if (success)
                {
                    MessageBox.Show("Participant registered successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Ask if user wants to add another participant
                    DialogResult result = MessageBox.Show("Do you want to register another participant?",
                        "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        ClearFields();
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Failed to register participant. Please try again.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Validate all inputs
        private bool ValidateInputs()
        {
            // Check Full Name
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Please enter the participant's full name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            // Check Phone Number
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter a phone number.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            // Basic phone validation (must contain digits)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"\d"))
            {
                MessageBox.Show("Phone number must contain digits.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            // Check Address
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Please enter an address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return false;
            }

            // Check Bib Number
            if (string.IsNullOrWhiteSpace(txtBibNumber.Text))
            {
                MessageBox.Show("Please enter a bib number.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBibNumber.Focus();
                return false;
            }

            // Check Marathon Type
            if (cmbMarathonType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a marathon type.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMarathonType.Focus();
                return false;
            }

            // Check Payment Status
            if (cmbPaymentStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a payment status.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPaymentStatus.Focus();
                return false;
            }

            // Check age (must be at least 5 years old)
            int age = DateTime.Now.Year - dtpBirthday.Value.Year;
            if (dtpBirthday.Value > DateTime.Now.AddYears(-age)) age--;

            if (age < 5)
            {
                MessageBox.Show("Participant must be at least 5 years old.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpBirthday.Focus();
                return false;
            }

            return true;
        }

        // Clear all fields
        private void ClearFields()
        {
            txtFullName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            txtBibNumber.Clear();
            cmbMarathonType.SelectedIndex = -1;
            cmbPaymentStatus.SelectedIndex = 1; // Default to Unpaid
            dtpBirthday.Value = DateTime.Now.AddYears(-18);
            txtFullName.Focus();
        }

        // Clear Button Click
        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all fields?",
                "Clear Fields", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ClearFields();
            }
        }

        // Cancel Button Click
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel? Any unsaved data will be lost.",
                "Cancel Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnRegister_Click_1(object sender, EventArgs e)
        {

        }
    }
}
