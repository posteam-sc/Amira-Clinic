﻿using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.mPOSUI.MedicalPerson
{
    public partial class FrmMedicalPersonList : Form
    {
        private POSEntities entity = new POSEntities();
        public FrmMedicalPersonList()
        {
            InitializeComponent();
        }

        private void FrmMedicalPersonList_Load(object sender, EventArgs e)
        {
            bindMedicalPersonType();
            Localization.Localize_FormControls(this);
            dgvMedicalPersonList.AutoGenerateColumns = false;
            LoadData();
        }

        public void DataBind()
        {
            entity = new POSEntities();
            dgvMedicalPersonList.DataSource = (from c in entity.Customers where c.CustomerTypeId != 1 select c).ToList();
        }
        public void bindMedicalPersonType()
        {
            entity = new POSEntities();
            List<APP_Data.CustomerType> customerTypeList = new List<APP_Data.CustomerType>();
            APP_Data.CustomerType customerType = new APP_Data.CustomerType();
            customerType.Id = 0;
            customerType.TypeName = "Select";
            customerTypeList.Add(customerType);
            customerTypeList.AddRange(entity.CustomerTypes.Where(x => x.TypeName != "Patient").ToList());
            cboMedicalPersonType.DataSource = customerTypeList;
            cboMedicalPersonType.DisplayMember = "TypeName";
            cboMedicalPersonType.ValueMember = "Id";
        }
        public void LoadData()
        {
            POSEntities entities = new POSEntities();
            var medicalPersonList = entities.Customers.Where(x => x.CustomerTypeId != 1).ToList();            

            if (cboMedicalPersonType.SelectedIndex != 0)
            {
                medicalPersonList = (from c in entity.Customers.AsEnumerable() where c.CustomerTypeId == Convert.ToInt32(cboMedicalPersonType.SelectedValue) select c).ToList();
            }
           
            medicalPersonList = medicalPersonList.Where(x => x.Name.Trim().ToLower().Contains(txtMedicalName.Text.Trim().ToLower())).ToList();
            dgvMedicalPersonList.DataSource = medicalPersonList;
            
        }

        private void dgvMedicalPersonList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //View detail information of MedicalPerson
                if (e.ColumnIndex == 6)
                {
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Customer.ViewDetail || MemberShip.isAdmin)
                    {
                        if (System.Windows.Forms.Application.OpenForms["FrmMedicalPersonDetailInfo"] != null)
                        {
                            FrmMedicalPersonDetailInfo newForm = (FrmMedicalPersonDetailInfo)System.Windows.Forms.Application.OpenForms["FrmMedicalPersonDetailInfo"];
                            newForm.customerId = Convert.ToInt32(dgvMedicalPersonList.Rows[e.RowIndex].Cells[0].Value);
                            newForm.ShowDialog();
                        }
                        else
                        {
                            FrmMedicalPersonDetailInfo newForm = new FrmMedicalPersonDetailInfo();
                            newForm.customerId = Convert.ToInt32(dgvMedicalPersonList.Rows[e.RowIndex].Cells[0].Value);
                            newForm.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to view detail  customer", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }
                //Edit this User
                else if (e.ColumnIndex == 7)
                {
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Customer.EditOrDelete || MemberShip.isAdmin)
                    {
                        FrmMedicalPerson form = new FrmMedicalPerson();
                        form.isEdit = true;
                        form.Text = "Edit Employee";
                        form.CustomerId = Convert.ToInt32(dgvMedicalPersonList.Rows[e.RowIndex].Cells[0].Value);
                        form.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit customer", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                //Delete this User
                else if (e.ColumnIndex == 8)
                {
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);

                    if (controller.Customer.EditOrDelete || MemberShip.isAdmin)
                    {

                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            DataGridViewRow row = dgvMedicalPersonList.Rows[e.RowIndex];
                            Customer cust = (Customer)row.DataBoundItem;
                            cust = (from c in entity.Customers where c.Id == cust.Id select c).FirstOrDefault<Customer>();
                            var isDoctorData = entity.PackageUsedHistories.Any(x => x.CustomerIDAsDoctor == cust.Id || x.CustomerIDAsAssistantNurse==cust.Id || x.CustomerIDAsTherapist==cust.Id || x.CustomerIDAsSalePerson==cust.Id);
                            //Need to recheck
                            if (cust.Transactions.Count > 0)
                            {
                                MessageBox.Show("This Employee already made transactions!", "Unable to Delete");
                                return;
                            }
                            else if (isDoctorData)
                            {
                                MessageBox.Show("This Employee already used by Offset. It cannot be deleted!");
                                return;
                            }

                            else
                            {
                                entity.Customers.Remove(cust);
                                entity.SaveChanges();
                                LoadData();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete customer", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                {
                    Sales newForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];
                    newForm.Clear();
                }
            }
        }

        private void dgvMedicalPersonList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvMedicalPersonList.Rows)
            {
                POSEntities entities = new POSEntities();
                var customerType = entities.CustomerTypes.ToList();
                Customer cs = (Customer)row.DataBoundItem;
                row.Cells[0].Value = cs.Id;
                row.Cells[1].Value = cs.Name;
                row.Cells[2].Value = cs.PhoneNumber;
                row.Cells[3].Value = cs.Address;
                row.Cells[4].Value = cs.NRC;
                row.Cells[5].Value =customerType.Where(x=>x.Id==cs.CustomerTypeId).Select(x=>x.TypeName).FirstOrDefault();

            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtMedicalName.Text = "";
            cboMedicalPersonType.Text = "Select";
            LoadData();
        }
    }
}