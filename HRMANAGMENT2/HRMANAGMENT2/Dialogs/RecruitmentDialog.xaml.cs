using System;
using System.Windows;
using HRMANAGMENT2.Controllers;
using HRMANAGMENT2.Models;

namespace HRMANAGMENT2.Dialogs
{
    public partial class RecruitmentDialog : Window
    {
        private readonly RecruitmentController _controller;
        private readonly string _id;
        private readonly bool _isReadOnly;

        public RecruitmentDialog(RecruitmentController controller, string id = null, bool isReadOnly = false)
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
                LoadRecruitment(id);
                Title = "Xem/Sửa Tuyển dụng";
            }
            else
            {
                Title = "Thêm Tuyển dụng";
            }
        }

        private void LoadRecruitment(string id)
        {
            var recruitment = _controller.GetRecruitmentById(id);
            if (recruitment != null)
            {
                TxtRecruitmentId.Text = recruitment.RecruitmentId;
                CbEmployeeId.SelectedValue = recruitment.EmployeeId;
                TxtJobApplicationPath.Text = recruitment.JobApplicationPath;
                TxtResumePath.Text = recruitment.ResumePath;
                TxtDegreesPath.Text = recruitment.DegreesPath;
                TxtHealthCheckPath.Text = recruitment.HealthCheckPath;
                TxtCVPath.Text = recruitment.CVPath;
                TxtReferenceLetterPath.Text = recruitment.ReferenceLetterPath;
                TxtInterviewMinutesPath.Text = recruitment.InterviewMinutesPath;
                TxtOfferLetterPath.Text = recruitment.OfferLetterPath;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var recruitment = new Recruitment
                {
                    RecruitmentId = TxtRecruitmentId.Text,
                    EmployeeId = CbEmployeeId.SelectedValue?.ToString(),
                    JobApplicationPath = TxtJobApplicationPath.Text,
                    ResumePath = TxtResumePath.Text,
                    DegreesPath = TxtDegreesPath.Text,
                    HealthCheckPath = TxtHealthCheckPath.Text,
                    CVPath = TxtCVPath.Text,
                    ReferenceLetterPath = TxtReferenceLetterPath.Text,
                    InterviewMinutesPath = TxtInterviewMinutesPath.Text,
                    OfferLetterPath = TxtOfferLetterPath.Text
                };
                if (string.IsNullOrEmpty(_id))
                {
                    _controller.AddRecruitment(recruitment);
                }
                else
                {
                    _controller.UpdateRecruitment(recruitment);
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