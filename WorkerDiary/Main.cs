﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WorkerDiary.Properties;

namespace WorkerDiary
{
    public partial class Main : Form
    {
        private FileHelper<List<Employee>> fileHelper = new FileHelper<List<Employee>>(Program.FilePath);
        private static string allShift = "Wszystkie zmiany";

        public bool IsMaximize
        {
            get
            {
                return Settings.Default.IsMaximize;
            }
            set
            {
                Settings.Default.IsMaximize = value;
            }
        }
        public Main()
        {
            InitializeComponent();
            
            DiaryRefresh(allShift);

            SetColumnsHeader();

            SelectShift(Program.Shift);
            
        }
        private void SelectShift(List<string> Shift)
        {
            cbbJob.Items.Add(allShift);
            foreach (var item in Shift)
            {
                cbbJob.Items.Add(item.ToString());
            }
        }
        public void SetColumnsHeader()
        {
            dgvDiary.Columns[0].HeaderText = "Number";
            dgvDiary.Columns[1].HeaderText = "Imie";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Etat";
            dgvDiary.Columns[4].HeaderText = "Zmiana";
            dgvDiary.Columns[5].HeaderText = "Numer telefonu";
            dgvDiary.Columns[6].HeaderText = "Adres e-mail";
            dgvDiary.Columns[7].HeaderText = "Data zatrudnienia";
            dgvDiary.Columns[8].HeaderText = "Komentarz";
            dgvDiary.Columns[9].HeaderText = "Zwolniony";
            
        }
        private void DiaryRefresh(string shift)
        {
            var employees = fileHelper.DeserializeFromFile();
            if (shift != allShift)
                employees = employees.FindAll(x => x.Shift == shift);

            dgvDiary.DataSource =   employees;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditEmployee = new AddEditEmployee();

            addEditEmployee.FormClosing += AddEditEmployee_FormClosing;
            addEditEmployee.ShowDialog();
            addEditEmployee.FormClosing -= AddEditEmployee_FormClosing;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego dane chcesz edytopać");
                return;
            }
            var addEditEmployee = new AddEditEmployee(
                Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));

            addEditEmployee.FormClosing += AddEditEmployee_FormClosing;
            addEditEmployee.ShowDialog();
            addEditEmployee.FormClosing -= AddEditEmployee_FormClosing;
        }

        private void AddEditEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            DiaryRefresh(allShift);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz Pracownika, którego dane chcesz usunąć");
                return;
            }

            var selectedEmployee = dgvDiary.SelectedRows[0];

            var confirmDelet =
                MessageBox.Show($"Czy napewno chesz usunąć pracownika " +
                $"{(selectedEmployee.Cells[0].Value.ToString() + " " + selectedEmployee.Cells[2].Value.ToString()).Trim()}",
                "Usuwanie pracownika", MessageBoxButtons.OKCancel);

            if (confirmDelet == DialogResult.OK)
            {
                DeleteEmployee(Convert.ToInt32(selectedEmployee.Cells[0].Value));
                DiaryRefresh(allShift);
            }
        }
        private void DeleteEmployee(int id)
        {
            var employees = fileHelper.DeserializeFromFile();
            employees.RemoveAll(x => x.Id == id);
            fileHelper.SerializeToFile(employees);
        }

        private void btnRefreh_Click(object sender, EventArgs e)
        {
            DiaryRefresh(cbbJob.Text);
        }

        private void btnDismiss_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz Pracownika, którego dane chcesz zwolnić");
                return;
            }

            var selectedEmployee = dgvDiary.SelectedRows[0];

            var confirmDismiss =
                MessageBox.Show($"Czy napewno chesz zwolnić pracownika " +
                $"{(selectedEmployee.Cells[0].Value.ToString() + " " + selectedEmployee.Cells[2].Value.ToString()).Trim()}",
                "Zwalnianie pracownika", MessageBoxButtons.OKCancel);

            if (confirmDismiss == DialogResult.OK)
            {
                DismissEmployee(Convert.ToInt32(selectedEmployee.Cells[0].Value));
                DiaryRefresh(allShift);
            }
        }

        private void DismissEmployee(int id)
        {
            var employees = fileHelper.DeserializeFromFile();
            var employeeToDismiss = employees.First(x => x.Id == id);
            employeeToDismiss.Dismiss = true;
            fileHelper.SerializeToFile(employees);
            DiaryRefresh(allShift);
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                IsMaximize = true;
            else
                IsMaximize = false;

            Settings.Default.Save();
        }
    }
}
