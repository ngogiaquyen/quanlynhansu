using System;
using System.Windows;
using HRMANAGMENT2.Controllers;
using HRMANAGMENT2.Models;

namespace HRMANAGMENT2.Dialogs
{
    public partial class SalaryDialog : Window
    {
        private readonly SalaryController _controller;
        private readonly string _id;
        private readonly bool _isReadOnly;

        public SalaryDialog(SalaryController controller, string id = null, bool isReadOnly = false)
        {
            InitializeComponent();
            _controller = controller;
            _id = id;
            _isReadOnly = isReadOnly;
            BtnSave.IsEnabled = !_isReadOnly;
            var employees = _controller.GetEmployeesForComboBox();
            CbEmployeeId.ItemsSource = employees.DefaultView;
            CbEmployeeId.DisplayMemberPath = "Name";
            CbEmployeeId.SelectedValuePath = "EmployeeId";
            if (!string.IsNullOrEmpty(id))
            {
                LoadSalary(id);
                Title = "Xem/Sửa Lương";
            }
            else
            {
                Title = "Thêm Lương";
            }
        }

        private void LoadSalary(string id)
        {
            var salary = _controller.GetSalaryById(id);
            if (salary != null)
            {
                TxtSalaryId.Text = salary.SalaryId;
                CbEmployeeId.SelectedValue = salary.EmployeeId;
                TxtMonthlySalary.Text = salary.MonthlySalary?.ToString("N0");
                TxtPaySlipPath.Text = salary.PaySlipPath;
                TxtSalaryIncreaseDecisionPath.Text = salary.SalaryIncreaseDecisionPath;
                TxtBankAccount.Text = salary.BankAccount;
                TxtInsuranceInfo.Text = salary.InsuranceInfo;
                TxtAllowances.Text = salary.Allowances?.ToString("N0");
                TxtBonuses.Text = salary.Bonuses?.ToString("N0");
                TxtLeavePolicy.Text = salary.LeavePolicy;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var salary = new Salary
                {
                    SalaryId = TxtSalaryId.Text,
                    EmployeeId = CbEmployeeId.SelectedValue?.ToString(),
                    MonthlySalary = decimal.TryParse(TxtMonthlySalary.Text, out decimal ms) ? ms : null,
                    PaySlipPath = TxtPaySlipPath.Text,
                    SalaryIncreaseDecisionPath = TxtSalaryIncreaseDecisionPath.Text,
                    BankAccount = TxtBankAccount.Text,
                    InsuranceInfo = TxtInsuranceInfo.Text,
                    Allowances = decimal.TryParse(TxtAllowances.Text, out decimal al) ? al : null,
                    Bonuses = decimal.TryParse(TxtBonuses.Text, out decimal bo) ? bo : null,
                    LeavePolicy = TxtLeavePolicy.Text
                };
                if (string.IsNullOrEmpty(_id))
                {
                    _controller.AddSalary(salary);
                }
                else
                {
                    _controller.UpdateSalary(salary);
                }
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}