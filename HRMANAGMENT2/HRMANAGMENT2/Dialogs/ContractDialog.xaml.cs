using System;
using System.Windows;
using HRMANAGMENT2.Controllers;
using HRMANAGMENT2.Models;

namespace HRMANAGMENT2.Dialogs
{
    public partial class ContractDialog : Window
    {
        private readonly ContractController _controller;
        private readonly string _id;
        private readonly bool _isReadOnly;

        public ContractDialog(ContractController controller, string id = null, bool isReadOnly = false)
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
                LoadContract(id);
                Title = "Xem/Sửa Hợp đồng";
            }
            else
            {
                Title = "Thêm Hợp đồng";
            }
        }

        private void LoadContract(string id)
        {
            var contract = _controller.GetContractById(id);
            if (contract != null)
            {
                TxtContractId.Text = contract.ContractId;
                CbEmployeeId.SelectedValue = contract.EmployeeId;
                DpStartDate.SelectedDate = contract.StartDate;
                DpEndDate.SelectedDate = contract.EndDate;
                TxtContractType.Text = contract.ContractType;
                TxtContractAnnexPath.Text = contract.ContractAnnexPath;
                TxtConfidentialityAgreementPath.Text = contract.ConfidentialityAgreementPath;
                TxtNonCompeteAgreementPath.Text = contract.NonCompeteAgreementPath;
                TxtAppointmentDecisionPath.Text = contract.AppointmentDecisionPath;
                TxtSalaryIncreaseDecisionPath.Text = contract.SalaryIncreaseDecisionPath;
                TxtRewardDecisionPath.Text = contract.RewardDecisionPath;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var contract = new Contract
                {
                    ContractId = TxtContractId.Text,
                    EmployeeId = CbEmployeeId.SelectedValue?.ToString(),
                    StartDate = DpStartDate.SelectedDate,
                    EndDate = DpEndDate.SelectedDate,
                    ContractType = TxtContractType.Text,
                    ContractAnnexPath = TxtContractAnnexPath.Text,
                    ConfidentialityAgreementPath = TxtConfidentialityAgreementPath.Text,
                    NonCompeteAgreementPath = TxtNonCompeteAgreementPath.Text,
                    AppointmentDecisionPath = TxtAppointmentDecisionPath.Text,
                    SalaryIncreaseDecisionPath = TxtSalaryIncreaseDecisionPath.Text,
                    RewardDecisionPath = TxtRewardDecisionPath.Text
                };
                if (string.IsNullOrEmpty(_id))
                {
                    _controller.AddContract(contract);
                }
                else
                {
                    _controller.UpdateContract(contract);
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