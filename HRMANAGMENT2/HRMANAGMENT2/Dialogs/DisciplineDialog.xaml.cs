using System;
using System.Windows;
using HRMANAGMENT2.Controllers;
using HRMANAGMENT2.Models;

namespace HRMANAGMENT2.Dialogs
{
    public partial class DisciplineDialog : Window
    {
        private readonly DisciplineController _controller;
        private readonly string _id;
        private readonly bool _isReadOnly;

        public DisciplineDialog(DisciplineController controller, string id = null, bool isReadOnly = false)
        {
            InitializeComponent();
            _controller = controller;
            _id = id;
            _isReadOnly = isReadOnly;
            BtnSave.IsEnabled = !_isReadOnly;
            CbEmployeeId.ItemsSource = _controller.GetEmployeesForComboBox();
            if (!string.IsNullOrEmpty(id))
            {
                LoadDiscipline(id);
                Title = "Xem/Sửa Kỷ luật";
            }
            else
            {
                Title = "Thêm Kỷ luật";
            }
        }

        private void LoadDiscipline(string id)
        {
            var discipline = _controller.GetDisciplineById(id);
            if (discipline != null)
            {
                TxtDisciplineId.Text = discipline.DisciplineId;
                CbEmployeeId.SelectedValue = discipline.EmployeeId;
                TxtViolationPath.Text = discipline.ViolationPath;
                TxtDisciplinaryDecisionPath.Text = discipline.DisciplinaryDecisionPath;
                TxtResignationLetterPath.Text = discipline.ResignationLetterPath;
                TxtTerminationDecisionPath.Text = discipline.TerminationDecisionPath;
                TxtHandoverPath.Text = discipline.HandoverPath;
                TxtLiquidationPath.Text = discipline.LiquidationPath;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var discipline = new Discipline
                {
                    DisciplineId = TxtDisciplineId.Text,
                    EmployeeId = CbEmployeeId.SelectedValue?.ToString(),
                    ViolationPath = TxtViolationPath.Text,
                    DisciplinaryDecisionPath = TxtDisciplinaryDecisionPath.Text,
                    ResignationLetterPath = TxtResignationLetterPath.Text,
                    TerminationDecisionPath = TxtTerminationDecisionPath.Text,
                    HandoverPath = TxtHandoverPath.Text,
                    LiquidationPath = TxtLiquidationPath.Text
                };
                if (string.IsNullOrEmpty(_id))
                {
                    _controller.AddDiscipline(discipline);
                }
                else
                {
                    _controller.UpdateDiscipline(discipline);
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