using Microsoft.Reporting.WinForms;
using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.mPOSUI.Reporting
{
    public partial class frmSaleReport : Form
    {
        public frmSaleReport()
        {
            InitializeComponent();
        }

        private void frmSaleReport_Load(object sender, EventArgs e)
        {
            inititalState();
            this.reportViewer1.RefreshReport();
            dtFrom.Value= new DateTime(dtFrom.Value.Year, dtFrom.Value.Month, 1);
            dtTo.Value = DateTime.Now.Date;
            loadData();
        }

        #region Method

        private void bindPatient()
        {
            var entity = new POSEntities();
            List<APP_Data.Customer> customerList = new List<APP_Data.Customer>();
            APP_Data.Customer customer = new APP_Data.Customer();
            customer.Id = 0;
            customer.Name = "Select";
            customerList.Add(customer);
            customerList.AddRange(entity.Customers.Where(x => x.CustomerTypeId == 1).ToList());
            cboCustomerName.DataSource = customerList;
            cboCustomerName.DisplayMember = "Name";
            cboCustomerName.ValueMember = "Id";
        }

        private void bindProduct()
        {
            var entity = new POSEntities();
            List<Product> productList = new List<Product>();
            Product product = new Product();
            product.Id = 0;
            product.Name = "Select";
            productList.Add(product);
            productList.AddRange(entity.Products.ToList());
            cboProductName.DataSource = productList;
            cboProductName.DisplayMember = "Name";
            cboProductName.ValueMember = "Id";
        }
        private void bindCategory()
        {
            var entity = new POSEntities();
            List<APP_Data.ProductCategory> productList = new List<APP_Data.ProductCategory>();
            APP_Data.ProductCategory productCategoy = new APP_Data.ProductCategory();
            productCategoy.Id = 0;
            productCategoy.Name = "Select";
            productList.Add(productCategoy);
            productList.AddRange(entity.ProductCategories.ToList());
            cboPCategory.DataSource = productList;
            cboPCategory.DisplayMember = "Name";
            cboPCategory.ValueMember = "Id";
        }

        private void bindSubCategory()
        {
            var entity = new POSEntities();
            List<APP_Data.ProductSubCategory> productList = new List<APP_Data.ProductSubCategory>();
            APP_Data.ProductSubCategory productCategoy = new APP_Data.ProductSubCategory();
            productCategoy.Id = 0;
            productCategoy.Name = "Select";
            productList.Add(productCategoy);
            productList.AddRange(entity.ProductSubCategories.ToList());
            cboPSCategory.DataSource = productList;
            cboPSCategory.DisplayMember = "Name";
            cboPSCategory.ValueMember = "Id";
        }

        private void loadData()
        {
            POSEntities entity = new POSEntities();
            List<SaleForReport> saleList = new List<SaleForReport>();
            var data = (from t in entity.Transactions
                        join td in entity.TransactionDetails on t.Id equals td.TransactionId
                        join p in entity.Products on td.ProductId equals p.Id
                        join c in entity.Customers on t.CustomerId equals c.Id
                        where t.IsDeleted == false
                        select new
                        {
                            TransationID = t.Id,
                            Date = t.DateTime,
                            ProductCode = p.ProductCode,
                            ProductName = p.Name,
                            Category = p.ProductCategory.Name,
                            SubCategory = p.ProductSubCategory.Name,
                            PatientName = c.Name,
                            PatientID = c.CustomerCode,
                            SalePersonName = t.SalePerson,
                            UnitPrice = td.UnitPrice,
                            SaleQty = td.Qty,
                            GrossSale = td.UnitPrice * td.Qty,
                            DiscountPercentage = td.DiscountRate,
                            DiscountAmount = t.DiscountAmount,
                            IsFOC = td.IsFOC,
                            RecieveAmount=t.RecieveAmount,                            
                            PaymentType = t.Type,
                            PaymentMethod = t.PaymentTypeId,
                            PaymentMethodName = t.PaymentType.Name,
                            isPaid = t.IsPaid,
                            GiftAmt = t.GiftCardAmount,                           
                            Note=t.Note,

                        
                        }).OrderBy(x=>x.Date).ToList();


            #region Search

            if (cboCustomerName.SelectedIndex > 0 && cboPCategory.SelectedIndex <= 0 && cboProductName.SelectedIndex <= 0 && cboPSCategory.SelectedIndex <= 0)

            {
                data = data.Where(x => x.Date.Value.Date >= dtFrom.Value.Date && x.Date.Value.Date <= dtTo.Value.Date && x.PatientName == cboCustomerName.Text).ToList();

                if (data.Count != 0)
                {

                    foreach (var item in data)
                    {
                        var saleCount = entity.TransactionDetails.Where(x => x.TransactionId == item.TransationID).Count();
                        var additionalDis = (int)(item.DiscountAmount - (item.UnitPrice * item.DiscountPercentage / 100)) / saleCount;
                        var sale = new SaleForReport();
                        sale.Date = Convert.ToDateTime(item.Date);
                        sale.TransactionID = item.TransationID;
                        sale.ProductCode = item.ProductCode;
                        sale.ProductName = item.ProductName;
                        sale.Category = item.Category;
                        sale.SubCategory = item.SubCategory;
                        sale.PatientName = item.PatientName;
                        sale.PatientID = item.PatientID;
                        sale.SalePersonName = entity.Customers.Where(x => x.Id == item.SalePersonName).Select(x => x.Name).FirstOrDefault();
                        sale.UnitPrice = (int)item.UnitPrice;
                        sale.SaleQty = (int)item.SaleQty;
                        sale.GrossSale = (int)item.GrossSale;
                        sale.DiscountPercentage = item.DiscountPercentage;
                        sale.DiscountAmount = (int)((item.UnitPrice * item.DiscountPercentage / 100)) + additionalDis + (int)item.GiftAmt / saleCount;
                        sale.NetSaleAmount = (int)(item.GrossSale - (sale.DiscountAmount));
                        if (item.isPaid == false)
                        {
                            sale.RecieveAmount = (int)((item.RecieveAmount / saleCount));
                            sale.OutstandingAmount = (int)(item.GrossSale - sale.RecieveAmount) - sale.DiscountAmount;
                        }
                        else
                        {

                            sale.RecieveAmount = (int)(int)item.GrossSale - (sale.DiscountAmount);
                            sale.OutstandingAmount = 0;
                        }
                        if (item.IsFOC == true)
                        {
                            sale.FOCQty = (int)item.SaleQty;
                            sale.FOCAmount = (int)(item.UnitPrice * item.SaleQty);
                            sale.RecieveAmount = 0;
                            sale.NetSaleAmount = 0;
                        }


                        sale.PaymentType = item.PaymentType;
                        if (item.PaymentMethod == 1)
                        {
                            sale.PaymentMethod = "Cash";
                        }
                        else if (item.PaymentMethod == 2)
                        {
                            sale.PaymentMethod = "Credit";
                        }
                        else if (item.PaymentMethod == 5 || item.PaymentMethod >= 501)
                        {
                            sale.PaymentMethod = "Bank";
                            sale.BankPayment = item.PaymentMethodName;
                        }
                        else if (item.PaymentMethod == 4)
                        {
                            sale.PaymentMethod = "FOC";
                        }

                        sale.Note = item.Note;
                        saleList.Add(sale);
                    }

                }

            }
            else if (cboCustomerName.SelectedIndex > 0 && cboPCategory.SelectedIndex > 0 && cboProductName.SelectedIndex <= 0 && cboPSCategory.SelectedIndex <= 0)
            {
                data = data.Where(x => x.Date.Value.Date >= dtFrom.Value.Date && x.Date.Value.Date <= dtTo.Value.Date && x.PatientName == cboCustomerName.Text && x.Category == cboPCategory.Text).ToList();

                if (data.Count != 0)
                {

                    foreach (var item in data)
                    {
                        var saleCount = entity.TransactionDetails.Where(x => x.TransactionId == item.TransationID).Count();
                        var additionalDis = (int)(item.DiscountAmount - (item.UnitPrice * item.DiscountPercentage / 100)) / saleCount;
                        var sale = new SaleForReport();
                        sale.Date = Convert.ToDateTime(item.Date);
                        sale.TransactionID = item.TransationID;
                        sale.ProductCode = item.ProductCode;
                        sale.ProductName = item.ProductName;
                        sale.Category = item.Category;
                        sale.SubCategory = item.SubCategory;
                        sale.PatientName = item.PatientName;
                        sale.PatientID = item.PatientID;
                        sale.SalePersonName = entity.Customers.Where(x => x.Id == item.SalePersonName).Select(x => x.Name).FirstOrDefault();
                        sale.UnitPrice = (int)item.UnitPrice;
                        sale.SaleQty = (int)item.SaleQty;
                        sale.GrossSale = (int)item.GrossSale;
                        sale.DiscountPercentage = item.DiscountPercentage;
                        sale.DiscountAmount = (int)((item.UnitPrice * item.DiscountPercentage / 100)) + additionalDis + (int)item.GiftAmt / saleCount;
                        sale.NetSaleAmount = (int)(item.GrossSale - (sale.DiscountAmount));
                        if (item.isPaid == false)
                        {
                            sale.RecieveAmount = (int)((item.RecieveAmount / saleCount));
                            sale.OutstandingAmount = (int)(item.GrossSale - sale.RecieveAmount) - sale.DiscountAmount;
                        }
                        else
                        {

                            sale.RecieveAmount = (int)(int)item.GrossSale - (sale.DiscountAmount);
                            sale.OutstandingAmount = 0;
                        }
                        if (item.IsFOC == true)
                        {
                            sale.FOCQty = (int)item.SaleQty;
                            sale.FOCAmount = (int)(item.UnitPrice * item.SaleQty);
                            sale.RecieveAmount = 0;
                            sale.NetSaleAmount = 0;
                        }


                        sale.PaymentType = item.PaymentType;
                        if (item.PaymentMethod == 1)
                        {
                            sale.PaymentMethod = "Cash";
                        }
                        else if (item.PaymentMethod == 2)
                        {
                            sale.PaymentMethod = "Credit";
                        }
                        else if (item.PaymentMethod == 5 || item.PaymentMethod >= 501)
                        {
                            sale.PaymentMethod = "Bank";
                            sale.BankPayment = item.PaymentMethodName;
                        }
                        else if (item.PaymentMethod == 4)
                        {
                            sale.PaymentMethod = "FOC";
                        }

                        sale.Note = item.Note;
                        saleList.Add(sale);
                    }

                }
            }

            else if (cboCustomerName.SelectedIndex > 0 && cboPCategory.SelectedIndex > 0 && cboProductName.SelectedIndex > 0 && cboPSCategory.SelectedIndex <= 0)
            {
                data = data.Where(x => x.Date.Value.Date >= dtFrom.Value.Date && x.Date.Value.Date <= dtTo.Value.Date && x.PatientName == cboCustomerName.Text && x.Category == cboPCategory.Text
                                    && x.ProductName == cboProductName.Text).ToList();

                if (data.Count != 0)
                {

                    foreach (var item in data)
                    {
                        var saleCount = entity.TransactionDetails.Where(x => x.TransactionId == item.TransationID).Count();
                        var additionalDis = (int)(item.DiscountAmount - (item.UnitPrice * item.DiscountPercentage / 100)) / saleCount;
                        var sale = new SaleForReport();
                        sale.Date = Convert.ToDateTime(item.Date);
                        sale.TransactionID = item.TransationID;
                        sale.ProductCode = item.ProductCode;
                        sale.ProductName = item.ProductName;
                        sale.Category = item.Category;
                        sale.SubCategory = item.SubCategory;
                        sale.PatientName = item.PatientName;
                        sale.PatientID = item.PatientID;
                        sale.SalePersonName = entity.Customers.Where(x => x.Id == item.SalePersonName).Select(x => x.Name).FirstOrDefault();
                        sale.UnitPrice = (int)item.UnitPrice;
                        sale.SaleQty = (int)item.SaleQty;
                        sale.GrossSale = (int)item.GrossSale;
                        sale.DiscountPercentage = item.DiscountPercentage;
                        sale.DiscountAmount = (int)((item.UnitPrice * item.DiscountPercentage / 100)) + additionalDis + (int)item.GiftAmt / saleCount;
                        sale.NetSaleAmount = (int)(item.GrossSale - (sale.DiscountAmount));
                        if (item.isPaid == false)
                        {
                            sale.RecieveAmount = (int)((item.RecieveAmount / saleCount));
                            sale.OutstandingAmount = (int)(item.GrossSale - sale.RecieveAmount) - sale.DiscountAmount;
                        }
                        else
                        {

                            sale.RecieveAmount = (int)(int)item.GrossSale - (sale.DiscountAmount);
                            sale.OutstandingAmount = 0;
                        }
                        if (item.IsFOC == true)
                        {
                            sale.FOCQty = (int)item.SaleQty;
                            sale.FOCAmount = (int)(item.UnitPrice * item.SaleQty);
                            sale.RecieveAmount = 0;
                            sale.NetSaleAmount = 0;
                        }


                        sale.PaymentType = item.PaymentType;
                        if (item.PaymentMethod == 1)
                        {
                            sale.PaymentMethod = "Cash";
                        }
                        else if (item.PaymentMethod == 2)
                        {
                            sale.PaymentMethod = "Credit";
                        }
                        else if (item.PaymentMethod == 5 || item.PaymentMethod >= 501)
                        {
                            sale.PaymentMethod = "Bank";
                            sale.BankPayment = item.PaymentMethodName;
                        }
                        else if (item.PaymentMethod == 4)
                        {
                            sale.PaymentMethod = "FOC";
                        }

                        sale.Note = item.Note;
                        saleList.Add(sale);
                    }

                }
            }

            else if (cboCustomerName.SelectedIndex > 0 && cboPCategory.SelectedIndex > 0 && cboProductName.SelectedIndex > 0 && cboPSCategory.SelectedIndex > 0)
            {
                data = data.Where(x => x.Date.Value.Date >= dtFrom.Value.Date && x.Date.Value.Date <= dtTo.Value.Date && x.PatientName == cboCustomerName.Text && x.Category == cboPCategory.Text
                                    && x.ProductName == cboProductName.Text && x.SubCategory == cboPSCategory.Text).ToList();

                if (data.Count != 0)
                {

                    foreach (var item in data)
                    {
                        var saleCount = entity.TransactionDetails.Where(x => x.TransactionId == item.TransationID).Count();
                        var additionalDis = (int)(item.DiscountAmount - (item.UnitPrice * item.DiscountPercentage / 100)) / saleCount;
                        var sale = new SaleForReport();
                        sale.Date = Convert.ToDateTime(item.Date);
                        sale.TransactionID = item.TransationID;
                        sale.ProductCode = item.ProductCode;
                        sale.ProductName = item.ProductName;
                        sale.Category = item.Category;
                        sale.SubCategory = item.SubCategory;
                        sale.PatientName = item.PatientName;
                        sale.PatientID = item.PatientID;
                        sale.SalePersonName = entity.Customers.Where(x => x.Id == item.SalePersonName).Select(x => x.Name).FirstOrDefault();
                        sale.UnitPrice = (int)item.UnitPrice;
                        sale.SaleQty = (int)item.SaleQty;
                        sale.GrossSale = (int)item.GrossSale;
                        sale.DiscountPercentage = item.DiscountPercentage;
                        sale.DiscountAmount = (int)((item.UnitPrice * item.DiscountPercentage / 100)) + additionalDis + (int)item.GiftAmt / saleCount;
                        sale.NetSaleAmount = (int)(item.GrossSale - (sale.DiscountAmount));
                        if (item.isPaid == false)
                        {
                            sale.RecieveAmount = (int)((item.RecieveAmount / saleCount));
                            sale.OutstandingAmount = (int)(item.GrossSale - sale.RecieveAmount) - sale.DiscountAmount;
                        }
                        else
                        {

                            sale.RecieveAmount = (int)(int)item.GrossSale - (sale.DiscountAmount);
                            sale.OutstandingAmount = 0;
                        }
                        if (item.IsFOC == true)
                        {
                            sale.FOCQty = (int)item.SaleQty;
                            sale.FOCAmount = (int)(item.UnitPrice * item.SaleQty);
                            sale.RecieveAmount = 0;
                            sale.NetSaleAmount = 0;
                        }


                        sale.PaymentType = item.PaymentType;
                        if (item.PaymentMethod == 1)
                        {
                            sale.PaymentMethod = "Cash";
                        }
                        else if (item.PaymentMethod == 2)
                        {
                            sale.PaymentMethod = "Credit";
                        }
                        else if (item.PaymentMethod == 5 || item.PaymentMethod >= 501)
                        {
                            sale.PaymentMethod = "Bank";
                            sale.BankPayment = item.PaymentMethodName;
                        }
                        else if (item.PaymentMethod == 4)
                        {
                            sale.PaymentMethod = "FOC";
                        }

                        sale.Note = item.Note;
                        saleList.Add(sale);
                    }

                }
            }
            else if (cboCustomerName.SelectedIndex <= 0 && cboPCategory.SelectedIndex > 0 && cboProductName.SelectedIndex <= 0 && cboPSCategory.SelectedIndex <= 0)
            {
                data = data.Where(x => x.Date.Value.Date >= dtFrom.Value.Date && x.Date.Value.Date <= dtTo.Value.Date && x.Category == cboPCategory.Text).ToList();

                if (data.Count != 0)
                {

                    foreach (var item in data)
                    {
                        var saleCount = entity.TransactionDetails.Where(x => x.TransactionId == item.TransationID).Count();
                        var additionalDis = (int)(item.DiscountAmount - (item.UnitPrice * item.DiscountPercentage / 100)) / saleCount;
                        var sale = new SaleForReport();
                        sale.Date = Convert.ToDateTime(item.Date);
                        sale.TransactionID = item.TransationID;
                        sale.ProductCode = item.ProductCode;
                        sale.ProductName = item.ProductName;
                        sale.Category = item.Category;
                        sale.SubCategory = item.SubCategory;
                        sale.PatientName = item.PatientName;
                        sale.PatientID = item.PatientID;
                        sale.SalePersonName = entity.Customers.Where(x => x.Id == item.SalePersonName).Select(x => x.Name).FirstOrDefault();
                        sale.UnitPrice = (int)item.UnitPrice;
                        sale.SaleQty = (int)item.SaleQty;
                        sale.GrossSale = (int)item.GrossSale;
                        sale.DiscountPercentage = item.DiscountPercentage;
                        sale.DiscountAmount = (int)((item.UnitPrice * item.DiscountPercentage / 100)) + additionalDis + (int)item.GiftAmt / saleCount;
                        sale.NetSaleAmount = (int)(item.GrossSale - (sale.DiscountAmount));
                        if (item.isPaid == false)
                        {
                            sale.RecieveAmount = (int)((item.RecieveAmount / saleCount));
                            sale.OutstandingAmount = (int)(item.GrossSale - sale.RecieveAmount) - sale.DiscountAmount;
                        }
                        else
                        {

                            sale.RecieveAmount = (int)(int)item.GrossSale - (sale.DiscountAmount);
                            sale.OutstandingAmount = 0;
                        }
                        if (item.IsFOC == true)
                        {
                            sale.FOCQty = (int)item.SaleQty;
                            sale.FOCAmount = (int)(item.UnitPrice * item.SaleQty);
                            sale.RecieveAmount = 0;
                            sale.NetSaleAmount = 0;
                        }


                        sale.PaymentType = item.PaymentType;
                        if (item.PaymentMethod == 1)
                        {
                            sale.PaymentMethod = "Cash";
                        }
                        else if (item.PaymentMethod == 2)
                        {
                            sale.PaymentMethod = "Credit";
                        }
                        else if (item.PaymentMethod == 5 || item.PaymentMethod >= 501)
                        {
                            sale.PaymentMethod = "Bank";
                            sale.BankPayment = item.PaymentMethodName;
                        }
                        else if (item.PaymentMethod == 4)
                        {
                            sale.PaymentMethod = "FOC";
                        }

                        sale.Note = item.Note;
                        saleList.Add(sale);
                    }

                }
            }
            else if (cboCustomerName.SelectedIndex <= 0 && cboPCategory.SelectedIndex > 0 && cboProductName.SelectedIndex > 0 && cboPSCategory.SelectedIndex <= 0)
            {
                data = data.Where(x => x.Date.Value.Date >= dtFrom.Value.Date && x.Date.Value.Date <= dtTo.Value.Date && x.Category == cboPCategory.Text && x.ProductName == cboProductName.Text).ToList();

                if (data.Count != 0)
                {

                    foreach (var item in data)
                    {
                        var saleCount = entity.TransactionDetails.Where(x => x.TransactionId == item.TransationID).Count();
                        var additionalDis = (int)(item.DiscountAmount - (item.UnitPrice * item.DiscountPercentage / 100)) / saleCount;
                        var sale = new SaleForReport();
                        sale.Date = Convert.ToDateTime(item.Date);
                        sale.TransactionID = item.TransationID;
                        sale.ProductCode = item.ProductCode;
                        sale.ProductName = item.ProductName;
                        sale.Category = item.Category;
                        sale.SubCategory = item.SubCategory;
                        sale.PatientName = item.PatientName;
                        sale.PatientID = item.PatientID;
                        sale.SalePersonName = entity.Customers.Where(x => x.Id == item.SalePersonName).Select(x => x.Name).FirstOrDefault();
                        sale.UnitPrice = (int)item.UnitPrice;
                        sale.SaleQty = (int)item.SaleQty;
                        sale.GrossSale = (int)item.GrossSale;
                        sale.DiscountPercentage = item.DiscountPercentage;
                        sale.DiscountAmount = (int)((item.UnitPrice * item.DiscountPercentage / 100)) + additionalDis + (int)item.GiftAmt / saleCount;
                        sale.NetSaleAmount = (int)(item.GrossSale - (sale.DiscountAmount));
                        if (item.isPaid == false)
                        {
                            sale.RecieveAmount = (int)((item.RecieveAmount / saleCount));
                            sale.OutstandingAmount = (int)(item.GrossSale - sale.RecieveAmount) - sale.DiscountAmount;
                        }
                        else
                        {

                            sale.RecieveAmount = (int)(int)item.GrossSale - (sale.DiscountAmount);
                            sale.OutstandingAmount = 0;
                        }
                        if (item.IsFOC == true)
                        {
                            sale.FOCQty = (int)item.SaleQty;
                            sale.FOCAmount = (int)(item.UnitPrice * item.SaleQty);
                            sale.RecieveAmount = 0;
                            sale.NetSaleAmount = 0;
                        }


                        sale.PaymentType = item.PaymentType;
                        if (item.PaymentMethod == 1)
                        {
                            sale.PaymentMethod = "Cash";
                        }
                        else if (item.PaymentMethod == 2)
                        {
                            sale.PaymentMethod = "Credit";
                        }
                        else if (item.PaymentMethod == 5 || item.PaymentMethod >= 501)
                        {
                            sale.PaymentMethod = "Bank";
                            sale.BankPayment = item.PaymentMethodName;
                        }
                        else if (item.PaymentMethod == 4)
                        {
                            sale.PaymentMethod = "FOC";
                        }

                        sale.Note = item.Note;
                        saleList.Add(sale);
                    }

                }
            }
            else if (cboCustomerName.SelectedIndex <= 0 && cboPCategory.SelectedIndex > 0 && cboProductName.SelectedIndex > 0 && cboPSCategory.SelectedIndex > 0)
            {
                data = data.Where(x => x.Date.Value.Date >= dtFrom.Value.Date && x.Date.Value.Date <= dtTo.Value.Date && x.Category == cboPCategory.Text && x.ProductName == cboProductName.Text
                                    && x.SubCategory == cboPSCategory.Text).ToList();

                if (data.Count != 0)
                {

                    foreach (var item in data)
                    {
                        var saleCount = entity.TransactionDetails.Where(x => x.TransactionId == item.TransationID).Count();
                        var additionalDis = (int)(item.DiscountAmount - (item.UnitPrice * item.DiscountPercentage / 100)) / saleCount;
                        var sale = new SaleForReport();
                        sale.Date = Convert.ToDateTime(item.Date);
                        sale.TransactionID = item.TransationID;
                        sale.ProductCode = item.ProductCode;
                        sale.ProductName = item.ProductName;
                        sale.Category = item.Category;
                        sale.SubCategory = item.SubCategory;
                        sale.PatientName = item.PatientName;
                        sale.PatientID = item.PatientID;
                        sale.SalePersonName = entity.Customers.Where(x => x.Id == item.SalePersonName).Select(x => x.Name).FirstOrDefault();
                        sale.UnitPrice = (int)item.UnitPrice;
                        sale.SaleQty = (int)item.SaleQty;
                        sale.GrossSale = (int)item.GrossSale;
                        sale.DiscountPercentage = item.DiscountPercentage;
                        sale.DiscountAmount = (int)((item.UnitPrice * item.DiscountPercentage / 100)) + additionalDis + (int)item.GiftAmt / saleCount;
                        sale.NetSaleAmount = (int)(item.GrossSale - (sale.DiscountAmount));
                        if (item.isPaid == false)
                        {
                            sale.RecieveAmount = (int)((item.RecieveAmount / saleCount));
                            sale.OutstandingAmount = (int)(item.GrossSale - sale.RecieveAmount) - sale.DiscountAmount;
                        }
                        else
                        {

                            sale.RecieveAmount = (int)(int)item.GrossSale - (sale.DiscountAmount);
                            sale.OutstandingAmount = 0;
                        }
                        if (item.IsFOC == true)
                        {
                            sale.FOCQty = (int)item.SaleQty;
                            sale.FOCAmount = (int)(item.UnitPrice * item.SaleQty);
                            sale.RecieveAmount = 0;
                            sale.NetSaleAmount = 0;
                        }


                        sale.PaymentType = item.PaymentType;
                        if (item.PaymentMethod == 1)
                        {
                            sale.PaymentMethod = "Cash";
                        }
                        else if (item.PaymentMethod == 2)
                        {
                            sale.PaymentMethod = "Credit";
                        }
                        else if (item.PaymentMethod == 5 || item.PaymentMethod >= 501)
                        {
                            sale.PaymentMethod = "Bank";
                            sale.BankPayment = item.PaymentMethodName;
                        }
                        else if (item.PaymentMethod == 4)
                        {
                            sale.PaymentMethod = "FOC";
                        }

                        sale.Note = item.Note;
                        saleList.Add(sale);
                    }

                }
            }
            else if (cboCustomerName.SelectedIndex <= 0 && cboPCategory.SelectedIndex > 0 && cboProductName.SelectedIndex <= 0 && cboPSCategory.SelectedIndex > 0)
            {
                data = data.Where(x => x.Date.Value.Date >= dtFrom.Value.Date && x.Date.Value.Date <= dtTo.Value.Date && x.Category == cboPCategory.Text && x.SubCategory == cboPSCategory.Text).ToList();

                if (data.Count != 0)
                {

                    foreach (var item in data)
                    {
                        var saleCount = entity.TransactionDetails.Where(x => x.TransactionId == item.TransationID).Count();
                        var additionalDis = (int)(item.DiscountAmount - (item.UnitPrice * item.DiscountPercentage / 100)) / saleCount;
                        var sale = new SaleForReport();
                        sale.Date = Convert.ToDateTime(item.Date);
                        sale.TransactionID = item.TransationID;
                        sale.ProductCode = item.ProductCode;
                        sale.ProductName = item.ProductName;
                        sale.Category = item.Category;
                        sale.SubCategory = item.SubCategory;
                        sale.PatientName = item.PatientName;
                        sale.PatientID = item.PatientID;
                        sale.SalePersonName = entity.Customers.Where(x => x.Id == item.SalePersonName).Select(x => x.Name).FirstOrDefault();
                        sale.UnitPrice = (int)item.UnitPrice;
                        sale.SaleQty = (int)item.SaleQty;
                        sale.GrossSale = (int)item.GrossSale;
                        sale.DiscountPercentage = item.DiscountPercentage;
                        sale.DiscountAmount = (int)((item.UnitPrice * item.DiscountPercentage / 100)) + additionalDis + (int)item.GiftAmt / saleCount;
                        sale.NetSaleAmount = (int)(item.GrossSale - (sale.DiscountAmount));
                        if (item.isPaid == false)
                        {
                            sale.RecieveAmount = (int)((item.RecieveAmount / saleCount));
                            sale.OutstandingAmount = (int)(item.GrossSale - sale.RecieveAmount) - sale.DiscountAmount;
                        }
                        else
                        {

                            sale.RecieveAmount = (int)(int)item.GrossSale - (sale.DiscountAmount);
                            sale.OutstandingAmount = 0;
                        }
                        if (item.IsFOC == true)
                        {
                            sale.FOCQty = (int)item.SaleQty;
                            sale.FOCAmount = (int)(item.UnitPrice * item.SaleQty);
                            sale.RecieveAmount = 0;
                            sale.NetSaleAmount = 0;
                        }


                        sale.PaymentType = item.PaymentType;
                        if (item.PaymentMethod == 1)
                        {
                            sale.PaymentMethod = "Cash";
                        }
                        else if (item.PaymentMethod == 2)
                        {
                            sale.PaymentMethod = "Credit";
                        }
                        else if (item.PaymentMethod == 5 || item.PaymentMethod >= 501)
                        {
                            sale.PaymentMethod = "Bank";
                            sale.BankPayment = item.PaymentMethodName;
                        }
                        else if (item.PaymentMethod == 4)
                        {
                            sale.PaymentMethod = "FOC";
                        }

                        sale.Note = item.Note;
                        saleList.Add(sale);
                    }

                }
            }
            else if (cboCustomerName.SelectedIndex <= 0 && cboPCategory.SelectedIndex <= 0 && cboProductName.SelectedIndex > 0 && cboPSCategory.SelectedIndex <= 0)
            {
                data = data.Where(x => x.Date.Value.Date >= dtFrom.Value.Date && x.Date.Value.Date <= dtTo.Value.Date && x.ProductName == cboProductName.Text).ToList();

                if (data.Count != 0)
                {

                    foreach (var item in data)
                    {
                        var saleCount = entity.TransactionDetails.Where(x => x.TransactionId == item.TransationID).Count();
                        var additionalDis = (int)(item.DiscountAmount - (item.UnitPrice * item.DiscountPercentage / 100)) / saleCount;
                        var sale = new SaleForReport();
                        sale.Date = Convert.ToDateTime(item.Date);
                        sale.TransactionID = item.TransationID;
                        sale.ProductCode = item.ProductCode;
                        sale.ProductName = item.ProductName;
                        sale.Category = item.Category;
                        sale.SubCategory = item.SubCategory;
                        sale.PatientName = item.PatientName;
                        sale.PatientID = item.PatientID;
                        sale.SalePersonName = entity.Customers.Where(x => x.Id == item.SalePersonName).Select(x => x.Name).FirstOrDefault();
                        sale.UnitPrice = (int)item.UnitPrice;
                        sale.SaleQty = (int)item.SaleQty;
                        sale.GrossSale = (int)item.GrossSale;
                        sale.DiscountPercentage = item.DiscountPercentage;
                        sale.DiscountAmount = (int)((item.UnitPrice * item.DiscountPercentage / 100)) + additionalDis + (int)item.GiftAmt / saleCount;
                        sale.NetSaleAmount = (int)(item.GrossSale - (sale.DiscountAmount));
                        if (item.isPaid == false)
                        {
                            sale.RecieveAmount = (int)((item.RecieveAmount / saleCount));
                            sale.OutstandingAmount = (int)(item.GrossSale - sale.RecieveAmount) - sale.DiscountAmount;
                        }
                        else
                        {

                            sale.RecieveAmount = (int)(int)item.GrossSale - (sale.DiscountAmount);
                            sale.OutstandingAmount = 0;
                        }
                        if (item.IsFOC == true)
                        {
                            sale.FOCQty = (int)item.SaleQty;
                            sale.FOCAmount = (int)(item.UnitPrice * item.SaleQty);
                            sale.RecieveAmount = 0;
                            sale.NetSaleAmount = 0;
                        }


                        sale.PaymentType = item.PaymentType;
                        if (item.PaymentMethod == 1)
                        {
                            sale.PaymentMethod = "Cash";
                        }
                        else if (item.PaymentMethod == 2)
                        {
                            sale.PaymentMethod = "Credit";
                        }
                        else if (item.PaymentMethod == 5 || item.PaymentMethod >= 501)
                        {
                            sale.PaymentMethod = "Bank";
                            sale.BankPayment = item.PaymentMethodName;
                        }
                        else if (item.PaymentMethod == 4)
                        {
                            sale.PaymentMethod = "FOC";
                        }

                        sale.Note = item.Note;
                        saleList.Add(sale);
                    }

                }
            }
            else if (cboCustomerName.SelectedIndex <= 0 && cboPCategory.SelectedIndex <= 0 && cboProductName.SelectedIndex > 0 && cboPSCategory.SelectedIndex > 0)
            {
                data = data.Where(x => x.Date.Value.Date >= dtFrom.Value.Date && x.Date.Value.Date <= dtTo.Value.Date && x.ProductName == cboProductName.Text && x.SubCategory == cboPSCategory.Text).ToList();

                if (data.Count != 0)
                {

                    foreach (var item in data)
                    {
                        var saleCount = entity.TransactionDetails.Where(x => x.TransactionId == item.TransationID).Count();
                        var additionalDis = (int)(item.DiscountAmount - (item.UnitPrice * item.DiscountPercentage / 100)) / saleCount;
                        var sale = new SaleForReport();
                        sale.Date = Convert.ToDateTime(item.Date);
                        sale.TransactionID = item.TransationID;
                        sale.ProductCode = item.ProductCode;
                        sale.ProductName = item.ProductName;
                        sale.Category = item.Category;
                        sale.SubCategory = item.SubCategory;
                        sale.PatientName = item.PatientName;
                        sale.PatientID = item.PatientID;
                        sale.SalePersonName = entity.Customers.Where(x => x.Id == item.SalePersonName).Select(x => x.Name).FirstOrDefault();
                        sale.UnitPrice = (int)item.UnitPrice;
                        sale.SaleQty = (int)item.SaleQty;
                        sale.GrossSale = (int)item.GrossSale;
                        sale.DiscountPercentage = item.DiscountPercentage;
                        sale.DiscountAmount = (int)((item.UnitPrice * item.DiscountPercentage / 100)) + additionalDis + (int)item.GiftAmt / saleCount;
                        sale.NetSaleAmount = (int)(item.GrossSale - (sale.DiscountAmount));
                        if (item.isPaid == false)
                        {
                            sale.RecieveAmount = (int)((item.RecieveAmount / saleCount));
                            sale.OutstandingAmount = (int)(item.GrossSale - sale.RecieveAmount) - sale.DiscountAmount;
                        }
                        else
                        {

                            sale.RecieveAmount = (int)(int)item.GrossSale - (sale.DiscountAmount);
                            sale.OutstandingAmount = 0;
                        }
                        if (item.IsFOC == true)
                        {
                            sale.FOCQty = (int)item.SaleQty;
                            sale.FOCAmount = (int)(item.UnitPrice * item.SaleQty);
                            sale.RecieveAmount = 0;
                            sale.NetSaleAmount = 0;
                        }


                        sale.PaymentType = item.PaymentType;
                        if (item.PaymentMethod == 1)
                        {
                            sale.PaymentMethod = "Cash";
                        }
                        else if (item.PaymentMethod == 2)
                        {
                            sale.PaymentMethod = "Credit";
                        }
                        else if (item.PaymentMethod == 5 || item.PaymentMethod >= 501)
                        {
                            sale.PaymentMethod = "Bank";
                            sale.BankPayment = item.PaymentMethodName;
                        }
                        else if (item.PaymentMethod == 4)
                        {
                            sale.PaymentMethod = "FOC";
                        }

                        sale.Note = item.Note;
                        saleList.Add(sale);
                    }

                }
            }
            else if (cboCustomerName.SelectedIndex <= 0 && cboPCategory.SelectedIndex <= 0 && cboProductName.SelectedIndex <= 0 && cboPSCategory.SelectedIndex > 0)
            {
                data = data.Where(x => x.Date.Value.Date >= dtFrom.Value.Date && x.Date.Value.Date <= dtTo.Value.Date && x.SubCategory == cboPSCategory.Text).ToList();

                if (data.Count != 0)
                {

                    foreach (var item in data)
                    {
                        var saleCount = entity.TransactionDetails.Where(x => x.TransactionId == item.TransationID).Count();
                        var additionalDis = (int)(item.DiscountAmount - (item.UnitPrice * item.DiscountPercentage / 100)) / saleCount;
                        var sale = new SaleForReport();
                        sale.Date = Convert.ToDateTime(item.Date);
                        sale.TransactionID = item.TransationID;
                        sale.ProductCode = item.ProductCode;
                        sale.ProductName = item.ProductName;
                        sale.Category = item.Category;
                        sale.SubCategory = item.SubCategory;
                        sale.PatientName = item.PatientName;
                        sale.PatientID = item.PatientID;
                        sale.SalePersonName = entity.Customers.Where(x => x.Id == item.SalePersonName).Select(x => x.Name).FirstOrDefault();
                        sale.UnitPrice = (int)item.UnitPrice;
                        sale.SaleQty = (int)item.SaleQty;
                        sale.GrossSale = (int)item.GrossSale;
                        sale.DiscountPercentage = item.DiscountPercentage;
                        sale.DiscountAmount = (int)((item.UnitPrice * item.DiscountPercentage / 100)) + additionalDis + (int)item.GiftAmt / saleCount;
                        sale.NetSaleAmount = (int)(item.GrossSale - (sale.DiscountAmount));
                        if (item.isPaid == false)
                        {
                            sale.RecieveAmount = (int)((item.RecieveAmount / saleCount));
                            sale.OutstandingAmount = (int)(item.GrossSale - sale.RecieveAmount) - sale.DiscountAmount;
                        }
                        else
                        {

                            sale.RecieveAmount = (int)(int)item.GrossSale - (sale.DiscountAmount);
                            sale.OutstandingAmount = 0;
                        }
                        if (item.IsFOC == true)
                        {
                            sale.FOCQty = (int)item.SaleQty;
                            sale.FOCAmount = (int)(item.UnitPrice * item.SaleQty);
                            sale.RecieveAmount = 0;
                            sale.NetSaleAmount = 0;
                        }


                        sale.PaymentType = item.PaymentType;
                        if (item.PaymentMethod == 1)
                        {
                            sale.PaymentMethod = "Cash";
                        }
                        else if (item.PaymentMethod == 2)
                        {
                            sale.PaymentMethod = "Credit";
                        }
                        else if (item.PaymentMethod == 5 || item.PaymentMethod >= 501)
                        {
                            sale.PaymentMethod = "Bank";
                            sale.BankPayment = item.PaymentMethodName;
                        }
                        else if (item.PaymentMethod == 4)
                        {
                            sale.PaymentMethod = "FOC";
                        }

                        sale.Note = item.Note;
                        saleList.Add(sale);
                    }

                }
            }
            else
            {
                data = data.Where(x => x.Date.Value.Date >= dtFrom.Value.Date && x.Date.Value.Date <= dtTo.Value.Date).ToList();

                if (data.Count != 0)
                {

                    foreach (var item in data)
                    {
                        var saleCount = entity.TransactionDetails.Where(x => x.TransactionId == item.TransationID).Count();
                        var additionalDis =(int)(item.DiscountAmount -(item.UnitPrice* item.DiscountPercentage/100))/saleCount;
                        var sale = new SaleForReport();
                        sale.Date = Convert.ToDateTime(item.Date);
                        sale.TransactionID = item.TransationID;
                        sale.ProductCode = item.ProductCode;
                        sale.ProductName = item.ProductName;
                        sale.Category = item.Category;
                        sale.SubCategory = item.SubCategory;
                        sale.PatientName = item.PatientName;
                        sale.PatientID = item.PatientID;
                        sale.SalePersonName = entity.Customers.Where(x => x.Id == item.SalePersonName).Select(x => x.Name).FirstOrDefault();
                        sale.UnitPrice = (int)item.UnitPrice;
                        sale.SaleQty = (int)item.SaleQty;
                        sale.GrossSale = (int)item.GrossSale;
                        sale.DiscountPercentage = item.DiscountPercentage;
                        sale.DiscountAmount = (int)((item.UnitPrice * item.DiscountPercentage/100)) + additionalDis + (int)item.GiftAmt/saleCount;
                        sale.NetSaleAmount = (int)(item.GrossSale - (sale.DiscountAmount));
                        if (item.isPaid == false)
                        {
                            sale.RecieveAmount = (int)((item.RecieveAmount / saleCount));
                            sale.OutstandingAmount = (int)(item.GrossSale - sale.RecieveAmount)-sale.DiscountAmount;
                        }
                        else
                        {
                           
                            sale.RecieveAmount = (int)(int)item.GrossSale - (sale.DiscountAmount);
                            sale.OutstandingAmount = 0;
                        }
                        if (item.IsFOC == true)
                        {
                            sale.FOCQty = (int)item.SaleQty;
                            sale.FOCAmount = (int)(item.UnitPrice * item.SaleQty);
                            sale.RecieveAmount = 0;
                            sale.NetSaleAmount = 0;
                        }
                        

                        sale.PaymentType = item.PaymentType;
                        if (item.PaymentMethod == 1)
                        {
                            sale.PaymentMethod = "Cash";
                        }
                        else if (item.PaymentMethod == 2)
                        {
                            sale.PaymentMethod = "Credit";
                        }
                        else if (item.PaymentMethod == 5 || item.PaymentMethod >= 501)
                        {
                            sale.PaymentMethod = "Bank";
                            sale.BankPayment = item.PaymentMethodName;
                        }
                        else if (item.PaymentMethod == 4)
                        {
                            sale.PaymentMethod = "FOC";
                        }
                       
                        sale.Note = item.Note;
                        saleList.Add(sale);
                    }

                }
            }
            #endregion


            reportViewer1.Visible = true;
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "SaleReport";
            rds.Value = saleList;

            string reportPath = Application.StartupPath + "\\Reports\\SaleReport.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }


        private void inititalState()
        {
           
            bindCategory();
            bindSubCategory();
            bindProduct();
            bindPatient();
            dtFrom.Value = new DateTime(dtFrom.Value.Year, dtFrom.Value.Month, 1);
            dtTo.Value = DateTime.Now.Date;
            loadData();
        }

        #endregion

        #region Value Change

        private void cboPCategory_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboPCategory.SelectedIndex > 0)
            {
                var entity = new POSEntities();
                List<Product> productList = new List<Product>();
                Product product = new Product();
                product.Id = 0;
                product.Name = "Select";
                productList.Add(product);
                productList.AddRange(entity.Products.Where(x => x.ProductCategoryId == (int)cboPCategory.SelectedValue).ToList());
                cboProductName.DataSource = productList;
                cboProductName.DisplayMember = "Name";
                cboProductName.ValueMember = "Id";

                List<APP_Data.ProductSubCategory> pSList = new List<APP_Data.ProductSubCategory>();
                APP_Data.ProductSubCategory pCategory = new APP_Data.ProductSubCategory();
                pCategory.Id = 0;
                pCategory.Name = "Select";
                pSList.Add(pCategory);
                pSList.AddRange(entity.ProductSubCategories.Where(x => x.ProductCategoryId == (int)cboPCategory.SelectedValue).ToList());
                cboPSCategory.DataSource = pSList;
                cboPSCategory.DisplayMember = "Name";
                cboPSCategory.ValueMember = "Id";
            }
        }

        private void cboPSCategory_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboPSCategory.SelectedIndex > 0)
            {
                var entity = new POSEntities();
                List<Product> productList = new List<Product>();
                Product product = new Product();
                product.Id = 0;
                product.Name = "Select";
                productList.Add(product);
                productList.AddRange(entity.Products.Where(x => x.ProductSubCategoryId == (int)cboPSCategory.SelectedValue).ToList());
                cboProductName.DataSource = productList;
                cboProductName.DisplayMember = "Name";
                cboProductName.ValueMember = "Id";
            }
        }
        #endregion

        #region Class


        private class SaleForReport
        {
            public DateTime Date { get; set; }
            public string TransactionID { get; set; }
            public string ProductName { get; set; }
            public string ProductCode { get; set; }
            public string Category { get; set; }
            public string  SubCategory { get; set; }
            public string PatientName { get; set; }
            public string PatientID { get; set; }
            public string SalePersonName { get; set; }
            public int UnitPrice { get; set; }
            public int  SaleQty { get; set; }
            public int GrossSale { get; set; }
            public Decimal DiscountPercentage { get; set; }
            public int DiscountAmount { get; set; }
            public int FOCQty { get; set; }
            public int FOCAmount { get; set; }
            public int NetSaleAmount { get; set; }
            public int RecieveAmount { get; set; }
            public string PaymentType { get; set; }
            public string PaymentMethod { get; set; }
            public string BankPayment { get; set; }
            public int OutstandingAmount { get; set; }
            public string Note { get; set; }



        }
        #endregion

        #region Button Click

        private void btnSearch_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            inititalState();
        }
        #endregion
    }
}
