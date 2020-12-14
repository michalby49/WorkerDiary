using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkerDiary
{
    public partial class AddEditEmployee : Form
    {
        private FileHelper<List<Employee>> fileHelper = new FileHelper<List<Employee>>(Program.FilePath);

        private int _employeeId;
        private Employee _employee;
        public AddEditEmployee(int id = 0)
        {
            InitializeComponent();

            _employeeId = id;
            GetEmployeeData();

            SelectShift(Program.Shift);
        }
        private void SelectShift(List<string> Shift)
        {
            foreach (var item in Shift)
            {
                cbbShift.Items.Add(item.ToString());
            }
        }
        private void GetEmployeeData()
        {
            if (_employeeId != 0)
            {
                Text = "Edytowanie pracownika";

                var employees = fileHelper.DeserializeFromFile();
                _employee = employees.FirstOrDefault(x => x.Id == _employeeId);

                if (_employeeId == null)
                    throw new Exception("Brak użytkownika o padanym ID");

                FillTextBoxes();
            }
            tbFirstName.Select();
        }
        private void FillTextBoxes()
        {
            tbId.Text = _employee.Id.ToString();
            tbFirstName.Text = _employee.FirstName.ToString();
            tbLastName.Text = _employee.LastName.ToString();
            tbJob.Text = _employee.Job.ToString();
            tbPhoneNumber.Text = _employee.PhoneNumber.ToString();
            tbEmail.Text = _employee.Email.ToString();
            dtpDateOfEmployment.Text = _employee.DateOfEmploment.ToString();
            rtbCommend.Text = _employee.Commend.ToString();
            cbbShift.SelectedItem = _employee.Shift;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var employees = fileHelper.DeserializeFromFile();

            if (_employeeId != 0)
                employees.RemoveAll(x => x.Id == _employeeId);
            else
                AssingIdToNewStudent(employees);

            AddNewUserToList(employees);

            fileHelper.SerializeToFile(employees);

            Close();
        }

        private void AssingIdToNewStudent(List<Employee> employees)
        {
            var employeesWithHighestId = employees.OrderByDescending(x => x.Id).FirstOrDefault();
            _employeeId = employeesWithHighestId == null ? 1 : employeesWithHighestId.Id + 1;
        }

        private void AddNewUserToList(List<Employee> employees)
        {
            var employee = new Employee()
            {
                Id = _employeeId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Job = tbJob.Text,
                PhoneNumber = tbPhoneNumber.Text,
                Email = tbEmail.Text,
                DateOfEmploment = dtpDateOfEmployment.Text,
                Commend = rtbCommend.Text,
                Shift = cbbShift.Text
                
            };
            employees.Add(employee);
        }
    }
}
