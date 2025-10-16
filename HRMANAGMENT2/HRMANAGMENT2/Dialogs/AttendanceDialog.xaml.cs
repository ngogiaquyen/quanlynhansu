using System;
using System.Windows;
using HRMANAGMENT2.Controllers;
using HRMANAGMENT2.Models;

namespace HRMANAGMENT2.Dialogs
{
    public partial class AttendanceDialog : Window
    {
        private readonly AttendanceController _controller;
        private readonly string _id;
        private readonly bool _isReadOnly;

        public AttendanceDialog(AttendanceController controller, string id = null, bool isReadOnly = false)
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
                LoadAttendance(id);
                Title = "Xem/Sửa Chấm công";
            }
            else
            {
                Title = "Thêm Chấm công";
            }
        }

        private void LoadAttendance(string id)
        {
            var attendance = _controller.GetAttendanceById(id);
            if (attendance != null)
            {
                TxtAttendanceId.Text = attendance.AttendanceId;
                CbEmployeeId.SelectedValue = attendance.EmployeeId;
                DpAttendanceDate.SelectedDate = attendance.AttendanceDate;
                DpCheckInTime.SelectedDate = attendance.CheckInTime;
                DpCheckOutTime.SelectedDate = attendance.CheckOutTime;
                CbStatus.SelectedItem = attendance.Status;
                TxtAdminHours.Text = attendance.AdminHours?.ToString();
                TxtOvertimeHours.Text = attendance.OvertimeHours?.ToString();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var attendance = new Attendance
                {
                    AttendanceId = TxtAttendanceId.Text,
                    EmployeeId = CbEmployeeId.SelectedValue?.ToString(),
                    AttendanceDate = DpAttendanceDate.SelectedDate,
                    CheckInTime = DpCheckInTime.SelectedDate,
                    CheckOutTime = DpCheckOutTime.SelectedDate,
                    Status = CbStatus.SelectedItem?.ToString(),
                    AdminHours = decimal.TryParse(TxtAdminHours.Text, out decimal ah) ? ah : null,
                    OvertimeHours = decimal.TryParse(TxtOvertimeHours.Text, out decimal ot) ? ot : null
                };
                if (string.IsNullOrEmpty(_id))
                {
                    _controller.AddAttendance(attendance);
                }
                else
                {
                    _controller.UpdateAttendance(attendance);
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