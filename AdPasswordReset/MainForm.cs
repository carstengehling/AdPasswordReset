using System;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;

namespace AdPasswordReset
{
    public partial class MainForm : Form
    {
        private UserPrincipal _currentUser = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            UpdateDetails();
            LoadUsers();
        }

        private void LoadUsers()
        {
            var dn = UserPrincipal.Current.DistinguishedName;
            var domain = WindowsIdentity.GetCurrent().Name.Split(new char[] { '\\' }).First();

            var items = dn.Split(new char[] { ',' }).Where(x => !x.Contains("CN="));
            var pattern = string.Join(",", items);
            var users = ADHelpers.GetUsersInOu(domain, pattern);

            lbUsers.Items.Clear();
            foreach (var user in users)
            {
                if (user.Enabled == null || user.Enabled == false)
                    continue;

                lbUsers.Items.Add(user);
            }
        }

        private void lbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentUser = lbUsers.SelectedItem as UserPrincipal;

            UpdateDetails();
        }

        private void UpdateDetails()
        {
            SuspendLayout();

            tbUser.Text = "";
            tbPassword.Text = "";

            tbPassword.Enabled = _currentUser != null;
            btnGenerate.Enabled = _currentUser != null;
            btnSetPassword.Enabled = _currentUser != null;
            btnUnlock.Enabled = _currentUser != null;

            if (_currentUser == null)
                return;

            var passwordExpiry = ADHelpers.PasswordExpirationDate(_currentUser);
            var passwordExpired = passwordExpiry <= DateTime.UtcNow ? "YES" : "NO";

            var s = string.Format("{1}{0}{0}Login: {2}{0}{0}Password expiry: {3}{0}Password expired: {4}",
                Environment.NewLine, _currentUser.DisplayName, _currentUser.UserPrincipalName, passwordExpiry, passwordExpired);
            if (_currentUser.IsAccountLockedOut())
                s += string.Format("{0}{0}ACCOUNT IS LOCKED", Environment.NewLine);
            tbUser.Text = s;


            btnUnlock.Enabled = _currentUser.IsAccountLockedOut();

            ResumeLayout();
        }

        private void btnSetPassword_Click(object sender, EventArgs e)
        {
            if (_currentUser == null)
                return;

            if (string.IsNullOrWhiteSpace(tbPassword.Text))
                MessageBox.Show("Password is empty");

            if (MessageBox.Show("Are you sure you want to set a new password?", "Set password", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            try
            {
                _currentUser.SetPassword(tbPassword.Text);
                MessageBox.Show("Password has been set");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Access denied.");
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            tbPassword.Text = PasswordHelpers.Generate();
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            if (_currentUser == null)
                return;

            if (MessageBox.Show("Are you sure you want to unlock this user?", "Unlock user", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            try
            {
                _currentUser.UnlockAccount();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Access denied.");
            }

        }
    }
}
