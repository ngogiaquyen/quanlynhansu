using System;
using System.Windows;
using HRMANAGMENT2.Controllers;
using HRMANAGMENT2.Models;

namespace HRMANAGMENT2.Dialogs
{
    public partial class TrainingDialog : Window
    {
        private readonly TrainingController _controller;
        private readonly string _id;
        private readonly bool _isReadOnly;

        public TrainingDialog(TrainingController controller, string id = null, bool isReadOnly = false)
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
                LoadTraining(id);
                Title = "Xem/Sửa Đào tạo";
            }
            else
            {
                Title = "Thêm Đào tạo";
            }
        }

        private void LoadTraining(string id)
        {
            var training = _controller.GetTrainingById(id);
            if (training != null)
            {
                TxtTrainingId.Text = training.TrainingId;
                CbEmployeeId.SelectedValue = training.EmployeeId;
                TxtTrainingPlanPath.Text = training.TrainingPlanPath;
                TxtCertificatePath.Text = training.CertificatePath;
                TxtEvaluationPath.Text = training.EvaluationPath;
                TxtCareerPath.Text = training.CareerPath;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var training = new Training
                {
                    TrainingId = TxtTrainingId.Text,
                    EmployeeId = CbEmployeeId.SelectedValue?.ToString(),
                    TrainingPlanPath = TxtTrainingPlanPath.Text,
                    CertificatePath = TxtCertificatePath.Text,
                    EvaluationPath = TxtEvaluationPath.Text,
                    CareerPath = TxtCareerPath.Text
                };
                if (string.IsNullOrEmpty(_id))
                {
                    _controller.AddTraining(training);
                }
                else
                {
                    _controller.UpdateTraining(training);
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